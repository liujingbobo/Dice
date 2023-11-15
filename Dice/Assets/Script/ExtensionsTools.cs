using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public static class ExtendTools
{
    public static void Shuffle<T>(this List<T> list)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            int ind = UnityEngine.Random.Range(0, i + 1);
            var t = list[ind];
            list.RemoveAt(ind);
            list.Add(t);
        }
    }

    public static int FirstIndexOf<T>(this IList<T> collection, Predicate<T> predicate)
    {
        for (int i = 0; i < collection.Count; i++)
        {
            if (predicate(collection[i])) return i;
        }

        return collection.Count;
    }

    public static T Pop<T>(this IList<T> list, int index)
    {
        var t = list[index];
        list.RemoveAt(index);
        return t;
    }

    public static T PopFirst<T>(this IList<T> list, Predicate<T> predicate)
    {
        for (var i = 0; i < list.Count; i++)
        {
            var t = list[i];
            if (predicate(t))
            {
                list.RemoveAt(i);
                return t;
            }
        }

        throw new Exception("Cannot pop element in list");
    }

    public static T PopLast<T>(this IList<T> list)
    {
        return list.Pop(list.Count - 1);
    }

    public static void AddSafely<T>(this ISet<T> set, object item)
    {
        if (item is T i) set.Add(i);
    }

    public static void SetElementAndSwapAlreadyContains<T>(this IList<T> list, T newElement, int index,
        Func<T, T, bool> equal)
    {
        if (index >= list.Count || index < 0) throw new Exception($"Index error ({index})");
        var raw = list[index];
        for (int i = 0; i < list.Count; i++)
        {
            var ch = list[i];
            if (equal(ch, newElement))
            {
                list[i] = raw;
            }
        }

        list[index] = newElement;
    }

    public static void InitReactivePropertyList<T>(this IList<ReactiveProperty<T>> props, IList<T> values,
        Action<T, int> subscribe)
    {
        for (int i = 0; i < values.Count && i < props.Count; i++)
        {
            var index = i;
            var p = props[index];
            p?.Dispose();
            p = new ReactiveProperty<T>(values[index]);
            p.Subscribe(_ => subscribe(_, index));
            props[index] = p;
        }
    }

    public static void SetReactivePropertyList<T>(this IList<ReactiveProperty<T>> props, IList<T> values)
    {
        for (int i = 0; i < values.Count && i < props.Count; i++)
        {
            var index = i;
            var p = props[index];
            p.Value = values[i];
        }
    }

    public static IEnumerable<T> EnumReverse<T>(this List<T> list)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            yield return list[i];
        }
    }

    public static TL SetForEachElement<TL, T>(this TL array, Func<T> elementGetter) where TL : IList<T>
    {
        for (var i = 0; i < array.Count; i++) array[i] = elementGetter();
        return array;
    }

    public static TL SetForEachElement<TL, T>(this TL array, Func<int, T> elementGetter) where TL : IList<T>
    {
        for (var i = 0; i < array.Count; i++) array[i] = elementGetter(i);
        return array;
    }

    public static IEnumerable<(T1, T2)> PairWith<T1, T2>(this IEnumerable<T1> e1, IEnumerable<T2> e2)
    {
        var enumerator2 = e2.GetEnumerator();
        foreach (var t1 in e1)
        {
            if (enumerator2.MoveNext())
            {
                yield return (t1, enumerator2.Current);
            }
            else break;
        }

        enumerator2.Dispose();
    }

    public static IEnumerable<(T1, T2)> PairWithSafely<T1, T2>(this IEnumerable<T1> e1, IEnumerable<T2> e2,
        T2 defaultValue = default)
    {
        var enumerator2 = e2.GetEnumerator();
        foreach (var t1 in e1)
        {
            if (enumerator2.MoveNext())
            {
                yield return (t1, enumerator2.Current);
            }
            else
            {
                yield return (t1, defaultValue);
            }
        }

        enumerator2.Dispose();
    }

    public static IEnumerable<T1> Flatten<T1>(this IEnumerable<(T1, int)> e1)
    {
        foreach (var (t, count) in e1)
        {
            for (var i = 0; i < count; i++) yield return t;
        }
    }

    public static bool TryFindIndex<T>(this IList<T> list, T target, out int index) where T : class
    {
        index = -1;
        if (list == null) return false;
        for (var i = 0; i < list.Count; i++)
        {
            if (list[i] == target)
            {
                index = i;
                return true;
            }
        }

        return false;
    }

    public static List<T> SplitByte<T>(this T byteEnum) where T : Enum
    {
        if (!typeof(T).IsEnum) throw new ArgumentException("T must be an enum type");
        var list = new List<T>();
        foreach (T item in Enum.GetValues(typeof(T)))
        {
            if (byteEnum.HasFlag(item)) list.Add(item);
        }

        return list;
    }

    public static string ToStyle(this string s, string stylename)
    {
        return string.Format("<style=\"{0}\">{1}</style>", stylename, s);
    }

    public static string ToKeyword(this string s, string keyword)
    {
        return string.Format("<k={0}>{1}</k>", keyword, s);
    }

    public static string ToAlign(this string s, string alignname)
    {
        return string.Format("<align={0}>{1}</align>", alignname, s);
    }

    public static string ToFix(this float f)
    {
        return f.ToString("+0.###;-0.###;+0");
        /*
        if (f == 0f)
        {
            return "0";
        }
        else if (f > 0f)
        {
            return "+" + f.ToString();
        }
        else
        {
            return f.ToString();
        }
        */
    }

    public static string ToFix(this int f)
    {
        return f.ToString("+0;-0;+0");
        /*
        if (f == 0f)
        {
            return "0";
        }
        else if (f > 0f)
        {
            return "+" + f.ToString();
        }
        else
        {
            return f.ToString();
        }
        */
    }

    public static string ToPercentFix(this float f)
    {
        return f.ToString("+0%;-0%;+0%");

        /*
        if (f == 0f)
        {
            return "0";
        }
        else if (f > 0f)
        {
            return "+" + f.ToString();
        }
        else
        {
            return f.ToString();
        }
        */
    }

    public static string ToPercent(this float f)
    {
        return f.ToString("0%;0%;0%");
    }

    public static bool RangeCross(this Vector2Int l, Vector2Int r)
    {
        Vector2Int shortRange = l;
        Vector2Int longRange = r;
        if (l.y - l.x > r.y - r.x)
        {
            shortRange = r;
            longRange = l;
        }

        if ((shortRange.x >= longRange.x && shortRange.x <= longRange.y) ||
            ((shortRange.y >= longRange.x && shortRange.y <= longRange.y)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static byte ShiftByte(this byte b, int left)
    {
        if (b == 0) return 0;
        for (int i = 0; i < left; i++)
        {
            byte tb = (byte)(b << 1);
            tb >>= 1;
            bool fix = b - tb > 0;
            b <<= 1;
            if (fix) b |= 1;
        }

        return b;
    }

    public static string ToDirDesc(this int i)
    {
        i = i % 4;
        switch (i)
        {
            case 0:
                return "上";
            case 1:
                return "左";
            case 2:
                return "下";
            case 3:
                return "右";
        }

        return "?";
    }


    public static bool Compare(this MComparison comparison, float l, float r)
    {
        bool result = false;
        if ((comparison & MComparison.bigger) != 0)
        {
            result = result || l > r;
        }

        if ((comparison & MComparison.equal) != 0)
        {
            result = result || l == r;
        }

        if ((comparison & MComparison.smaller) != 0)
        {
            result = result || l < r;
        }

        return result;
    }
    
    public static IEnumerator Checked(this IEnumerator content)
    {
        for (IEnumerator it = content; it.MoveNext();)
            if (it.Current != null)
                yield return it.Current;
    }

    public static void GoIEnumerator(this IEnumerator content)
    {
        for (IEnumerator it = content; it.MoveNext();)
        {
            if (it.Current is IEnumerator)
            {
                GoIEnumerator((IEnumerator)it.Current);
            }
        }
    }

    public static float RGB2Gray(Color c)
    {
        return (c.r * 0.299f + c.g * 0.587f + c.b * 0.114f);
    }



    public static List<T> ResetTo<T>(this List<T> list, IEnumerable<T> e)
    {
        if (list != null)
            list.Clear();
        else
            list = new List<T>();

        list.AddRange(e);
        return list;
    }

    public static T[] RandomlyGetSome<T>(this List<T> list, int count, bool removeIt = false)
    {
        if (count <= 0) return new T[0];
        T[] result = new T[Mathf.Min(count, list.Count)];
        int l = list.Count;
        for (int i = 0; i < result.Length; i++)
        {
            int r = M.Rng(0, l);
            T o = list[r];
            list[r] = list[l - 1];
            list[l - 1] = o;
            result[i] = o;
            l--;
            if (removeIt) list.RemoveAt(l);
        }

        return result;
    }

    public static T RandomlyGetOne<T>(this List<T> list, bool removeIt = false)
    {
        if (list.Count <= 0) return default(T);
        int i = M.Rng(0, list.Count);
        T o = list[i];
        if (removeIt) list.RemoveAt(i);
        return o;
    }

    public static T RandomlyGetOne<T>(this List<T> list, Predicate<T> checkRemove)
    {
        if (list.Count <= 0) return default(T);
        int i = M.Rng(0, list.Count);
        T o = list[i];
        if (checkRemove(o)) list.RemoveAt(i);
        return o;
    }

    public static T RandomlyGetOne<T>(this T[] list)
    {
        if (list.Length > 0)
            return list[M.Rng(0, list.Length)];
        return default;
    }

    public static (T, int) RandomlyGetOneWithIndex<T>(this T[] list)
    {
        if (list.Length > 0)
        {
            int index = M.Rng(0, list.Length);
            return (list[index], index);
        }

        return default;
    }

    public static T RandomlyGetOne<T>(this List<T> list, out int index)
    {
        index = 0;
        if (list.Count <= 0) return default;
        index = M.Rng(0, list.Count);
        var o = list[index];
        return o;
    }

    public static T RandomlyGetOneWithWeight<T>(this IEnumerable<T> collection, Func<T, float> weightGetter)
    {
        var pool = new WeightedRandomList<T>();
        foreach (var t in collection)
        {
            var weight = weightGetter(t);
            pool.Add(t, weight);
        }

        if (pool.Count > 0) return pool[pool.GetRandomIndex()];
        return default;
    }

    public static bool Compare(this Comparison comparison, float left, float right)
    {
        bool b = left > right && (comparison & Comparison.bigger) > 0;
        bool e = left == right && (comparison & Comparison.equal) > 0;
        bool l = left < right && (comparison & Comparison.less) > 0;
        return b || e || l;
    }

    public static bool Check(this SourceCheck sc, bool source)
    {
        var only = (sc & SourceCheck.Only) > 0;
        var disabled = (sc & SourceCheck.Disabled) > 0;
        return sc == SourceCheck.Any || !(disabled && source || only && !source);
    }

    public static bool Check(this DisabledSourceCheck sc, bool source)
    {
        return sc switch
        {
            DisabledSourceCheck.Disabled => !source,
            DisabledSourceCheck.Only => source,
            DisabledSourceCheck.Any => true,
            _ => true
        };
    }

    public static bool IsIn<TE>(this TE source, TE range) where TE : Enum
    {
        return (Convert.ToInt32(source) & Convert.ToInt32(range)) > 0;
    }

    public static int FlagCount<TE>(this TE e) where TE : Enum
    {
        var count = 0;
        foreach (TE item in Enum.GetValues(typeof(TE)))
        {
            if ((Convert.ToInt32(item) & Convert.ToInt32(e)) > 0)
            {
                count++;
            }
        }

        return count;
    }

    public static bool IsEmpty(this string s)
    {
        return string.IsNullOrEmpty(s);
    }

    public static bool IsNotEmpty(this string s)
    {
        return !s.IsEmpty();
    }

    public static bool TrueForAll<T>(this T[] s, Predicate<T> check)
    {
        foreach (T i in s)
        {
            if (!check(i)) return false;
        }

        return true;
    }

    public static IEnumerator Insert(this IEnumerator enumerator, params IEnumerator[] ies)
    {
        foreach (IEnumerator ie in ies)
        {
            yield return ie;
        }

        yield return enumerator;
    }

    public static IEnumerator Append(this IEnumerator enumerator, params IEnumerator[] ies)
    {
        yield return enumerator;
        foreach (IEnumerator ie in ies)
        {
            yield return ie;
        }
    }

    public static T RandomlyGetWithPossibility<T>(this List<Pair<float, T>> source)
    {
        float temp = 0;
        foreach (var p in source)
        {
            temp += p.Key;
        }

        float randnum = Random.Range(0, temp * 100);
        temp = 0;
        foreach (var p in source)
        {
            temp += p.Key * 100;
            if (randnum <= temp)
            {
                return p.Value;
            }
        }

        return source[0].Value;
    }

    public static List<T> RandomlyGetWithPossibility<T>(this List<Pair<float, T>> source, int amt)
    {
        List<T> result = new List<T>();
        for (int i = 0; i < amt; i++)
        {
            result.Add(source.RandomlyGetWithPossibility());
        }

        return result;
    }

    public static IWeighted RandomlyGetWithWeighted(this List<IWeighted> source)
    {
        float temp = 0;
        foreach (var p in source)
        {
            temp += p.GetWeight();
        }

        float randnum = Random.Range(0, temp * 100);
        temp = 0;
        foreach (var p in source)
        {
            temp += p.GetWeight() * 100;
            if (randnum <= temp)
            {
                return p;
            }
        }

        return source[0];
    }

    public static T RandomlyGetWithPossibility<T>(this Dictionary<T, float> source)
    {
        float temp = 0;
        foreach (var p in source)
        {
            temp += p.Value;
        }

        float randnum = Random.Range(0, temp * 100);
        temp = 0;

        foreach (var p in source)
        {
            temp += p.Value * 100;
            if (randnum <= temp)
            {
                return p.Key;
            }
        }

        return source.Keys.First();
    }


    public static IEnumerator CallBack(this IEnumerator enumerator, Action callBack)
    {
        yield return enumerator;
        callBack();
    }

    public static Texture2D ToSquare(this Sprite sprite)
    {
        Rect rect = sprite.textureRect;
        int x = (int)rect.x;
        int y = (int)rect.y;
        int w = (int)rect.width;
        int h = (int)rect.height;
        int s = Mathf.Min(w, h);
        Texture2D texture = sprite.texture;
        Texture2D square = new Texture2D(s, s);
        square.filterMode = FilterMode.Point;
        Color[] colors = texture.GetPixels(x, y + h - s, s, s);
        square.SetPixels(colors);
        square.Apply();
        return square;
    }

    public static Texture2D TransparentTex2D(int width, int height)
    {
        Texture2D tex = new Texture2D(width, height);
        Color[] colors = tex.GetPixels();
        for (int i = 0; i > colors.Length; i++) colors[i].a = 0;
        tex.SetPixels(colors);
        tex.Apply();
        return tex;
    }

    public static Texture2D ToTex2D(this Sprite sprite)
    {
        Rect rect = sprite.textureRect;
        Texture2D texture = sprite.texture;
        return texture.ToTex2D(rect);
    }

    public static Texture2D CopyTex2D(this Texture2D tex)
    {
        int x = (int)0;
        int y = (int)0;
        int w = (int)tex.width;
        int h = (int)tex.height;
        Texture2D texture = tex;
        Texture2D new_tex = new Texture2D(w, h);
        new_tex.filterMode = FilterMode.Point;
        Color[] colors = texture.GetPixels(x, y, w, h);
        new_tex.SetPixels(colors);
        new_tex.Apply();
        return new_tex;
    }

    public static Texture2D ToTex2D(this Texture2D tex, Rect rect)
    {
        int x = (int)rect.x;
        int y = (int)rect.y;
        int w = Mathf.CeilToInt(rect.width);
        int h = Mathf.CeilToInt(rect.height);
        Texture2D texture = tex;
        Texture2D new_tex = new Texture2D(w, h);
        new_tex.filterMode = FilterMode.Point;
        Color[] colors = texture.GetPixels(x, y, w, h);
        new_tex.SetPixels(colors);
        new_tex.Apply();
        return new_tex;
    }

    public static Texture2D BlendSprite(this Texture2D raw, Sprite add, bool useRawAlpha = false)
    {
        Rect rect = add.textureRect;
        return raw.BlendSprite(add.texture, rect, useRawAlpha);
    }

    public static Texture2D BlendSprite(this Texture2D raw, Texture2D add, bool useRawAlpha = false,
        Vector2Int offset = default)
    {
        Rect rect = new Rect(0, 0, add.width, add.height);
        return raw.BlendSprite(add, rect, useRawAlpha, offset);
    }

    public static Texture2D BlendSprite(this Texture2D raw, Texture2D add, Rect rect, bool useRawAlpha = false,
        Vector2Int offset = default)
    {
        int xo = (int)rect.x;
        int yo = (int)rect.y;
        int w = Mathf.Min((int)rect.width, raw.width);
        int h = Mathf.Min((int)rect.height, raw.height);
        int wof = (raw.width - w) / 2 + offset.x;
        int hof = (raw.height - h) / 2 + offset.y;
        Color[] color_raw = raw.GetPixels(wof, hof, w, h);
        Color[] color_add = add.GetPixels(xo, yo, w, h);
        for (int i = 0; i < color_raw.Length; i++)
        {
            float a_raw = color_raw[i].a;
            float a_add = color_add[i].a;
            color_raw[i] = Color.Lerp(color_raw[i], color_add[i], a_add);
            color_raw[i].a = useRawAlpha ? a_raw : 1f - (1f - a_raw) * (1f - a_add);
        }

        raw.SetPixels(wof, hof, w, h, color_raw);
        raw.Apply();
        return raw;
    }

    public static Texture2D MaskSprite(this Texture2D raw, Texture2D mask)
    {
        int xo = 0;
        int yo = 0;
        // int xo = (int) rect.x;
        // int yo = (int) rect.y;
        int w = raw.width;
        int h = raw.height;
        int wof = 0;
        int hof = 0;
        Color[] color_raw = raw.GetPixels(wof, hof, w, h);
        Color[] color_mask = mask.GetPixels(xo, yo, w, h);
        for (int i = 0; i < color_raw.Length; i++)
        {
            float a_mask = color_mask[i].a;
            color_raw[i] = a_mask > 0f ? color_raw[i] : Color.clear;
        }

        raw.SetPixels(wof, hof, w, h, color_raw);
        raw.Apply();
        return raw;
    }

    //原图在左，叠加在右
    public static Texture2D BlendSpriteHorizontalGradient(this Texture2D raw, Texture2D add, float gradientPixel)
    {
        //俩图得一样大
        int w = raw.width;
        int h = raw.height;
        int xo = 0;
        int yo = 0;
        float md = (w - 1) / 2f;
        float half = w / 2f;
        float mdf = Mathf.Clamp(md - gradientPixel / 2f, 0f, half);
        float mdt = Mathf.Clamp(md + gradientPixel / 2f, half, w);
        float mdl = Mathf.Abs(mdt - mdf);
        Color[] color_raw = raw.GetPixels(xo, yo, w, h);
        Color[] color_add = add.GetPixels(xo, yo, w, h);
        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                int i = y * w + x;
                float blend = BlendRatio(x);
                float alpha_add = color_add[i].a;
                float alpha_raw = color_raw[i].a;
                float alpha = Mathf.Max(alpha_add, alpha_raw);
                var c = Color.Lerp(color_raw[i], color_add[i], alpha_add * blend);
                c.a = alpha;
                color_raw[i] = c;
            }
        }

        raw.SetPixels(xo, xo, w, h, color_raw);
        raw.Apply();
        return raw;

        float BlendRatio(int x)
        {
            if (mdl < 0.1f) return x < mdf ? 1f : 0f;
            return Mathf.Clamp01((x - mdf) / mdl);
        }
    }

    public static void SetLayerRecursively(this GameObject obj, int newLayer)
    {
        if (null == obj)
        {
            return;
        }

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }

            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    public static void Flash(this SpriteRenderer sr, float baseValue, float endValue, float duration)
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(sr.DOFade(endValue, duration / 2f));
        seq.Append(sr.DOFade(baseValue, duration / 2f));
        seq.Play();
    }

    public static void Fade(this SpriteRenderer sr, float endValue, float duration)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(sr.DOFade(endValue, duration));
        seq.Play();
    }

    public static void SetParentAndReset(this Transform transform, Transform parent)
    {
        var localScale = transform.localScale;
        transform.SetParent(parent);
        transform.localEulerAngles = new Vector3(0f, 0f, transform.localEulerAngles.z);
        transform.localScale = localScale;
    }

    public static void SetParentAndResetLocal(this Transform transform, Transform parent)
    {
        transform.SetParentAndReset(parent);
        transform.localPosition = Vector3.zero;
    }

    public static IEnumerable<T[]> GetTByLevel<T>(this ICollection<T[]> tar, int level)
    {
        foreach (var cld in tar)
        {
            int l = Mathf.Min(level, cld.Length - 1);
            if (cld[l] != null) yield return cld;
        }
    }

    public static bool Usable(this Array array)
    {
        return array != null && array.Length > 0;
    }

    public static TValue GetSafely<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key,
        TValue defaultValue = default)
    {
        if (dic.TryGetValue(key, out var value))
        {
            return value;
        }
        else
        {
            dic.Add(key, defaultValue);
            return defaultValue;
        }
    }

    public static TValue GetSafely<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key,
        Func<TKey, TValue> defaultValueGenerator)
    {
        if (dic.TryGetValue(key, out var value))
        {
            return value;
        }
        else
        {
            var v = defaultValueGenerator(key);
            dic.Add(key, v);
            return v;
        }
    }

    /// 用于安全地修改目标值，指定原始值的条件，并返回修改成功的结果。可以在没找到值时将value作为默认值。
    public static bool Change<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, TValue value,
        Predicate<TValue> check, bool asDefaultValueWhenMissing = false)
    {
        if (dic.TryGetValue(key, out var v))
        {
            if (check(v))
            {
                dic[key] = value;
                return true;
            }
            else return false;
        }
        else if (asDefaultValueWhenMissing)
        {
            dic.Add(key, value);
            return true;
        }

        return false;
    }

    public static void Set<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, TValue value)
    {
        if (dic.ContainsKey(key)) dic[key] = value;
        else dic.Add(key, value);
    }

    public static T GetElementSafely<T>(this IList<T> list, int index, T defaultValue = default)
    {
        if (list == null || index < 0 || index >= list.Count) return defaultValue;
        return list[index];
    }
    
    public static T GetElementClamped<T>(this IList<T> list, int index)
    {
        return list[Mathf.Clamp(index, 0, list.Count - 1)];
    }

    public static Vector3 ToSelfLocalPoint(this Transform transform, Vector3 worldPoint)
    {
        var parent = transform.parent;
        if (parent) return parent.InverseTransformPoint(worldPoint);
        return worldPoint;
    }

    public static Vector3 ToSelfWorldPoint(this Transform transform, Vector3 localPoint)
    {
        var parent = transform.parent;
        if (parent) return parent.TransformPoint(localPoint);
        return transform.TransformPoint(localPoint);
    }
}

