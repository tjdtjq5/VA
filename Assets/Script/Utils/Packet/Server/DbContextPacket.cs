using System;
using System.Collections.Generic;
using System.Xml.Linq;

public class DbContextPacket
{
    static SecretOptionFile secretFile = new SecretOptionFile();

    public static void Add(string tableName)
    {
        string file = GetTableFile();
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"DbContextPacket Add Error Must Link ApplicationDbContext.cs");
            return;
        }

        bool isExist = Exist(tableName);

        if (isExist)
        {
            UnityHelper.LogError_H($"DbContextPacket Add Error Already Exist TableDB\nTableName : {tableName}");
            return;
        }

        string dbSetF = CSharpHelper.Format_H(dbSetFormat, tableName);

        string text = "";
        bool findDbSet = false;
        bool addFlag = false;

        foreach (var line in FileHelper.ReadLines(file))
        {

            if (line.Trim().Contains(dbSetText))
            {
                findDbSet = true;
            }
            else
            {
                if (!addFlag && findDbSet)
                {
                    addFlag = true;

                    text += $"\t{dbSetF}\n";
                }
            }

            text += $"{line}\n";
        }

        FileHelper.Write(file, text, false);
    }
    public static void Remove(string tableName)
    {
        string file = GetTableFile();
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"DbContextPacket Remove Error Must Link ApplicationDbContext.cs");
            return;
        }

        bool isExist = Exist(tableName);

        if (!isExist)
        {
            UnityHelper.LogError_H($"DbContextPacket Add Error Not Exist TableDB\nTableName : {tableName}");
            return;
        }

        string dbSetF = CSharpHelper.Format_H(dbSetFormat, tableName);

        string text = "";
        bool findCheck = false;

        foreach (var line in FileHelper.ReadLines(file))
        {
            if (line.Trim().Equals(dbSetF))
            {
                findCheck = true;
            }

            if (!findCheck)
            {
                text += $"{line}\n";
            }
            else
            {
                findCheck = false;
            }
        }

        FileHelper.Write(file, text, false);
    }

    public static bool Exist(string tableName)
    {
        string file = GetTableFile();
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"DbContextPacket Exist Error Must Link ApplicationDbContext.cs");
            return false;
        }

        string dbSetF = CSharpHelper.Format_H(dbSetFormat, tableName);

        foreach (var line in FileHelper.ReadLines(file))
        {
            if (line.Trim().Equals(dbSetF))
            {
                return true;
            }
        }

        return false;
    }
    public static string GetTableFile()
    {
        string file = secretFile.Read("ApplicationDbContextPath");

        if (FileHelper.FileExist(file))
        {
            return file;
        }
        else
        {
            UnityHelper.LogError_H($"TableDbPacket GetTableFile No Linked File");
            return "";
        }
    }

    #region Format

    // {0} TableName
    static string dbSetFormat =
@"public DbSet<{0}Db> {0}s {{ get; set; }}";

    static string dbSetText = "DbSet";

    #endregion
}
