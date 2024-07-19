using System;
using System.Collections.Generic;

public class UIFrameInitFormat
{
    public static void Set(Type scriptType, Dictionary<Type, string> bindDics)
    {
        string file = $"{FileHelper.GetScriptPath(scriptType)}";

        bool isInitCheck = IsInitSet(scriptType);

        string text = "";

        bool readCheck = true;
        int leftBracketCount = 0;
        int rightBracketCount = 0;
        bool writeCheck = false;
        bool writeFlag = false;

        int leftBracketCount_check = 0;
        int rightBracketCount_check = 0;

        if (isInitCheck)
        {
            foreach (string line in FileHelper.ReadLines(file))
            {
                if (line.Trim().Contains(CSharpHelper.Format_H(classFormat, scriptType.Name)))
                {
                    readCheck = false;
                }

                if (!readCheck)
                {
                    if (line.Contains('{'))
                        leftBracketCount++;
                    if (line.Contains('}'))
                        rightBracketCount++;

                    if (!writeFlag && line.Trim().Equals(initializeCheckFormat))
                    {
                        writeCheck = true;
                    }

                    if (writeCheck)
                    {
                        if (line.Contains('{'))
                            leftBracketCount_check++;
                        if (line.Contains('}'))
                            rightBracketCount_check++;

                        if (!writeFlag && leftBracketCount_check == 1 && leftBracketCount_check != rightBracketCount_check)
                        {
                            writeFlag = true;

                            text += "\t" + initializeCheckFormat + "\n";
                            text += "\t" + "{" + "\n";

                            text += "\t" + "\t" + baseFormat + "\n";

                            foreach (var bind in bindDics)
                            {
                                text += "\t" + "\t" + CSharpHelper.Format_H(bindFormat, bind.Key, bind.Value) + "\n";
                            }
                        }

                        if (leftBracketCount_check != 0 && leftBracketCount_check == rightBracketCount_check)
                        {
                            writeCheck = false;
                        }
                    }

                    if (leftBracketCount != 0 && leftBracketCount == rightBracketCount)
                    {
                        readCheck = true;
                    }
                }

                if (!writeCheck)
                    text += line + "\n";
            }
        }
        else
        {
            foreach (string line in FileHelper.ReadLines(file))
            {
                if (line.Trim().Contains(CSharpHelper.Format_H(classFormat, scriptType.Name)))
                {
                    readCheck = false;
                }

                if (!readCheck)
                {
                    if (line.Contains('{'))
                        leftBracketCount++;
                    if (line.Contains('}'))
                        rightBracketCount++;

                    if (!writeFlag && leftBracketCount == 1 && leftBracketCount != rightBracketCount)
                    {
                        writeFlag = true;
                        writeCheck = true;

                        text += line + "\n";
                        text += "\t" + initializeCheckFormat + "\n";
                        text += "\t" + "{" + "\n";
                        text += "\t" + "\t" + baseFormat + "\n";

                        foreach (var bind in bindDics)
                        {
                            text += "\t" + "\t" + CSharpHelper.Format_H(bindFormat, bind.Key, bind.Value) + "\n";
                        }

                        text += "\t" + "}" + "\n";
                    }

                    if (leftBracketCount != 0 && leftBracketCount == rightBracketCount)
                    {
                        readCheck = true;
                    }
                }

                if (!writeCheck)
                    text += line + "\n";
                else
                    writeCheck = false;
            }
        }

        FileHelper.Write(file, text, true);
    }
    static bool IsInitSet(Type type)
    {
        string file = $"{FileHelper.GetScriptPath(type)}";

        bool readCheck = true;
        int leftBracketCount = 0;
        int rightBracketCount = 0;

        foreach (string line in FileHelper.ReadLines(file))
        {
            if (line.Trim().Contains(CSharpHelper.Format_H(classFormat, type.Name)))
            {
                readCheck = false;
            }

            if (!readCheck)
            {
                if (line.Contains('{'))
                    leftBracketCount++;
                if (line.Contains('}'))
                    rightBracketCount++;

                if (line.Trim().Equals(initializeCheckFormat))
                {
                    return true;
                }

                if (leftBracketCount != 0 && leftBracketCount == rightBracketCount)
                {
                    readCheck = true;
                    return false;
                }
            }
        }

        return false;
    }

    #region Format
    // {0} : TypeName
    public static string classFormat =
@"public class {0} :";

    static string initializeCheckFormat =
@"public override void Initialize()";

    static string baseFormat =
@"base.Initialize();";

    // {0} Type
    // {1} EnumName
    static string bindFormat =
@"Bind<{0}>(typeof({1}));";
    #endregion
}
