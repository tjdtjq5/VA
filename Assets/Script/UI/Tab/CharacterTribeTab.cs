using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTribeTab : UIFrame
{
    [SerializeField] Transform onTr;
    [SerializeField] GameObject onIconObj;
    [SerializeField] GameObject onTextObj;
	[SerializeField] Transform tabParentTr;
    public int Index { get; set; }
    public Action<int> TabHandler;
    List<UITabButton> tabs = new();

    float onTrWidth;
    float horizontalSpacingX = 0;
    float moveSpeed = 0.12f;
    Vector2 movePos = Vector2.zero;
    bool isMove = false;

    protected override void Initialize()
    {
		Bind<UITabButton>(typeof(UITabButtonE));
		Bind<UIImage>(typeof(UIImageE));

        base.Initialize();

        onTrWidth = onTr.GetComponent<RectTransform>().rect.width;

        TabInitialize();
        SelectInitialize();
    }
    void TabInitialize()
    {
        GetTabButton(UITabButtonE.BtnList_All).Set(0, false);
        GetTabButton(UITabButtonE.BtnList_Cat).Set(1, false);
        GetTabButton(UITabButtonE.BtnList_Dragon).Set(2, false);
        GetTabButton(UITabButtonE.BtnList_Druid).Set(3, false);
        GetTabButton(UITabButtonE.BtnList_Pirate).Set(4, false);
        GetTabButton(UITabButtonE.BtnList_Robot).Set(5, false);
        GetTabButton(UITabButtonE.BtnList_Thief).Set(6, false);

        GetTabButton(UITabButtonE.BtnList_All).SwitchOnHandler += UISet;
        GetTabButton(UITabButtonE.BtnList_Cat).SwitchOnHandler += UISet;
        GetTabButton(UITabButtonE.BtnList_Dragon).SwitchOnHandler += UISet;
        GetTabButton(UITabButtonE.BtnList_Druid).SwitchOnHandler += UISet;
        GetTabButton(UITabButtonE.BtnList_Pirate).SwitchOnHandler += UISet;
        GetTabButton(UITabButtonE.BtnList_Robot).SwitchOnHandler += UISet;
        GetTabButton(UITabButtonE.BtnList_Thief).SwitchOnHandler += UISet;

        tabs.Add(GetTabButton(UITabButtonE.BtnList_All));
        tabs.Add(GetTabButton(UITabButtonE.BtnList_Cat));
        tabs.Add(GetTabButton(UITabButtonE.BtnList_Dragon));
        tabs.Add(GetTabButton(UITabButtonE.BtnList_Druid));
        tabs.Add(GetTabButton(UITabButtonE.BtnList_Pirate));
        tabs.Add(GetTabButton(UITabButtonE.BtnList_Robot));
        tabs.Add(GetTabButton(UITabButtonE.BtnList_Thief));
    }
    void SelectInitialize()
    {
        Index = 0;
        onTr.position = new Vector2(tabParentTr.GetChild(Index).position.x + onTrWidth / 2, onTr.position.y);
        SelectUISet();
    }

    public void UISet(int index)
    {
        if (Index.Equals(index))
            return;

        Index = index;

        for (int i = 0; i < tabs.Count; i++)
        {
            if (tabs[i].Index != index)
                tabs[i].Switch(false);
        }
        for (int i = 0; i < tabs.Count; i++)
        {
            if (tabs[i].Index == index)
                tabs[i].Switch(true);
        }

        SelectUISet();
        SelectMove();

        if (TabHandler != null)
            TabHandler.Invoke(index);
    }
    void SelectMove()
    {
        movePos = new Vector2(tabParentTr.GetChild(Index).position.x + (Index == 0 ? 0 : horizontalSpacingX), onTr.position.y);
        isMove = true;
    }
    void SelectUISet()
    {
        if (Index > 0)
        {
            onIconObj.SetActive(true);
            onTextObj.SetActive(false);
            Tribe tribe = (Tribe)(Index - 1);
            GetImage(UIImageE.TabOn_Bg_Icon).sprite = Managers.Atlas.GetTribe(tribe);
            GetImage(UIImageE.TabOn_Bg_Icon).SetNativeSize();
            GetImage(UIImageE.TabOn_Bg_Icon).color = DefineColor.GetTribe(tribe);
        }
        else
        {
            // all
            onIconObj.SetActive(false);
            onTextObj.SetActive(true);
        }
    }
    private void FixedUpdate()
    {
        if (isMove)
        {
            onTr.position = Vector2.Lerp(onTr.position, movePos, moveSpeed);

            if (onTr.position.GetDistance(movePos) <= 0.1f)
                isMove = true;
        }
    }

    public enum UITabButtonE
    {
		BtnList_All,
		BtnList_Cat,
		BtnList_Dragon,
		BtnList_Druid,
		BtnList_Pirate,
		BtnList_Robot,
		BtnList_Thief,
    }
	public enum UIImageE
    {
		TabOn,
		TabOn_Bg,
		TabOn_Bg_Icon,
    }
}