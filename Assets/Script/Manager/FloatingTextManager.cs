using System.Collections.Generic;
using UnityEngine;

public class FloatingTextManager
{
    string prefabPath = "Prefab/InGame/FloatingText";
    Color basicDamageColor = Color.white;
    Color critialDamageColor = Color.yellow;

    Dictionary<GameObject, JobSerializer> jobSerializerDics = new Dictionary<GameObject, JobSerializer>();
    float jobTime = 0.2f;
    float jobTimer = 0;
    bool isJobPlay = false;
    bool isJobCountCheck = false;

    public void DamageSpawn(Entity targetEntity, object causer, BBNumber damage, bool isCritial)
    {
        isJobPlay = true;

        if (jobSerializerDics.ContainsKey(targetEntity.gameObject))
            jobSerializerDics[targetEntity.gameObject].Push(DamageSpawnJob, targetEntity, causer, damage, isCritial);
        else
        {
            JobSerializer jobSerializer = new JobSerializer();
            jobSerializer.Push(DamageSpawnJob, targetEntity, causer, damage, isCritial);
            jobSerializerDics.Add(targetEntity.gameObject, jobSerializer);
        }
    }
    void DamageSpawnJob(Entity targetEntity, object causer, BBNumber damage, bool isCritial)
    {
        FloatingText floatingText = Managers.Resources.Instantiate<FloatingText>(prefabPath);

        floatingText.Text.text = $"{damage.Alphabet()}";
        floatingText.Text.color = isCritial ? critialDamageColor : basicDamageColor;

        floatingText.Play(GetPosByTargetEntity(targetEntity));
    }
    public void TextSpawn(Entity targetEntity, string text, Color color)
    {
        isJobPlay = true;

        if (jobSerializerDics.ContainsKey(targetEntity.gameObject))
            jobSerializerDics[targetEntity.gameObject].Push(TextSpawnJob, targetEntity, text, color);
        else
        {
            JobSerializer jobSerializer = new JobSerializer();
            jobSerializer.Push(TextSpawnJob, targetEntity, text, color);
            jobSerializerDics.Add(targetEntity.gameObject, jobSerializer);
        }
    }
    void TextSpawnJob(Entity targetEntity, string text, Color color)
    {
        FloatingText floatingText = Managers.Resources.Instantiate<FloatingText>(prefabPath);

        floatingText.Text.text = $"{text}";
        floatingText.Text.color = color;

        floatingText.Play(GetPosByTargetEntity(targetEntity));
    }

    Vector3 GetPosByTargetEntity(Entity target)
    {
        BoxCollider collider = target.Collider;
        Vector3 pos = target.transform.position;
        pos.y += collider.size.y * .5f;
        pos.z += collider.size.z * .5f;
        return pos;
    }

    public void OnFixedUpdate()
    {
        if (isJobPlay)
        {
            jobTimer += Managers.Time.FixedDeltaTime;
            if (jobTimer > jobTime)
            {
                jobTimer = 0;
                isJobCountCheck = false;
                foreach (var job in jobSerializerDics)
                {
                    if (job.Value.Count > 0)
                    {
                        job.Value.Pop().Execute();

                        if (!isJobCountCheck && job.Value.Count > 0)
                            isJobCountCheck = true;
                    }
                }

                if (!isJobCountCheck)
                    isJobPlay = false;
            }
        }
    }
}
