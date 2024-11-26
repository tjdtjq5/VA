using EasyButtons;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UITabSlider : UIFrame 
{
    [SerializeField] private List<string> TabNames = new();
    public int Index {  get; set; }
    public Action<int> TabHandler;

    private Transform selectTr;
    private Transform tabParentTr;

    string tabBtnPrefabName = "Prefab/UI/Tab/TabSliderBtn";
    string selectTrName = "Select";
    string tabParentTrName = "TabList";

    float tabWidth;
    float moveSpeed = 0.25f;

    Vector2 movePos = Vector2.zero;
    bool isMove = false;

    protected override void Initialize()
	{
		base.Initialize();
		Bind<UIText>(typeof(UITextE));

        isMove = false;

        selectTr = this.gameObject.FindChild<Transform>(selectTrName);
        tabParentTr = this.gameObject.FindChild<Transform>(tabParentTrName);

        float width = RectTransform.rect.width;
        RectTransform selectRect = selectTr.GetComponent<RectTransform>();
        selectRect.sizeDelta = new Vector2(width / TabNames.Count, selectRect.sizeDelta.y);

        TabInitialize();
        SelectInitialize();
    }

    void TabInitialize()
    {
        float tabParentTrWidth = tabParentTr.GetComponent<RectTransform>().rect.width;
        tabWidth = tabParentTrWidth / TabNames.Count;
        float tw = tabParentTrWidth / TabNames.Count;

        for (int i = 0; i < TabNames.Count; i++) 
        {
            TabSliderBtn tabBtn = Managers.Resources.Instantiate<TabSliderBtn>(tabBtnPrefabName, tabParentTr);
            tabBtn.Set(TabNames[i], tw);
            int index = i;
            tabBtn.AddClickEvent((ped) => { UISet(index); });
        }
    }
    void SelectInitialize()
    {
        Index = 0;
        selectTr.position = new Vector2(tabParentTr.GetChild(Index).position.x + tabWidth / 2, selectTr.position.y);
        SelectTextSet();
    }
    
    protected override void UISet()
    {
        base.UISet();
    }

    public void UISet(int index)
    {
        if (Index.Equals(index))
            return;

        Index = index;
        SelectTextSet();
        SelectMove();

        if (TabHandler != null)
            TabHandler.Invoke(index);
    }
    void SelectMove()
    {
        movePos = new Vector2(tabParentTr.GetChild(Index).position.x , selectTr.position.y);
        isMove = true;
    }
    void SelectTextSet()
    {
        GetText(UITextE.Select_Text).text = TabNames[Index];
    }
    private void FixedUpdate()
    {
        if (isMove)
        {
            selectTr.position = Vector2.Lerp(selectTr.position, movePos, moveSpeed);

            if (selectTr.position.GetDistance(movePos) <= 0.1f)
                isMove = true;
        }
    }

    [Button]
    public void InspecterSetting()
    {
        selectTr = this.gameObject.FindChild<Transform>(selectTrName);
        tabParentTr = this.gameObject.FindChild<Transform>(tabParentTrName);

        float width = RectTransform.rect.width;
        RectTransform selectRect = selectTr.GetComponent<RectTransform>();
        selectRect.sizeDelta = new Vector2(width / TabNames.Count, selectRect.sizeDelta.y);
    }

    public enum UITextE
    {
		Select_Text,
    }
}
