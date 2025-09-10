using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineMaterialBlink : MonoBehaviour
{
    SkeletonAnimation sa;
    MaterialPropertyBlock meterialBlock;
    MeshRenderer mr;

    string meterialBlockBlackID = "_Black";
    string meterialBlockColorID = "_Color";
    Color darkBlinkColor = new Color(1f, 1f, 1f, 0);

    float blinkTime = 0.06f;
    float blinkTimer;
    bool isBlink = false;

    private void Awake()
    {
        meterialBlock = new MaterialPropertyBlock();
        mr = GetComponent<MeshRenderer>();
        sa = GetComponent<SkeletonAnimation>();
    }

    private void OnEnable()
    {
        blinkTimer = 0;
        isBlink = false;

        SetBlack(Color.clear);
        // SetColor(Color.white);
    }

    void SetBlack(Color color)
    {
        meterialBlock.SetColor(Shader.PropertyToID(meterialBlockBlackID), color);
        mr.SetPropertyBlock(meterialBlock);
    }
    void SetColor(Color color)
    {
        meterialBlock.SetColor(Shader.PropertyToID(meterialBlockColorID), color);
        mr.SetPropertyBlock(meterialBlock);
    }

    public void Blink()
    {
        SetBlack(darkBlinkColor);

        blinkTimer = 0;
        isBlink = true;
    }

    private void FixedUpdate()
    {
        if (isBlink)
        {
            blinkTimer += Managers.Time.FixedDeltaTime;
            if (blinkTimer > blinkTime)
            {
                isBlink = false;
                SetBlack(Color.clear);
               //  SetColor(Color.white);
            }
        }
    }
}
