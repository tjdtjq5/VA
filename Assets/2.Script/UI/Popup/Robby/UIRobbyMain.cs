using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRobbyMain : UIRobby
{
    protected override void Initialize()
    {
		Bind<UseItemButton>(typeof(UseItemButtonE));
		Bind<UIImage>(typeof(UIImageE));
		Bind<UITextPro>(typeof(UITextProE));
		Bind<UIButton>(typeof(UIButtonE));

        Get<UseItemButton>(UseItemButtonE.SafeArea_GameStartButton).AddClickEvent((ped) => OnClickGameStart());

        base.Initialize();
    }

    private void OnClickGameStart()
    {
        Managers.Scene.LoadScene(SceneType.InGame);
    }

	public enum UseItemButtonE
    {
		SafeArea_GameStartButton,
    }
	public enum UIImageE
    {
		SafeArea_GuildButton_Icon,
		SafeArea_EventButton_Icon,
    }
	public enum UITextProE
    {
		SafeArea_GuildButton_Text,
		SafeArea_EventButton_Text,
    }
	public enum UIButtonE
    {
		SafeArea_MirrorButton,
		SafeArea_RewardButton,
    }
}