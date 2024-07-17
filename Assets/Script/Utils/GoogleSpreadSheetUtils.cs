using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class GoogleSpreadSheetUtils
{
    public static string GetTSVAdress(string adress, string range, long sheetId)
    {
        return $"{adress}/export?format=tsv&range={range}&gid={sheetId}";
    }
    public static string GetUrl(string adress, long sheetId)
    {
        return $"{adress}/edit?gid={sheetId}";
    }
    public static List<T> GetListTableDatas<T>(string tableName, string tableData) where T : new()
    {
        List<T> updateDatas = new List<T>();

        string[] lineDatas = tableData.Split('\n');
        string variableNameLine = lineDatas[0];

        string[] variables = variableNameLine.Split('\t');
        for (int i = 0; i < variables.Length; i++)
        {
            variables[i] = variables[i].Trim().ToLower();
        }

        for (int i = 1; i < lineDatas.Length; i++)
        {
            string[] datas = lineDatas[i].Split("\t");
            T updateData = new T();
            List<FieldInfo> fieldInfos = CSharpHelper.GetFieldInfos(updateData).ToList();

            for (int j = 0; j < variables.Length; j++)
            {
                string variable = variables[j];
                string data = datas[j];
                data = CSharpHelper.GetReplaceRegex(data);
                object dataObj = CSharpHelper.AutoParse(data);

                FieldInfo fieldInfo = fieldInfos.Find(f => f.Name.ToLower().Contains(variable));
                if (fieldInfo != null)
                {
                    fieldInfo.SetValue(updateData, dataObj);
                }
            }

            updateDatas.Add(updateData);
        }

        return updateDatas;
    }
}
