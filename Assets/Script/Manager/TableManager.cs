using System.Collections.Generic;

public class TableManager
{
    ItemTable _itemTable = new ItemTable();
    public void DbGets()
    {
        Managers.Web.SendGetRequest<MasterTableGetsResponse>("masterTable/gets", (res) =>
        {
            UnityHelper.LogSerialize(res);

            for (int i = 0; i < res.datas.Count; i++) 
            {
                switch (res.datas[i].tableName)
                {
                    case "Item": _itemTable.Push(CSharpHelper.DeserializeObject<List<ItemTableData>>(res.datas[i].tableDatas)); break;
                }
            }
        });
    }
    public ItemTable ItemTable => _itemTable;
}