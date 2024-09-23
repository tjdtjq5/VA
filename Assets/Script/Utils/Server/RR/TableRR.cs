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





