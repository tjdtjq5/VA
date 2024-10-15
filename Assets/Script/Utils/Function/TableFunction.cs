using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableFunction
{
	public static string GetUpdateTableName { get => "Character"; }

    public static async void UpdateTable(string tableName, string tableData)
	{
        if (!tableName.Equals(GetUpdateTableName))
        {
            UnityHelper.Error_H($"Error Deference Table Function");
            return;
        }

        List<CharacterTableData> tableDatas = GoogleSpreadSheetUtils.GetListTableDatas<CharacterTableData>(tableName, tableData);

        string addUrl = $"{tableName.ToLower_H()}Table/update";
        var result = await WebTaskCall.Post<CharacterTableUpdateResponse>(true, addUrl, tableDatas);

        string resultSeri = CSharpHelper.SerializeObject(result);
        UnityHelper.Log_H(resultSeri);
	}
}
