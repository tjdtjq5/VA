using System;
using UnityEngine;

public class TimeManager
{
    public DateTime Current { get; private set; }

    public float Magnification { get; private set; } = 1;

    float _updateTime = 1f;
    float _updateTimer = 0f;

    float _serverUpdateTime = 10f;
    float _serverUpdateTimer = 0f;

    public void Initialize()
    {
        Current = DateTime.Now;
        SetServerTime();
    }

    private void SetServerTime()
    {
        Managers.Web.SendGetRequest<string>("Time/CurrentDate", (res) => 
        {
            DateTime serverTime = res.ToDateTime(true);
            Current = serverTime;
        });
    }

    public void OnFixedUpdate()
    {
        _updateTimer += FixedDeltaTime;

        if (_updateTimer > _updateTime)
        {
            Current = Current.AddSeconds(_updateTimer);
            _updateTimer = 0;
        }

        _serverUpdateTimer += FixedDeltaTime;

        if (_serverUpdateTimer > _serverUpdateTime)
        {
            SetServerTime();
            _serverUpdateTimer = 0;
        }
    }

    public void TimeMultiple(float value)
    {
        Magnification = value;
    }

    public float FixedDeltaTime
    {
        get 
        {
            return Time.fixedDeltaTime * Magnification;
        }
    }
    public float DeltaTime
    {
        get
        {
            return Time.deltaTime * Magnification;
        }
    }
}
