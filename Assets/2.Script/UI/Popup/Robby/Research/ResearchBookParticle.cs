using System.Collections;
using System.Collections.Generic;
using AssetKits.ParticleImage;
using Shared.Enums;
using UnityEngine;

public class ResearchBookParticle : UIFrame
{
    private ParticleImage _particleImage;

    private readonly Color _redParticleColor = new Color(0xFF/255f, 0x4C/255f, 0x00/255f, 1);
    private readonly Color _blueParticleColor = new Color(0x00/255f, 0xD7/255f, 0xFF/255f, 1);
    private readonly Color _greenParticleColor = new Color(0x74/255f, 0xFF/255f, 0x6B/255f, 1);
    private readonly Color _yellowParticleColor = new Color(0xFF/255f, 0xE2/255f, 0x00/255f, 1);
    private readonly Color _masterParticleColor = new Color(0xFF/255f, 0xFF/255f, 0xFF/255f, 1);

    protected override void Initialize()
    {
        _particleImage = this.GetComponent<ParticleImage>();

        base.Initialize();
    }

    public void UISet(PlayerGrowResearch growResearch)
    {
        switch (growResearch)
        {
            case PlayerGrowResearch.RedBook:
                _particleImage.startColor = _redParticleColor;
                break;
            case PlayerGrowResearch.BlueBook:
                _particleImage.startColor = _blueParticleColor;
                break;
            case PlayerGrowResearch.GreenBook:
                _particleImage.startColor = _greenParticleColor;
                break;
            case PlayerGrowResearch.YellowBook:
                _particleImage.startColor = _yellowParticleColor;
                break;
            case PlayerGrowResearch.MasterBook:
                _particleImage.startColor = _masterParticleColor;
                break;
        }
    }
}
