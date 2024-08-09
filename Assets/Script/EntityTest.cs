using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityTest : MonoBehaviour
{
    Entity entity;

    void Start()
    {
        entity = GetComponent<Entity>();

        Managers.Input.AddKeyDownAction(KeyCode.Space, HpMinus);
    }

    void HpMinus()
    {
        if (entity)
        {
            entity.TakeDamage(null, null , 1);
            UnityHelper.Log_H(entity.Stats.HPStat.Value.ToCountString());
        }
    }
}