[System.Serializable]
public struct RangeInt
{
    [HorizontalGroup, HideLabel, SuffixLabel("Min", true)]
    public int Min;

    [HorizontalGroup, HideLabel, SuffixLabel("Max", true)]
    public int Max;

    public int GRng
    {
        get { return M.Rng(Min, Max + 1); }
    }

    public int Rng
    {
        get { return UnityEngine.Random.Range(Min, Max + 1); }
    }

    public int ByRatio(float ratio)
    {
        return Min + Mathf.FloorToInt((Max - Min) * ratio);
    }

    public float GetRatio(int c)
    {
        if (Max == Min) return 1f;
        return Mathf.Clamp01((float)(c - Min) / (Max - Min));
    }

    public bool In(int c)
    {
        return c >= Min && c <= Max;
    }

    public RangeInt(int min, int max)
    {
        this.Min = min;
        this.Max = max;
    }
}

[System.Serializable]
public struct Range
{
    [HorizontalGroup, HideLabel, SuffixLabel("Min", true)]
    public float Min;

    [HorizontalGroup, HideLabel, SuffixLabel("Max", true)]
    public float Max;

    public float ByRatio(float ratio)
    {
        return Min + ((Max - Min) * ratio);
    }

    public float GetRatio(float c)
    {
        if (Mathf.Approximately(Max, Min)) return 1f;
        return Mathf.Clamp01((c - Min) / (Max - Min));
    }

