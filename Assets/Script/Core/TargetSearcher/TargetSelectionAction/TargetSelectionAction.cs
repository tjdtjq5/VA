using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class TargetSelectionAction : ICloneable
{
    #region Events
    public delegate void SelectCompletedHandler(TargetSelectionResult result);
    #endregion

    // Indicator�� �����ִ� Module
    [Header("Indicator")]
    [SerializeReference, SubclassSelector]
    private IndicatorViewAction indicatorViewAction;

    // Range�� Scale�� �������� ����
    [Header("Option")]
    [SerializeField]
    private bool isUseScale;

    // Range�� ����Ǿ� Range ���� ������ �� ���Ǵ� ����
    // Skill�� Charge ������ ���� �˻� ������ �޶�w�� �� �� Ȱ���� �� ����
    private float scale;

    public float Scale
    {
        get => scale;
        set
        {
            if (scale == value)
                return;

            scale = value;
            // scale�� Indicator�� FillAmount�� ����
            indicatorViewAction?.SetFillAmount(scale);
            // scale �� ������ ���� ó���� ��
            OnScaleChanged(scale);
        }
    }

    // Ž�� ����
    // Ž�� ������ �ܼ��� �Ÿ��� ��Ÿ���� float���� �� ����, ������ ��Ÿ���� Rect�� Vector���� �� ���� �����Ƿ� object type
    public abstract object Range { get; }
    // �� Range�� Scale�� ����� ��
    public abstract object ScaledRange { get; }
    // Ž�� ����
    public abstract float Angle { get; }
    // isUseScale ���� ����, �Ϲ� Range Ȥ�� ScaledReange�� ��ȯ����.
    public object ProperRange => isUseScale ? ScaledRange : Range;

    public bool IsUseScale => isUseScale;

    public TargetSelectionAction() { }

    public TargetSelectionAction(TargetSelectionAction copy)
    {
        indicatorViewAction = copy.indicatorViewAction?.Clone() as IndicatorViewAction;
        isUseScale = copy.isUseScale;
    }

    // Player�� �˻��� ��û���� �� ��� �������� ã�� �Լ�
    protected abstract TargetSelectionResult SelectImmediateByPlayer(TargetSearcher targetSearcher, Entity requesterEntity,
        GameObject requesterObject, Vector3 position);
    // AI�� �˻��� ��û���� �� ��� �������� ã�� �Լ�
    protected abstract TargetSelectionResult SelectImmediateByAI(TargetSearcher targetSearcher, Entity requesterEntity,
        GameObject requesterObject, Vector3 position);

    // Entity�� Player����, AI������ ���� �� �� �Լ��� �� ������ �Լ��� ��������
    public TargetSelectionResult SelectImmeidiate(TargetSearcher targetSearcher, Entity requesterEntity,
        GameObject requesterObject, Vector3 position)
        => requesterEntity.IsPlayer ? SelectImmediateByPlayer(targetSearcher, requesterEntity, requesterObject, position) :
        SelectImmediateByAI(targetSearcher, requesterEntity, requesterObject, position);

    // �񵿱������� �������� ã�� �Լ�
    // �������� ã�Ұų�, �˻��� �������� �� onSelectCompleted callback �Լ��� ����� return����.
    public abstract void Select(TargetSearcher targetSearcher, Entity requesterEntity,
        GameObject requesterObject, SelectCompletedHandler onSelectCompleted);

    // ���� Select �Լ��� �������� �񵿱� �˻� ���� ��, �˻��� �����
    public abstract void CancelSelect(TargetSearcher targetSearcher);

    // ���ڷ� ���� ��ǥ�� ������ �˻� ���� �ȿ� �ִ��� Ȯ���ϴ� �Լ�
    public abstract bool IsInRange(TargetSearcher targetSearcher, Entity requesterEntity, GameObject requesterObject, Vector3 targetPosition);

    public abstract object Clone();

    public virtual void ShowIndicator(TargetSearcher targetSearcher, GameObject requesterObject, float fillAmount)
        => indicatorViewAction?.ShowIndicator(targetSearcher, requesterObject, Range, Angle, fillAmount);

    public virtual void HideIndicator() => indicatorViewAction?.HideIndicator();
    
    // prefixKeyword.selectionAction.keyword
    // ex. targetSearcher.selectionAction.range
    public string BuildDescription(string description, string prefixKeyword)
        => TextReplacer.Replace(description, prefixKeyword + ".selectionAction", GetStringsByKeyword());

    protected virtual IReadOnlyDictionary<string, string> GetStringsByKeyword() => null;

    // Scale ���� �����Ǿ��� ���� ó���� �ϴ� �Լ�
    protected virtual void OnScaleChanged(float newScale) { }
}
