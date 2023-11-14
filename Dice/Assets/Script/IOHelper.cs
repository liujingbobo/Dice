// using UnityEngine;
// using System.Collections;
// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Reflection;
// using System.Runtime.Serialization.Formatters;
// using System.Text;
// using System.Security.Cryptography;
// using System.Threading;
// using System.Threading.Tasks;
// using Cysharp.Threading.Tasks;
// using Unity.Plastic.Newtonsoft.Json;
// using Unity.Plastic.Newtonsoft.Json.Serialization;
// using UnityEngine.Scripting;
//
// public static class IOHelper
// {
//     /// <summary>
//     /// 判断文件是否存在
//     /// </summary>
//     public static bool IsFileExists(string fileName) { return File.Exists(fileName); }
//
//     /// <summary>
//     /// 判断文件夹是否存在
//     /// </summary>
//     public static bool IsDirectoryExists(string fileName) { return Directory.Exists(fileName); }
//
//     /// <summary>
//     /// 创建一个文本文件    
//     /// </summary>
//     /// <param name="fileName">文件路径</param>
//     /// <param name="content">文件内容</param>
//     public static void CreateFile(string fileName, string content)
//     {
//         StreamWriter streamWriter = File.CreateText(fileName);
//         streamWriter.Write(content);
//         streamWriter.Close();
//     }
//
//     public static void DeleteFile(string fileName)
//     {
//         if (IsFileExists(fileName))
//         {
//             File.Delete(fileName);
//         }
//     }
//
//     /// <summary>
//     /// 创建一个文件夹
//     /// </summary>
//     public static void CreateDirectory(string fileName)
//     {
//         //文件夹存在则返回
//         if (IsDirectoryExists(fileName))
//             return;
//         Directory.CreateDirectory(fileName);
//     }
//
//     /// <summary>
//     /// 获取文件夹下所有文件路径
//     /// </summary>
//     public static FileInfo[] AllFileInDirectory(string fullpath)
//     {
//         //文件夹不存在则返回
//         if (!IsDirectoryExists(fullpath))
//             return null;
//         var di = new DirectoryInfo(fullpath);
//         FileInfo[] files = di.GetFiles("*", SearchOption.AllDirectories);
//         return files;
//     }
//
//     public static void SetData(string fileName, object pObject)
//     {
//         string toSave = SerializeObject_Newton(pObject);
//         //对字符串进行加密,32位加密密钥
//         //toSave = RijndaelEncrypt(toSave, "12345678910111213141516171819202");
//         var streamWriter = File.CreateText(fileName);
//         streamWriter.Write(toSave);
//         streamWriter.Close();
//     }
//     
//     public static async UniTask SetDataAsync(string fileName, string serializedText, CancellationToken c)
//     {
//         var toSave = serializedText;
//         //对字符串进行加密,32位加密密钥
//         //toSave = RijndaelEncrypt(toSave, "12345678910111213141516171819202");
//         var streamWriter = File.CreateText(fileName);
//         var task = streamWriter.WriteAsync(toSave);
//         await UniTask.WaitWhile(() => task.Status.Equals(TaskStatus.Running), cancellationToken: c);
//         streamWriter.Close();
//         if (c.IsCancellationRequested)
//         {
//             Debug.Log("Data Saving Cancelled");
//             c.ThrowIfCancellationRequested();
//         }
//     }
//
//     public static object GetData(string fileName, Type pType)
//     {
//         StreamReader streamReader = File.OpenText(fileName);
//         string data = streamReader.ReadToEnd();
//         //对数据进行解密，32位解密密钥
//         //data = RijndaelDecrypt(data, "12345678910111213141516171819202");
//         streamReader.Close();
//         //return DeserializeObject(data, pType);
//         return DeserializeObject_Newton(data, pType);
//     }
//
//     public static T GetData<T>(string filename) { return (T) GetData(filename, typeof(T)); }
//
//     /// <summary>
//     /// Rijndael加密算法
//     /// </summary>
//     /// <param name="pString">待加密的明文</param>
//     /// <param name="pKey">密钥,长度可以为:64位(byte[8]),128位(byte[16]),192位(byte[24]),256位(byte[32])</param>
//     /// <param name="iv">iv向量,长度为128（byte[16])</param>
//     /// <returns></returns>
//     private static string RijndaelEncrypt(string pString, string pKey)
//     {
//         //密钥
//         byte[] keyArray = UTF8Encoding.UTF8.GetBytes(pKey);
//         //待加密明文数组
//         byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(pString);
//
//         //Rijndael解密算法
//         RijndaelManaged rDel = new RijndaelManaged();
//         rDel.Key = keyArray;
//         rDel.Mode = CipherMode.ECB;
//         rDel.Padding = PaddingMode.PKCS7;
//         ICryptoTransform cTransform = rDel.CreateEncryptor();
//
//         //返回加密后的密文
//         byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
//         return Convert.ToBase64String(resultArray, 0, resultArray.Length);
//     }
//
//     /// <summary>
//     /// ijndael解密算法
//     /// </summary>
//     /// <param name="pString">待解密的密文</param>
//     /// <param name="pKey">密钥,长度可以为:64位(byte[8]),128位(byte[16]),192位(byte[24]),256位(byte[32])</param>
//     /// <param name="iv">iv向量,长度为128（byte[16])</param>
//     /// <returns></returns>
//     private static String RijndaelDecrypt(string pString, string pKey)
//     {
//         //解密密钥
//         byte[] keyArray = UTF8Encoding.UTF8.GetBytes(pKey);
//         //待解密密文数组
//         byte[] toEncryptArray = Convert.FromBase64String(pString);
//
//         //Rijndael解密算法
//         RijndaelManaged rDel = new RijndaelManaged();
//         rDel.Key = keyArray;
//         rDel.Mode = CipherMode.ECB;
//         rDel.Padding = PaddingMode.PKCS7;
//         ICryptoTransform cTransform = rDel.CreateDecryptor();
//
//         //返回解密后的明文
//         byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
//         return UTF8Encoding.UTF8.GetString(resultArray);
//     }
//
//
//     /// <summary>
//     /// 将一个对象序列化为字符串
//     /// </summary>
//     /// <returns>The object.</returns>
//     /// <param name="pObject">对象</param>
//     /// <param name="pType">对象类型</param>
//     private static string SerializeObject(object pObject)
//     {
//         //序列化后的字符串
//         string serializedString = string.Empty;
//         //使用Json.Net进行序列化
//         //serializedString = JsonConvert.SerializeObject(pObject);
//         serializedString = JsonUtility.ToJson(pObject);
//         return serializedString;
//     }
//
//     /// <summary>
//     /// 将一个字符串反序列化为对象
//     /// </summary>
//     /// <returns>The object.</returns>
//     /// <param name="pString">字符串</param>
//     /// <param name="pType">对象类型</param>
//     private static object DeserializeObject(string pString, Type pType)
//     {
//         //反序列化后的对象
//         object deserializedObject = null;
//         //使用Json.Net进行反序列化
//         //deserializedObject = JsonConvert.DeserializeObject(pString, pType);
//         deserializedObject = JsonUtility.FromJson(pString, pType);
//         return deserializedObject;
//     }
//
//     public class GlaboContractResolver : DefaultContractResolver
//     {
//         protected override List<MemberInfo> GetSerializableMembers(Type objectType)
//         {
//             var result = base.GetSerializableMembers(objectType);
//             result.RemoveAll(_ => !_.IsDefined(typeof(JsonPropertyAttribute)) && _.MemberType == MemberTypes.Property);
//             return result;
//         }
//     }
//
//     public static string SerializeObject_Newton(object pObject)
//     {
//         string serializedString = string.Empty;
//         JsonSerializerSettings settings = new JsonSerializerSettings()
//         {
//             ContractResolver = new GlaboContractResolver()
//         };
//         //Debug.Log(pObject.GetType());
//         serializedString = JsonConvert.SerializeObject(pObject, pObject.GetType(), settings);
//         return serializedString;
//     }
//
//     public static object DeserializeObject_Newton(string pString, Type pType)
//     {
//         object deserializedObject = null;
//         JsonSerializerSettings settings = new JsonSerializerSettings()
//         {
//             ContractResolver = new GlaboContractResolver()
//         };
//         deserializedObject = JsonConvert.DeserializeObject(pString, pType);
//         return deserializedObject;
//     }
//
//     // public static object Resolve(this IAbstractSerializable obj)
//     // {
//     //     string type = obj.GetTypeName();
//     //     if (type.IsEmpty()) return null;
//     //     object ser_o = DeserializeObject_Newton(obj.GetSerializedData(), Type.GetType(type));
//     //     return ser_o;
//     // }
//
//     public static void Desolve(this IAbstractSerializable save, object obj)
//     {
//         if (obj == null)
//         {
//             save.SetTypeName("");
//             save.SetSerializedData("");
//         }
//         else
//         {
//             Type ty = obj.GetType();
//             save.SetTypeName(ty.FullName);
//             save.SetSerializedData(SerializeObject_Newton(obj));
//         }
//     }
//
//     public static string ToGLBTimeStamp(this System.DateTime date)
//     {
//         return $"{date.Year}-{date.Month}-{date.Day}-{date.Hour}-{date.Minute}-{date.Second}";
//     }
// }
//
// public interface IAbstractSerializable
// {
//     string GetTypeName();
//     void SetTypeName(string n);
//     string GetSerializedData();
//     void SetSerializedData(string data);
// }
//
// public class SerializableState : IAbstractSerializable
// {
//     [JsonIgnore] public object State;
//     public string TypeName;
//     public string StateSerializedInfo;
//
//     public SerializableState() { }
//
//     public SerializableState(object state) { this.Desolve(state); }
//
//     public SerializableState(string typeName, string stateSerializedInfo)
//     {
//         TypeName = typeName;
//         StateSerializedInfo = stateSerializedInfo;
//         // State = this.Resolve();
//     }
//
//     public T1 GetState<T1>()
//     {
//         if (State == null || State.GetType() != typeof(T1))
//             // State = this.Resolve();
//         return (T1) State;
//     }
//
//     string IAbstractSerializable.GetSerializedData() { return StateSerializedInfo; }
//
//     string IAbstractSerializable.GetTypeName() { return TypeName; }
//
//     void IAbstractSerializable.SetSerializedData(string data) { StateSerializedInfo = data; }
//
//     void IAbstractSerializable.SetTypeName(string n) { TypeName = n; }
// }
// [Serializable,Preserve]
// public struct SerializableText
// {
//     public string TypeName;
//     public string StateSerializedInfo;
//
//     public SerializableText(string typeName, string states)
//     {
//         TypeName = typeName;
//         StateSerializedInfo = states;
//     }
//
//     // public bool HasData => TypeName.IsNotEmpty() && StateSerializedInfo.IsNotEmpty();
//     public SerializableState State => (SerializableState) this;
//
//     public static implicit operator SerializableText(SerializableState state)
//     {
//         return new SerializableText()
//         {
//             TypeName = state.TypeName,
//             StateSerializedInfo = state.StateSerializedInfo
//         };
//     }
//
//     public static implicit operator SerializableState(SerializableText text)
//     {
//         return new SerializableState(text.TypeName, text.StateSerializedInfo);
//     }
// }