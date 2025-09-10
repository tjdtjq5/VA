using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    SceneBase _currentScene;
    InGameManager _inGameManager;

    public SceneBase CurrentScene
    {
        get
        {
            if (_currentScene == null)
                _currentScene = GameObject.FindObjectOfType<SceneBase>();
            return _currentScene;
        }
    }
    public InGameManager InGameManager
    {
        get
        {
            if (_inGameManager == null)
                _inGameManager = GameObject.FindObjectOfType<InGameManager>();
            return _inGameManager;
        }
    }
    public void LoadScene(SceneType sceneType)
    {
        SceneClear();
        SceneManager.LoadScene(sceneType.ToString());
    }
    void SceneClear()
    {
        _currentScene = null;
        _inGameManager = null;

        Managers.Instance.Clean();
    }
}
