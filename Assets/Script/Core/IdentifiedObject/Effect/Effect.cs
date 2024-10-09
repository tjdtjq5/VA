using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Effect : IdentifiedObject
{
    // ��� ������ ���� 0�̸� ������ �ǹ���
    private const int kInfinity = 0;

    public delegate void StartedHandler(Effect effect);
    public delegate void AppliedHandler(Effect effect, int currentApplyCount, int prevApplyCount);
    public delegate void ReleasedHandler(Effect effect);
    public delegate void StackChangedHandler(Effect effect, int currentApplyCount, int prevApplyCount);

    [SerializeField]
    private EffectType type;
    // Effect�� �ߺ� ���� ���� ����
    [SerializeField]
    private bool isAllowDuplicate = true;
    [SerializeField]
    private EffectRemoveDuplicateTargetOption removeDuplicateTargetOption;

    // UI�� Effect ������ ���������� ���� ����
    [SerializeField]
    private bool isShowInUI;

    // maxLevel�� effectDatas�� Length�� �ʰ��� �� �ִ��� ����
    // �� Option�� false�� maxLevel�� effectDatas�� Length�� ������
    [SerializeField]
    private bool isAllowLevelExceedDatas;
    [SerializeField]
    private int maxLevel;
    // Level�� Data, Level�� 1���� �����ϰ� Array�� Index�� 0���� �����ϹǷ�
    // Level�� �´� Data�� ���������� [���� Level - 1]��° Data�� �����;���
    // ex. Level�� 1�̶��, 1 - 1 = 0, 0��° Data�� �����;���(= effectDatas[0])
    [SerializeField]
    private EffectData[] effectDatas;

    // Level�� �´� ���� Data
    private EffectData currentData;

    // ���� Effect Level
    private int level;
    // ���� ���� Stack
    private int currentStack = 1;
    private float currentDuration;
    private int currentApplyCount;
    private float currentApplyCycle;
    // Action�� Apply �Լ��� �����Ϸ� �õ��� ���� �ִ��� ����, �� ���� ���� Apply �����ÿ� currentApplyCycle ������ ���� �ٸ��� �ʱ�ȭ ��.
    // Action�� Apply �Լ��� ����� �� true���ǰ�, Apply �Լ��� true�� return�ϸ� false�� �ʱ�ȭ ��.
    private bool isApplyTried;

    // ���� Stack�� ���� ���� ����� Stack Actions
    private readonly List<EffectStackAction> aplliedStackActions = new();

    public EffectType Type => type;
    public bool IsAllowDuplicate => isAllowDuplicate;
    public EffectRemoveDuplicateTargetOption RemoveDuplicateTargetOption => removeDuplicateTargetOption;

    public bool IsShowInUI => isShowInUI;

    public IReadOnlyList<EffectData> EffectDatas => effectDatas;
    public IReadOnlyList<EffectStackAction> StackActions => currentData.stackActions;

    public int MaxLevel => maxLevel;
    public int Level
    {
        get => level;
        set
        {
            UnityHelper.Assert_H(value > 0 && value <= MaxLevel, $"Effect.Rank = {value} - value�� 0���� ũ�� MaxLevel���� ���ų� �۾ƾ��մϴ�.");

            if (level == value)
                return;

            level = value;

            // ���� Effect Level���� �����鼭 ���� ����� Level�� Data�� ã�ƿ�
            // ���� ���, Data�� Level 1, 3, 5 �̷��� ���� ��, Effect�� Level�� 4�� ���,
            // Level 3�� Data�� ã�ƿ�
            var newData = effectDatas.Last(x => x.level <= level);
            if (newData.level != currentData.level)
                currentData = newData;
        }
    }
    public bool IsMaxLevel => level == maxLevel;
    // ���� Effect�� EffectData�� Level ����
    // Action �ʿ��� Bonus Value�� �ִµ� Ȱ���� �� ����
    // ex. totalValue = defaultValue + (effect.DataBonusLevel * bonusValuePerLevel)
    // Level�� 1000���� �ִ� Clicker Game�� ��� Data�� 1000�� ������ �ʾƵ�
    // ���� ���� BonusLevel�� Ȱ���� Level �� ��ġ�� ������ �� ����.
    public int DataBonusLevel => Mathf.Max(level - currentData.level, 0);

    // Effect�� ���� �ð�
    public float Duration => currentData.duration.GetValue(User.Stats).Float();
    // Duration�� 0�̸� ���� ����
    public bool IsTimeless => Mathf.Approximately(Duration, kInfinity);
    public float CurrentDuration
    {
        get => currentDuration;
        set => currentDuration = Mathf.Clamp(value, 0f, Duration);
    }
    public float RemainDuration => Mathf.Max(0f, Duration - currentDuration);

    public int MaxStack => currentData.maxStack;
    public int CurrentStack
    {
        get => currentStack;
        set
        {
            var prevStack = currentStack;
            currentStack = Mathf.Clamp(value, 1, MaxStack);

            // Stack�� ���̸� currentDuration�� �ʱ�ȭ�Ͽ� Effect�� ���� �ð��� �÷���
            if (currentStack >= prevStack)
                currentDuration = 0f;

            if (currentStack != prevStack)
            {
                Action?.OnEffectStackChanged(this, User, Target, level, currentStack, Scale);

                TryApplyStackActions();

                onStackChanged?.Invoke(this, currentStack, prevStack);
            }
        }
    }

    public int ApplyCount => currentData.applyCount;
    public bool IsInfinitelyApplicable => ApplyCount == kInfinity;
    public int CurrentApplyCount
    {
        get => currentApplyCount;
        set => currentApplyCount = IsInfinitelyApplicable ? value : Mathf.Clamp(value, 0, ApplyCount);
    }
    public float ApplyCycle => Mathf.Approximately(currentData.applyCycle, 0f) && ApplyCount > 1 ?
        (Duration / (ApplyCount - 1)) : currentData.applyCycle;
    public float CurrentApplyCycle
    {
        get => currentApplyCycle;
        set => currentApplyCycle = Mathf.Clamp(value, 0f, ApplyCycle);
    }

    private EffectAction Action => currentData.action;

    private CustomAction[] CustomActions => currentData.customActions;

    public object Owner { get; private set; }
    public Entity User { get; private set; }
    public Entity Target { get; private set; }
    public float Scale { get; set; }
    public override string Description => BuildDescription(base.Description, 0);

    private bool IsApplyAllWhenDurationExpires => currentData.isApplyAllWhenDurationExpires;
    private bool IsDurationEnded => !IsTimeless && Mathf.Approximately(Duration, CurrentDuration);
    private bool IsApplyCompleted => !IsInfinitelyApplicable && CurrentApplyCount == ApplyCount;
    public bool IsFinished => IsDurationEnded ||
        (currentData.runningFinishOption == EffectRunningFinishOption.FinishWhenApplyCompleted && IsApplyCompleted);
    public bool IsReleased { get; private set; }

    public bool IsApplicable => Action != null &&
        (CurrentApplyCount < ApplyCount || ApplyCount == kInfinity) &&
        CurrentApplyCycle >= ApplyCycle;

    public event StartedHandler onStarted;
    public event AppliedHandler onApplied;
    public event ReleasedHandler onReleased;
    public event StackChangedHandler onStackChanged;

    public void Setup(object owner, Entity user, int level, float scale = 1f)
    {
        Owner = owner;
        User = user;
        Level = level;
        CurrentApplyCycle = ApplyCycle;
        Scale = scale;
    }

    public void SetTarget(Entity target) => Target = target;

    private void ReleaseStackActionsAll()
    {
        aplliedStackActions.ForEach(x => x.Release(this, level, User, Target, Scale));
        aplliedStackActions.Clear();
    }

    private void ReleaseStackActions(System.Func<EffectStackAction, bool> predicate)
    {
        var stackActions = aplliedStackActions.Where(predicate).ToList();
        foreach (var stackAction in stackActions)
        {
            stackAction.Release(this, level, User, Target, Scale);
            aplliedStackActions.Remove(stackAction);
        }
    }

    private void TryApplyStackActions()
    {
        ReleaseStackActions(x => x.Stack > currentStack);

        var stackActions = StackActions.Where(x => x.Stack <= currentStack && !aplliedStackActions.Contains(x) && x.IsApplicable);

        int aplliedStackHighestStack = aplliedStackActions.Any() ? aplliedStackActions.Max(x => x.Stack) : 0;
        int stackActionsHighestStack = stackActions.Any() ? stackActions.Max(x => x.Stack) : 0;
        var highestStack = Mathf.Max(aplliedStackHighestStack, stackActionsHighestStack);
        if (highestStack > 0)
        {
            var except = stackActions.Where(x => x.Stack < highestStack && x.IsReleaseOnNextApply);
            stackActions = stackActions.Except(except);
        }

        if (stackActions.Any())
        {
            ReleaseStackActions(x => x.Stack < currentStack && x.IsReleaseOnNextApply);

            foreach (var stackAction in stackActions)
                stackAction.Apply(this, level, User, Target, Scale);

            aplliedStackActions.AddRange(stackActions);
        }
    }

    public void Start()
    {
        UnityHelper.Assert_H(!IsReleased, "Effect::Start - �̹� ����� Effect�Դϴ�.");

        Action?.Start(this, User, Target, Level, Scale);

        TryApplyStackActions();

        foreach (var customAction in CustomActions)
            customAction.Start(this);

        onStarted?.Invoke(this);
    }

    public void FixedUpdate()
    {
        CurrentDuration += Managers.Time.FixedDeltaTime;
        currentApplyCycle += Managers.Time.FixedDeltaTime;

        if (IsApplicable)
            Apply();

        if (IsApplyAllWhenDurationExpires && IsDurationEnded && !IsInfinitelyApplicable)
        {
            for (int i = currentApplyCount; i < ApplyCount; i++)
                Apply();
        }
    }

    public void Apply()
    {
        UnityHelper.Assert_H(!IsReleased, "Effect::Apply - �̹� ����� Effect�Դϴ�.");

        if (Action == null)
            return;

        if (Action.Apply(this, User, Target, level, currentStack, Scale))
        {
            foreach (var customAction in CustomActions)
                customAction.Run(this);

            var prevApplyCount = CurrentApplyCount++;

            if (isApplyTried)
                currentApplyCycle = 0f;
            else
                currentApplyCycle %= ApplyCycle;

            isApplyTried = false;

            onApplied?.Invoke(this, CurrentApplyCount, prevApplyCount);
        }
        else
            isApplyTried = true;
    }

    public void Release()
    {
        UnityHelper.Assert_H(!IsReleased, "Effect::Release - �̹� ����� Effect�Դϴ�.");

        Action?.Release(this, User, Target, level, Scale);
        ReleaseStackActionsAll();

        foreach (var customAction in CustomActions)
            customAction.Release(this);

        IsReleased = true;

        onReleased?.Invoke(this);
    }


    public EffectData GetData(int level) => effectDatas[level - 1];

    public string BuildDescription(string description, int effectIndex)
    {
        Dictionary<string, string> stringsByKeyword = new Dictionary<string, string>()
        {
            { "duration", Duration.ToString("0.##") },
            { "applyCount", ApplyCount.ToString() },
            { "applyCycle", ApplyCycle.ToString("0.##") }
        };

        description = TextReplacer.Replace(description, stringsByKeyword, effectIndex.ToString());

        description = Action.BuildDescription(this, description, 0, 0, effectIndex);

        var stackGroups = StackActions.GroupBy(x => x.Stack);
        foreach (var stackGroup in stackGroups)
        {
            int i = 0;
            foreach (var stackAction in stackGroup)
                description = stackAction.BuildDescription(this, description, i++, effectIndex);
        }

        return description;
    }

    public override object Clone()
    {
        var clone = Instantiate(this);

        if (Owner != null)
            clone.Setup(Owner, User, Level, Scale);

        return clone;
    }
}