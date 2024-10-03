using System.Collections.Generic;

public class MasterTableGetsResponse
{
    public List<MasterTableGetsData> datas {  get; set; }
}
public class MasterTableGetsData
{
    public string tableName { get; set; }
    public string tableDatas { get; set; }
}

public class ItemTableGetsResponse
{
    public string tableName { get; set; }
    public List<ItemTableData> datas { get; set; }
}

public class ItemTableUpdateResponse
{
    public string tableName { get; set; }
    public List<string> changeDatas { get; set; }
}

public class CharacterTableUpdateResponse
{
    public string tableName { get; set; }
    public List<string> changeDatas { get; set; }
}

public class CharacterTableGetsResponse
{
    public string tableName { get; set; }
    public List<CharacterTableData> datas { get; set; }
}

public class FormulaTableUpdateResponse
{
    public string tableName { get; set; }
    public List<string> changeDatas { get; set; }
}

public class FormulaTableGetsResponse
{
    public string tableName { get; set; }
    public List<FormulaTableData> datas { get; set; }
}