using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct EffectData
{
    public int level;

    [UnderlineTitle("Stack")]
    [Min(1)]
    public int maxStack;
    public EffectStackAction[] stackActions;

    [UnderlineTitle("Action")]
    [SerializeReference, SubclassSelector]
    public EffectAction action;

    [UnderlineTitle("Setting")]
    public EffectRunningFinishOption runningFinishOption;
    public bool isApplyAllWhenDurationExpires;
    public StatScaleFloat duration;
    [Min(0)]
    public int applyCount;
    [Min(0f)]
    public float applyCycle;

    [UnderlineTitle("Custom Action")]
    [SerializeReference, SubclassSelector]
    public CustomAction[] customActions;
}
