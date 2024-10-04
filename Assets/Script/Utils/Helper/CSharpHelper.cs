using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Reflection;
using System.Text.RegularExpressions;
using static UnityEngine.Rendering.DebugUI;

public static class CSharpHelper
{
    #region Format
    public static string Format_H(string format, params object[] args)
    {
        try
        {
            return string.Format(CultureInfo.InvariantCulture, format, args);
        }
        catch (Exception e)
        {
            UnityHelper.LogError_H($"{e.Message}");
            UnityHelper.LogError_H($"CSharpHelper Format_H Error\nformat : {format}");
            return "";
        }
    }
    #endregion

    #region Parse
    public static bool ExistEnumData<T>(string enumData) where T : Enum
    {
        try
        {
            return (T)Enum.Parse(typeof(T), enumData) != null;
        }
        catch
        {
            return false;
        }
    }
    public static T EnumParse<T>(string value, bool isDebug) where T : Enum
    {
        try
        {
            return (T)Enum.Parse(typeof(T), value);
        }
        catch
        {
            if (isDebug)
                UnityHelper.Log_H($"CSharpHelper Parse Error\nvalue : {value}");

            return default(T);
        }
    }
    public static int GetEnumLength<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T)).Length;
    }
    public static int EnumClamp<T>(int value, bool isDebug) where T : Enum
    {
        try
        {
            int len = GetEnumLength<T>();
            value = Math.Clamp(value, 0, len - 1);

            return value;
        }
        catch
        {
            if (isDebug)
                UnityHelper.Log_H($"CSharpHelper Parse Error\nvalue : {value}");

            return 0;
        }
    }
    public static int IntParse(string value, bool isDebug)
    {
        try
        {
            return int.Parse(value);
        }
        catch
        {
            if (isDebug)
                UnityHelper.Log_H($"CSharpHelper Parse Error\nvalue : {value}");

            return 0;
        }
    }
    public static float FloatParse(string value, bool isDebug)
    {
        try
        {
            return float.Parse(value);
        }
        catch
        {
            if (isDebug)
                UnityHelper.Log_H($"CSharpHelper Parse Error\nvalue : {value}");

            return 0;
        }
    }
    public static object AutoParse(string value)
    {
        if (float.TryParse(value, out float floatResult))
        {
            if (int.TryParse(value, out int intResult))
            {
                return intResult;
            }

            return floatResult;
        }
        if (byte.TryParse(value, out byte btyeResult))
        {
            return btyeResult;
        }

        return value;
    }
    public static string ToString_H(this DateTime dateTime)
    {
        return dateTime.ToString("yyyy-mm-dd hh:mm:ss", CultureInfo.InvariantCulture);
    }
    public static DateTime ToDateTime(this string str, bool isDebug)
    {
        DateTime result = new DateTime(0);

        if (!DateTime.TryParseExact(str, "yyyy-mm-dd hh:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
        {
            if (isDebug)
                UnityHelper.LogError_H($"CSharpHelper ToDateTime Error\nstr : {str}\nresult : {result}");
        }

        return result;
    }
    #endregion

    #region Type Field
    public static Type GetType(string value)
    {
        object obj = AutoParse(value);
        return obj.GetType();
    }
    public static string GetTypeString(Type type)
    {
        TypeCollect tc = EnumParse<TypeCollect>(type.Name, false);
        switch (tc)
        {
            case TypeCollect.Int32:
                return "int";
            case TypeCollect.Int64:
                return "long";
            case TypeCollect.String:
                return "string";
            case TypeCollect.Single:
                return "float";
            case TypeCollect.Double:
                return "double";
            case TypeCollect.Byte:
                return "byte";
            default:
                UnityHelper.LogError_H($"Not Found Type\nType Name : {type.Name}");
                return "";
        }
    }
    public static FieldInfo[] GetFieldInfos<T>(this T arg)
    {
        FieldInfo[] fieldInfos = arg.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField);
        return fieldInfos;
    }
    public static FieldInfo GetFieldInfoByVariableName<T>(this T arg, string name)
    {
        FieldInfo[] fieldInfos = arg.GetFieldInfos();
        for (int i = 0; i < fieldInfos.Length; i++) 
        {
            if (fieldInfos[i].GetVariableNameByField().Equals(name))
            {
                return fieldInfos[i];
            }
        }
        return null;
    }
    public static FieldInfo GetFieldInfoByValue<T>(this T arg, object value)
    {
        FieldInfo[] fieldInfos = arg.GetFieldInfos();
        for (int i = 0; i < fieldInfos.Length; i++)
        {
            if (fieldInfos[i].GetValue(arg) == value)
            {
                return fieldInfos[i];
            }
        }
        return null;
    }
    public static MethodInfo[] GetMethodInfos<T>(this T arg)
    {
        MethodInfo[] methodInfos = arg.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField);
        return methodInfos;
    }
    public static string GetVariableNameByField(this FieldInfo arg)
    {
        string name = arg.Name;
        name = name.Replace("<", "");
        name = name.Replace(">", "");
        name = name.Replace("k__BackingField", "");

        return name;
    }
    public static TypeCollect GetTypeCollectByName(object obj)
    {
        try
        {
            string typeName = obj.GetType().Name;

            if (typeName.Contains('`'))
                typeName = typeName.Substring(0, typeName.IndexOf('`'));

            return (TypeCollect)Enum.Parse(typeof(TypeCollect), typeName);
        }
        catch (NullReferenceException e)
        {
            UnityHelper.LogError_H($"ÁöÁ¤ÇÒ ¼ö ¾ø´Â Å¸ÀÔ (Ä¿½ºÅÒ Å¬·¡½º)\n{e.Message}");
            return TypeCollect.None;
        }
        catch
        {
            UnityHelper.LogError_H($"¼³Á¤µÇÁö ¾ÊÀº Å¸ÀÔ GetTypeName : {obj.GetType().Name}");
            return TypeCollect.None;
        }
    }
    public static bool SetCopyValue<T1, T2>(this T1 copied, T2 original)
    {
        FieldInfo[] fieldInfos = copied.GetFieldInfos();
        bool isDeferenceCheck = false;

        for (int i = 0; i < fieldInfos.Length; i++)
        {
            string fieldName = fieldInfos[i].GetVariableNameByField();
            FieldInfo originField = original.GetFieldInfoByVariableName(fieldName);

            if (originField != null)
            {
                object value = originField.GetValue(original);
                object beforeValue = fieldInfos[i].GetValue(copied);

                if (!isDeferenceCheck)
                {
                    string valueStr = value != null ? value.ToString() : "";
                    string beforeValueStr = beforeValue != null ? beforeValue.ToString() : "";
                    isDeferenceCheck = !valueStr.Trim().Equals(beforeValueStr.Trim());
                }

                fieldInfos[i].SetValue(copied, value);
            }
        }

        return isDeferenceCheck;
    }
    public static bool SetCopyValue<T1, T2>(this List<T1> copied, List<T2> original) where T1 : new()
    {
        bool isDeferenceCheck = true;

        copied.Clear();
        for (int i = 0; i < original.Count; i++)
        {
            copied.Add(new T1());
        }

        for (int i = 0; i < copied.Count; i++)
        {
            bool flag = copied[i].SetCopyValue(original[i]);

            if (isDeferenceCheck)
                isDeferenceCheck = flag;
        }

        return isDeferenceCheck;
    }
    #endregion

    #region Json
    public static string SerializeObject<T>(T value)
    {
        try
        {
            if (typeof(T) == typeof(string))
            {
                return value.ToString();
            }
            else
            {
                return JsonConvert.SerializeObject(value, Formatting.Indented);
            }

        }
        catch
        {
            UnityHelper.LogError_H($"CSharpHelper SerializeObject Error\nvalue : {value}");
            return "";
        }
    }
    public static T DeserializeObject<T>(string value)
    {
        try
        {
            if (typeof(T) == typeof(string))
            {
                return (T)(object)value;
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
        }
        catch
        {
            UnityHelper.LogError_H($"CSharpHelper DeserializeObject Error\nvalue : {value}");
            return default;
        }
    } 
    #endregion

    #region String 
    public static string StartCharToLower(string value, int lowerLen = 1)
    {
        try
        {
            string result = value;
            string startLowerResult = result.Substring(0, lowerLen).ToLower();
            result = $"{startLowerResult}{result.Substring(lowerLen, result.Length - 1)}";
            return result;
        }
        catch
        {
            return value;
        }
    }
    public static string GetReplaceRegex(string value)
    {
        try
        {
            string result = Regex.Replace(value, @"[^a-zA-Z0-9°¡-ÆR\s]", "");
            result = result.Replace("\r", "");
            result = result.Replace("\n", "");
            result = result.Replace("\t", "");
            return result;
        }
        catch
        {
            UnityHelper.LogError_H($"GetReplaceRegex Error\nvalue : {value}");
            return value;
        }
    }
    public static string GetReplaceRNT(string value)
    {
        try
        {
            string result = value;
            result = result.Replace("\r", "");
            result = result.Replace("\n", "");
            result = result.Replace("\t", "");
            result = result.Replace(" ", "");
            return result;
        }
        catch
        {
            UnityHelper.LogError_H($"GetReplaceRNT Error\nvalue : {value}");
            return value;
        }
    }
    public static bool IsRegex(string value)
    {
        string regex = GetReplaceRegex(value);
        return regex.Equals(value);
    }
    public static string RemoveSemi(string path)
    {
        string result = path;
        result = result.Replace("\\\\\\", "");
        result = result.Replace("\\\\", "\\");
        result = result.Replace("\"", "");

        return result;
    }
    #endregion

    #region Encoding
    public static string Base64Encode(string data)
    {
        try
        {
            byte[] encData_byte = new byte[data.Length];
            encData_byte = System.Text.Encoding.UTF8.GetBytes(data);
            string encodedData = Convert.ToBase64String(encData_byte);
            return encodedData;
        }
        catch (Exception e)
        {
            throw new Exception("Error in Base64Encode: " + e.Message);
        }
    }
    public static string Base64Decode(string data)
    {
        try
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();

            byte[] todecode_byte = Convert.FromBase64String(data);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }
        catch (Exception e)
        {
            throw new Exception("Error in Base64Decode: " + e.Message);
        }
    }
    #endregion

    #region Dictionary
    public static string ToString_H<T1, T2>(this Dictionary<T1, T2> dics) where T1 : new() where T2 : new()
    {
        string result = "";

        foreach (var data in dics)
        {
            string keyData = SerializeObject(data.Key);
            string valueData = SerializeObject(data.Value);

            result += $"{keyData} : {valueData}\n";
        }

        return result;
    }
    public static bool TryAdd_H<T1, T2>(this Dictionary<T1, T2> dics, T1 key, T2 value, bool force)
    {
        if (force)
        {
            if (dics.ContainsKey(key))
            {
                dics[key] = value;
                return true;
            }
            else
            {
                dics.Add(key, value);
                return true;
            }
        }
        else
        {
            return dics.TryAdd(key, value);
        }
    }
    public static T2 TryGet_H<T1, T2>(this Dictionary<T1, T2> dics, T1 key)
    {
        if (dics.ContainsKey(key))
        {
            return dics[key];
        }
        else
        {
            return default;
        }
    }
    public static int TryGet_H<T1>(this Dictionary<T1, int> dics, T1 key)
    {
        if (dics.ContainsKey(key))
        {
            return dics[key];
        }
        else
        {
            return -1;
        }
    }
    public static float TryGet_H<T1>(this Dictionary<T1, float> dics, T1 key)
    {
        if (dics.ContainsKey(key))
        {
            return dics[key];
        }
        else
        {
            return -1;
        }
    }
    public static string TryGet_H<T1>(this Dictionary<T1, string> dics, T1 key)
    {
        if (dics.ContainsKey(key))
        {
            return dics[key];
        }
        else
        {
            return "";
        }
    }
    #endregion

    #region List
    public static string ToString_H<T>(this List<T> list)
    {
        return SerializeObject(list);
    }
    public static string ToString_H<T>(this IEnumerable<T> list)
    {
        return SerializeObject(list);
    }
    public static void ForceAdd_H<T>(this List<T> list, T value, object keyColumn)
    {
        FieldInfo fi = value.GetFieldInfoByValue(keyColumn);
        string columnName = fi.GetVariableNameByField();
        int findIndex = list.FindIndex(d =>
        (
            d.GetFieldInfoByVariableName(columnName).GetValue(d) == value.GetFieldInfoByVariableName(columnName).GetValue(value)
        ));

        if (findIndex < 0)
            list.Add(value);
        else
            list[findIndex] = value;
    }
    #endregion


}
