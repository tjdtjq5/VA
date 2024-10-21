using System;
using System.Collections.Generic;
using System.Linq;

public class MainCharacterViewPopup : UIPopup
{
	protected override void Initialize()
	{
		Bind<CharacterHaveCheck>(typeof(CharacterHaveCheckE));
		Bind<UIImage>(typeof(UIImageE));
		Bind<CharacterTribeTab>(typeof(CharacterTribeTabE));
		Bind<UIScrollView>(typeof(UIScrollViewE));
		Bind<CharacterFilterBtn>(typeof(CharacterFilterBtnE));
		Bind<CharacterOrderTabBtn>(typeof(CharacterOrderTabBtnE));
		Bind<UIButton>(typeof(UIButtonE));

        base.Initialize();


        Get<CharacterTribeTab>(CharacterTribeTabE.CharacterViews_CharacterTribeTab).TabHandler += TribeTabAction;
        Get<CharacterHaveCheck>(CharacterHaveCheckE.CharacterHaveCheck).CheckHandler += HaveCheckAction;
        Get<CharacterFilterBtn>(CharacterFilterBtnE.CharacterViews_Under_Filter).FilterHandler += FilterAction;
		Get<CharacterOrderTabBtn>(CharacterOrderTabBtnE.CharacterViews_Under_Order).SwitchHandler += OrderAction;
    }

    protected override void UISet()
    {
        base.UISet();

		Get<CharacterHaveCheck>(CharacterHaveCheckE.CharacterHaveCheck).UISet(false);
        Get<CharacterTribeTab>(CharacterTribeTabE.CharacterViews_CharacterTribeTab).UISet(0);
    }

	void HaveCheckAction(bool flag)
	{
        ActionControll();
    }

	void TribeTabAction(int index)
	{
        ActionControll();
    }

	void FilterAction(CharacterFilterType type)
	{
        ActionControll();
    }

	void OrderAction(bool flag)
	{
        ActionControll();
    }

    void ActionControll()
    {
        CharacterFilterType filterType = Get<CharacterFilterBtn>(CharacterFilterBtnE.CharacterViews_Under_Filter).FilterType;
        bool isHaveCheck = Get<CharacterHaveCheck>(CharacterHaveCheckE.CharacterHaveCheck).IsChecked;
        int tabIndex = Get<CharacterTribeTab>(CharacterTribeTabE.CharacterViews_CharacterTribeTab).Index;
        bool isAll = tabIndex == 0;
        Tribe tribe = (Tribe)CSharpHelper.EnumClamp<Tribe>(tabIndex - 1);
        bool isOrder = Get<CharacterOrderTabBtn>(CharacterOrderTabBtnE.CharacterViews_Under_Order).IsSwitch;
        ScrollSet(isHaveCheck, isAll, tribe, filterType, isOrder);
    }

	void ScrollSet(bool isHave, bool isAll, Tribe tribe, CharacterFilterType filterType, bool isOrder)
	{
		List<CharacterCardData> datas = new List<CharacterCardData>();

	 	var tableDatas = Managers.Table.CharacterTable.Gets().ToList();
        tableDatas = tableDatas.OrderBy(t => t.grade).ToList();

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

        datas = datas.OrderBy(d => d.tableData.grade).ToList();

        switch (filterType)
		{
			case CharacterFilterType.Level:
                datas = datas.OrderBy(d => (d.playerData == null) ? 999999 : d.playerData.Level).ToList();
                break;
			case CharacterFilterType.Awake:
                datas = datas.OrderBy(d => (d.playerData == null) ? 999999 : d.playerData.Awake).ToList();
                break;
		}

		if (isOrder)
            datas.Reverse();

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
	public enum CharacterTribeTabE
    {
		CharacterViews_CharacterTribeTab,
    }
	public enum UIScrollViewE
    {
		CharacterViews_ScrollView,
    }
	public enum CharacterFilterBtnE
    {
		CharacterViews_Under_Filter,
    }
	public enum CharacterOrderTabBtnE
    {
		CharacterViews_Under_Order,
    }
	public enum UIButtonE
    {
		CharacterViews_Under_AllAwakeBtn,
    }
}