using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DefinePath
{
    public static string TableSODirectory(string tableName) => CSharpHelper.Format_H("Assets/Resources/TableSO/{0}", tableName);
    public static string TableSOName(string tableName, string code) => CSharpHelper.Format_H("{0}_{1}.asset", tableName, code);
    public static string TableSOPath(string tableName, string code) => $"{TableSODirectory(tableName)}/{TableSOName(tableName, code)}";
    public static string TableSOResourcesPath(string tableName, string code) => $"{CSharpHelper.Format_H("TableSO/{0}/{0}_{1}", tableName, code)}";
    public static string StatSOResourcesPath(string code) => $"{CSharpHelper.Format_H("Stat/STAT_{0}", code)}";
}
