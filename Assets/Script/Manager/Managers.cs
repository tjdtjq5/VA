using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance; // 유일성이 보장된다
    public static Managers Instance { get { Init(); return s_instance; } } // 유일한 매니저를 갖고온다

    WebManager _web = new WebManager();
    InputManager _input = new InputManager();
    ResourcesManager _resources = new ResourcesManager();
    UIManager _ui = new UIManager();

    public static WebManager Web { get { return Instance._web; } }
    public static InputManager Input { get { return Instance._input; } }
    public static ResourcesManager Resources { get { return Instance._resources; } }
    public static UIManager UI { get { return Instance._ui; } }

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

        }
    }

    private void Update()
    {
        _input.OnUpdate();
    }
}
