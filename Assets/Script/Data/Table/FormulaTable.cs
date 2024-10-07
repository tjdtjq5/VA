using System;
using System.Collections.Generic;

public class FormulaTable : Table<FormulaTableData>
{
    public override void DbGets(Action<List<FormulaTableData>> result)
    {
        Managers.Web.SendGetRequest<FormulaTableGetsResponse>("formulaTable/gets", (_result) =>
        {
            datas = _result.datas;
            result.Invoke(_result.datas);
        });
    }
}
