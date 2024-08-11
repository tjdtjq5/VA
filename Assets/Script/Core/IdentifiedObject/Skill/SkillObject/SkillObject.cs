using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SkillObject : MonoBehaviour
{
    // Skill�� ù ���뿡 ApplyCycle �ð���ŭ Delay�� �� ���ΰ�?
    // EX. ApplyCycle = 0.5, 0.5�� �ں��� Skill ���� ���� 
    [SerializeField]
    private bool isDelayFirstApplyByCycle;
    // SkillObject�� Destroy�Ǵ� �ð��� ApplyCycle��ŭ ������ �� ���ΰ�?
    // EX. ApplyCycle = 0.5, ������� 3�ʿ� Skill�� �� �����ϰ� Destroy �ؾ������� 3.5�ʿ� Destroy��
    [SerializeField]
    private bool isDelayDestroyByCycle;

    private float currentDuration;
    private float currentApplyCycle;
    private int currentApplyCount;

    private TargetSearcher targetSearcher;

    // Skill�� ������
    public Entity Owner { get; private set; }
    // �� SkillObject�� Spawn�� Skill
    public Skill Spawner { get; private set; }
    // SkillObject�� Skill�� ������ Target�� ã������ TargetSearcher
    public TargetSearcher TargetSearcher => targetSearcher;
    // SkillObject�� Transform Scale
    public Vector3 ObjectScale { get; private set; }
    public float Duration { get; private set; }
    public int ApplyCount { get; private set; }
    public float ApplyCycle { get; private set; }
    // SkillObject�� Destroy�Ǵ� �ð�.
    public float DestroyTime { get; private set; }

    private bool IsApplicable => (ApplyCount == 0 || currentApplyCount < ApplyCount) &&
        currentApplyCycle >= ApplyCycle;

    public void Setup(Skill spawner, TargetSearcher targetSearcher, float duration, int applyCount, Vector3 objectScale)
    {
        Spawner = spawner.Clone() as Skill;
        Owner = spawner.Owner;
        this.targetSearcher = new TargetSearcher(targetSearcher);
        ApplyCount = applyCount;
        Duration = duration;
        ObjectScale = objectScale;
        ApplyCycle = CalculateApplyCycle(duration, applyCount);
        DestroyTime = Duration + (isDelayDestroyByCycle ? ApplyCycle : 0f);

        // SkillObject�� ���� ������ �����ϱ����� ������� SkillObjectComponent Script�� �����ͼ� Callback �Լ��� ȣ������
        foreach (var component in GetComponents<ISkillObjectComponent>())
            component.OnSetupSkillObject(this);

        if (!isDelayFirstApplyByCycle)
            Apply();
    }

    private void OnDestroy()
    {
        Destroy(Spawner);
    }

    private void Update()
    {
        currentDuration += Time.deltaTime;
        currentApplyCycle += Time.deltaTime;

        if (IsApplicable)
            Apply();

        if (currentDuration >= DestroyTime)
            Destroy(gameObject);
    }

    public float CalculateApplyCycle(float duration, int applyCount)
    {
        if (applyCount == 1)
            return 0f;
        else
            return isDelayFirstApplyByCycle ? (duration / applyCount) : (duration / (applyCount - 1));
    }

    private void Apply()
    {
        targetSearcher.SelectImmediate(Owner, gameObject, transform.position);
        var result = targetSearcher.SearchTargets(Owner, gameObject);

        foreach (var target in result.targets)
            target.GetComponent<SkillSystem>().Apply(Spawner);

        currentApplyCount++;
        currentApplyCycle %= ApplyCycle;
    }
}