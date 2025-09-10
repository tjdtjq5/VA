using Shared.Enums;
using Unity.VisualScripting;

public class ResearchBGColor : UIFrame
{
    private UIImage _bgImg;

    private readonly string _redBGColor = "412727FF";
    private readonly string _redFlowColor = "372323FF";
    private readonly string _blueBGColor = "283243FF";
    private readonly string _blueFlowColor = "202937FF";
    private readonly string _greenBGColor = "263C2CFF";
    private readonly string _greenFlowColor = "253525FF";
    private readonly string _yellowBGColor = "433A2AFF";
    private readonly string _yellowFlowColor = "3A3325FF";
    private readonly string _masterBGColor = "FFFFFFFF";
    private readonly string _masterFlowColor = "FFFFFFFF";

    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));

        _bgImg = this.GetOrAddComponent<UIImage>();

        base.Initialize();
    }

    public void UISet(PlayerGrowResearch growResearch)
    {
        switch (growResearch)
        {
            case PlayerGrowResearch.RedBook:
                _bgImg.SetColor(_redBGColor);
                GetImage(UIImageE.Flow1).SetColor(_redFlowColor);
                GetImage(UIImageE.Flow2).SetColor(_redFlowColor);
                GetImage(UIImageE.Flow3).SetColor(_redFlowColor);
                break;
            case PlayerGrowResearch.BlueBook:
                _bgImg.SetColor(_blueBGColor);
                GetImage(UIImageE.Flow1).SetColor(_blueFlowColor);
                GetImage(UIImageE.Flow2).SetColor(_blueFlowColor);
                GetImage(UIImageE.Flow3).SetColor(_blueFlowColor);
                break;
            case PlayerGrowResearch.GreenBook:
                _bgImg.SetColor(_greenBGColor);
                GetImage(UIImageE.Flow1).SetColor(_greenFlowColor);
                GetImage(UIImageE.Flow2).SetColor(_greenFlowColor);
                GetImage(UIImageE.Flow3).SetColor(_greenFlowColor);
                break;
            case PlayerGrowResearch.YellowBook:
                _bgImg.SetColor(_yellowBGColor);
                GetImage(UIImageE.Flow1).SetColor(_yellowFlowColor);
                GetImage(UIImageE.Flow2).SetColor(_yellowFlowColor);
                GetImage(UIImageE.Flow3).SetColor(_yellowFlowColor);
                break;
            case PlayerGrowResearch.MasterBook:
                _bgImg.SetColor(_masterBGColor);
                GetImage(UIImageE.Flow1).SetColor(_masterFlowColor);
                GetImage(UIImageE.Flow2).SetColor(_masterFlowColor);
                GetImage(UIImageE.Flow3).SetColor(_masterFlowColor);
                break;
        }
    }

	public enum UIImageE
    {
		Flow1,
		Flow2,
		Flow3,
    }
}