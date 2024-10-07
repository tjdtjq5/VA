using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIJoystic : UIPopup
{
    protected override bool IsSort => false;

    [SerializeField] Transform tr;
	[SerializeField] Transform in_tr;
	float MaxRadisus => 75f;

	Vector3 Direction
	{
		get
		{
			if (!isPointDown)
                return Vector3.zero;

			return in_tr.localPosition.normalized;
        }
    }

	bool isPointDown;

	protected override void Initialize()
	{
		base.Initialize();
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIButton>(typeof(UIButtonE));
	}

	protected override void UISet()
	{
        tr.gameObject.SetActive(false);

        GetButton(UIButtonE.TouchBackGround).AddPointDownEvent(PointDownEvent);
		GetButton(UIButtonE.TouchBackGround).AddPointUpEvent(PointUpEvent);
		GetButton(UIButtonE.TouchBackGround).AddDragEvent(DrayEvent);
	}

	void PointDownEvent(PointerEventData ped)
	{
		isPointDown = true;
        tr.gameObject.SetActive(true);
        tr.transform.position = ped.position;
    }
    void PointUpEvent(PointerEventData ped)
    {
        isPointDown = false;
        tr.gameObject.SetActive(false);

        Managers.Observer.OnJoystic.Invoke(Vector3.zero);
    }
	void DrayEvent(PointerEventData ped)
    {
        in_tr.position = ped.position;
		in_tr.localPosition = Vector3.ClampMagnitude(in_tr.localPosition, MaxRadisus);
    }

    private void FixedUpdate()
    {
		if (isPointDown) 
		{
            Managers.Observer.OnJoystic.Invoke(Direction);
        }
    }

    public enum UIImageE
    {
		TouchBackGround,
		Out,
		Out_In,
    }
	public enum UIButtonE
    {
		TouchBackGround,
    }
}
