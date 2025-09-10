using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

public class IdentifiedObjectWindow : EditorWindow
{
    private static int _toolbarIndex = 0;
    private static readonly Dictionary<Type, Vector2> ScrollPositionsByType = new();
    private static Vector2 _drawingEditorScrollPosition;
    private static readonly Dictionary<Type, IdentifiedObject> SelectedObjectsByType = new();
    
    private readonly Dictionary<Type, IODatabase> _databasesByType = new();
    private Type[] _databaseTypes;
    private string[] _databaseTypeNames;
    
    private readonly string _noneDirectoryName = "None";
    private int _currentDirectoryIndex;
    
    private Editor _cachedEditor;
    
    private Texture2D _selectedBoxTexture;
    private GUIStyle _selectedBoxStyle;
    
    [MenuItem("Tool/SO")]
    private static void OpenWindow()
    {
        var window = GetWindow<IdentifiedObjectWindow>("SO");
        window.minSize = new Vector2(800, 700);
        window.Show();
    }
    
    private void SetupStyle()
    {
        _selectedBoxTexture = new Texture2D(1, 1);
        _selectedBoxTexture.SetPixel(0, 0, new Color(0.31f, 0.40f, 0.50f));
        _selectedBoxTexture.Apply();
        _selectedBoxTexture.hideFlags = HideFlags.DontSave;
    
        _selectedBoxStyle = new GUIStyle();
        _selectedBoxStyle.normal.background = _selectedBoxTexture;

        _currentDirectoryIndex = 0;
    }
    private void SetupDatabases(Type[] dataTypes)
    {
        if (_databasesByType.Count == 0)
        {
            if (!AssetDatabase.IsValidFolder("Assets/Resources/Database"))
            {
                AssetDatabase.CreateFolder("Assets/Resources", "Database");
            }

            foreach (var type in dataTypes)
            {
                var database = AssetDatabase.LoadAssetAtPath<IODatabase>($"Assets/Resources/Database/{type.Name}Database.asset");
                if (database == null)
                {
                    database = CreateInstance<IODatabase>();
                    AssetDatabase.CreateAsset(database, $"Assets/Resources/Database/{type.Name}Database.asset");
                    AssetDatabase.CreateFolder("Assets/Resources/SO", type.Name);
                }

                _databasesByType[type] = database;
                ScrollPositionsByType[type] = Vector2.zero;
                SelectedObjectsByType[type] = null;
            }

            _databaseTypeNames = dataTypes.Select(x => x.Name).ToArray();
            _databaseTypes = dataTypes;
        }
    }
    
    private void OnEnable()
    {
        SetupStyle();
        SetupDatabases(new[] { typeof(Item), typeof(Stat), typeof(Skill), typeof(Buff), typeof(DungeonRoom), typeof(Research), typeof(Equip), typeof(Pet) });
    }

    private void OnDisable()
    {
        DestroyImmediate(_cachedEditor);
        DestroyImmediate(_selectedBoxTexture);
    }
    
    private void OnGUI()
    {
        _toolbarIndex = GUILayout.Toolbar(_toolbarIndex, _databaseTypeNames);
        EditorGUILayout.Space(4f);
        CustomEditorUtility.DrawUnderline();
        EditorGUILayout.Space(4f);

        DrawDatabase(_databaseTypes[_toolbarIndex]);
    }
    
    private void DrawDatabase(Type dataType)
    {
        var database = _databasesByType[dataType];
        List<string> folders = GetFolders(dataType);
        string currentFolder = GetCurrentFolderName(folders);
        AssetPreview.SetPreviewTextureCacheSize(Mathf.Max(32, 32 + database.Count));

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(300f));
            {
                GUI.color = Color.green;
                if (GUILayout.Button($"New {dataType.Name}"))
                {
                    var guid = Guid.NewGuid();
                    var newData = CreateInstance(dataType) as IdentifiedObject;
                    dataType.BaseType.GetField("codeName", BindingFlags.NonPublic | BindingFlags.Instance)
                        .SetValue(newData, guid.ToString());

                    if (currentFolder.Equals(_noneDirectoryName))
                    {
                        AssetDatabase.CreateAsset(newData, $"Assets/Resources/SO/{dataType.Name}/{dataType.Name.ToUpper()}_{guid}.asset");
                    }
                    else
                    {
                        AssetDatabase.CreateAsset(newData, $"Assets/Resources/SO/{dataType.Name}/{currentFolder}/{dataType.Name.ToUpper()}_{guid}.asset");
                    }

                    database.Add(newData);
                    EditorUtility.SetDirty(database);
                    AssetDatabase.SaveAssets();

                    SelectedObjectsByType[dataType] = newData;
                }

                GUI.color = Color.red;
                if (GUILayout.Button($"Remove Last {dataType.Name}"))
                {
                    var lastData = database.Count > 0 ? database.Datas.Last() : null;
                    if (lastData)
                    {
                        database.Remove(lastData);

                        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(lastData));
                        EditorUtility.SetDirty(database);
                        AssetDatabase.SaveAssets();
                    }
                }

