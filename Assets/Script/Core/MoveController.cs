using System;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    public Action onMove;
    public Action onStop;

    float _speed;
    float _adjustSpeed = .01f;
    public float Speed { get { return _speed / _adjustSpeed; } set { _speed = value * _adjustSpeed; } }
    public float NoneAdjustSpeed { get { return _speed; } }

    Vector3 _destination;
    public Vector3 Destination
    {
        get { return _destination; }
        set
        {
            IsMove = true; _destination = value;
            onMove?.Invoke();
        }
    }
    public float DistinationDistance => Vector3.Distance(Destination, this.transform.position);
    public float DistinationSqrMagnitude => Vector3.SqrMagnitude(Destination - this.transform.position);

    public bool IsMove { get; private set; } = false;

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


    public void Stop() { IsMove = false; onStop?.Invoke(); }

    private void FixedUpdate()
    {
        if (IsMove)
        {
            SetFlipScale(IsLeft);

            this.transform.position = Vector3.MoveTowards(this.transform.position, Destination, _speed);

            if (DistinationSqrMagnitude < 0.1f)
            {
                Stop();
            }
        }
    }
    public void LookAtImmediate(Transform target) => SetFlipScale(this.transform.position.x > target.position.x);
    public void LookAtImmediate(Vector3 target) => SetFlipScale(this.transform.position.x > target.x);
    public void LookAtImmediateDirection(Vector3 direction) => SetFlipScale(direction.x < 0);
    void SetFlipScale(bool isLeft) => this.transform.localScale = isLeft ? _scaleLeft : _scaleRight;
}
