using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityHelper
{
    #region Log
    public static void Log_H(object message)
    {
        Debug.Log(message);
    }
    public static void LogError_H(object message)
    {
        Debug.LogError(message);
    }
    public static void LogSerialize(object message)
    {
        Debug.Log(CSharpHelper.SerializeObject(message));
    }
    #endregion
}
