using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssetKits.ParticleImage;

public class PuzzleCrash : MonoBehaviour
{
    [SerializeField] private ParticleImage particleImage1;
    [SerializeField] private ParticleImage particleImage2;

    [SerializeField] private Sprite red1;
    [SerializeField] private Sprite red2;
    [SerializeField] private Sprite blue1;
    [SerializeField] private Sprite blue2;
    [SerializeField] private Sprite green1;
    [SerializeField] private Sprite green2;
    [SerializeField] private Sprite yellow1;
    [SerializeField] private Sprite yellow2;
    
    public void Play(PuzzleType puzzleType)
    {
        switch (puzzleType)
        {
            case PuzzleType.Red:
                particleImage1.sprite = red1;
                particleImage2.sprite = red2;
                break;
            case PuzzleType.Blue:
                particleImage1.sprite = blue1;
                particleImage2.sprite = blue2;
                break;
            case PuzzleType.Green:
                particleImage1.sprite = green1;
                particleImage2.sprite = green2;
                break;
            case PuzzleType.Yellow:
                particleImage1.sprite = yellow1;
                particleImage2.sprite = yellow2;
                break;
        }

         particleImage1.Play();
         particleImage2.Play();
    }
}
