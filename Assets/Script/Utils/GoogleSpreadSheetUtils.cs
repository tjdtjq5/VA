using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
                data = CSharpHelper.GetReplaceRNT(data);
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
    public static List<string> GetKeyDatas(string tableData)
    {
        List<string> resultDatas = new List<string>();

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

            string variable = variables[0];
            string data = datas[0];
            data = CSharpHelper.GetReplaceRNT(data);
            resultDatas.Add(data);
        }

        return resultDatas;
    }
    public static List<string> GetVariables(string tableData)
    {
        List<string> resultDatas = new List<string>();

        string[] lineDatas = tableData.Split('\n');
        string variableNameLine = lineDatas[0];

        string[] variables = variableNameLine.Split('\t');
        for (int i = 0; i < variables.Length; i++)
        {
            variables[i] = variables[i].Trim().ToLower();
        }

        string[] datas = lineDatas[0].Split("\t");

        for (int i = 0;i < datas.Length; i++)
        {
            resultDatas.Add(CSharpHelper.GetReplaceRNT(datas[i]));
        }

        return resultDatas;
    }
    public static string GetValueData(string tableData, int index ,int dataIndex)
    {
        string[] lineDatas = tableData.Split('\n');
        string variableNameLine = lineDatas[0];

        string[] variables = variableNameLine.Split('\t');
        for (int i = 0; i < variables.Length; i++)
        {
            variables[i] = variables[i].Trim().ToLower();
        }

        string[] datas = lineDatas[index + 1].Split("\t");

        string variable = variables[0];
        string data = datas[dataIndex];
        data = CSharpHelper.GetReplaceRNT(data);

        return data;
    }
    public static int GetDataCount(string tableData)
    {
        string[] lineDatas = tableData.Split('\n');
        return lineDatas.Length - 1;
    }
}
