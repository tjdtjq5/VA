using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameStart : AniPlay
{
    protected override void Initialize()
    {
      Bind<UIImage>(typeof(UIImageE));
      Bind<UIText>(typeof(UITextE));

        base.Initialize();
    }
    public void Initialize(DungeonTree _dungeonTree)
    {
        // GetText(UITextE.ChapterName_Chapter).text = _dungeonTree.DisplayName;
        GetText(UITextE.Main_Text).text = _dungeonTree.Description;
    }
	public enum UIImageE
    {
		Main,
    }
	public enum UITextE
    {
		Main_Text,
    }
}