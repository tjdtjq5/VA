using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CircleIndicatorViewAction : IndicatorViewAction
{
    [SerializeField]
    private GameObject indicatorPrefab;
    // ������ indicator�� radius ��
    // ���� 0�̸� targetSearcher�� range�� ��� �����
    [SerializeField]
    private float indicatorRadiusOverride;
    // ������ indicator�� angle ��
    // ���� 0�̸� targetSearcher�� angle�� ��� �����
    [SerializeField]
    private float indicatorAngleOverride;
    // Indicator�� ���� ä��� fillAmount Property�� ����� ���ΰ�?
    [SerializeField]
    private bool isUseIndicatorFillAmount;
    // Indicator�� requsterObject�� �ڽ� Object�� ���� ���ΰ�?
    [SerializeField]
    private bool isAttachIndicatorToRequester;

    // ShowIndicator �Լ��� ������ Indicator
    private Indicator spawnedRangeIndicator;

    public override void ShowIndicator(TargetSearcher targetSearcher, GameObject requesterObject,
        object range, float angle, float fillAmount)
    {
        Debug.Assert(range is float, "CircleIndicatorViewAction::ShowIndicator - range�� null �Ǵ� float���� ���˴ϴ�.");

        // �̹� Indicator�� �����ְ� �ִٸ� ���� Hide ó���� ����
        HideIndicator();

        // isUseIndicatorFillAmount Option�� true�� �ƴϸ� fillAmount ������ 0�� ��
        fillAmount = isUseIndicatorFillAmount ? fillAmount : 0f;
        // isAttachIndicatorToRequester Option�� true��� requesterObject�� transform�� ������
        var attachTarget = isAttachIndicatorToRequester ? requesterObject.transform : null;
        // indicatorRadiusOverride�� 0�̶�� ���ڷ� ���� targetSearcher�� range��,
        // �ƴ϶�� indicatorRadiusOverride�� Indicator�� radius�� ��
        float radius = Mathf.Approximately(indicatorRadiusOverride, 0f) ? (float)range : indicatorRadiusOverride;
        // indicatorAngleOverride�� 0�̶�� ���ڷ� ���� targetSearcher�� angle��,
        // �ƴ϶�� indicatorAngleOverride�� Indicator�� angle�� ��
        angle = Mathf.Approximately(indicatorAngleOverride, 0f) ? angle : indicatorAngleOverride;

        // Indicator�� �����ϰ�, Setup �Լ��� ������ ���� ������ Setting����
        spawnedRangeIndicator = GameObject.Instantiate(indicatorPrefab).GetComponent<Indicator>();
        spawnedRangeIndicator.Setup(angle, radius, fillAmount, attachTarget);
    }

    public override void HideIndicator()
    {
        if (!spawnedRangeIndicator)
            return;

        GameObject.Destroy(spawnedRangeIndicator.gameObject);
    }

    public override void SetFillAmount(float fillAmount)
    {
        if (!isUseIndicatorFillAmount || spawnedRangeIndicator == null)
            return;

        spawnedRangeIndicator.FillAmount = fillAmount;
    }

    public override object Clone()
    {
        return new CircleIndicatorViewAction()
        {
            indicatorPrefab = indicatorPrefab,
            indicatorAngleOverride = indicatorAngleOverride,
            indicatorRadiusOverride = indicatorRadiusOverride,
            isUseIndicatorFillAmount = isUseIndicatorFillAmount,
            isAttachIndicatorToRequester = isAttachIndicatorToRequester
        };
    }
}
