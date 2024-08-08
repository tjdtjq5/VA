using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
  

    float _speed;
    float _adjustSpeed = .01f;
    float _maxSpeed = 1;
    public float Speed { get { return _speed / _adjustSpeed; } set { _speed = value * _adjustSpeed; } }

    Vector3 _destination;
    public Vector3 Destination { get { return _destination; } set { IsMove = true; _destination = value; } }
    public float DistinationDistance => Vector3.Distance(Destination, this.transform.position);
    public float DistinationSqrMagnitude => Vector3.SqrMagnitude(Destination - this.transform.position);

    public bool IsMove { get; private set; } = false;

    float _weight;
    float _weightAdjustValue = 1f;
    public float Weight { get { return _weight; } private set { _weight = Math.Clamp(value, 0, 1); } }

    bool isLeft = false;
    bool IsLeft
    {
        get
        {
            if (Destination.x < this.transform.position.x)
                isLeft = true;
            else if (Destination.x > this.transform.position.x)
                isLeft = false;

            return isLeft;
        }
    }

    public Action<bool, float> _moveFiexedUpdateAction;


    public void Stop()
    {
        IsMove = false;
    }

    float aniWeight = 0;
    bool aniIsLeft = false;
    private void FixedUpdate()
    {
        if (IsMove)
        {
            aniIsLeft = IsLeft;

            Weight += Managers.Time.FixedDeltaTime * Speed * _weightAdjustValue;
            this.transform.position = Vector3.MoveTowards(this.transform.position, Destination, _speed);

            if (DistinationSqrMagnitude < 0.1f)
            {
                Stop();
            }
        }
        else
        {
            Weight -= Managers.Time.FixedDeltaTime * Speed * _weightAdjustValue;
        }

        aniWeight = Weight;

        _moveFiexedUpdateAction?.Invoke(aniIsLeft, aniWeight);
    }
}
