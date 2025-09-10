using Shared.Enums;

public class RobbyGrowResearchBook : UIRobby
{
    protected override void Initialize()
    {
		Bind<ResearchBGColor>(typeof(ResearchBGColorE));
		Bind<UIImage>(typeof(UIImageE));
		Bind<ResearchBookParticle>(typeof(ResearchBookParticleE));
		Bind<ResearchBook>(typeof(ResearchBookE));

        base.Initialize();
    }

    private UIRobbyGrowResearch _growResearch;
    private PlayerGrowResearch _selectBookType;
    private PlayerGrowResearch[] _underBookTypes = new PlayerGrowResearch[3];

    private readonly string _robbyGrowResearchPrefabPath = "Robby/Research/RobbyGrowResearch";
    private readonly string _researchTreePrefabPath = "Robby/Research/RobbyGrowResearchTree";

    private readonly string _redLight1Color = "FF020031";
    private readonly string _redLight2Color = "FF000531";
    private readonly string _blueLight1Color = "0091FF31";
    private readonly string _blueLight2Color = "0087FF31";
    private readonly string _greenLight1Color = "24FF0031";
    private readonly string _greenLight2Color = "5DFF0031";
    private readonly string _yellowLight1Color = "FFD50031";
    private readonly string _yellowLight2Color = "FFC40031";
    private readonly string _masterLight1Color = "FFFFFF31";
    private readonly string _masterLight2Color = "FFFFFF31";


    protected override void UISet()
    {
        base.UISet();

        Get<ResearchBook>(ResearchBookE.SafeArea_SelectBook_SelectBook).AddClickAniEvent((ped) => OnClickSelectBook());
        Get<ResearchBook>(ResearchBookE.SafeArea_UnderBooks_LeftBook).AddClickAniEvent((ped) => OnClickUnderBook(0));
        Get<ResearchBook>(ResearchBookE.SafeArea_UnderBooks_MiddleBook).AddClickAniEvent((ped) => OnClickUnderBook(1));
        Get<ResearchBook>(ResearchBookE.SafeArea_UnderBooks_RightBook).AddClickAniEvent((ped) => OnClickUnderBook(2));

        InitSet();
    }

    public void UISet(UIRobbyGrowResearch growResearch)
    {
        _growResearch = growResearch;
    }

    private void InitSet()
    {
        _selectBookType = PlayerGrowResearch.RedBook;
        _underBookTypes[0] = PlayerGrowResearch.BlueBook;
        _underBookTypes[1] = PlayerGrowResearch.GreenBook;
        _underBookTypes[2] = PlayerGrowResearch.YellowBook;

        BookSet();
        SetColor(_selectBookType);
    }

    private void BookSet()
    {
        Get<ResearchBook>(ResearchBookE.SafeArea_SelectBook_SelectBook).UISet(_selectBookType, true);
        Get<ResearchBook>(ResearchBookE.SafeArea_UnderBooks_LeftBook).UISet(_underBookTypes[0], false);
        Get<ResearchBook>(ResearchBookE.SafeArea_UnderBooks_MiddleBook).UISet(_underBookTypes[1], false);
        Get<ResearchBook>(ResearchBookE.SafeArea_UnderBooks_RightBook).UISet(_underBookTypes[2], false);
    }

    private void SetColor(PlayerGrowResearch type)
    {
        Get<ResearchBGColor>(ResearchBGColorE.BG).UISet(type);

        switch (type)
        {
            case PlayerGrowResearch.RedBook:
                GetImage(UIImageE.SafeArea_SelectBook_Light1).SetColor(_redLight1Color);
                GetImage(UIImageE.SafeArea_SelectBook_Light2).SetColor(_redLight2Color);
                Get<ResearchBookParticle>(ResearchBookParticleE.SafeArea_SelectBook_Particle).UISet(PlayerGrowResearch.RedBook);
                break;
            case PlayerGrowResearch.BlueBook:
                GetImage(UIImageE.SafeArea_SelectBook_Light1).SetColor(_blueLight1Color);
                GetImage(UIImageE.SafeArea_SelectBook_Light2).SetColor(_blueLight2Color);
                Get<ResearchBookParticle>(ResearchBookParticleE.SafeArea_SelectBook_Particle).UISet(PlayerGrowResearch.BlueBook);
                break;
            case PlayerGrowResearch.GreenBook:
                GetImage(UIImageE.SafeArea_SelectBook_Light1).SetColor(_greenLight1Color);
                GetImage(UIImageE.SafeArea_SelectBook_Light2).SetColor(_greenLight2Color);
                Get<ResearchBookParticle>(ResearchBookParticleE.SafeArea_SelectBook_Particle).UISet(PlayerGrowResearch.GreenBook);
                break;
            case PlayerGrowResearch.YellowBook:
                GetImage(UIImageE.SafeArea_SelectBook_Light1).SetColor(_yellowLight1Color);
                GetImage(UIImageE.SafeArea_SelectBook_Light2).SetColor(_yellowLight2Color);
                Get<ResearchBookParticle>(ResearchBookParticleE.SafeArea_SelectBook_Particle).UISet(PlayerGrowResearch.YellowBook);
                break;
            case PlayerGrowResearch.MasterBook:
                GetImage(UIImageE.SafeArea_SelectBook_Light1).SetColor(_masterLight1Color);
                GetImage(UIImageE.SafeArea_SelectBook_Light2).SetColor(_masterLight2Color);
                Get<ResearchBookParticle>(ResearchBookParticleE.SafeArea_SelectBook_Particle).UISet(PlayerGrowResearch.MasterBook);
                break;
        }
    }

    private void OnClickUnderBook(int underIndex)
    {
        PlayerGrowResearch selectType = _selectBookType;
        _selectBookType = _underBookTypes[underIndex];
        _underBookTypes[underIndex] = selectType;

        BookSet();
        SetColor(_selectBookType);
    }

    private void OnClickSelectBook()
    {
        RobbyGrowResearchTree tree = Managers.Observer.RobbyManager.ShopUI<RobbyGrowResearchTree>(_researchTreePrefabPath, _robbyGrowResearchPrefabPath);
        tree.UISet(_growResearch, _selectBookType);
    }

	public enum ResearchBGColorE
    {
		BG,
    }
	public enum UIImageE
    {
		SafeArea_SelectBook_Light1,
		SafeArea_SelectBook_Light2,
    }
	public enum ResearchBookParticleE
    {
		SafeArea_SelectBook_Particle,
    }
	public enum ResearchBookE
    {
		SafeArea_SelectBook_SelectBook,
		SafeArea_UnderBooks_LeftBook,
		SafeArea_UnderBooks_MiddleBook,
		SafeArea_UnderBooks_RightBook,
    }
}