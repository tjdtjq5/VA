using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SkillData
{
    public int level;

    // Skill Level Up�� ���� ����
    [UnderlineTitle("Level Up")]
    [SerializeReference, SubclassSelector]
    public EntityCondition[] levelUpConditions;
    // Skill Level Up�� ���� ���
    [SerializeReference, SubclassSelector]
    public Cost[] levelUpCosts;

    // Skill�� ���� ���Ǳ� �� ���� ������ Action, �ƹ� ȿ�� ���� � ������ �����ϱ� ���� ����
    // ex. ���濡�� �޷���, �����⸦ ��, Jump�� �� ��
    [UnderlineTitle("Preceding Action")]
    [SerializeReference, SubclassSelector]
    public SkillPrecedingAction precedingAction;

    // Skill�� ��� ����� ����ϴ� Module
    // ex. ����ü �߻�, Target���� ��� ����, Skill Object Spawn ��
    [UnderlineTitle("Action")]
    [SerializeReference, SubclassSelector]
    public SkillAction action;

    [UnderlineTitle("Setting")]
    public SkillRunningFinishOption runningFinishOption;
    // runningFinishOption�� FinishWhenDurationEnded�̰� duration�� 0�̸� ���� ����
    [Min(0)]
    public float duration;
    // applyCount�� 0�̸� ���� ����
    [Min(0)]
    public int applyCount;
    // ù �ѹ��� ȿ���� �ٷ� ����� ���̱� ������, �ѹ� ����� �ĺ��� ApplyCycle�� ���� �����
    // ���� ��, ApplyCycle�� 1�ʶ��, �ٷ� �ѹ� ����� �� 1�ʸ��� ����ǰ� ��. 
    [Min(0f)]
    public float applyCycle;

    public StatScaleFloat cooldown;

    // Skill�� ���� ����� ã�� ���� Class
    [UnderlineTitle("Target Searcher")]
    public TargetSearcher targetSearcher;

    // Skill ����� ���� ���
    [UnderlineTitle("Cost")]
    [SerializeReference, SubclassSelector]
    public Cost[] costs;

    [UnderlineTitle("Cast")]
    public bool isUseCast;
    public StatScaleFloat castTime;

    [UnderlineTitle("Charge")]
    public bool isUseCharge;
    public SkillChargeFinishActionOption chargeFinishActionOption;
    // Charge�� ���� �ð�
    [Min(0f)]
    public float chargeDuration;
    // Full Charge���� �ɸ��� �ð�
    [Min(0f)]
    public float chargeTime;
    // Skill�� ����ϱ� ���� �ʿ��� �ּ� ���� �ð�
    [Min(0f)]
    public float needChargeTimeToUse;
    // Charge�� ���� Power
    [Range(0f, 1f)]
    public float startChargePower;
    
    [UnderlineTitle("Effect")]
    public EffectSelector[] effectSelectors;

    [UnderlineTitle("Animation")]
    public InSkillActionFinishOption inSkillActionFinishOption;
    public AnimatorParameter castAnimatorParamter;
    public AnimatorParameter chargeAnimatorParameter;
    public AnimatorParameter precedingActionAnimatorParameter;
    public AnimatorParameter actionAnimatorParameter;

    [SerializeReference, SubclassSelector]
    public CustomAction[] customActionsOnCast;
    [SerializeReference, SubclassSelector]
    public CustomAction[] customActionsOnCharge;
    [SerializeReference, SubclassSelector]
    public CustomAction[] customActionsOnPrecedingAction;
    [SerializeReference, SubclassSelector]
    public CustomAction[] customActionsOnAction;
}
