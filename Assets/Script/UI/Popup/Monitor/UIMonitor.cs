using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMonitor : UIPopup
{
	bool _isOpen;

	[SerializeField] GameObject obj;
	[SerializeField] Reporter reporter;

    private void Start()
    {
        DontDestroyOnLoad(obj);
        Close();
        Managers.Input.AddMousePressedTimeAction(3, Switch);
    }

    void Switch()
	{
		if (_isOpen)
		{
			Close();
        }
		else
		{
			Open();
        }
    }

    void Open()
	{
		_isOpen = true;
        obj.SetActive(_isOpen);
        reporter.enabled = _isOpen;
    }
	void Close()
	{
        _isOpen = false;
        obj.SetActive(_isOpen);
        reporter.enabled = _isOpen;
    }
}
