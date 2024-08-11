using System;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    float _speed;
    float _adjustSpeed = .01f;
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

    Vector3 _scaleRight = new Vector3(1, 1, 1);
    Vector3 _scaleLeft = new Vector3(-1, 1, 1);

    public void Stop() => IsMove = false;

    private void FixedUpdate()
    {
        if (IsMove)
        {
            SetFlipScale(IsLeft);

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
    }
    public void LookAtImmediate(Transform target) => SetFlipScale(this.transform.position.x > target.position.x);
    public void LookAtImmediate(Vector3 direction) => SetFlipScale(direction.x < 0);
    void SetFlipScale(bool isLeft) => this.transform.localScale = isLeft ? _scaleLeft : _scaleRight;
}
