using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.AddressableAssets.GUI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;


[CreateAssetMenu(menuName = "Res/ResourcesDataBase")]
public class DB : SerializedScriptableObject
{
    public AssetReference refa;
    public AssetPack Dice;
    public AssetPack Sides;
    
    [Serializable]
    public struct AssetPair
    {
        [HideLabel] public string Key;
        [HideLabel] public AssetReference Reference;

        public AssetPair(string key, AssetReference asset)
        {
            Key = key;
            Reference = asset;
        }
    }

    [Button]
    public void Build()
    {
        Dice.Build();
        Sides.Build();
    }
}

[Serializable]
public struct AssetPack
{
    public string Path;
    public List<DB.AssetPair> pairs;

    public void Build()
    {
        string[] guids = AssetDatabase.FindAssets("t:Object", new[] { Path });
        pairs = new List<DB.AssetPair>();
        foreach (string assetGUID in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(assetGUID);
            Object a = AssetDatabase.LoadAssetAtPath<Object>(path);
            AssetReference aRef = new AssetReference(assetGUID);
            pairs.Add(new DB.AssetPair(aRef.editorAsset.name, aRef));
        }
    }
}