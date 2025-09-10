using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PathTrailFollower : MonoBehaviour 
{
    public Action OnEnd;

    [SerializeField] private Transform trailPrefab;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float trailSpawnInterval = 0.1f;

    private List<Transform> _pathPoints;

    private Transform _parent;
    private bool isMoving;
    private Vector3 _dir;
    private float _trailTimer;
    private int _index;
    private Transform _currentTarget;
    private Transform _nextTarget;
    private WaitForEndOfFrame waitForEndOfFrame;

    private void Awake()
    {
        waitForEndOfFrame = new WaitForEndOfFrame();
    }

    public void StartPath(Transform[] points, Transform parent)
    {
        if (isMoving) return;
        
        if(_pathPoints == null)
            _pathPoints = new List<Transform>(points.Length);
        else
            _pathPoints.Clear();

        _parent = parent;
            
        _pathPoints.AddRange(points);
        MoveAnglePath(0);
    }

    private void MoveAnglePath(int index)
    {
        if (index >= _pathPoints.Count - 1)
        {
            isMoving = false;
            OnEnd?.Invoke();
            return;
        }

        _index = index;
        _currentTarget = _pathPoints[index];
        _nextTarget = _pathPoints[index + 1];
        _trailTimer = 0f;
        
        this.transform.position = _currentTarget.position;
        _dir = (_nextTarget.position - _currentTarget.position).normalized;

        isMoving = true;
    }

    void FixedUpdate()
    {
        if(isMoving)
        {
            if (UnityHelper.GetDistance(transform.position, _nextTarget.position) < 1f)
            {
                MoveAnglePath(_index + 1);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, _nextTarget.position, moveSpeed * Managers.Time.DeltaTime);
            }


            _trailTimer += Managers.Time.DeltaTime;
            if(_trailTimer >= trailSpawnInterval)
            {
                Transform trail = Managers.Resources.Instantiate(trailPrefab, _parent).transform;
                trail.localScale = Vector3.one;
                trail.position = transform.position;
                trail.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg);
                _trailTimer = 0;
            }
        }
    }
}
