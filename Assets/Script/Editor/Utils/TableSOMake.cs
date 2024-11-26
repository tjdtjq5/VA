using log4net.Util;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TableSOMake : EditorWindow
{
   public static string TableName { get => ""; }

    public static void CreateSO(string tableName, string tableData)
    {
        bool isExistSOEnum = CSharpHelper.ExistEnumData<SOTableType>(tableName);
        if (!isExistSOEnum)
            return;

        if (!tableName.Equals(TableName))
        {
            UnityHelper.Error_H($"Error Deference Table CreateSO");
            return;
        }

        List<string> tableKeys = GoogleSpreadSheetUtils.GetKeyDatas(tableData);
        for (int i = 0; i < tableKeys.Count; i++)
        {
            string code = tableKeys[i];

            string path = DefinePath.TableSODirectory(tableName);
            if (!FileHelper.DirectoryExist(path))
                FileHelper.DirectoryCreate(path);

            string soName = DefinePath.TableSOName(tableName, code);

            path = DefinePath.TableSOPath(tableName, code);
            var so = AssetDatabase.LoadAssetAtPath<TTT>(path);

            if (so == null)
            {
                so = CreateInstance<TTT>();
                so.codeName = code;
                AssetDatabase.CreateAsset(so, path);
            }
        }
    }
}

public class TTT : ScriptableObject
{
    public string codeName;
}