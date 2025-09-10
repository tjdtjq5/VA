using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance; // 유일성이 보장된다
    public static Managers Instance { get { Init(); return s_instance; } } // 유일한 매니저를 갖고온다
    private static bool _isInitialized = false;

    WebManager _web = new WebManager();
    ObscuredManager _obscured = new ObscuredManager();
    TweenManager _tween = new TweenManager();
    SceneManagerEx _scene = new SceneManagerEx();
    InputManager _input = new InputManager();
    SoundManager _sound = new SoundManager();
    ResourcesManager _resources = new ResourcesManager();
    PoolManager _pool = new PoolManager();
    UIManager _ui = new UIManager();
    TimeManager _time = new TimeManager();
    SseManager _sse = new SseManager();
    ChatManager _chat = new ChatManager();
    TableManager _table = new TableManager();
    ObserverManager _observer = new ObserverManager();
    PlayerDataManager _playerData = new PlayerDataManager();
    AtlasManager _atlas = new AtlasManager();
    SOManager _so = new SOManager();
    ScriptManager _script = new ScriptManager();
    FloatingTextManager _floatingText = new FloatingTextManager();
    RandomManager _random = new RandomManager();
    LanguageManager _language = new LanguageManager();
    NodeTreeManager _nodeTree = new NodeTreeManager();

    public static WebManager Web { get { return Instance._web; } }
    public static ObscuredManager Obscured { get { return Instance._obscured; } }
    public static TweenManager Tween { get { return Instance._tween; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static InputManager Input { get { return Instance._input; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static ResourcesManager Resources { get { return Instance._resources; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static UIManager UI { get { return Instance._ui; } }
    public static TimeManager Time { get { return Instance._time; } }
    public static SseManager Sse { get { return Instance._sse; } }
    public static ChatManager Chat { get { return Instance._chat; } }
    public static TableManager Table { get { return Instance._table; } }
    public static ObserverManager Observer { get { return Instance._observer; } }
    public static PlayerDataManager PlayerData { get { return Instance._playerData; } }
    public static AtlasManager Atlas { get { return Instance._atlas; } }
    public static SOManager SO { get { return Instance._so; } }
    public static ScriptManager Script { get { return Instance._script; } }
    public static FloatingTextManager FloatingText { get { return Instance._floatingText; } }
    public static RandomManager Random { get { return Instance._random; } }
    public static LanguageManager Language { get { return Instance._language; } }
    public static NodeTreeManager NodeTree { get { return Instance._nodeTree; } }
    void Start()
    {
        Init();
        _time.ChangeFrameRate(60);

        #if UNITY_EDITOR
        _input.AddKeyDownAction(KeyCode.C, () =>
        {
            _observer.Player.ChangeForm(_observer.Player.CurrentForm == PuzzleType.None ? PuzzleType.Red : PuzzleType.None);
            _observer.Player.CharacterAttackReady.CurrentGrade = AttackGrade.Basic;
        });
        _input.AddKeyDownAction(KeyCode.F5, () =>
        {
            UnityEditor.EditorApplication.isPaused = true;
        });
        #endif
    }
    static void Init()
    {
        if (!_isInitialized)
        {
            _isInitialized = true;
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();

            s_instance._sound.Initialize();
            s_instance._pool.Initialize();
            s_instance._time.Initialize();
            s_instance._chat.Initialize();
            s_instance._web.Initialize();
            s_instance._playerData.Initialize();
            s_instance._table.Initialize();
            s_instance._atlas.Initialize();
            s_instance._obscured.Initialize();
            s_instance._language.Initialize();
        }
    }

    private void Update()
    {
        _input.OnUpdate();
        _tween.Update();    
    }
    private void FixedUpdate()
    {
        _time.OnFixedUpdate();
        _chat.OnFixedUpdate();
    }

    public void Clean()
    {
        if (_scene.CurrentScene)
            _scene.CurrentScene.Clear();

        _resources.Clear();
        _sound.Clear();
        _input.Clear();
        _ui.Clear();
        _pool.Clear();
        _sse.Clear();
        _chat.Clear();
        _observer.Clear();
    }
    private void OnDisable()
    {
        Clean();
    }

    private void OnDestroy()
    {
        GameObject go = GameObject.Find("@Managers");
        if (go != null)
            Destroy(go);
    }
}
