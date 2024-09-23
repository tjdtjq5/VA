public class TableControllerPacket
{
    static SecretOptionFile secretFile = new SecretOptionFile();

    public static void Create(string tableName)
    {
        string file = GetFile();
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"TableControllerPacket Create Error Must Link TableController.cs");
            return;
        }

        bool isExist = Exist(tableName);

        if (isExist)
        {
            UnityHelper.LogError_H($"TableControllerPacket Create Error Alread Exist Table\nTable : {tableName}");
            return;
        }

        string text = "";

        string routeF = CSharpHelper.Format_H(routeFormat, LowerTableName(tableName));
        text += $"{routeF}\n";

        string classF = CSharpHelper.Format_H(classFormat, tableName);

        if (string.IsNullOrEmpty(classF))
            return;

        text += $"{classF}";

        SimpleFormat.OuterCreate(file, text);
    }
    public static void Remove(string tableName)
    {
        string file = GetFile();
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"Must Link TableController.cs");
            return;
        }

        bool isExist = Exist(tableName);

        if (!isExist)
        {
            UnityHelper.LogError_H($"TableControllerPacket Remove Error Not Exist Table\nTable : {tableName}");
            return;
        }

        string tableCheckF = CSharpHelper.Format_H(routeFormat, LowerTableName(tableName));
        SimpleFormat.RemoveStruct(file, tableCheckF);
    }
    public static bool Exist(string tableName)
    {
        string file = GetFile();
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"Must Link TableController.cs");
            return false;
        }

        string tableCheckF = CSharpHelper.Format_H(routeFormat, LowerTableName(tableName));
        return SimpleFormat.Exist(file, tableCheckF);
    }

    static string GetFile()
    {
        string file = secretFile.Read("TableControllerPath");

        if (FileHelper.FileExist(file))
        {
            return file;
        }
        else
        {
            UnityHelper.LogError_H($"TableControllerPacket GetTableFile No Linked File");
            return "";
        }
    }
    static string LowerTableName(string tableName)
    {
        return CSharpHelper.StartCharToLower(tableName);
    }

    #region Format

    // {0} Lower Table Name
    static string routeFormat =
@"[Route(""{0}Table"")]";

    // {0} Table Name
    static string classFormat =
@"[ApiController]
public class {0}TableController : ControllerBase
{{
    ILogger<{0}TableController> _logger;
    IConfiguration _configuration;
    {0}TableService _service;
    private readonly IDistributedCache _cache;

    public {0}TableController(IConfiguration configuration, ILogger<{0}TableController> logger, {0}TableService service, IDistributedCache cache)
    {{
        _configuration = configuration;
        _logger = logger;
        _service = service;
        _cache = cache;
    }}

    [Route(""update"")]
    public string TableUpdate([FromBody] List<{0}TableData> allDatas)
    {{
        {0}TableUpdateResponse res = new {0}TableUpdateResponse();

        List<string>? changeDatas = _service.Update(allDatas);
        res.tableName = ""{0}"";
        res.changeDatas = changeDatas;

        return CSharpHelper.SerializeObject(res, _logger);
    }}

    [Route(""gets"")]
    public string TableGets()
    {{
        {0}TableGetsResponse res = new {0}TableGetsResponse();

        List<{0}TableData>? datas = _service.Gets();

        res.tableName = ""{0}"";
        res.datas = datas;

        _cache.SetValue_H(RadisKey.GetTableKey(res.tableName), CSharpHelper.SerializeObject(datas), _logger);

        return CSharpHelper.SerializeObject(res, _logger);
    }}
}}";

    #endregion
}
