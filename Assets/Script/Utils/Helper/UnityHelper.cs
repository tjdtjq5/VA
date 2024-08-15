using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public static class UnityHelper
{
    #region Log
    public static void Log_H(object message)
    {
        if (!GameOptionManager.IsRelease)
            Debug.Log(message);
    }
    public static void Log_H<T1, T2>(Dictionary<T1, T2> dics) where T1 : new() where T2 : new()
    {
        if (!GameOptionManager.IsRelease)
            Log_H(dics.ToString());
    }
    public static void Log_H<T>(List<T> list)
    {
        if (!GameOptionManager.IsRelease)
            Log_H(list.ToString());
    }
    public static void LogError_H(object message)
    {
        if (!GameOptionManager.IsRelease)
        {
            string msg = $"{message}";
            Debug.LogError(msg);
        }
    }
    public static void LogSerialize(object message)
    {
        if (!GameOptionManager.IsRelease)
            Log_H($"<color=#006AFF>{CSharpHelper.SerializeObject(message)}</color>");
    }
    public static void Assert_H(bool condition, string message)
    {
        if (!GameOptionManager.IsRelease)
            Debug.Assert(condition, message);
    }
    #endregion

    #region FindChild
    public static GameObject FindChild(GameObject _go, string _name, bool _recursive = false)
    {
        Transform tr = FindChild<Transform>(_go, _name, _recursive);
        if (tr != null)
            return tr.gameObject;

        return null;
    }
    public static T FindChild<T>(GameObject _go, string _name, bool _recursive = false) where T : UnityEngine.Object
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
    public static T FindChild<T>(GameObject _go, bool _recursive = false) where T : UnityEngine.Object
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

                T component = childTr.GetComponent<T>();
                if (component != null)
                    return component;
            }
        }
        else
        {
            foreach (T component in _go.GetComponentsInChildren<T>())
            {
                return component;
            }
        }

        return null;
    }
    public static T FindChildPath<T>(GameObject _go, string path)
    {
        string parentsName = _go.name.Replace("(Clone)", "");
        List<string> objNames = path.Split('/').ToList();
        for (int i = 0; i < objNames.Count; i++)
        {
            if (objNames[i].Equals(parentsName))
            {
                objNames.RemoveAt(i);
                break;
            }
        }

        path = "";
        for (int i = 0; i < objNames.Count; i++)
        {
            path += $"{objNames[i]}/";
        }
        path = path.Substring(0, path.Length - 1);

        if (objNames.Count >= 1)
        {
            string childName = objNames[0];
            GameObject child = FindChild(_go, childName);

            if (child == null)
            {
                LogError_H($"Not Found Object\npath : {path}\nparents : {parentsName}");
                return default(T);
            }

            if (objNames.Count == 1)
            {
                return child.GetComponent<T>();
            }
            else
            {
                return FindChildPath<T>(child, path);
            }
        }

        LogError_H($"Not Found Object\npath : {path}\nparents : {parentsName}");
        return default(T);
    }
    public static List<T> FlindChilds<T>(GameObject _go, bool _recursive = false) where T : UnityEngine.Object
    {
        if (_go == null)
        {
            return null;
        }

        List<T> list = new List<T>();

        if (!_recursive)
        {
            for (int i = 0; i < _go.transform.childCount; i++)
            {
                Transform childTr = _go.transform.GetChild(i);

                T component = childTr.GetComponent<T>();

                if (component != null)
                    list.Add(component);
            }
        }
        else
        {
            foreach (T component in _go.GetComponentsInChildren<T>())
            {
                list.Add(component);
            }
        }

        return list;
    }
    public static List<Transform> IgnoreFindChilds<T>(Transform _go) where T : UnityEngine.Object
    {
        if (_go == null)
        {
            return null;
        }

        List<Transform> list = new List<Transform>();

        for (int i = 0; i < _go.transform.childCount; i++)
        {
            Transform childTr = _go.transform.GetChild(i);

            bool isIgnore = childTr.GetComponent<T>();

            if (!isIgnore)
            {
                list.Add(childTr);
                list.AddRange(IgnoreFindChilds<T>(childTr));
            }
        }

        return list;
    }
    public static T GetOrAddComponent<T>(GameObject _go) where T : UnityEngine.Component
    {
        T component = _go.GetComponent<T>();
        if (component == null) component = _go.AddComponent<T>();
        return component;
    }

    #endregion

    #region Build
    public static int GetVersionValue(string version, int _index)
    {
        string[] values = version.Split('.');

        if (values.Length > _index)
        {
            return CSharpHelper.IntParse(values[_index], true);
        }
        else
        {
            return 0;
        }
    }
    public static string GetVersionCode(params int[] values)
    {
        string versionCode = "0.0";

        for (int i = 0; i < values.Length; i++) 
        {
            if (i == 0)
            {
                versionCode = $"{values[i]}";
            }
            else
            {
                versionCode += $".{values[i]}";
            }
        }

        return versionCode;
    }
    public static string GetBuildPath(string buildTarget)
    {
        string path = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/'));
        path += $"/Build/{buildTarget}";
        return path;
    }
    #endregion

    #region Direction
    public static Vector3 GetDirection(this Vector3 originPos, Vector3 targetPos)
    {
        targetPos = targetPos - originPos;
        return targetPos.normalized;
    }
    #endregion
}
