using System.Collections.Generic;
using UnityEditor;

public class TableSOMake : EditorWindow
{
   public static string TableName { get => "Character"; }

    public static void CreateSO(string tableName, string tableData)
    {
        if (!tableName.Equals(TableName))
        {
            UnityHelper.Error_H($"Error Deference Table CreateSO");
            return;
        }

        bool isExistSOEnum = CSharpHelper.ExistEnumData<SOTableType>(tableName);

        if (isExistSOEnum)
        {
            List<string> tableKeys = GoogleSpreadSheetUtils.GetKeyDatas(tableData);
            for (int i = 0; i < tableKeys.Count; i++)
            {
                string code = tableKeys[i];

                string path = DefinePath.TableSODirectory(tableName);
                if (!FileHelper.DirectoryExist(path))
                    FileHelper.DirectoryCreate(path);

                string soName = DefinePath.TableSOName(tableName, code);

                path = DefinePath.TableSOPath(tableName, code);
                var so = AssetDatabase.LoadAssetAtPath<CharacterSO>(path);

                if (so == null)
                {
                    so = CreateInstance<CharacterSO>();
                    so.codeName = code;
                    AssetDatabase.CreateAsset(so, path);
                }
            }
        }
    }
}