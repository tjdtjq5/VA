using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachPosition : MonoBehaviour
{
    private Transform _target;
    
    public void Initialize(Transform target) => this._target = target;
    private void Update()
    {
        if (_target) this.transform.position = _target.position;
    }
    void OnDisable() => _target = null;
}
