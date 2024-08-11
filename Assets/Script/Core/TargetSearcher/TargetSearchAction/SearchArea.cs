using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SearchArea: TargetSearchAction
{
    [Header("Data")]
    [Min(0f)]
    [SerializeField]
    private float range;
    [Range(0f, 360f)]
    [SerializeField]
    private float angle = 360f;
    // �˻��� ��û�� Entity�� �˻� ��� ������ ���ΰ�?
    [SerializeField]
    private bool isIncludeSelf;
    // Target�� �˻��� ��û�� Entity�� ���� Category�� ������ �־���ϴ°�?
    [SerializeField]
    private bool isSearchSameCategory;

    public override object Range => range;
    public override object ScaledRange => range * Scale;
    public override float Angle => angle;

    public SearchArea() { }

    public SearchArea(SearchArea copy)
        : base(copy)
    {
        range = copy.range;
        isIncludeSelf = copy.isIncludeSelf;
        isSearchSameCategory = copy.isSearchSameCategory;
    }

    public override TargetSearchResult Search(TargetSearcher targetSearcher, Entity requesterEntity,
        GameObject requesterObject, TargetSelectionResult selectResult)
    {
        var targets = new List<GameObject>();
        var spherePosition = selectResult.resultMessage == SearchResultMessage.FindTarget ?
            selectResult.selectedTarget.transform.position : selectResult.selectedPosition;
        var colliders = Physics.OverlapSphere(spherePosition, (float)ProperRange);

        Vector3 requesterPosition = requesterObject.transform.position;

        foreach (var collider in colliders)
        {
            var entity = collider.GetComponent<Entity>();
            // Entity�� null�̰ų�, �̹� ���� ���°ų�, �˻��� ����� Entity�ε� isIncludeSelf�� true�� �ƴ� ��� �Ѿ
            if (!entity || entity.IsDead || (entity == requesterEntity && !isIncludeSelf))
                continue;

            if (entity != requesterEntity)
            {
                // Requester�� Entity�� �����ϴ� Category�� �ִ��� Ȯ��
                var hasCategory = requesterEntity.Categories.Any(x => entity.HasCategory(x));
                // �����ϴ� Category�� ������ isSearchSameCategory�� false�ų�,
                // �����ϴ� Category�� ������ isSearchSameCategory�� true��� �Ѿ
                if ((hasCategory && !isSearchSameCategory) || (!hasCategory && isSearchSameCategory))
                    continue;
            }

            Vector3 entityPosition = entity.transform.position;
            entityPosition.y = requesterPosition.y;
            var direction = entityPosition - requesterPosition;

            if (Vector3.Angle(requesterObject.transform.forward, direction) < (angle * 0.5f))
                targets.Add(entity.gameObject);
        }
        return new(targets.ToArray());
    }

    protected override IReadOnlyDictionary<string, string> GetStringsByKeyword()
    {
        var dictionary = new Dictionary<string, string>() { { "range", range.ToString("0.##") } };
        return dictionary;
    }

    public override object Clone() => new SearchArea(this);
}
