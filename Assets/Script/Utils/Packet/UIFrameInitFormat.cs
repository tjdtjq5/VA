using System;
using System.Collections.Generic;

public class UIFrameInitFormat
{
    public static void Set(Type scriptType, Dictionary<Type, string> bindDics)
    {
        string file = $"{FileHelper.GetScriptPath(scriptType)}";

        bool isInitCheck = SimpleFormat.Exist(scriptType, initializeCheckFormat);
        UnityHelper.Log_H($"[{scriptType.Name}] [{isInitCheck}]");

        if (!isInitCheck)
            SimpleFormat.InnerTypeUpperAdd(scriptType, initFormat);

        string text = "";
        foreach (var bind in bindDics)
        {
            text += "\t" + "\t" + CSharpHelper.Format_H(bindFormat, bind.Key, bind.Value) + "\n";
        }

        SimpleFormat.RemoveLineInner(file, initializeCheckFormat, "Bind");
        SimpleFormat.InnerUpperAdd(file, initializeCheckFormat, text);
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

    static string initFormat =
@"    protected override void Initialize()
    {
        base.Initialize();
    }";
    #endregion
}
