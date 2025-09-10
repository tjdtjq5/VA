using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUITime : UIFrame
{
	public Action OnTimeOut;

	private readonly float _checkTime = 0.1f;
	private float _checkTimer;

	private float _time;
	private float _timer;
	private bool _isTime;
	
    protected override void Initialize()
    {
		Bind<UISlider>(typeof(UISliderE));
		Bind<UIImage>(typeof(UIImageE));
		Bind<UITextPro>(typeof(UITextProE));


        base.Initialize();
    }

    private void TimeSet(float timer)
    {
	    float value = 1 - timer / _time;
	    GetImage(UIImageE.SafeArea_Time_Fill).FillAmount = value;
    }
    public void Initialize(float time)
    {
	    this._time = time;
	    this._timer = 0;
	    this._checkTimer = 0;
	    _isTime = false;

	    TimeSet(_timer);
    }
    public void Play()
    {
	    this._timer = 0;
	    this._checkTimer = 0;
	    this._isTime = true;
	    
	    TimeSet(_timer);
    }
    public void Stop() =>  _isTime = false;
    
    private void TimeOut()
    {
	    Stop();
	    OnTimeOut?.Invoke();
    }

    public void FixedUpdate()
    {
	    if (_isTime)
	    {
		    _checkTimer += Managers.Time.FixedDeltaTime;

		    if (_checkTimer > _checkTime)
		    {
			    _checkTimer = 0;
			    
			    _timer += _checkTime;
			    TimeSet(_timer);

			    if (_timer > _time)
			    {
				    TimeOut();
			    }
		    }
	    }
    }

	public enum UISliderE
    {
		SafeArea_Stage_Bg_Slider,
    }
	public enum UIImageE
    {
		SafeArea_Stage_Bg_Circle_Black,
		SafeArea_Stage_Bg_Circle_Fill,
		SafeArea_Stage_Bg_Circle_Outline,
		SafeArea_Stage_Left_Icon,
		SafeArea_Stage_Middle_Icon,
		SafeArea_Stage_Right_Icon,
		SafeArea_Time_Black,
		SafeArea_Time_Fill,
		SafeArea_Time_Icon,
		SafeArea_Time_Outline,
		SafeArea_Dot,
		SafeArea_Boss_Icon,
    }
	public enum UITextProE
    {
		SafeArea_Stage_Left_Stage,
		SafeArea_Stage_Middle_Stage,
		SafeArea_Stage_Right_Stage,
		SafeArea_Boss_Stage,
    }
}