    public float Rng
    {
        get { return UnityEngine.Random.Range(Min, Max); }
    }

    public float GRng
    {
        get { return M.Rng(Min, Max); }
    }

    public Range(float min, float max)
    {
        this.Min = min;
        this.Max = max;
    }

    public bool InRange(float f)
    {
        return f >= Min && f <= Max;
    }
}

[Serializable]
public struct MathCurve
{
    [LabelText("基础"), LabelWidth(75)] public float basic;

    [LabelText("线性(X*"), SuffixLabel(")", true), LabelWidth(75)]
    public float liner;

    [HorizontalGroup(Width = 100, GroupID = "Pow"), LabelText("指数(X+"), SuffixLabel(")", true), LabelWidth(45)]
    public float phase;

    [HorizontalGroup(Width = 100, GroupID = "Pow"), LabelText("^"), LabelWidth(10), GUIColor(1f, 0.5f, 0.5f)]
    public float pow;

    [HorizontalGroup(Width = 100, GroupID = "Pow"), LabelText("*"), LabelWidth(15)]
    public float multi;

    [HorizontalGroup(Width = 100, GroupID = "SelfPow"), LabelText("底数"), SuffixLabel("^X", true)
     , LabelWidth(45), GUIColor(1f, 0.5f, 0.5f)]
    public float selfPow;

