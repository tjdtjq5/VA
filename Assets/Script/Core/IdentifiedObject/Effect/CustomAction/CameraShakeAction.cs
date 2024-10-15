using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraShakeAction : CustomAction
{
    public override void Run(object data)
    {
        UnityHelper.SerializeL($"camera action : {data}");
        Camera.main.GetComponent<Cinemachine.CinemachineImpulseSource>().GenerateImpulse();
    }

    public override object Clone() => new CameraShakeAction();
}