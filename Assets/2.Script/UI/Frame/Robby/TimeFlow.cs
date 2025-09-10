using System;

public class TimeFlow : UIFrame
{
    public Action OnTimeFlow;
    public Action OnTimeEnd;

    public TimeSpan TimeSpan => _timeSpan;
    private TimeSpan _timeSpan;

    private float _flowSec = 1f;
    private float _flowTimer = 0f;
    private bool _isFlow = false;

    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UITextPro>(typeof(UITextProE));

        base.Initialize();
    }

    public void UISet(int timeSec, bool isFlow = true)
    {
        UISet(TimeSpan.FromSeconds(timeSec), isFlow);
    }
    public void UISet(TimeSpan timeSpan, bool isFlow = true)
    {
        _timeSpan = timeSpan;
        TimeSet();
        _isFlow = isFlow;
    }

    private void TimeSet()
    {
        int hour = _timeSpan.Hours;
        int minute = _timeSpan.Minutes;
        int second = _timeSpan.Seconds;

        GetTextPro(UITextProE.Text).text = $"{hour:D2}:{minute:D2}:{second:D2}";

        _flowTimer = 0f;
    }

    private void FixedUpdate()
    {
        if (!_isFlow)
        {
            return;
        }

        _flowTimer += Managers.Time.FixedDeltaTime;
        if (_flowTimer >= _flowSec)
        {
            _flowTimer = 0f;
            _timeSpan = _timeSpan.Subtract(TimeSpan.FromSeconds(_flowSec));

            if (_timeSpan.TotalSeconds <= 0)
                _timeSpan = TimeSpan.Zero;
                
            TimeSet();
            OnTimeFlow?.Invoke();

            if (_timeSpan.TotalSeconds <= 0)
            {
                OnTimeEnd?.Invoke();
                _isFlow = false;
            }
        }
    }
	public enum UIImageE
    {
		Icon,
    }
	public enum UITextProE
    {
		Text,
    }
}