    [HorizontalGroup(Width = 100, GroupID = "SelfPow"), LabelText("*"), LabelWidth(15)]
    public float selfPowMulti;


    [DisplayAsString, ShowInInspector, HideLabel]
    private string Preview1 =>
        $"0 [{Calc(0):f0}__{Calc(1):f0}__{Calc(2):f0}__{Calc(3):f0}__{Calc(4):f0}__{Calc(5):f0}__{Calc(6):f0}__{Calc(7):f0}__{Calc(8):f0}__{Calc(9):f0}__{Calc(10):f0}] 10";

    [DisplayAsString, ShowInInspector, HideLabel]
    private string Preview10 =>
        $"0 [{Calc(10):f0}__{Calc(25):f0}__{Calc(40):f0}__{Calc(55):f0}__{Calc(70):f0}__{Calc(85):f0}__{Calc(100):f0}] 100";

    public float Calc(float x)
    {
        return multi * Mathf.Pow(Mathf.Max(0f, x + phase), pow)
               + selfPowMulti * Mathf.Pow(selfPow, Mathf.Max(0f, x))
               + x * liner
               + basic;
    }
}

[System.Flags]
public enum Comparison
{
    bigger = 1 << 1,
    equal = 1 << 2,
    less = 1 << 3
}

[System.Flags]
public enum SourceCheck
{
    Any = 0,
    Only = 1 << 0,
    Disabled = 1 << 2
}

[System.Flags]
public enum DisabledSourceCheck
{
    Disabled = 0,
    Only = 1 << 0,
    Any = 1 << 2
}

public class Checker
{
    public bool check = false;
}

public class MultiCoroutineRunner
{
    private readonly MonoBehaviour _runner;
    private readonly List<IEnumerator> _undo;
    private int _doing;

    public MultiCoroutineRunner(MonoBehaviour runner)
    {
        _runner = runner;
        _undo = new List<IEnumerator>();
        _doing = 0;
    }

    public bool Ready => _undo.Count > 0 && _doing == 0;
    public bool End => _undo.Count == 0 && _doing == 0;
    public bool Doing => _doing > 0;

    public void Add(IEnumerator coSaction)
    {
        if (coSaction != null)
            _undo.Add(coSaction);
    }

    public IEnumerator Do()
    {
        foreach (var cs in _undo)
        {
            _doing++;
            _runner.StartCoroutine(cs.CallBack(() => _doing--));
        }

        _undo.Clear();
        while (_doing > 0)
        {
            yield return 0;
        }
    }

    public void Clear()
    {
        _undo.Clear();
        _doing = 0;
    }
}

public class ParallelRunner
{
    private readonly MonoBehaviour _runner;
    private int _doing;

    public ParallelRunner(MonoBehaviour runner)
    {
        _runner = runner;
        _doing = 0;
    }

    public bool Ended => _doing <= 0;
    public bool Doing => _doing > 0;

    public void Do(IEnumerator action)
    {
        _doing++;
        _runner.StartCoroutine(Append(action));
    }

    private IEnumerator Append(IEnumerator action)
    {
        yield return action;
        _doing--;
    }

    public void EndAndClear()
    {
        _doing = 0;
    }

    public IEnumerator WaitForEnd()
    {
        yield return new WaitUntil(() => Ended);
    }
}

