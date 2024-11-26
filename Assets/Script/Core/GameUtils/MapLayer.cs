using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLayer : MonoBehaviour
{
    private Transform _target;
    private float _distanceX;
    private int _count;
    private float _minX;
    private float _maxX;
    private int _currentIndex;

    private List<Transform> _strList = new();
    
    public void Initialize(Transform target, float distanceX , int count)
    {
        this._target = target;  
        this._distanceX = distanceX;  
        this._count = count;

        this._currentIndex = 0;
        MinMaxSetting(_currentIndex);
        
        Transform structTransform = this.transform.GetChild(0);
        _strList.Add(structTransform);

        for (int i = 0; i < count - 1; i++)
        {
            Transform clone = Instantiate(structTransform, this.transform);
            _strList.Add(clone);
        }
    }

    private void FixedUpdate()
    {
        if (_target)
        {
            CheckAndChange();
        }
    }

    void CheckAndChange()
    {
        float targetX = _target.position.x;

        if (targetX >= this._maxX)
        {
            UpChange();
        }
        else if (targetX < this._minX)
        {
            DownChange();
        }
    }

    void UpChange()
    {
        _currentIndex++;
        MinMaxSetting(_currentIndex);

        int bottomIndex = 0;
        Transform bottom = _strList[bottomIndex];
        _strList.RemoveAt(bottomIndex);
        _strList.Add(bottom);
        
        PositionSetting();
    }

    void DownChange()
    {
        _currentIndex--;
        MinMaxSetting(_currentIndex);
        
        int topIndex = _strList.Count - 1;
        Transform top = _strList[topIndex];
        _strList.RemoveAt(topIndex);
        _strList.Insert(0, top);

        PositionSetting();
    }

    void MinMaxSetting(int index)
    {
        this._minX = -(_distanceX * _count / 4) + _distanceX * index;
        this._maxX = (_distanceX * _count / 4) + _distanceX * index;
    }
    void PositionSetting()
    {
        for (int i = 0; i < _strList.Count; i++)
        {
            int index = i - _count / 2 + _currentIndex;
            float x = index * this._distanceX;
            _strList[i].position = new Vector3(x,     _strList[i].position.y,     _strList[i].position.z);
        }
    }
}