                GUI.color = Color.cyan;
                if (GUILayout.Button($"Sort By Name"))
                {
                    database.SortByCodeName();
                    EditorUtility.SetDirty(database);
                    AssetDatabase.SaveAssets();
                }
                GUI.color = Color.white;

                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button($"<"))
                    {
                        DownDirectoryIndex(folders);
                    }
                    EditorGUILayout.LabelField(currentFolder, GUILayout.Width(120));
                    if (GUILayout.Button($">"))
                    {
                        UpDirectoryIndex(folders);
                    }
                }
                EditorGUILayout.EndHorizontal();
          

                EditorGUILayout.Space(2f);
                CustomEditorUtility.DrawUnderline();
                EditorGUILayout.Space(4f);

                ScrollPositionsByType[dataType] = EditorGUILayout.BeginScrollView(ScrollPositionsByType[dataType], false, true,
                    GUIStyle.none, GUI.skin.verticalScrollbar, GUIStyle.none);
                {
                    foreach (var data in database.Datas)
                    {
                        string folderName = GetFolder(dataType, data);
                        if (!currentFolder.Equals(folderName))
                            continue;

                        float labelWidth = data.Icon != null ? 200f : 245f;

                        var style = SelectedObjectsByType[dataType] == data ? _selectedBoxStyle : GUIStyle.none;
                        EditorGUILayout.BeginHorizontal(style, GUILayout.Height(40f));
                        {
                            if (data.Icon)
                            {
                                var preview = AssetPreview.GetAssetPreview(data.Icon);
                                GUILayout.Label(preview, GUILayout.Height(40f), GUILayout.Width(40f));
                            }

                            EditorGUILayout.LabelField(data.CodeName, GUILayout.Width(labelWidth), GUILayout.Height(40f));

                            EditorGUILayout.BeginVertical();
                            {
                                EditorGUILayout.Space(10f);

                                GUI.color = Color.red;
                                if (GUILayout.Button("x", GUILayout.Width(20f)))
                                {
                                    database.Remove(data);
                                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(data));
                                    EditorUtility.SetDirty(database);
                                    AssetDatabase.SaveAssets();
                                }
                            }
                            EditorGUILayout.EndVertical();

                            GUI.color = Color.white;
                        }
                        EditorGUILayout.EndHorizontal();

                        if (data == null)
                            break;

                        var lastRect = GUILayoutUtility.GetLastRect();
                        if (Event.current.type == EventType.MouseDown && lastRect.Contains(Event.current.mousePosition))
                        {
                            SelectedObjectsByType[dataType] = data;
                            _drawingEditorScrollPosition = Vector2.zero;
                            Event.current.Use();
                        }
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();

            if (SelectedObjectsByType[dataType])
            {
                _drawingEditorScrollPosition = EditorGUILayout.BeginScrollView(_drawingEditorScrollPosition);
                {
                    EditorGUILayout.Space(2f);
                    Editor.CreateCachedEditor(SelectedObjectsByType[dataType], null, ref _cachedEditor);
                    _cachedEditor.OnInspectorGUI();
                }
                EditorGUILayout.EndScrollView();
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    private List<string> GetFolders(Type dataType)
    {
        List<string> directories = new List<string>() { _noneDirectoryName };
        string path = FileHelper.GetCurrentDirectory() + $"\\Assets\\Resources\\SO\\{dataType.Name}";

        if (!FileHelper.FileExist(path))
        {
            FileHelper.DirectoryCreate(path);
        }
        
        System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(path);
        foreach (var dirInfo in dir.GetDirectories())
        {
            string fileDir = dirInfo.Name;
            directories.Add(fileDir);
        }
        
        return directories;
    }

    private string GetFolder(Type dataType, Object obj)
    {
        string path = AssetDatabase.GetAssetPath(obj);
        string typeName = $"{dataType.Name}/";
        int typeIndex = path.LastIndexOf(typeName, StringComparison.Ordinal) + typeName.Length;
        path = path.Substring(typeIndex, path.Length - typeIndex);
        int slashIndex = path.LastIndexOf('/');
        if (slashIndex < 0)
            return _noneDirectoryName;
        else
            return path.Substring(0, slashIndex);
    }

    private string GetCurrentFolderName(List<string> folders)
    {
        if (_currentDirectoryIndex < 0 || _currentDirectoryIndex >= folders.Count)
        {
            return _noneDirectoryName;
        }
        else
        {
            return folders[_currentDirectoryIndex];
        }
    }

    private void UpDirectoryIndex(List<string> folders)
    {
        _currentDirectoryIndex++;
        
        if (_currentDirectoryIndex < 0 || _currentDirectoryIndex >= folders.Count)
            _currentDirectoryIndex = folders.Count - 1;
    }
    private void DownDirectoryIndex(List<string> folders)
    {
        _currentDirectoryIndex--;
        
        if (_currentDirectoryIndex < 0 || _currentDirectoryIndex >= folders.Count)
            _currentDirectoryIndex = 0;
    }
}
