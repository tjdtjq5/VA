using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static UnityEngine.Rendering.DebugUI;

public class MainCharacterViewPopup : UIPopup
{
	protected override void Initialize()
	{
		base.Initialize();
		Bind<CharacterHaveCheck>(typeof(CharacterHaveCheckE));
		Bind<UIImage>(typeof(UIImageE));
		Bind<UITabButtonParent>(typeof(UITabButtonParentE));
		Bind<UIScrollView>(typeof(UIScrollViewE));
		Bind<UIButton>(typeof(UIButtonE));

		GetTabButtonParent(UITabButtonParentE.CharacterTribeTab).SwitchOnHandler += TribeTabAction;
		Get<CharacterHaveCheck>(CharacterHaveCheckE.CharacterHaveCheck).CheckHandler += HaveCheckAction;
    }

    protected override void UISet()
    {
        base.UISet();

		Get<CharacterHaveCheck>(CharacterHaveCheckE.CharacterHaveCheck).UISet(false);
		GetTabButtonParent(UITabButtonParentE.CharacterTribeTab).UISet(0);
    }

	void HaveCheckAction(bool flag)
	{
		int tabIndex = GetTabButtonParent(UITabButtonParentE.CharacterTribeTab).Index;
        bool isAll = tabIndex == 0;
        Tribe tribe = (Tribe)CSharpHelper.EnumClamp<Tribe>(tabIndex - 1);
        ScrollSet(flag, isAll, tribe);
    }

	void TribeTabAction(int index)
	{
		bool isHaveCheck = Get<CharacterHaveCheck>(CharacterHaveCheckE.CharacterHaveCheck).IsChecked;
		bool isAll = index == 0;
		Tribe tribe = (Tribe)CSharpHelper.EnumClamp<Tribe>(index - 1);
		ScrollSet(isHaveCheck, isAll, tribe);
    }

	void ScrollSet(bool isHave, bool isAll, Tribe tribe)
	{
		List<CharacterCardData> datas = new List<CharacterCardData>();

	 	var tableDatas = Managers.Table.CharacterTable.Gets().ToList();

        if (!isAll)
            tableDatas = tableDatas.FindAll(t => t.tribeType.Equals((int)tribe));

		var playerDatas = Managers.PlayerData.Character.Gets();

		if (isHave)
		{
			List<CharacterTableData> tempTableDatas = new List<CharacterTableData>();
			for (int i = 0; i < playerDatas.Count; i++) 
			{
				CharacterPlayerData cpd = playerDatas[i];
				CharacterTableData ttd = tableDatas.Find(t => t.characterCode.Equals(cpd.Code));
				if (ttd != null)
                    tempTableDatas.Add(ttd);
            }
            tableDatas.Clear();
            tableDatas.AddRange(tempTableDatas);
        }

        for (int i = 0; i < tableDatas.Count; i++) 
		{
            CharacterCardData data = new CharacterCardData();

            CharacterTableData tData = tableDatas[i];

            data.tableData = tData;
			data.playerData = playerDatas.Find(p => p.Code.Equals(tData.characterCode));
            data.soData = Managers.Table.CharacterTable.GetTableSO(tData.characterCode);

            datas.Add(data);
        }

        GetScrollView(UIScrollViewE.CharacterViews_ScrollView).UISet( UIScrollViewLayoutStartAxis.Vertical, "CharacterCard", new List<ICardData>(datas), 0, 4, UIScrollViewLayoutStartCorner.Middle, 3.5f, 10);
    }

    public enum CharacterHaveCheckE
    {
		CharacterHaveCheck,
    }
	public enum UIImageE
    {
		LineTop,
    }
	public enum UITabButtonParentE
    {
		CharacterTribeTab,
    }
	public enum UIScrollViewE
    {
		CharacterViews_ScrollView,
    }
	public enum UIButtonE
    {
		CharacterViews_Under_Filter,
		CharacterViews_Under_Order,
		CharacterViews_Under_AllAwakeBtn,
    }
}
