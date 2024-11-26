using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform _target;

    private float _noneLength = 0.1f;
    private float _speed = 4f;
    
    public void Initialize(Transform target)
    {
        this._target = target;
    }

    void Update()
    {
        if (_target is not null)
        {
            Vector3 destPos = this.transform.position;
            destPos.x = _target.position.x; 
            if (this.transform.position.GetDistance(destPos) > _noneLength)
            {
                this.transform.position = Vector3.Lerp(this.transform.position, destPos, _speed * Managers.Time.DeltaTime);
            }
        }
    }
}
