using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SOPacketFormat
{
    static string[] folders = new string[] { "Script", "ScriptObject" };
    static string fileNameFormat = "{0}SO.cs";

    public static void Create(string className, string menueName)
    {
        if (Exist(className))
        {
            UnityHelper.LogError_H($"SOPacketFormat Create Already Exist Error\nclassName : {className}");
            return;
        }

        string file = GetFile(className);

        string text = CSharpHelper.Format_H(soFormat, menueName, className, "");
        FileHelper.Write(file, text, true);
    }
    public static void Delete(string className)
    {
        if (!Exist(className))
        {
            UnityHelper.LogError_H($"SOPacketFormat Delete Not Exist Error\nclassName : {className}");
            return;
        }

        string file = GetFile(className);
        FileHelper.FileDelete(file);
    }
    public static void AddValue(string className, bool isPublic , TypeCollect type, string valueName)
    {
        if (!Exist(className))
        {
            UnityHelper.LogError_H($"SOPacketFormat AddValue Not Exist Error\nclassName : {className}");
            return;
        }

        string file = GetFile(className);

        string text = "";

        bool readCheck = false;
        int leftBracketCount = 0;
        int rightBracketCount = 0;
        bool modifyCheck = false;

        foreach (string line in File.ReadAllLines(file))
        {
            if (line.Contains('{'))
            {
                readCheck = true;
            }

            if (readCheck)
            {
                if (line.Contains('{'))
                    leftBracketCount++;

                if (line.Contains('}'))
                    rightBracketCount++;

                if (leftBracketCount != 0 && leftBracketCount == rightBracketCount)
                {
                    readCheck = false;

                    if (line.Contains('}'))
                    {
                        modifyCheck = true;
                        string format = isPublic ? publicVariableFormat : privateVariableFormat;
                        string typeStr = Utils.GetTypeByString(type);
                        string modifyText = "\t" + CSharpHelper.Format_H(format, typeStr, valueName) + "}";
                        text += modifyText + '\n';
                    }
                }
            }

            if (!modifyCheck)
                text += line + '\n';

            modifyCheck = false;
        }

        FileHelper.Write(file, text, true);
    }
    public static void AddListValue(string className, bool isPublic, TypeCollect listType, string valueName)
    {
        if (!Exist(className))
        {
            UnityHelper.LogError_H($"SOPacketFormat AddListValue Not Exist Error\nclassName : {className}");
            return;
        }

        string file = GetFile(className);

        string text = "";

        bool readCheck = false;
        int leftBracketCount = 0;
        int rightBracketCount = 0;
        bool modifyCheck = false;

        foreach (string line in File.ReadAllLines(file))
        {
            if (line.Contains('{'))
            {
                readCheck = true;
            }

            if (readCheck)
            {
                if (line.Contains('{'))
                    leftBracketCount++;

                if (line.Contains('}'))
                    rightBracketCount++;

                if (leftBracketCount != 0 && leftBracketCount == rightBracketCount)
                {
                    readCheck = false;

                    if (line.Contains('}'))
                    {
                        modifyCheck = true;
                        string format = isPublic ? publicVariableFormat : privateVariableFormat;
                        string typeStr = Utils.GetTypeByString(TypeCollect.List, listType);
                        List<string> list = new List<string>(); 
                        string modifyText = "\t" + CSharpHelper.Format_H(format, typeStr, valueName + $" = new {typeStr}()") + "}";
                        text += modifyText + "\n";
                    }   
                }
            }

            if (!modifyCheck)
                text += line + '\n';

            modifyCheck = false;
        }

        FileHelper.Write(file, text, true);
    }
    public static void AddDictionayValue(string className,bool isPublic, TypeCollect keyType, TypeCollect valueType, string valueName)
    {
        if (!Exist(className))
        {
            UnityHelper.LogError_H($"SOPacketFormat AddDictionayValue Not Exist Error\nclassName : {className}");
            return;
        }

        string file = GetFile(className);

        string text = "";

        bool readCheck = false;
        int leftBracketCount = 0;
        int rightBracketCount = 0;
        bool modifyCheck = false;

        foreach (string line in File.ReadAllLines(file))
        {
            if (line.Contains('{'))
            {
                readCheck = true;
            }

            if (readCheck)
            {
                if (line.Contains('{'))
                    leftBracketCount++;

                if (line.Contains('}'))
                    rightBracketCount++;

                if (leftBracketCount != 0 && leftBracketCount == rightBracketCount)
                {
                    readCheck = false;

                    if (line.Contains('}'))
                    {
                        modifyCheck = true;
                        string format = isPublic ? publicVariableFormat : privateVariableFormat;
                        string typeStr = Utils.GetTypeByString(TypeCollect.Dictionary, keyType, valueType);
                        string modifyText = "\t" + CSharpHelper.Format_H(format, typeStr, valueName + $" = new {typeStr}()") + "}";
                        text += modifyText + "\n";
                    }
                }
            }

            if (!modifyCheck)
                text += line + '\n';

            modifyCheck = false;
        }

        FileHelper.Write(file, text, true);
    }
    public static void RemoveValue(string className, string valueName)
    {

    }
    public static bool Exist(string className)
    {
        string file = GetFile(className);
        return File.Exists(file);
    }
    static string GetFile(string className)
    {
        string soName = CSharpHelper.Format_H(fileNameFormat, className);
        string file = $"{FileHelper.GetFile(folders, soName)}";
        return file;
    }

    #region Format

    // {0} menueName
    // {1} className
    // {2} variableName
    public static string soFormat =
@"using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = ""{1}"", menuName = ""{0}"")]
public class {1}SO : ScriptableObject
{{
{2}
}}
";
    // {0} variable Type
    // {1} variable Name
    public static string privateVariableFormat =
@"[SerializeField] private {0} {1};
";
    public static string publicVariableFormat =
@"public {0} {1};
";
    #endregion
}
