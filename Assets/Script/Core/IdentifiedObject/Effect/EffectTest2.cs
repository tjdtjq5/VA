using EasyButtons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTest2 : MonoBehaviour
{
    [SerializeField]
    private Effect testEffect;
    private Effect cloneEffect;

    [SerializeField, Min(1)]
    private int effectLevel = 1;

    [SerializeField] Entity entity;

    [Button]
    private void Create()
    {
        if (cloneEffect == null || cloneEffect.IsReleased)
        {
            cloneEffect = testEffect.Clone() as Effect;
        }
    }

    [Button]
    public void SetUp()
    {
        cloneEffect.Setup(this, entity, effectLevel, 1f);
    }
    [Button]
    public void SetTarget()
    {
        cloneEffect.SetTarget(entity);
    }

    [Button]
    public void Startt()
    {
        cloneEffect.Start();
    }

    [Button]
    public void Apply(int stack)
    {
        cloneEffect.CurrentStack = stack;

        if (cloneEffect.IsApplicable)
            cloneEffect.Apply();
    }

    [Button]
    public void Updatee()
    {
        StartCoroutine("InstantTestCoroutine");
    }

    private IEnumerator InstantTestCoroutine()
    {
        while (!cloneEffect.IsFinished)
        {
            yield return null;
            cloneEffect.Update();
        }

        cloneEffect.Release();
        Destroy(cloneEffect);
    }
}
