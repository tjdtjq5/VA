using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public SceneBase CurrentScene
    {
        get
        {
            return GameObject.FindObjectOfType<SceneBase>();
        }
    }
    public void LoadScene(SceneType sceneType)
    {
        SceneClear();
        SceneManager.LoadScene(GetSceneName(sceneType));
    }
    string GetSceneName(SceneType sceneType)
    {
        switch (sceneType)
        {
            case SceneType.Title:
                return "Title";
            case SceneType.InGame:
                return "InGame";
            case SceneType.Dungeon:
                return "Dungeon";
            case SceneType.Life:
                return "Life";
            case SceneType.Guild:
                return "Guild";
            default:
                return "";
        }
    }
    void SceneClear()
    {
        CurrentScene.Clear();

        Managers.Resources.Clear();
        Managers.Sound.Clear();
        Managers.Input.Clear();
        Managers.UI.Clear();
        Managers.Pool.Clear();
        Managers.Sse.Clear();
        Managers.Chat.Clear();
    }
}
