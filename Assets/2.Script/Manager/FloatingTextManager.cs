using System.Collections;
using System.Collections.Generic;
using Shared.BBNumber;
using UnityEngine;

public class FloatingTextManager
{
    string prefabPath = "Prefab/InGame/FloatingText";
    
    private readonly float _verticalSpacing = 0.6f;   // 위로 쌓이는 간격
    private readonly float _destroyTime = 0.1f;      // 몇 초 뒤 파괴할지
    private int _stackCount = 0;
    
    Tween<float> _tween;

    public void Damage(Character target, BBNumber totalDamage, DamageType damageType, CriticalType criticalType)
    {
        FloatingText floatingText = Managers.Resources.Instantiate<FloatingText>(prefabPath);
        Vector3 pos = GetFloatingTextPos(target);

        floatingText.Damage(pos, totalDamage, damageType, criticalType);
    }

    public void Heal(Character target, BBNumber value)
    {
        FloatingText floatingText = Managers.Resources.Instantiate<FloatingText>(prefabPath);
        Vector3 pos = GetFloatingTextPos(target);

        floatingText.Heal(pos, value);
    }

    public void Miss(Character target)
    {
        FloatingText floatingText = Managers.Resources.Instantiate<FloatingText>(prefabPath);
        Vector3 pos = GetFloatingTextPos(target);

        floatingText.Miss(pos);
    }

    Vector3 GetFloatingTextPos(Character target)
    {
        Vector3 pos = new Vector3(target.transform.position.x, target.BoxHeight, target.transform.position.z);
        
        Vector3 offset = new Vector3(0, _verticalSpacing * _stackCount, 0);
        pos += offset;

        _stackCount++;

        _tween?.FullKill();
        _tween = Managers.Tween.TweenInvoke(_destroyTime).SetOnComplete(()=> _stackCount = 0);

        return pos;
    }
}
