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

    public static WebManager Web { get { return Instance._web; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static InputManager Input { get { return Instance._input; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static ResourcesManager Resources { get { return Instance._resources; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static UIManager UI { get { return Instance._ui; } }
    public static ProcessDeepLinkManager DeepLink { get { return Instance._deepLink; } }

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
        }
    }

    private void Update()
    {
        _input.OnUpdate();
    }
}
