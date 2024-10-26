using EasyButtons;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterDetailInfo : UIPopup
{
    protected override void Initialize()
    {
		Bind<UIScrollView>(typeof(UIScrollViewE));

        base.Initialize();
    }
    [SerializeField]
    private List<Stat> _stats = new();

    public void UISet(string characterCode)
    {
        //List<Stat> stats = Managers.PlayerData.GetStats();

        List<CharacterDetailInfoCardData> datas = new List<CharacterDetailInfoCardData>();

        for (int i = 0; i < _stats.Count; i++) 
        {
            Stat stat = _stats[i];
            datas.Add(new CharacterDetailInfoCardData() { stat = stat });
        }

        GetScrollView(UIScrollViewE.Main_ScrollView).UISet(UIScrollViewLayoutStartAxis.Vertical, "CharacterDetailInfoCard", new List<ICardData>(datas), 0, 1, UIScrollViewLayoutStartCorner.Middle, 0, 10);
    }

#if UNITY_EDITOR
    [Button]
    public void LoadStats()
    {
        var stats = Resources.LoadAll<Stat>("Stat").OrderBy(x => x.ID);
        _stats = stats.ToList();
    }
#endif
	public enum UIScrollViewE
    {
		Main_ScrollView,
    }
}