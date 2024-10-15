public class ServerProgramPacket
{
    static SecretOptionFile secretFile = new SecretOptionFile();

    public static void AddSingleton(string name)
    {
        string file = GetFile();
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.Error_H($"ServerProgramPacket AddSingleton Error Must Link ServerProgram.cs");
            return;
        }

        bool isExist = ExistSingleton(name);

        if (isExist)
        {
            UnityHelper.Error_H($"ServerProgramPacket AddSingleton Error Alread Exist\nname : {name}");
            return;
        }

        string text = "";
        bool isReadRegion = false;
        bool isReadEndRegion = false;
        string singletonF = CSharpHelper.Format_H(singletonFormat, name);

        foreach (var line in FileHelper.ReadLines(file))
        {
            bool isWrite = false;

            if (line.Trim().Equals(checkAddSingletonRegion))
            {
                isReadRegion = true;
            }

            if (line.Trim().Equals(checkEndRegion))
            {
                if (isReadRegion && !isReadEndRegion)
                {
                    isWrite = true;

                    text += $"{singletonF}\n";
                    text += $"{line}\n";
                }

                isReadEndRegion = true;
            }

            if (!isWrite)
                text += $"{line}\n";
        }

        FileHelper.Write(file, text, false);
    }
    public static void RemoveSingleton(string name) 
    {
        string file = GetFile();
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.Error_H($"ServerProgramPacket RemoveSingleton Error Must Link ServerProgram.cs");
            return;
        }

        bool isExist = ExistSingleton(name);

        if (!isExist)
        {
            UnityHelper.Error_H($"ServerProgramPacket RemoveSingleton Error Not Exist\nname : {name}");
            return;
        }

        string text = "";
        string singletonF = CSharpHelper.Format_H(singletonFormat, name);

        foreach (var line in FileHelper.ReadLines(file))
        {
            bool isWrite = false;

            if (line.Trim().Equals(singletonF))
            {
                isWrite = true;
            }

            if (!isWrite)
                text += $"{line}\n";
        }

        FileHelper.Write(file, text, false);
    }
    public static bool ExistSingleton(string name)
    {
        string file = GetFile();
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.Error_H($"Must Link ServerProgram.cs");
            return false;
        }

        string checkF = CSharpHelper.Format_H(singletonFormat, name);

        foreach (var line in FileHelper.ReadLines(file))
        {
            if (line.Trim().Equals(checkF))
            {
                return true;
            }
        }

        return false;
    }
    public static void AddScoped(string name) 
    {
        string file = GetFile();
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.Error_H($"ServerProgramPacket AddScoped Error Must Link ServerProgram.cs");
            return;
        }

        bool isExist = ExistScoped(name);

        if (isExist)
        {
            UnityHelper.Error_H($"ServerProgramPacket AddScoped Error Alread Exist\nname : {name}");
            return;
        }

        string text = "";
        bool isReadRegion = false;
        bool isReadEndRegion = false;
        string scopedF = CSharpHelper.Format_H(scopedFormat, name);

        foreach (var line in FileHelper.ReadLines(file))
        {
            bool isWrite = false;

            if (line.Trim().Equals(checkAddSingletonRegion))
            {
                isReadRegion = true;
            }

            if (line.Trim().Equals(checkEndRegion))
            {
                if (isReadRegion && !isReadEndRegion)
                {
                    isWrite = true;

                    text += $"{scopedF}\n";
                    text += $"{line}\n";
                }

                isReadEndRegion = true;
            }

            if (!isWrite)
                text += $"{line}\n";
        }

        FileHelper.Write(file, text, false);
    }
    public static void RemoveScoped(string name) 
    {
        string file = GetFile();
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.Error_H($"ServerProgramPacket RemoveScoped Error Must Link ServerProgram.cs");
            return;
        }

        bool isExist = ExistScoped(name);

        if (!isExist)
        {
            UnityHelper.Error_H($"ServerProgramPacket RemoveScoped Error Not Exist\nname : {name}");
            return;
        }

        string text = "";
        string scopedF = CSharpHelper.Format_H(scopedFormat, name);

        foreach (var line in FileHelper.ReadLines(file))
        {
            bool isWrite = false;

            if (line.Trim().Equals(scopedF))
            {
                isWrite = true;
            }

            if (!isWrite)
                text += $"{line}\n";
        }

        FileHelper.Write(file, text, false);
    }
    public static bool ExistScoped(string name)
    {
        string file = GetFile();
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.Error_H($"Must Link ServerProgram.cs");
            return false;
        }

        string checkF = CSharpHelper.Format_H(scopedFormat, name);

        foreach (var line in FileHelper.ReadLines(file))
        {
            if (line.Trim().Equals(checkF))
            {
                return true;
            }
        }

        return false;
    }

    static string GetFile()
    {
        string file = secretFile.Read("ServerProgramPath");

        if (FileHelper.FileExist(file))
        {
            return file;
        }
        else
        {
            UnityHelper.Error_H($"ServerProgramPacket GetTableFile No Linked File");
            return "";
        }
    }

    #region Format
    static string checkAddSingletonRegion =
@"#region AddSingleton";
    static string checkEndRegion =
@"#endregion";
    // {0} Name
    static string singletonFormat =
@"builder.Services.AddSingleton<{0}>();";
    // {0} Name
    static string scopedFormat =
@"builder.Services.AddScoped<{0}>();";
    #endregion
}
