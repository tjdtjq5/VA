using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EffectStackAction
{
    // �� Action�� Effect�� Stack�� �� �϶� ����� ���ΰ�?
    [SerializeField, Min(1)]
    private int stack;
    // Effect�� ���� StackAction�� ������ �� �� Action�� Release �� ���ΰ�?
    [SerializeField]
    private bool isReleaseOnNextApply;
    // Effect���� 1������ ����� ���ΰ�?
    // ex. �� Option�� ����������, Action�� Stack�� 2�� �� Effect�� Stack�� 2���Ǹ� Action�� ����ǰ�,
    // Effect�� Stack�� 3���� �����Ǿ��ٰ� �ٽ� 2�� ��������, Action�� �̹� �ѹ� ����Ǿ��� ������ �ٽ� ������� ����
    [SerializeField]
    private bool isApplyOnceInLifeTime;

    // ������ ȿ��
    [UnderlineTitle("Action")]
    [SerializeReference, SubclassSelector]
    private EffectAction action;

    // �� StackAction�� ����� ���� �ִ°�?
    private bool hasEverApplied;

    public int Stack => stack;
    public bool IsReleaseOnNextApply => isReleaseOnNextApply;
    // isAppliedOnceInLifeTime�� true��� ��������� ����� ���� ������
    public bool IsApplicable => !isApplyOnceInLifeTime || (isApplyOnceInLifeTime && !hasEverApplied);

    public void Start(Effect effect, Entity user, Entity target, int level, float scale)
        => action.Start(effect, user, target, level, scale);

    public void Apply(Effect effect, int level, Entity user, Entity target, float scale)
    {
        action.Apply(effect, user, target, level, stack, scale);
        hasEverApplied = true;
    }

    public void Release(Effect effect, int level, Entity user, Entity target, float scale)
        => action.Release(effect, user, target, level, scale);

    public string BuildDescription(Effect effect, string baseDescription, int stackActionIndex, int effectIndex)
        => action.BuildDescription(effect, baseDescription, stackActionIndex, stack, effectIndex);
}
