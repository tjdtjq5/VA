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

        foreach (var line in FileHelper.ReadLines(file))
        {
            text += $"{line}\n";
        }

        text += '\n';

        string routeF = CSharpHelper.Format_H(routeFormat, LowerTableName(tableName));
        text += $"{routeF}\n";

        string classF = CSharpHelper.Format_H(classFormat, tableName);

        if (string.IsNullOrEmpty(classF))
            return;

        text += $"{classF}";

        FileHelper.Write(file, text, false);
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

        string text = "";
        int leftBracketCount = 0;
        int rightBracketCount = 0;
        bool readCheck = false;
        string tableCheckF = CSharpHelper.Format_H(routeFormat, LowerTableName(tableName));

        foreach (var line in FileHelper.ReadLines(file))
        {
            if (line.Contains(tableCheckF))
            {
                readCheck = true;
            }

            if (!readCheck)
            {
                text += $"{line}\n";
            }

            if (readCheck)
            {
                if (line.Contains('{'))
                    leftBracketCount++;

                if (line.Contains('}'))
                    rightBracketCount++;

                if (leftBracketCount != 0 && leftBracketCount == rightBracketCount)
                {
                    readCheck = false;
                }
            }
        }

        text = text.Substring(0, text.Length - 1);
        FileHelper.Write(file, text, false);
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

        foreach (var line in FileHelper.ReadLines(file))
        {
            if (line.Trim().Equals(tableCheckF))
            {
                return true;
            }
        }

        return false;
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
        {0}TableResponse res = new {0}TableResponse();

        List<string>? changeDatas = _service.Update(allDatas);
        res.tableName = ""{0}"";
        res.changeDatas = changeDatas;

        return CSharpHelper.SerializeObject(res, _logger);
    }}
}}";

    #endregion
}
