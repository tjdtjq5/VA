using System;
using System.Collections.Generic;

public class TableManager
{
    GameDefineTable _gameDefineTable = new GameDefineTable();
    FormulaTable _formulaTable = new FormulaTable();
    CharacterTable _characterTable = new CharacterTable();
    ItemTable _itemTable = new ItemTable();
    public void Initialize()
    {
        _gameDefineTable.InitialData();
        _formulaTable.InitialData();
        _characterTable.InitialData();
        _itemTable.InitialData();
    }
    public void DbGets(Action callback = null)
    {
        Managers.Web.SendGetRequest<MasterTableGetsResponse>("masterTable/gets", (res) =>
        {
            for (int i = 0; i < res.datas.Count; i++) 
            {
                switch (res.datas[i].tableName)
                {
                    case "GameDefine": _gameDefineTable.Push(CSharpHelper.DeserializeObject<List<GameDefineTableData>>(res.datas[i].tableDatas)); break;
                    case "Formula": _formulaTable.Push(CSharpHelper.DeserializeObject<List<FormulaTableData>>(res.datas[i].tableDatas)); break;
                    case "Character": _characterTable.Push(CSharpHelper.DeserializeObject<List<CharacterTableData>>(res.datas[i].tableDatas)); break;
                    case "Item": _itemTable.Push(CSharpHelper.DeserializeObject<List<ItemTableData>>(res.datas[i].tableDatas)); break;
                }
            }

            if (callback != null)
                callback.Invoke();
        });
    }
    public ItemTable ItemTable => _itemTable;
    public CharacterTable CharacterTable => _characterTable;
    public FormulaTable FormulaTable => _formulaTable;
    public GameDefineTable GameDefineTable => _gameDefineTable;
}