using System.Collections;
using System.Collections.Generic;
using Shared.CSharp;
using UnityEngine;

public static class DefinePath
{
    #region SO
    public static string TableSODirectory(string tableName) => CSharpHelper.Format_H("Assets/Resources/SO/Table/{0}", tableName);
    public static string TableSOName(string tableName, string code) => CSharpHelper.Format_H("{0}_{1}.asset", tableName, code);
    public static string TableSOPath(string tableName, string code) => $"{TableSODirectory(tableName)}/{TableSOName(tableName, code)}";
    public static string TableSOResourcesPath(string tableName, string code) => $"{CSharpHelper.Format_H("SO/Table/{0}/{0}_{1}", tableName, code)}";
    public static string StatSOResourcesPath(string code) => $"{CSharpHelper.Format_H("SO/Stat/{0}", code)}";
    public static string StatSOResourcesPath() => "SO/Stat";
    public static string SkillSOResourcesPath() => $"SO/Skill";
    public static string SkillSOResourcesPath(string code) => $"{CSharpHelper.Format_H("SO/Skill/{0}", code)}";
    public static string BuffSOResourcesPath() => $"SO/Buff";
    public static string BuffSOResourcesPath(string code) => $"{CSharpHelper.Format_H("SO/Buff/{0}", code)}";
    public static string ItemSOResourcesPath() => $"SO/Item";
    public static string ItemSOResourcesPath(string code) => $"{CSharpHelper.Format_H("SO/Item/{0}", code)}";
    public static string EquipSOResourcesPath() => $"SO/Equip";
    public static string EquipSOResourcesPath(string code) => $"{CSharpHelper.Format_H("SO/Equip/{0}", code)}";
    #endregion

    #region Node
    private static string NodeDirectory(string nodeName) => CSharpHelper.Format_H("Node/{0}", nodeName);
    public static string NodeResourcesPath(string nodeName, string code) => $"{NodeDirectory(nodeName)}/{code}";
    #endregion
}
