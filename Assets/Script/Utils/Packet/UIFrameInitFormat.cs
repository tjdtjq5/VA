using System;
using System.Collections.Generic;

public class UIFrameInitFormat
{
    public static void Set(Type scriptType, Dictionary<Type, string> bindDics)
    {
        string file = $"{FileHelper.GetScriptPath(scriptType)}";

        bool isInitCheck = SimpleFormat.Exist(scriptType, initializeCheckFormat);

        string text = "";
        foreach (var bind in bindDics)
        {
            text += "\t" + "\t" + CSharpHelper.Format_H(bindFormat, bind.Key, bind.Value) + "\n";
        }

        if (isInitCheck)
        {
            SimpleFormat.RemoveLineInner(file, initializeCheckFormat, "Bind");
            SimpleFormat.InnerUpperAdd(file, initializeCheckFormat, text);
        }
        else
            SimpleFormat.InnerTypeUpperAdd(scriptType, text);
    }
    #region Format
    // {0} : TypeName
    public static string classFormat =
@"public class {0} :";

    static string initializeCheckFormat =
@"protected override void Initialize()";

    static string baseFormat =
@"base.Initialize();";

    // {0} Type
    // {1} EnumName
    static string bindFormat =
@"Bind<{0}>(typeof({1}));";
    #endregion
}
