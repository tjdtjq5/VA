using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableFunction
{
	public static string GetUpdateTableName { get => "Formula"; }

    public static async void UpdateTable(string tableName, string tableData)
	{
        if (!tableName.Equals(GetUpdateTableName))
        {
            UnityHelper.LogError_H($"Error Deference Table Function");
            return;
        }

        List<FormulaTableData> tableDatas = GoogleSpreadSheetUtils.GetListTableDatas<FormulaTableData>(tableName, tableData);

        string addUrl = $"{tableName.ToLower_H()}Table/update";
        var result = await WebTaskCall.Post<FormulaTableUpdateResponse>(true, addUrl, tableDatas);

        string resultSeri = CSharpHelper.SerializeObject(result);
        UnityHelper.Log_H(resultSeri);
	}
}
