using System;
using System.Collections.Generic;
using Shared.CSharp;
using UnityEngine;

public class TimeManager
{
    public DateTime Current { get; private set; }
    public List<ITime> Times = new();

    private float _magnification = 1;
    public float Magnification
    {
        get => _magnification;
        set
        {
            _magnification = value;
            
            for (int i = 0; i < Times.Count; i++)
                Times[i].TimeScale(value);
        }
    }

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
        // Managers.Web.SendGetRequest<string>("time/now", (res) =>
        // {
        //     DateTime serverTime = CSharpHelper.ToDateTime_H(res, true);
        //     Current = serverTime;
        // });
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

    public void TimeScale(float value) => Magnification = value;
    public float FixedDeltaTime => Time.fixedDeltaTime * Magnification;
    public float UnscaledTime => Time.fixedDeltaTime;
    public float DeltaTime => Time.deltaTime * Magnification;

    public void ChangeFrameRate(int value)
    {
        Application.targetFrameRate = value;
        // Time.fixedDeltaTime = 1f / value; // float 캐스팅이 불필요하므로 제거하고, 가독성을 위해 1f로 표현
        Time.fixedDeltaTime = 0.02f;
    }
    public void TimeAdd(ITime time)
    {
        if (!Times.Contains(time))
            Times.Add(time);

        time.TimeScale(Magnification);
    }
    public void TimeRemove(ITime time)
    {
        if (Times.Contains(time))
            Times.Remove(time);
    }
}

public interface ITime
{
    public void TimeScale(float value);
    public void TimeUnScale();
}