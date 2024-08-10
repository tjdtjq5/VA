using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TestEffectAction2 : EffectAction
{
    [SerializeField]
    private int value;

    private int increaseValue;

    public override bool Apply(Effect effect, Entity user, Entity target, int level, int stack, float scale)
    {
        increaseValue = value * stack;

        Debug.Log($"Effect: {effect?.CodeName} Apply - User: {user?.name}, Target: {target?.name}, Scale: {scale}, Stack: {stack}");
        Debug.Log($"«‘ {increaseValue} ¡ı∞° / {value} * Stack = {increaseValue}");

        return true;
    }

    public override void Release(Effect effect, Entity user, Entity target, int level, float scale)
    {
        Debug.Log($"Effect: {effect.CodeName} Release");
    }

    public override void OnEffectStackChanged(Effect effect, Entity user, Entity target, int level, int stack, float scale)
    {
        Debug.Log($"Effect: {effect.CodeName}, New Stack: {stack}");
    }

    protected override IReadOnlyDictionary<string, string> GetStringsByKeyword(Effect effect)
    {
        var stringsByKeyword = new Dictionary<string, string>();
        stringsByKeyword["value"] = value.ToString();
        return stringsByKeyword;
    }

    public override object Clone()
    {
        return new TestEffectAction2() { value = value };
    }
}
