using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[System.Serializable]
public abstract class EffectAction : ICloneable
{
    // Effect�� ���۵� �� ȣ��Ǵ� ���� �Լ�
    public virtual void Start(Effect effect, Entity user, Entity target, int level, float scale) { }
    // ���� Effect�� ȿ���� �����ϴ� �Լ�
    public abstract bool Apply(Effect effect, Entity user, Entity target, int level, int stack, float scale);
    // Effect�� ����� �� ȣ��Ǵ� ���� �Լ�
    public virtual void Release(Effect effect, Entity user, Entity target, int level, float scale) { }

    // Effect�� Stack�� �ٲ���� �� ȣ��Ǵ� �Լ�
    // Stack���� Bonus ���� �ִ� Action�� ���, �� �Լ��� ���ؼ� Bonus ���� ���� ������ �� ����
    public virtual void OnEffectStackChanged(Effect effect, Entity user, Entity target, int level, int stack, float scale) { }

    // Key�� Text Mark, Value�� Text Mark�� ��ü�� Text�� ���� Dctionary�� ����� �Լ�
    protected virtual IReadOnlyDictionary<string, string> GetStringsByKeyword(Effect effect) => null;

    // Effect�� ������ Description Text�� GetStringsByKeyword �Լ��� ���� ���� Dictionary�� Replace �۾��� �ϴ� �Լ�
    public string BuildDescription(Effect effect, string description, int stackActionIndex, int stack, int effectIndex)
    {
        var stringsByKeyword = GetStringsByKeyword(effect);
        if (stringsByKeyword == null)
            return description;

        if (stack == 0)
            // ex. description = "������ $[EffectAction.defaultDamage.0] ���ظ� �ݴϴ�."
            // defaultDamage = 300, effectIndex = 0, stringsByKeyword = new() { { "defaultDamage", defaultDamage.ToString() } };
            // description.Replace("$[EffectAction.defaultDamage.0]", "300") => "������ 300 ���ظ� �ݴϴ�."
            description = TextReplacer.Replace(description, "effectAction", stringsByKeyword, effectIndex.ToString());
        else
            // Mark = $[EffectAction.Keyword.StackActionIndex.Stack.EffectIndex]
            description = TextReplacer.Replace(description, "effectAction", stringsByKeyword, $"{stackActionIndex}.{stack}.{effectIndex}");

        return description;
    }

    public abstract object Clone();
}