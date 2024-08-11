using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
public abstract class TargetSearchAction : ICloneable
{
    // Indicator�� �����ִ� Module
    [Header("Indicator")]
    [SerializeReference, SubclassSelector]
    private IndicatorViewAction indicatorViewAction;

    // Range�� Scale�� �������� ����
    [Header("Option")]
    [SerializeField]
    private bool isUseScale;

    // Range�� ����Ǿ� Range ���� ������ �� ���Ǵ� ����
    // Skill�� Charge ������ ���� �˻� ������ �޶����� �� �� Ȱ���� �� ����
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

    public TargetSearchAction() { }

    public TargetSearchAction(TargetSearchAction copy)
    { 
        indicatorViewAction = copy.indicatorViewAction?.Clone() as IndicatorViewAction;
        isUseScale = copy.isUseScale;
    }

    // selectResult�� ������� Target�� ã�� �Լ�, �˻� ����� �ﰢ ��ȯ��
    public abstract TargetSearchResult Search(TargetSearcher targetSearcher, Entity requesterEntity,
        GameObject requesterObject, TargetSelectionResult selectResult);

    public abstract object Clone();

    public virtual void ShowIndicator(TargetSearcher targetSearcher, GameObject requesterObject, float fillAmount)
        => indicatorViewAction?.ShowIndicator(targetSearcher, requesterObject, Range, Angle, fillAmount);

    public virtual void HideIndicator() => indicatorViewAction?.HideIndicator();

    public string BuildDescription(string description, string prefixKeyword) 
        => TextReplacer.Replace(description, prefixKeyword + ".searchAction", GetStringsByKeyword());

    protected virtual IReadOnlyDictionary<string, string> GetStringsByKeyword() => null;

    // Scale ���� �����Ǿ��� ���� ó���� �ϴ� �Լ�
    protected virtual void OnScaleChanged(float newScale) { }
}
