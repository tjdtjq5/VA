using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAnglePositionSetting : MonoBehaviour
{
    [Button]
    public void PositionSetting(float height)
    {
        this.transform.localPosition = transform.up * -(height / 2);
    }
}
