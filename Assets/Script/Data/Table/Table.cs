using System;
using System.Collections;
using System.Collections.Generic;

public abstract class Table<T>
{
    protected virtual string TableName {  get; set; }
    protected virtual Dictionary<string, TableSO> tableSOs { get; set; } = new();
    protected List<T> datas = new List<T>();
    public List<T> Gets() => datas;
    public void Push(List<T> pushDatas) => datas = pushDatas;
    public abstract void DbGets(Action<List<T>> result);
    public abstract void InitialData();
    public TableSO GetTableSO(string code)
    {
        if (tableSOs.ContainsKey(code))
            return tableSOs[code];

        string path = DefinePath.TableSOResourcesPath(TableName, code);
        TableSO tableSO = Managers.Resources.Load<TableSO>(path);

        UnityHelper.Log_H($"[{path}] : {tableSO != null}");

        tableSOs.Add(code, tableSO);
        return tableSO;
    }
}
