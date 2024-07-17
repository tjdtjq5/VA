using System;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static GameObject FindChild(GameObject _go, string _name, bool _recursive = false)
    {
        Transform tr = FindChild<Transform>(_go, _name, _recursive);
        if (tr != null)
            return tr.gameObject;

        return null;
    }
    public static T FindChild<T> (GameObject _go, string _name, bool _recursive = false) where T : UnityEngine.Object
    {
        if (_go == null)
        {
            return null;
        }

        if (!_recursive)
        {
            for (int i = 0; i < _go.transform.childCount; i++)
            {
                Transform childTr = _go.transform.GetChild(i);

                if (string.IsNullOrEmpty(_name) || childTr.name == _name)
                { 
                    T component = childTr.GetComponent<T>();    
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in _go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(_name) || component.name == _name)
                    return component;
            }
        }

        return null;
    }
    public static T GetOrAddComponent<T>(GameObject _go) where T : UnityEngine.Component
    {
        T component = _go.GetComponent<T>();
        if (component == null) component = _go.AddComponent<T>();
        return component;
    }
    public static TypeCollect GetTypeName(object obj)
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
            UnityHelper.LogError_H($"지정할 수 없는 타입 (커스텀 클래스)\n{e.Message}");
            return TypeCollect.None;
        }
        catch
        {
            Debug.LogError($"설정되지 않은 타입 GetTypeName : {obj.GetType().Name}");
            return TypeCollect.None;
        }
    }
    public static string GetTypeByString(TypeCollect type, params TypeCollect[] args)
    {
        try
        {
            switch (type)
            {
                case TypeCollect.None:
                    return "";
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
                case TypeCollect.List:
                    return $"List<{GetTypeByString(args[0])}>";
                case TypeCollect.Dictionary:
                    return $"Dictionary<{GetTypeByString(args[0])},{GetTypeByString(args[1])}>";
                default:
                    return "";
            }
        }
        catch
        {
            UnityHelper.LogError_H($"GetTypeByString Error\ntype : {type}");
            return "";
        }
    }
}
