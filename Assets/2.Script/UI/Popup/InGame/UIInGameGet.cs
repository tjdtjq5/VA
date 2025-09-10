public class UIInGameGet : UIPopup
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIText>(typeof(UITextE));
		Bind<InGameRewardCard>(typeof(InGameRewardCardE));
		Bind<UIButton>(typeof(UIButtonE));

		GetButton(UIButtonE.Main_OkBtn).AddClickEvent((ped) => OnClickOK());

        base.Initialize();
    }

    public void UISetSkillOrBuff(IdentifiedObject skillOrBuff)
    {
	    Get<InGameRewardCard>(InGameRewardCardE.Main_InGameRewardCard).Setting(skillOrBuff);
    }
    public void UISetGesso(int gesso)
    {
	    Get<InGameRewardCard>(InGameRewardCardE.Main_InGameRewardCard).Setting("Gesso", gesso, true, false, 1, 650f);
    }
    public void UISetNesso(int nesso)
    {
	    Get<InGameRewardCard>(InGameRewardCardE.Main_InGameRewardCard).Setting("Nesso", nesso, true, false, 1, 650f);
    }
    void OnClickOK()
    {
	    ClosePopupUIPlayAni();
    }
    
	public enum UIImageE
    {
		BlackPannel,
		Main_Title,
    }
	public enum UITextE
    {
		Main_Title_Text,
    }
	public enum InGameRewardCardE
    {
		Main_InGameRewardCard,
    }
	public enum UIButtonE
    {
		Main_OkBtn,
    }
}