public class Timer
{
    private float max;
    private float current;
    private bool triggered = false;
    private Action trigger;
    public bool Pause = false;
    public bool Loop = false;

    public Timer(float max, Action trigger = null)
    {
        this.max = max;
        this.trigger = trigger;
    }

    public bool Arrived
    {
        get { return current >= max; }
    }

    public void Update(float deltaTime)
    {
        if (Pause) return;
        current += deltaTime;
        if (!triggered && Arrived)
        {
            triggered = true;
            trigger?.Invoke();
            if (Loop) Reset();
        }
    }

    public void Reset()
    {
        triggered = false;
        current = 0f;
    }

    public void Ready()
    {
        triggered = false;
        current = max;
    }

    public void ResetMax(float max)
    {
        this.max = max;
    }
}

public class QueueActions<T>
{
    public delegate void QueueItem(T t, Action end);

    private struct Process
    {
        public T Param;
        public Action<T> Start;
        public Action<T> End;

        public Process(T param, Action<T> start, Action<T> final)
        {
            Param = param;
            Start = start;
            End = final;
        }
    }

    private readonly List<QueueItem> queue = new List<QueueItem>();
    private readonly Action baseStart;
    private readonly Action baseFinal;
    private readonly Queue<Process> doingProcesses = new Queue<Process>();

    private void EnqueueFinal(Process process)
    {
        doingProcesses.Enqueue(process);
    }

    private Process DequeueFinal()
    {
        if (doingProcesses.Count <= 0) return new Process(default, null, null);
        return doingProcesses.Dequeue();
    }

    public void Insert(QueueItem action)
    {
        queue.Insert(0, action);
    }

    public void Append(QueueItem action)
    {
        queue.Add(action);
    }

    public QueueActions()
    {
    }

    public QueueActions(Action start, Action final)
    {
        baseStart = start;
        baseFinal = final;
    }

    public static implicit operator Action<T>(QueueActions<T> queueActions)
    {
        return queueActions.Do;
    }

    public void Do(T t)
    {
        Do(t, null, null);
    }

    public void Do(T t, Action<T> start, Action<T> final)
    {
        EnqueueFinal(new Process(t, start, final));
        DoNew();
    }

    private void DoNew()
    {
        if (doingProcesses.Count > 1) return;
        baseStart?.Invoke();
        Do(0);
    }

    private void Do(int index)
    {
        if (doingProcesses.Count <= 0)
        {
            baseFinal?.Invoke();
            return;
        }

        if (index < 0 || index >= queue.Count)
        {
            var f = DequeueFinal();
            f.End?.Invoke(f.Param);
            if (doingProcesses.Count > 0)
                Do(0);
            else
            {
                baseFinal?.Invoke();
                return;
            }
        }

        var p = doingProcesses.Peek();
        var t = p.Param;
        if (index == 0) p.Start?.Invoke(t);

        var act = queue[index];
        if (act != null)
        {
            act(t, () => Do(index + 1));
        }
        else
        {
            Do(index + 1);
        }
    }
}

public class RecursionQueueActions
{
    public delegate void QueueItem(End end);

    public delegate void End(bool shouldRecurse);

    private readonly List<QueueItem> queue = new List<QueueItem>();
    private readonly Action baseFinal;

    public void Insert(QueueItem action)
    {
        queue.Insert(0, action);
    }

    public void Append(QueueItem action)
    {
        queue.Add(action);
    }

    public RecursionQueueActions()
    {
    }

    public RecursionQueueActions(Action final)
    {
        baseFinal = final;
    }

    public static implicit operator Action(RecursionQueueActions queueActions)
    {
        return queueActions.Do;
    }

    public void Do()
    {
        Do(0, false);
    }

    private void Do(int index, bool shouldRecurse)
    {
        if (index < 0 || index >= queue.Count)
        {
            if (shouldRecurse) Do(0, false);
            else
            {
                baseFinal?.Invoke();
            }

            return;
        }

        var act = queue[index];
        if (act != null)
        {
            act((re) => Do(index + 1, shouldRecurse || re));
        }
        else
        {
            Do(index + 1, shouldRecurse);
        }
    }
}

public class LocalQueueActions
{
    public delegate void QueueItem(Action end);


    private readonly List<QueueItem> queue = new List<QueueItem>();
    private readonly Action baseFinal;

    public void Insert(QueueItem action)
    {
        queue.Insert(0, action);
    }

    public void Append(QueueItem action)
    {
        queue.Add(action);
    }

    public LocalQueueActions()
    {
    }

    public LocalQueueActions(Action final)
    {
        baseFinal = final;
    }

    public static implicit operator Action(LocalQueueActions queueActions)
    {
        return queueActions.Do;
    }

    public void Do()
    {
        Do(0);
    }

    private void Do(int index)
    {
        if (index < 0 || index >= queue.Count)
        {
            baseFinal?.Invoke();
            return;
        }

        var act = queue[index];
        if (act != null)
        {
            act(() => Do(index + 1));
        }
        else
        {
            Do(index + 1);
        }
    }
}

public interface IWeighted
{
    public float GetWeight();
}


#region ObjectPool

public class MyObjectPool
{
    private readonly List<IPoolable> _items = new List<IPoolable>();

    public bool Enable
    {
        get => _items.Exists((it) => it.Enable());
    }

    public int Count => _items.Count;

    public IPoolable[] GetEnableItems()
    {
        List<IPoolable> list = new List<IPoolable>();
        foreach (IPoolable ip in _items)
        {
            if (ip.Enable())
            {
                list.Add(ip);
            }
        }

        return list.ToArray();
    }

    public float Total
    {
        get
        {
            float _total = 0f;
            float t = 0f;
            foreach (IPoolable p in _items)
            {
                if (p.Enable())
                {
                    t += p.GetWeight();
                }
            }

            _total = t;
            return _total;
        }
    }

    public void FillWith(IEnumerable<IPoolable> poolable)
    {
        _items.Clear();
        foreach (IPoolable p in poolable)
        {
            _items.Add(p);
        }
    }

    public void FillWith<T>(IEnumerable<T> poolable) where T : IPoolable
    {
        _items.Clear();
        foreach (T p in poolable)
        {
            _items.Add(p);
        }
    }

    public MyObjectPool()
    {
    }

    public MyObjectPool(IEnumerable<IPoolable> poolable)
    {
        FillWith(poolable);
    }

    public T1 RandomGet<T1>(bool removeit = false)
    {
        return (T1)RandomGet(removeit);
    }

    public IPoolable RandomGet(bool removeit = false)
    {
        IPoolable[] list = GetEnableItems();
        int count = list.Length;
        float _total = Total;
        if (_items.Count == 0)
        {
            if (!removeit) Debug.Log("No items.");
            return null;
        }

        if (Total == 0f)
        {
            if (!removeit) Debug.Log("No items Enable.");
            return null;
        }

        float r = M.Rng(0f, Total);

        float front = 0f;
        float back = 0f;
        for (int i = 0; i < count; i++)
        {
            //Debug.Log(items[i].GetName() + "[" + items[i].Enable() + "]");
            /*
            if (!items[i].Enable())
            {
                //Debug.Log(items[i].GetName() + "Not Enable");
                continue;
            }
            */
            if (front == 0f)
            {
                front = list[i].GetWeight();
            }

            //Debug.Log(items[0].GetName() + ":" + r + "/" + total + "[" + front + "/" + back +"]");
            if (r >= back && r <= front)
            {
                IPoolable ip = list[i];
                if (removeit) _items.Remove(ip);
                return ip;
            }
            else
            {
                if (i + 1 >= count)
                {
                    front = Mathf.Infinity;
                }
                else
                {
                    front += list[i + 1].GetWeight();
                }

                back += list[i].GetWeight();
            }
        }

        Debug.Log(r + "/" + Total);
        Debug.LogWarning("Event pool [" + _items[0].GetName() + "] random failed...");
        return null;
    }

