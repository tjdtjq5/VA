using System;
using AssetKits.ParticleImage;
using UnityEngine;

public class Week : UIFrame
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));

        base.Initialize();
    }
    
	public bool IsCrash { get; private set; } = false;

    [SerializeField] private Color weekRed;
    [SerializeField] private Color weekBlue;
    [SerializeField] private Color weekGreen;
    [SerializeField] private Color weekYellow;
	[SerializeField] private ParticleImage _particleImage;

    public void UISet(PuzzleType puzzleType)
    {
		GetImage(UIImageE.Image).Fade(1);
		
	    switch (puzzleType)
	    {
		    case PuzzleType.Red:
			    GetImage(UIImageE.Image).color = weekRed;
			    break;
		    case PuzzleType.Blue:
			    GetImage(UIImageE.Image).color = weekBlue;
			    break;
		    case PuzzleType.Green:
			    GetImage(UIImageE.Image).color = weekGreen;
			    break;
		    case PuzzleType.Yellow:
			    GetImage(UIImageE.Image).color = weekYellow;
			    break;
		    default:
			    break;
	    }

		IsCrash = false;
    }

    public void Crash()
    {
		_particleImage.Play();
		Faint();

		IsCrash = true;
    }

	public void Faint()
	{
		GetImage(UIImageE.Image).Fade(0);
	}

	public enum UIImageE
    {
		Image,
    }
}