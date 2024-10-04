using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableFunction
{
	public static string GetUpdateTableName { get => "GameDefine"; }

    public static async void UpdateTable(string tableName, string tableData)
	{
        if (!tableName.Equals(GetUpdateTableName))
        {
            UnityHelper.LogError_H($"Error Deference Table Function");
            return;
        }

        List<GameDefineTableData> tableDatas = GoogleSpreadSheetUtils.GetListTableDatas<GameDefineTableData>(tableName, tableData);

        string addUrl = $"{CSharpHelper.StartCharToLower(tableName)}Table/update";
        var result = await WebTaskCall.Post<GameDefineTableUpdateResponse>(true, addUrl, tableDatas);

        string resultSeri = CSharpHelper.SerializeObject(result);
        UnityHelper.Log_H(resultSeri);
	}
}
