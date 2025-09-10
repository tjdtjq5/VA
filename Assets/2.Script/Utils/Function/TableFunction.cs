using System.Collections;
using System.Collections.Generic;
using Shared.CSharp;
using UnityEngine;

public class TableFunction
{
	public static string GetUpdateTableName { get => "Item"; }

    public static async void UpdateTable(string tableName, string tableData)
	{
        if (!tableName.Equals(GetUpdateTableName))
        {
            UnityHelper.Error_H($"Error Deference Table Function");
            return;
        }

        List<ItemTableData> tableDatas = GoogleSpreadSheetUtils.GetListTableDatas<ItemTableData>(tableName, tableData);

        string addUrl = $"{tableName.ToLower_H()}Table/update";
        // var result = await WebTaskCall.Post<ItemTableUpdateResponse>(true, addUrl, tableDatas);

        // string resultSeri = CSharpHelper.SerializeObject(result);
        // UnityHelper.Log_H(resultSeri);
	}
}
