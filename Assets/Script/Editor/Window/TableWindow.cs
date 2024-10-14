using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TableWindow : EditorWindow
{
    static Vector2 _windowSize;

    static string[] _upperToolbarModelList;
    static int _upperToolbarIndex;
    static float _upperToolbarHeight;

    GoogleSheetFile googleSheetFile = new GoogleSheetFile();
    SecretOptionFile secretFileTxt = new SecretOptionFile();

    static float _selectViewListWidth;
    static string[] _selectKeys;
    static string _selectKey;

    static string _connectTableName;
    static string _connectSheetId;
    static string _connectRange;

    static string _tableData;
    static Vector2 _tableScrollPos;

    static float _subHeight;

    [MenuItem("Tool/Table %t")]
    static void Open()
    {
        var window = GetWindow<TableWindow>();
        window.titleContent = new GUIContent() { text = "Table" };
        window.minSize = _windowSize;
        window.Show();
    }

    private void OnEnable()
    {
        _windowSize = new Vector2(600, 700);

        _upperToolbarModelList = new string[] { "View Mode", "Connet Mode" };
        _upperToolbarHeight = 40f;

        _selectViewListWidth = 150f;
        _selectKey = "";

        _tableData = "";

        _subHeight = 85f;
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        {
            UpperSetting();

            EditorGUILayout.Space(4);
            CustomEditorUtility.DrawUnderline();
            EditorGUILayout.Space(4);

            if (_upperToolbarIndex == 0)
            {
                ViewModeSetting();
            }
            else if (_upperToolbarIndex == 1) 
            {
                ConnetModeSetting();
            }

        }
        EditorGUILayout.EndVertical();

    }
    void UpperSetting()
    {
        // Tab : View Mode, Connect Mode

        _upperToolbarIndex = GUILayout.Toolbar(_upperToolbarIndex, _upperToolbarModelList, GUILayout.Height(_upperToolbarHeight));

    }
    void ViewModeSetting()
    {
        // Left : Select View List

        // Title : Table Name

        // Main
        // View Table

        // Sub
        // GoTo Sheet Btn
        // Push Server DB

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(_selectViewListWidth));
            {
                _selectKeys = googleSheetFile.Keys().ToArray();

                Color originColor = GUI.backgroundColor;

                for (int i = 0; i < _selectKeys.Length; i++)
                {
                    GUI.backgroundColor = (_selectKeys[i] == _selectKey) ? Color.cyan : originColor;

                    if (GUILayout.Button($"{_selectKeys[i]}", GUILayout.Height(35)))
                    {
                        string sk = _selectKeys[i];
                        long sheetId = googleSheetFile.Read<GoogleSheetFileModel>(sk).sheetId;
                        string range = googleSheetFile.Read<GoogleSheetFileModel>(sk).range;

                        _selectKey = "";
                        _tableData = "";

                        GoogleSheetLoadTable(sheetId, range, (tableData) => 
                        {
                            _selectKey = sk;
                            _tableData = tableData;
                        });
                    }
                }

                GUI.backgroundColor = originColor;
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(4);
            var height = CustomEditorUtility.GetScreenHeight - 50;
            CustomEditorUtility.DrawLine(1, height);
            EditorGUILayout.Space(4);

            EditorGUILayout.BeginVertical();
            TableSetting();
            SubSetting();
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
    }
    void TableSetting()
    {
        if (string.IsNullOrEmpty(_selectKey) || string.IsNullOrEmpty(_tableData))
        {
            return;
        }

        float tableWidth = CustomEditorUtility.GetScreenWidth - _selectViewListWidth - 30;
        float tableHeight = CustomEditorUtility.GetScreenHeight - _upperToolbarHeight - _subHeight;
        var labelStyle = CustomEditorUtility.GetMiddleLabel;
        float lineSize = 2;

        string[] lineDatas = _tableData.Split('\n');
        int lineCount = 0;

        _tableScrollPos = EditorGUILayout.BeginScrollView(_tableScrollPos, GUILayout.Height(tableHeight), GUILayout.Width(tableWidth));
        EditorGUILayout.BeginVertical(GUILayout.Height(tableHeight), GUILayout.Width(tableWidth));
        {
            foreach (var line in lineDatas)
            {
                string[] datas = line.Split('\t');

                float dataWidth = tableWidth / datas.Length;

                EditorGUILayout.BeginHorizontal();
                {
                    for (int i = 0; i < datas.Length; i++)
                    {
                        string data = datas[i];

                        // Name
                        if (lineCount == 0)
                        {
                            GUILayout.Box(data, GUILayout.Width(dataWidth));

                            EditorGUILayout.Space(2);
                            CustomEditorUtility.DrawLine(lineSize, tableHeight + _tableScrollPos.y);
                            EditorGUILayout.Space(2);
                        }
                        else
                        {
                            EditorGUILayout.SelectableLabel(data, labelStyle, GUILayout.Width(dataWidth + 4 + lineSize), GUILayout.Height(25));
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space(2);

                CustomEditorUtility.DrawUnderline(lineSize);

                lineCount++;
            }
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();
    }
    void SubSetting()
    {
        if (string.IsNullOrEmpty(_selectKey) || string.IsNullOrEmpty(_tableData))
        {
            return;
        }

        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Google Sheet"))
            {
                long sheetId = googleSheetFile.Read<GoogleSheetFileModel>($"{_selectKey}").sheetId;
                OnClickGoToGoogleSheet(sheetId);
            }

            if (GUILayout.Button("Create Table DB"))
            {
                OnClickCreateTableDB(_selectKey, _tableData);
            }

            if (GUILayout.Button("Update Table DB"))
            {
                OnClickTableUpdate(_selectKey, _tableData);
            }

            Color origin = GUI.color;
            GUI.color = Color.red;
            if (GUILayout.Button("Delete Table DB"))
            {
                OnClickTableDelete(_selectKey);
            }
            GUI.color = origin;
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Server Start"))
            {
                OnClickDebugServerStart();
            }
        }
        EditorGUILayout.EndHorizontal();
    }
    async void GoogleSheetLoadTable(long sheetId, string range, System.Action<string> res)
    {
        string address = secretFileTxt.Read("GoogleSheetAdress");
        string url = GoogleSpreadSheetUtils.GetTSVAdress(address, range, sheetId);

        string data = await WebTaskCall.Get(url);

        res.Invoke(data);
    }
    void OnClickGoToGoogleSheet(long sheetId)
    {
        string address = secretFileTxt.Read("GoogleSheetAdress");
        Application.OpenURL(GoogleSpreadSheetUtils.GetUrl(address, sheetId));
    }
    void OnClickCreateTableDB(string tableName, string tableData)
    {
        if (!TableDbPacket.Exist(tableName))
        {
            TableDbPacket.Create(tableName, tableData);
        }
        else
        {
            TableDbPacket.Modify(tableName, tableData);
        }

        if (!DbContextPacket.Exist(tableName))
        {
            DbContextPacket.Add(tableName);
        }

        if (!TableDataPacket.Exist(tableName))
        {
            TableDataPacket.Create(tableName, tableData);
        }
        else
        {
            TableDataPacket.Modify(tableName, tableData);
        }

        if (!TableRRPacket.Exist(tableName))
        {
            TableRRPacket.Create(tableName);
        }

        if (!TableControllerPacket.Exist(tableName))
        {
            TableControllerPacket.Create(tableName);
        }

        if (!TableServicePacket.Exist(tableName))
        {
            TableServicePacket.Create(tableName, tableData);
        }

        if (!TableCollectionPacket.Exist(tableName))
        {
            TableCollectionPacket.Add(tableName);
        }

        if (!TableManagerPacket.Exist(tableName))
        {
            TableManagerPacket.Add(tableName);
        }

        TablePacket.Add(tableName, tableData);

        MasterTableServicePacket.Create(tableName);

        //ItemTableService
        string addScopedName = $"{tableName}TableService";
        if (!ServerProgramPacket.ExistScoped(addScopedName))
        {
            ServerProgramPacket.AddScoped(addScopedName);
        }

        DefineCopy();

        bool isDialogMsg = EditorMessageUtils.DialogMessage("CreateTableDB", "Do It\n1. Add-Migration [Message]\n2. Update-Database\n3. Build");
        if (isDialogMsg)
        {
            //string serverPath = secretFileTxt.Read("ServerSlnPath");
            //FileHelper.ProcessStart(serverPath);
        }
    }
    void OnClickTableUpdate(string tableName, string tableData)
    {
        if (!TableFunctionPacket.IsCheckUpdateTable(tableName))
        {
            TableFunctionPacket.ChangeUpdateFunction(tableName);
            EditorMessageUtils.DialogMessage("TableFunction Loading", "After Loading, Try Again!");
        }
        else
        {
            TableFunction.UpdateTable(tableName, tableData);

            TableCodeDefine(tableName, tableData);
            CreateSOs(tableName, tableData);
            DefineCopy();
        }
    }
    void OnClickTableDelete(string tableName)
    {
        bool isDialogMsg = EditorMessageUtils.DialogMessage("DeleteTableDB", "Do It\n1. Add-Migration [Message]\n2. Update-Database\n3. Build");
        if (isDialogMsg)
        {
            if (TableDbPacket.Exist(tableName))
            {
                TableDbPacket.Remove(tableName);
            }

            if (DbContextPacket.Exist(tableName))
            {
                DbContextPacket.Remove(tableName);
            }

            if (TableDataPacket.Exist(tableName))
            {
                TableDataPacket.Remove(tableName);
            }

            if (TableRRPacket.Exist(tableName))
            {
                TableRRPacket.Remove(tableName);
            }

            if (TableControllerPacket.Exist(tableName))
            {
                TableControllerPacket.Remove(tableName);
            }

            if (TableServicePacket.Exist(tableName))
            {
                TableServicePacket.Remove(tableName);
            }

            if (TableCollectionPacket.Exist(tableName))
            {
                TableCollectionPacket.Remove(tableName);
            }

            if (TableManagerPacket.Exist(tableName))
            {
                TableManagerPacket.Remove(tableName);
            }

            TablePacket.Remove(tableName);

            MasterTableServicePacket.Remove(tableName);

            //ItemTableService
            string addScopedName = $"{tableName}TableService";
            if (ServerProgramPacket.ExistScoped(addScopedName))
            {
                ServerProgramPacket.RemoveScoped(addScopedName);
            }

            // 복사하기 DefineCopyPath
            string defineCopyPath = secretFileTxt.Read("DefineCopyPath");
            FileHelper.ProcessStart(defineCopyPath);

            //string serverPath = secretFileTxt.Read("ServerSlnPath");
            //FileHelper.ProcessStart(serverPath);
        }
    }
    void OnClickDebugServerStart()
    {
        // 서버 실행 
        ServerEditor.StartServer();
    }
    void ConnetModeSetting()
    {
        // TableName
        // SheetId
        // Range

        if (!string.IsNullOrEmpty(_selectKey) && string.IsNullOrEmpty(_connectTableName))
        {
            _connectTableName = _selectKey;
            _connectSheetId = googleSheetFile.Read<GoogleSheetFileModel>(_selectKey).sheetId.ToString();
            _connectRange = googleSheetFile.Read<GoogleSheetFileModel>(_selectKey).range;
        }

        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.LabelField("Table Name");
            _connectTableName = EditorGUILayout.TextField(_connectTableName);
            EditorGUILayout.LabelField("Sheet Id");
            _connectSheetId = EditorGUILayout.TextField(_connectSheetId);
            EditorGUILayout.LabelField("Range");
            _connectRange = EditorGUILayout.TextField(_connectRange);

            EditorGUILayout.Space(10);
            CustomEditorUtility.DrawUnderline();
            EditorGUILayout.Space(10);

            Color originColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Connect"))
            {
                if (string.IsNullOrEmpty(_connectTableName) || string.IsNullOrEmpty(_connectSheetId) || string.IsNullOrEmpty(_connectRange))
                {
                    UnityHelper.LogError_H($"TableWindow ConnetModeSetting Null Or Empty Error");
                    return;
                }

                long sheetId = 0;
                if (!long.TryParse(_connectSheetId, out sheetId))
                {
                    UnityHelper.LogError_H($"TableWindow ConnetModeSetting SheetId Parse Error\n_connectSheetId : {_connectSheetId}");
                    return;
                }

                googleSheetFile.Add(_connectTableName, new GoogleSheetFileModel(sheetId, _connectTableName, _connectRange));

                _upperToolbarIndex = 0;

                GoogleSheetLoadTable(sheetId, _connectRange, (tableData) =>
                {
                    _selectKey = _connectTableName;
                    _tableData = tableData;

                    _connectTableName = "";
                    _connectSheetId = "";
                    _connectRange = "";
                });
            }

            EditorGUILayout.Space(4);

            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Remove"))
            {
                OnClickConnectRemove(_connectTableName);
            }
            GUI.backgroundColor = originColor;
        }
        EditorGUILayout.EndVertical();
    }

    void OnClickConnectRemove(string tableName)
    {
        bool notAlready = false;
        if (TableDbPacket.Exist(tableName))
        {
            notAlready = true;
        }

        if (DbContextPacket.Exist(tableName))
        {
            notAlready = true;
        }

        if (TableDataPacket.Exist(tableName))
        {
            notAlready = true;
        }

        if (TableRRPacket.Exist(tableName))
        {
            notAlready = true;
        }

        if (TableControllerPacket.Exist(tableName))
        {
            notAlready = true;
        }

        if (TableServicePacket.Exist(tableName))
        {
            notAlready = true;
        }

        if (TableCollectionPacket.Exist(tableName))
        {
            notAlready = true;
        }

        if (TableManagerPacket.Exist(tableName))
        {
            notAlready = true;
        }

        //ItemTableService
        string addScopedName = $"{tableName}TableService";
        if (ServerProgramPacket.ExistScoped(addScopedName))
        {
            notAlready = true;
        }

        if (notAlready)
        {
            UnityHelper.LogError_H($"First you need to delete all table data");
            return;
        }


        googleSheetFile.Remove(_connectTableName);

        _upperToolbarIndex = 0;

        _selectKey = "";
        _tableData = "";

        _connectTableName = "";
        _connectSheetId = "";
        _connectRange = "";
    }

    void DefineCopy()
    {
        string defineCopyPath = secretFileTxt.Read("DefineCopyPath");
        FileHelper.ProcessStart(defineCopyPath);
    }
    void TableCodeDefine(string tableName, string tableData)
    {
        bool isExistDefineEnum = CSharpHelper.ExistEnumData<DefineTableCodeType>(tableName);
        if (isExistDefineEnum) 
        {
            List<string> tableKeys = GoogleSpreadSheetUtils.GetKeyDatas(tableData);
            TableDefineCodePacket.Update(tableName, tableKeys);
        }
    }

    // SO
    public void CreateSOs(string tableName, string tableData)
    {
        bool isExistSOEnum = CSharpHelper.ExistEnumData<SOTableType>(tableName);

        if (isExistSOEnum)
        {
            List<string> tableKeys = GoogleSpreadSheetUtils.GetKeyDatas(tableData);
            for (int i = 0; i < tableKeys.Count; i++)
            {
                TableSO soData = CreateSO(tableName, tableKeys[i]);
            }
        }
    }
    public TableSO CreateSO(string tableName, string code)
    {
        string path = DefinePath.TableSODirectory(tableName);
        if (!FileHelper.DirectoryExist(path))
            FileHelper.DirectoryCreate(path);

        string soName = DefinePath.TableSOName(tableName, code);

        path = DefinePath.TableSOPath(tableName, code);
        var so = AssetDatabase.LoadAssetAtPath<TableSO>(path);

        if (so == null)
        {
            so = CreateInstance<TableSO>();
            so.codeName = code;
            AssetDatabase.CreateAsset(so, path);
        }

        return so;
    }
}
