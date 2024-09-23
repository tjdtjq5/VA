using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableFunction
{
	public static string GetUpdateTableName { get => "TestT"; }

    public static async void UpdateTable(string tableName, string tableData)
	{
        if (!tableName.Equals(GetUpdateTableName))
        {
            UnityHelper.LogError_H($"Error Deference Table Function");
            return;
        }

        List<ItemTableData> tableDatas = GoogleSpreadSheetUtils.GetListTableDatas<ItemTableData>(tableName, tableData);

        string addUrl = $"{CSharpHelper.StartCharToLower(tableName)}Table/update";
        var result = await WebTaskCall.Post<ItemTableUpdateResponse>(true, addUrl, tableDatas);

        string resultSeri = CSharpHelper.SerializeObject(result);
        UnityHelper.Log_H(resultSeri);
	}
}