    public IPoolable Find(string name)
    {
        foreach (IPoolable p in _items)
        {
            if (p.GetName().Equals(name))
            {
                return p;
            }
        }

        return null;
    }
}

public interface IPoolable
{
    float GetWeight();
    string GetName();
    bool Enable();
}

[Serializable]
public struct Pair<TKey, TValue>
{
    public Pair(TKey key, TValue value)
    {
        Key = key;
        Value = value;
    }

    [HorizontalGroup, HideLabel, SuffixLabel("Key")]
    public TKey Key;

    [HorizontalGroup, HideLabel, SuffixLabel("Value")]
    public TValue Value;

    public void Deconstruct(out TKey key, out TValue value)
    {
        key = Key;
        value = Value;
    }
}

public struct Triplet<TKey, TValue1, TValue2>
{
    public Triplet(TKey key, TValue1 value1, TValue2 value2)
    {
        Key = key;
        Value1 = value1;
        Value2 = value2;
    }

    [HorizontalGroup] public TKey Key;
    [HorizontalGroup] public TValue1 Value1;
    [HorizontalGroup] public TValue2 Value2;
}

public class NestedObjPool<T> : IPoolable, IEnumerable<NestedObjPool<T>>
{
    public string PoolName;
    public float Weight;
    public bool End;
    [ShowIf("End")] public List<T> List;
    [HideIf("End")] public List<NestedObjPool<T>> Branches;

    public List<T> GetEndListOnce()
    {
        if (End)
        {
            return new List<T>(List);
        }
        else
        {
            MyObjectPool pool = new MyObjectPool(Branches);
            NestedObjPool<T> item = pool.RandomGet() as NestedObjPool<T>;
            if (item != null)
                return item.GetEndListOnce();
            else return new List<T>();
        }
    }

    public bool Enable()
    {
        return Weight > 0f;
    }

    public string GetName()
    {
        return PoolName;
    }

    public float GetWeight()
    {
        return Weight;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        if (End) return List.GetEnumerator();
        else return Branches.GetEnumerator();
    }

    IEnumerator<NestedObjPool<T>> IEnumerable<NestedObjPool<T>>.GetEnumerator()
    {
        yield return this;
        if (!End)
            foreach (var it in Branches)
            foreach (var cl in it)
                yield return cl;
    }
}

[System.Flags]
public enum MComparison
{
    bigger = 1 << 1,
    smaller = 1 << 2,
    equal = 1 << 3
}

#endregion

public class ValueSet<T> where T : struct
{
    public Dictionary<string, T> dic = new Dictionary<string, T>();
    private readonly Dictionary<string, Validator> _rule = new Dictionary<string, Validator>();

    public T this[string k]
    {
        get => Get(k);
        set => Set(k, value);
    }

    public T this[Enum e]
    {
        get => Get(e.ToString());
        set => Set(e.ToString(), value);
    }

    public bool Has(string k)
    {
        return dic.ContainsKey(k);
    }

    public delegate T Validator(T current, T raw);

    public T Get(Enum k)
    {
        return Get(k.ToString());
    }

    public T Get(string k)
    {
        if (dic.ContainsKey(k))
        {
            return dic[k];
        }

        LogNoKeyError(k);
        return default;
    }

    public bool TryGet(Enum k, out T v)
    {
        return TryGet(k.ToString(), out v);
    }

    public bool TryGet(string k, out T v)
    {
        return dic.TryGetValue(k, out v);
    }

    public void Set(Enum k, T v)
    {
        Set(k.ToString(), v);
    }

    public void Set(string k, T v)
    {
        if (k == null) return;
        if (dic.TryGetValue(k, out var rawV))
        {
            dic[k] = v;
            Validate(k, rawV);
        }
        else
            LogNoKeyError(k);
    }

    public T DirectGet(Enum k)
    {
        return DirectGet(k.ToString());
    }

    public T DirectGet(string k)
    {
        if (k == null) return default;
        if (dic.TryGetValue(k, out var v))
        {
            return v;
        }

        return default;
    }

    public void DirectGetSet(Enum k, Func<T, T> func)
    {
        DirectGetSet(k.ToString(), func);
    }

    public void DirectGetSet(string k, Func<T, T> func)
    {
        if (dic.TryGetValue(k, out var value))
        {
            dic[k] = func(value);
            Validate(k, value);
        }
        else
        {
            Add(k, func(default));
            Validate(k);
        }
    }

    public void DirectSet(Enum k, T v)
    {
        DirectSet(k.ToString(), v);
    }

    public void DirectSet(string k, T v)
    {
        if (k == null) return;
        if (dic.TryGetValue(k, out var rawV))
        {
            dic[k] = v;
            Validate(k, rawV);
        }
        else
        {
            Add(k, v);
        }
    }

    public void Add(Enum e, T v)
    {
        Add(e.ToString(), v);
    }

    public void Add(string k, T v)
    {
        if (k != null && !dic.ContainsKey(k))
        {
            dic.Add(k, v);
            Validate(k);
        }
    }

    public void Remove(Enum e)
    {
        Remove(e.ToString());
    }

    public void Remove(string k)
    {
        if (dic.ContainsKey(k)) dic.Remove(k);
    }

    public void SetRule(Enum key, Validator rule)
    {
        SetRule(key.ToString(), rule);
    }

    public void SetRule(string key, Validator rule)
    {
        if (_rule.ContainsKey(key)) _rule[key] = rule;
        else _rule.Add(key, rule);
    }

    protected void Validate(string key, T raw = default)
    {
        if (dic.TryGetValue(key, out var value) && _rule.TryGetValue(key, out var validator))
        {
            dic[key] = validator(value, raw);
        }
    }

    public IEnumerable<string> Keys(Predicate<string> filter = null)
    {
        foreach (string k in dic.Keys)
        {
            if (filter == null || filter(k)) yield return k;
        }
    }

    public IEnumerable<T> Values(Predicate<T> filter = null)
    {
        foreach (var v in dic.Values)
        {
            if (filter == null || filter(v)) yield return v;
        }
    }

    private void LogNoKeyError(string key)
    {
        Debug.LogError($"No Key {key} in valueSet");
    }
}

public class WeightFilter<T> : IEnumerable<T>
{
    private readonly List<T> list = new List<T>();
    private int currentWeight = int.MinValue;

    public void Add(T obj, int weight)
    {
        if (weight < currentWeight) return;
        if (weight > currentWeight)
        {
            list.Clear();
            currentWeight = weight;
        }

        list.Add(obj);
    }

    public void Reset()
    {
        list.Clear();
        currentWeight = int.MinValue;
    }

