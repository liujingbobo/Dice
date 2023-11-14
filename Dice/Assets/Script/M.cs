// //常用数学
//
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;
//
// public static class M
// {
//     private static System.Random _currentRandom = new System.Random(DateTime.Now.Millisecond);
//     private static System.Random _currentRandomSaved;
//
//     public static int GenRandomSeed()
//     {
//         return DateTime.Now.Millisecond;
//     }
//
//     public static void InitRandomSeed(int seed)
//     {
//         _currentRandom = new System.Random(seed);
//     }
//
//     public static void SwitchRandom(System.Random random)
//     {
//         if (_currentRandomSaved != null)
//         {
//             Debug.LogError("Saved random object not switched back");
//             return;
//         }
//
//         _currentRandomSaved = _currentRandom;
//         _currentRandom = random;
//     }
//
//     public static void SwitchRandomBack()
//     {
//         if (_currentRandomSaved == null)
//         {
//             Debug.LogError("No random Saved");
//             return;
//         }
//
//         _currentRandom = _currentRandomSaved;
//         _currentRandomSaved = null;
//     }
//
//     public static float Rng()
//     {
//         return (float)_currentRandom.NextDouble();
//     }
//
//     ///r is not contained
//     public static int Rng(int l, int r)
//     {
//         return _currentRandom.Next(l, r);
//     }
//
//     ///r is not contained
//     public static float Rng(float l, float r)
//     {
//         return l + (float)_currentRandom.NextDouble() * (r - l);
//     }
//
//     public static float RngGaussian(float l, float r)
//     {
//         return Rng(l / 2f, r / 2f) + Rng(l / 2f, r / 2f);
//     }
//
//     public static bool RngBool()
//     {
//         return Rng(0, 2) == 0;
//     }
//
//     public static bool RngBool(float trueChance)
//     {
//         return Rng() <= trueChance;
//     }
//
//     public static bool HitProbability(float probability)
//     {
//         return Rng(0f, 1f) < probability;
//     }
//
//     public static int RngFlag()
//     {
//         return RngBool() ? 1 : -1;
//     }
//
//     public static int RngToInt(float f)
//     {
//         var result = Mathf.FloorToInt(f);
//         var rng = Rng();
//         if (rng <= f - result) result++;
//         return result;
//     }
//
//     public static int Sign(float f)
//     {
//         if (Mathf.Approximately(f, 0f)) return 0;
//         else if (f > 0f) return 1;
//         else return -1;
//     }
//
//     public static float Min(float left, float right)
//     {
//         return Mathf.Min(left, right);
//     }
//
//     public static Vector2 Angle2Vec2(float angle)
//     {
//         angle.Mod(360f);
//         var rad = angle * Mathf.Deg2Rad;
//         return new Vector2(Mathf.Sin(rad), -Mathf.Cos(rad));
//     }
//
//     public static float Vec2Angle(Vector2 v2)
//     {
//         var a = Vector2.Angle(v2, Vector2.down);
//         if (v2.x < 0f) a = 360f - a;
//         return a;
//     }
//
//     public static float Vec2AngleUp(Vector2 v2)
//     {
//         float a = Vector2.Angle(v2, Vector2.up);
//         if (v2.x > 0)
//         {
//             a = -a;
//         }
//
//         return a;
//     }
//
//     public static float Mod(this float value, float mod)
//     {
//         var v = value % mod;
//         if (v < 0) v += mod;
//         return v;
//     }
//
//     public static int Mod(this int value, int mod)
//     {
//         if (mod == 0) return 0;
//         var v = value % mod;
//         if (v < 0) v += mod;
//         return v;
//     }
//
//     public static int RingMinDistance(this int value, int target, int length)
//     {
//         var half = length / 2;
//         var dis = (target - value).Mod(length);
//         if (dis > half) dis -= length;
//         return dis;
//     }
//
//     public static int RingPositiveDistance(this int value, int target, int length)
//     {
//         return (target - value).Mod(length);
//     }
//
//     public static float RingPositiveDistance(this float value, float target, int length)
//     {
//         return (target - value).Mod(length);
//     }
//
//     public static float CalcIncrease(this Pair<float, float> curve, float x)
//     {
//         return curve.Key + curve.Value * x;
//     }
//
//     public static bool IsIn(this int value, int min, int max)
//     {
//         return value >= min && value <= max;
//     }
//
//     public static float SinPhase(float time, float duration)
//     {
//         return Mathf.Sin(time / duration * 2f * Mathf.PI);
//     }
//
//     public static float SinPositivePhase(float time, float duration)
//     {
//         return (SinPhase(time, duration) + 1f) / 2f;
//     }
//
//     public static float CosPhase(float time, float duration)
//     {
//         return Mathf.Cos(time / duration * 2f * Mathf.PI);
//     }
//
//     public static float CosPositivePhase(float time, float duration)
//     {
//         return (CosPhase(time, duration) + 1f) / 2f;
//     }
//
//     public static int RoundToIntLocked(this float rawNum, int lockInt)
//     {
//         var sign = Math.Sign(lockInt);
//         var l = Mathf.Max(1, lockInt);
//         return Mathf.RoundToInt(rawNum / l) * l * sign;
//     }
//
//     public static int CeilToIntLocked(this float rawNum, int lockInt)
//     {
//         var l = Mathf.Max(1, lockInt);
//         return Mathf.CeilToInt(rawNum / l) * l;
//     }
//
//     public static int FloorToIntLocked(this float rawNum, int lockInt)
//     {
//         var l = Mathf.Max(1, lockInt);
//         return Mathf.FloorToInt(rawNum / l) * l;
//     }
//
//     public static Vector3 Liner(float t, Vector3 p0, Vector3 p1)
//     {
//         var u = 1 - t;
//         return u * p0 + t * p1;
//     }
//
//     public static Vector3 QuadBezier(float t, Vector3 p0, Vector3 p1, Vector3 p2)
//     {
//         var u = 1 - t;
//         var tt = t * t;
//         var uu = u * u;
//         return uu * p0 + 2f * u * t * p1 + tt * p2;
//     }
//
//     public static Vector2 Parabola(Vector2 start, Vector2 end, float height, float t)
//     {
//         float Func(float x) => 4 * (-height * x * x + height * x);
//
//         var mid = Vector2.Lerp(start, end, t);
//
//         return new Vector2(mid.x, Func(t) + Mathf.Lerp(start.y, end.y, t));
//     }
//
//     public static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
//     {
//         float Func(float x) => 4 * (-height * x * x + height * x);
//
//         var mid = Vector3.Lerp(start, end, t);
//
//         return new Vector3(mid.x, Func(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
//     }
//
//     public static Vector3 AutoQuadBezier(float t, float height, Vector3 p0, Vector3 p2,
//         float ratio = VslStgs.Bullet_Default_Ratio)
//     {
//         //if (ratio == 0f && height == 0f || Mathf.Approximately(p2.y, p0.y) && Mathf.Approximately(p2.z, p0.z))
//         if (ratio == 0f && height == 0f)
//             return Liner(t, p0, p2);
//         // OLD:
//         // var off = p2 - p0;
//         // off.x *= -1f;
//         // off *= ratio;
//         // var p1 = p0 + off;
//         // p1.z -= height;
//
//         var downToUp = p2.y > p0.y;
//         var handle = new Vector3(downToUp ? p0.x : p2.x, downToUp ? p2.y : p0.y, (p0.z + p2.z) / 2f);
//         var center = (p0 + p2) / 2f;
//         var p1 = Vector3.LerpUnclamped(center, handle, ratio) + height * Vector3.back;
//         return QuadBezier(t, p0, p1, p2);
//     }
//
//     public static int[] Shutter(int total, int split, int max, bool removeZero = false)
//     {
//         var l = new List<int>();
//         var result = new List<int>();
//         T.DoTimes(split, () => l.Add(0));
//         T.DoTimes(total, () =>
//         {
//             if (l.Count <= 0) return;
//             var ind = M.Rng(0, l.Count);
//             l[ind]++;
//             if (l[ind] >= max)
//             {
//                 result.Add(l[ind]);
//                 l.RemoveAt(ind);
//             }
//         });
//         if (removeZero)
//         {
//             l.RemoveAll((c) => c <= 0);
//         }
//
//         result.AddRange(l);
//         return result.ToArray();
//     }
//
//     public static Vector3 GetIntersectWithLineAndPlane(Vector3 point, Vector3 direct, Vector3 planeNormal,
//         Vector3 planePoint)
//     {
//         float d = Vector3.Dot(planePoint - point, planeNormal) / Vector3.Dot(direct.normalized, planeNormal);
//         return d * direct.normalized + point;
//     }
//
//     public static float CrossProduct(Vector2 A, Vector2 B, Vector2 C)
//     {
//         return (B.x - A.x) * (C.y - A.y) - (B.y - A.y) * (C.x - A.x);
//     }
//
//     public static float TriangleArea(Vector2 A, Vector2 B, Vector2 C)
//     {
//         return 0.5f * Math.Abs((B.x - A.x) * (C.y - A.y) - (C.x - A.x) * (B.y - A.y));
//     }
//
//     public static bool IsPointInsideTriangle(Vector2 P, Vector2 TriA, Vector2 TriB, Vector2 TriC)
//     {
//         var tolerance = 1e-3f;
//         double mainArea = TriangleArea(TriA, TriB, TriC);
//         double area1 = TriangleArea(P, TriA, TriB);
//         double area2 = TriangleArea(P, TriB, TriC);
//         double area3 = TriangleArea(P, TriC, TriA);
//
//         return Math.Abs(mainArea - (area1 + area2 + area3)) <= tolerance;
//     }
//
//     public static bool IsPointInsideTriangle(Vector2 P, Vector2[] Tri)
//     {
//         return IsPointInsideTriangle(P, Tri[0], Tri[1], Tri[2]);
//     }
//
//     [Serializable]
//     public class WeightedPool<T>
//     {
//         [InlineProperty, LabelText("Pool"), HideReferenceObjectPicker, SerializeField, JsonRequired
//          , ListDrawerSettings(Expanded = true, CustomAddFunction = "AddDefault")]
//         private List<Pair<T, float>> pool = new List<Pair<T, float>>();
//
//         private float TotalWeight => TempPool.Sum(_ => _.Value);
//
//         protected List<Pair<T, float>> TempPool
//         {
//             get
//             {
//                 temp_pool ??= new List<Pair<T, float>>(pool);
//                 return temp_pool;
//             }
//         }
//
//         private List<Pair<T, float>> temp_pool;
//
//         public T GetItemRng(bool removeIt = false)
//         {
//             var rng = Rng() * TotalWeight;
//             var temp = 0f;
//             for (var i = 0; i < TempPool.Count; i++)
//             {
//                 temp += TempPool[i].Value;
//                 if (temp >= rng)
//                 {
//                     var result = TempPool[i].Key;
//                     if (removeIt) TempPool.RemoveAt(i);
//                     return result;
//                 }
//             }
//
//             Debug.LogWarning("Pool Rng Failed");
//             return default;
//         }
//
//         public List<T> GetItemsRng(int count, bool distinct)
//         {
//             List<T> items = new List<T>();
//
//             if (distinct)
//             {
//                 if (TempPool.Count < count)
//                 {
//                     return TempPool.Select(_ => _.Key).ToList();
//                 }
//
//                 var tempPool = TempPool.ToList();
//
//                 for (int index = 0; index < count; index++)
//                 {
//                     var rng = Rng() * tempPool.Sum(_ => _.Value);
//                     var temp = 0f;
//                     for (var i = 0; i < tempPool.Count; i++)
//                     {
//                         temp += tempPool[i].Value;
//                         if (temp >= rng)
//                         {
//                             items.Add(tempPool[i].Key);
//                             tempPool.RemoveAt(i);
//                             break;
//                         }
//                     }
//                 }
//             }
//             else
//             {
//                 for (int i = 0; i < count; i++)
//                 {
//                     items.Add(GetItemRng());
//                 }
//             }
//
//             return items;
//         }
//
//         public int FindIndex(T result)
//         {
//             for (var i = 0; i < pool.Count; i++)
//             {
//                 if (pool[i].Key.Equals(result))
//                 {
//                     return i;
//                 }
//             }
//
//             return -1;
//         }
//
//         public T GetByIndex(int index)
//         {
//             return pool[index].Key;
//         }
//
//         public void ResetTemp()
//         {
//             temp_pool = null;
//         }
//
//         public bool Any(Predicate<T> filter = null)
//         {
//             if (filter == null) return pool.Count > 0;
//             return pool.Any(_ => filter(_.Key));
//         }
//
//         public bool AnyTemp(Predicate<T> filter = null)
//         {
//             if (filter == null) return TempPool.Count > 0;
//             return TempPool.Any(_ => filter(_.Key));
//         }
//         
//         private Pair<T, float> AddDefault()
//         {
//             return new Pair<T, float>(default, 1f);
//         }
//
//         public WeightedPool()
//         {
//         }
//
//         public WeightedPool(IEnumerable<Pair<T, float>> weights)
//         {
//             pool.AddRange(weights);
//         }
//
//         public WeightedPool(IEnumerable<(T, float)> weights)
//         {
//             pool.AddRange(weights.Select(_ => new Pair<T, float>(_.Item1, _.Item2)));
//         }
//
//         public WeightedPool<T> SelectBy(Predicate<T> filter)
//         {
//             return new WeightedPool<T>(pool.Where(_ => filter(_.Key)));
//         }
//         
//         public WeightedPool<T> GetCopy()
//         {
//             return new WeightedPool<T>(pool);
//         }
//
//         public IEnumerable<T> GetValues()
//         {
//             foreach (var pair in pool)
//             {
//                 yield return pair.Key;
//             }
//         }
//     }
// }