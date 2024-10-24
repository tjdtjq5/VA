using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance; // 유일성이 보장된다
    public static Managers Instance { get { Init(); return s_instance; } } // 유일한 매니저를 갖고온다

    WebManager _web = new WebManager();
    SceneManagerEx _scene = new SceneManagerEx();
    InputManager _input = new InputManager();
    SoundManager _sound = new SoundManager();
    ResourcesManager _resources = new ResourcesManager();
    PoolManager _pool = new PoolManager();
    UIManager _ui = new UIManager();
    TimeManager _time = new TimeManager();
    SseManager _sse = new SseManager();
    ChatManager _chat = new ChatManager();
    FloatingTextManager _floatingText = new FloatingTextManager();
    TableManager _table = new TableManager();
    ObserverManager _observer = new ObserverManager();
    PlayerDataManager _playerData = new PlayerDataManager();
    AtlasManager _atlas = new AtlasManager();
    SOManager _so = new SOManager();

    public static WebManager Web { get { return Instance._web; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static InputManager Input { get { return Instance._input; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static ResourcesManager Resources { get { return Instance._resources; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static UIManager UI { get { return Instance._ui; } }
    public static TimeManager Time { get { return Instance._time; } }
    public static SseManager Sse { get { return Instance._sse; } }
    public static ChatManager Chat { get { return Instance._chat; } }
    public static FloatingTextManager FloatingText { get { return Instance._floatingText; } }
    public static TableManager Table { get { return Instance._table; } }
    public static ObserverManager Observer { get { return Instance._observer; } }
    public static PlayerDataManager PlayerData { get { return Instance._playerData; } }
    public static AtlasManager Atlas { get { return Instance._atlas; } }
    public static SOManager SO { get { return Instance._so; } }

    void Start()
    {
        Init();

        Application.targetFrameRate = 60;
    }
    static void Init()
    {
        if (s_instance == null)
        {
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
        }
    }

    private void Update()
    {
        _input.OnUpdate();
    }
    private void FixedUpdate()
    {
        _time.OnFixedUpdate();
        _chat.OnFixedUpdate();
        _floatingText.OnFixedUpdate();
    }

    public void Clean()
    {
        if (_scene.CurrentScene != null)
            _scene.CurrentScene.Clear();

        _resources.Clear();
        _sound.Clear();
        _input.Clear();
        _ui.Clear();
        _pool.Clear();
        _sse.Clear();
        _chat.Clear();
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
