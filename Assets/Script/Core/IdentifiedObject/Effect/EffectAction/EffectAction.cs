using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[System.Serializable]
public abstract class EffectAction : ICloneable
{
    public virtual void Start(Effect effect, Entity user, Entity target, int level, float scale) { }
    public abstract bool Apply(Effect effect, Entity user, Entity target, int level, int stack, float scale);
    public virtual void Release(Effect effect, Entity user, Entity target, int level, float scale) { }

    public virtual void OnEffectStackChanged(Effect effect, Entity user, Entity target, int level, int stack, float scale) { }

    protected virtual IReadOnlyDictionary<string, string> GetStringsByKeyword(Effect effect) => null;

    public string BuildDescription(Effect effect, string description, int stackActionIndex, int stack, int effectIndex)
    {
        var stringsByKeyword = GetStringsByKeyword(effect);
        if (stringsByKeyword == null)
            return description;

        if (stack == 0)
            description = TextReplacer.Replace(description, "effectAction", stringsByKeyword, effectIndex.ToString());
        else
            description = TextReplacer.Replace(description, "effectAction", stringsByKeyword, $"{stackActionIndex}.{stack}.{effectIndex}");

        return description;
    }

    public abstract object Clone();
}