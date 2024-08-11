using System.Collections;
using System.Collections.Generic;
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
    ProcessDeepLinkManager _deepLink = new ProcessDeepLinkManager();
    TimeManager _time = new TimeManager();
    TweenManager _tween = new TweenManager();
    SseManager _sse = new SseManager();
    ChatManager _chat = new ChatManager();

    public static WebManager Web { get { return Instance._web; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static InputManager Input { get { return Instance._input; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static ResourcesManager Resources { get { return Instance._resources; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static UIManager UI { get { return Instance._ui; } }
    public static ProcessDeepLinkManager DeepLink { get { return Instance._deepLink; } }
    public static TimeManager Time { get { return Instance._time; } }
    public static TweenManager Tween { get { return Instance._tween; } }
    public static SseManager Sse { get { return Instance._sse; } }
    public static ChatManager Chat { get { return Instance._chat; } }

    void Start()
    {
        Init();
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
        }
    }

    private void Update()
    {
        _input.OnUpdate();
    }
    private void FixedUpdate()
    {
        _time.OnFixedUpdate();
        _tween.OnFixedUpdate();
        _chat.OnFixedUpdate();
    }
}