    public T RandomGetOne(bool removeIt = false)
    {
        return list.RandomlyGetOne(removeIt);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public class PublicList<T>
{
    private readonly List<T> _list = new List<T>();

    public List<T> List
    {
        get
        {
            _list.Clear();
            return _list;
        }
    }
}

public class CacheList<T>
{
    private readonly List<T> _list = new List<T>();
    private readonly Setter _setter;
    private readonly Creator _creator;
    public int UsingCount { get; private set; }
    public IEnumerable<T> AllCached => _list;

    protected CacheList()
    {
    }

    public void Clear()
    {
        _list.Clear();
        UsingCount = 0;
    }

    public delegate T Creator(int index);

    public delegate void Setter(T t, bool active);

    public CacheList(Setter setter, Creator creator, params T[] cached)
    {
        _setter = setter;
        _creator = creator;

        if (cached != null)
        {
            _list.AddRange(cached);
            UsingCount = cached.Length;
        }
    }

    public void QuickUse(int count)
    {
        foreach (var t in Use(count))
        {
        }
    }

    public T GetByIndex(int index)
    {
        return _list[index];
    }

    public IEnumerable<T> Using()
    {
        for (var i = 0; i < _list.Count; i++)
        {
            var t = _list[i];
            if (i < UsingCount) yield return t;
        }
    }

    public IEnumerable<Pair<T, int>> UsingByIndex()
    {
        return UseByIndex(UsingCount);
    }

    public IEnumerable<T> Use(int count)
    {
        while (_list.Count < count)
        {
            var newT = _creator(_list.Count);
            _list.Add(newT);
        }

        for (int i = 0; i < _list.Count; i++)
        {
            var t = _list[i];
            _setter(t, i < count);
            if (i < count) yield return t;
        }

        UsingCount = count;
    }

    public IEnumerable<Pair<T, T2>> Use<T2>(IEnumerable<T2> e)
    {
        int index = 0;
        foreach (var t2 in e)
        {
            if (_list.Count <= index) _list.Add(_creator(_list.Count));
            var t = _list[index];
            _setter(t, true);
            yield return new Pair<T, T2>(t, t2);
            index++;
        }

        for (int i = index; i < _list.Count; i++)
        {
            var t = _list[i];
            _setter(t, false);
        }

        UsingCount = index;
    }

    public IEnumerable<Triplet<T, T2, int>> UseByIndex<T2>(IEnumerable<T2> e)
    {
        int index = 0;
        foreach (var t in Use(e))
        {
            yield return new Triplet<T, T2, int>(t.Key, t.Value, index++);
        }
    }

    public IEnumerable<Pair<T, int>> UseByIndex(int count)
    {
        int index = 0;
        foreach (var t in Use(count))
        {
            yield return new Pair<T, int>(t, index++);
        }
    }

    public T GetNew()
    {
        UsingCount++;
        if (_list.Count < UsingCount)
        {
            var newT = _creator(_list.Count);
            _setter(newT, true);
            _list.Add(newT);
            return newT;
        }
        else
        {
            var t = _list[UsingCount - 1];
            _setter(t, true);
            return t;
        }
    }

    public void CloseAll()
    {
        foreach (var t in _list)
        {
            _setter(t, false);
        }

        UsingCount = 0;
    }
}

[Serializable]
public class CacheLayoutPattern
{
    [SerializeField, ChildGameObjectsOnly] private GameObject pattern;
    [SerializeField, ChildGameObjectsOnly] private Transform layout;
    private ExtraCreate _create;
    private ExtraSet _set;

    public bool Enable => pattern != null && layout != null;

    private GameObject GetSettlePattern()
    {
        var rawPattern = pattern;
        var newPattern = Object.Instantiate(rawPattern, pattern.transform.parent);
        rawPattern.SetActive(false);
        _create?.Invoke(newPattern, 0);
        return newPattern;
    }

    public delegate void ExtraCreate(GameObject newChild, int index);

    public delegate void ExtraSet(GameObject newChild, bool status);

    public CacheList<GameObject> Cache => _cache ??=
        new CacheList<GameObject>((o, b) =>
        {
            o.SetActive(b);
            _set?.Invoke(o, b);
        }, _ =>
        {
            var child = Object.Instantiate(pattern, layout);
            _create?.Invoke(child, _);
            return child;
        }, GetSettlePattern());

    private CacheList<GameObject> _cache;

    public CacheLayoutPattern()
    {
    }

    public CacheLayoutPattern(ExtraCreate extraCreate)
    {
        _create = extraCreate;
    }

    public CacheLayoutPattern(GameObject pattern, RectTransform layout, ExtraCreate extraCreate) : this(extraCreate)
    {
        this.pattern = pattern;
        this.layout = layout;
    }

    public void SetExtraCreateFunction(ExtraCreate extraFunction)
    {
        _create = extraFunction;
    }

    public void SetExtraSetFunction(ExtraSet extraFunction)
    {
        _set = extraFunction;
    }
}

public class RecyclableList<T>
{
    public delegate bool Checker(T t);

    private readonly List<T> _list = new List<T>();
    private readonly Checker _getter;
    private readonly Func<T> _creator;
    public T Last { get; private set; }

    protected RecyclableList()
    {
    }

    public RecyclableList(Checker getter, Func<T> creator)
    {
        _getter = getter;
        _creator = creator;
    }

    public T Pop()
    {
        foreach (var t in _list)
        {
            if (_getter(t))
            {
                Last = t;
                return t;
            }
        }

        var newT = _creator();
        _list.Add(newT);
        Last = newT;
        return newT;
    }

    public T CondPop(Checker cond)
    {
        foreach (var t in _list)
        {
            if (cond(t))
            {
                Last = t;
                return t;
            }
        }

        return Pop();
    }


    public IEnumerable<T> All()
    {
        return _list;
    }
}

[Serializable]
public class RecyclableLayoutPattern
{
    [SerializeField] private GameObject pattern;
    [SerializeField, ChildGameObjectsOnly] private RectTransform layout;

    public delegate void ExtraSet(GameObject newChild);

    public RecyclableList<GameObject> List => _list ??=
        new RecyclableList<GameObject>((o) => !o.activeSelf, () =>
        {
            var child = Object.Instantiate(pattern, layout);
            return child;
        });

    private RecyclableList<GameObject> _list;
}

public class BandList<T> : IList<T>
{
    private readonly List<T> _list = new List<T>();
    private readonly HashSet<T> _band;

    protected BandList()
    {
    }

    public BandList(HashSet<T> band)
    {
        _band = band;
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(T item)
    {
        if (_band.Contains(item))
        {
            _list.Add(item);
            _band.Remove(item);
        }
    }

    public void Clear()
    {
        _band.UnionWith(_list);
        _list.Clear();
    }

    public bool Contains(T item)
    {
        return _list.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        _list.CopyTo(array, arrayIndex);
    }

    public bool Remove(T item)
    {
        if (_list.Remove(item))
        {
            _band.Add(item);
            return true;
        }

        return false;
    }

    public int Count => _list.Count;

    public bool IsReadOnly => false;

    public int IndexOf(T item)
    {
        return _list.IndexOf(item);
    }

    public void Insert(int index, T item)
    {
        if (_band.Contains(item))
        {
            _list.Insert(index, item);
            _band.Remove(item);
        }
    }

    public void RemoveAt(int index)
    {
        var item = _list[index];
        _list.RemoveAt(index);
        _band.Add(item);
    }

    public T this[int index]
    {
        get => _list[index];
        set
        {
            var raw = _list[index];
            if (!raw.Equals(value))
            {
                _band.Add(raw);
                _band.Remove(value);
                _list[index] = value;
            }
        }
    }
}

public class CountDic<T> : IEnumerable<KeyValuePair<T, int>>
{
    private readonly Dictionary<T, int> _dic = new Dictionary<T, int>();

    public int this[T key]
    {
        get
        {
            if (_dic.TryGetValue(key, out var c)) return c;
            return 0;
        }
    }

    public void Add(T key)
    {
        if (_dic.TryGetValue(key, out var c))
        {
            _dic[key] = c + 1;
        }
        else
        {
            _dic.Add(key, 1);
        }
    }

    public void AddRange(IEnumerable<T> collection)
    {
        foreach (var c in collection)
        {
            Add(c);
        }
    }

    public void Clear()
    {
        _dic.Clear();
    }

    public IEnumerable<T> Keys => _dic.Keys;

    public IEnumerator<KeyValuePair<T, int>> GetEnumerator()
    {
        return _dic.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public class CachedGetter<T> where T : class
{
    private Func<T> _getter;
    private T _cache;

    public T Value => _cache ?? (_cache = _getter());

    protected CachedGetter()
    {
    }

    public CachedGetter(Func<T> getter)
    {
        _getter = getter;
    }

    public static implicit operator T(CachedGetter<T> source)
    {
        return source.Value;
    }
}

public class FixedSizeQueueList<T>
{
    public readonly List<T> _list;
    private int size;
    private Action<T> DequeueAction;

    public FixedSizeQueueList(int fixedSize, Action<T> act = null)
    {
        _list = new List<T>(fixedSize);
        size = fixedSize;
        DequeueAction = null;
    }

    public void Enqueue(T obj)
    {
        if (_list.Count == size) Dequeue();
        _list.Add(obj);
    }

    public void Dequeue()
    {
        DequeueAction?.Invoke(First());
        _list.RemoveAt(0);
    }

    public T Peek() => First();
    public T First() => _list.First();
    public T Last() => _list.Last();
    public int Count() => _list.Count;

    public void Remove(T obj)
    {
        DequeueAction?.Invoke(First());
        _list.Remove(obj);
    }

    public bool AtMax()
    {
        return size == _list.Count;
    }

    public void RemoveAt(int i) => _list.RemoveAt(i);
}

public class CustomToggleGroup
{
    private List<Toggle> _toggles;
    private int Capacity;
    private List<Toggle> _queue;
    private bool _lock;

    public CustomToggleGroup(int cap)
    {
        _toggles = new List<Toggle>();
        _queue = new List<Toggle>(cap);
        _lock = false;
        Capacity = cap;
    }

    public Toggle Register(Toggle tog)
    {
        _toggles.Add(tog);
        tog.onValueChanged.AddListener((_) => OnValueChange(tog, _));
        if (_lock) tog.interactable = false;
        return tog;
    }

    void OnValueChange(Toggle tog, bool value)
    {
        if (value)
        {
            _queue.Add(tog);
            if (_queue.Count == Capacity) Lock();
        }
        else
        {
            _queue.Remove(tog);
            if (_lock) Unlock();
        }
    }

    void Lock()
    {
        var rest = _toggles.Where(t => !t.isOn);
        foreach (var t in rest)
        {
            t.interactable = false;
        }

        _lock = true;
    }

    void Unlock()
    {
        foreach (var t in _toggles)
        {
            t.interactable = true;
        }

        _lock = false;
    }

    public List<Toggle> GetActive()
    {
        return _queue;
    }

    public void Clear()
    {
        _queue.Clear();
        _toggles.Clear();
        _lock = false;
    }
}

public class Bictionary<T1, T2>
{
    [SerializeField, HideLabel] private Dictionary<T1, T2> _forward = new Dictionary<T1, T2>();
    private Dictionary<T2, T1> _reverse;

    public int Count => _forward.Count;
    public IEnumerable<T1> Keys => _forward.Keys;
    public IEnumerable<T2> Values => _forward.Values;
    public IEnumerable<KeyValuePair<T1, T2>> Pairs => _forward;

    public void Add(T1 key, T2 value)
    {
        _forward.Add(key, value);
        _reverse?.Add(value, key);
    }

    public void Remove(T1 key, T2 value)
    {
        _forward.Remove(key);
        _reverse?.Remove(value);
    }

    public T2 this[T1 index] => _forward[index];

    public T1 R(T2 value)
    {
        return Reverse[value];
    }

    public bool Contains(T1 key)
    {
        return _forward.ContainsKey(key);
    }

    public bool RContains(T2 value)
    {
        return Reverse.ContainsKey(value);
    }

    public bool TryGet(T1 key, out T2 value)
    {
        return _forward.TryGetValue(key, out value);
    }

    public bool RTryGet(T2 value, out T1 key)
    {
        return Reverse.TryGetValue(value, out key);
    }

    public void Clear()
    {
        _forward.Clear();
        _reverse?.Clear();
    }

    protected Dictionary<T2, T1> Reverse
    {
        get
        {
            if (_reverse == null)
            {
                _reverse = new Dictionary<T2, T1>();
                foreach (var t2 in _forward)
                {
                    _reverse.Add(t2.Value, t2.Key);
                }
            }

            return _reverse;
        }
    }
}

[Serializable]
public class WeightedRandomList<T>
{
    [SerializeField, ListDrawerSettings(AddCopiesLastElement = true)]
    private List<Single> list = new List<Single>();

    public T this[int index] => list[index].Content;
    public int Count => list.Count;
    private float Total => list.Sum(_ => _.Weight);

    public void Add(T content, float weight)
    {
        list.Add(new Single { Content = content, Weight = weight });
    }

    public void Clear()
    {
        list.Clear();
    }

    public T GetRandomAndTakeIt()
    {
        var it = GetRandomIndex();
        var t = this[it];
        list.RemoveAt(it);
        return t;
    }

    public T GetRandom()
    {
        var it = GetRandomIndex();
        return this[it];
    }

    public int GetRandomIndex()
    {
        var rng = M.Rng(0f, Total);
        for (var i = 0; i < list.Count; i++)
        {
            var item = list[i];
            if (item.Weight < 0f) continue;
            if (rng < item.Weight) return i;
            rng -= item.Weight;
        }

        return list.Count - 1;
    }

    public struct Single
    {
        [HorizontalGroup, HideLabel] public T Content;

        [HorizontalGroup, HideLabel, SuffixLabel("权重", Overlay = true)]
        public float Weight;
    }
}

public class GlaboSquareMap<T>
{
    private readonly Dictionary<Vector2Int, Node> _map = new Dictionary<Vector2Int, Node>();

    public T this[Vector2Int coor]
    {
        get
        {
            if (_map.TryGetValue(coor, out var n) && n != null)
            {
                return n.Content;
            }

            throw new Exception("No content at coordinate");
        }
    }

    public bool IsEmpty(Vector2Int coor) => _map.TryGetValue(coor, out var node) && node != null;

    public bool IsLinkingTo(Vector2Int from, Vector2Int direction, out bool linkingToEmpty)
    {
        var n_from = _map.GetSafely(from);
        if (n_from == null)
        {
            linkingToEmpty = false;
            return false;
        }

        linkingToEmpty = _map.GetSafely(from + direction) == null;
        return n_from[direction];
    }

    private class Node
    {
        public T Content;
        private readonly HashSet<Vector2Int> _links = new HashSet<Vector2Int>();

        public bool this[Vector2Int direction]
        {
            get => _links.Contains(direction);
            set
            {
                if (value) _links.Add(direction);
                else _links.Remove(direction);
            }
        }
    }
}