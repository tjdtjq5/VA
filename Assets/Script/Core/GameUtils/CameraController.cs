using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 cameraOriginPos;
    Transform target;
    Vector3 targetOriginPos;
    Vector3 Destination { get { return target.transform.position - targetOriginPos + cameraOriginPos; } }
    float speed = .01f;

    void Start()
    {
        cameraOriginPos = transform.position;
    }
    public void SetTarget(Transform target)
    {
        this.target = target;
        targetOriginPos = target.transform.position;
    }

    void Update()
    {
        if (target != null)
            this.transform.position = Vector3.Lerp(this.transform.position, Destination, speed);
    }
}
