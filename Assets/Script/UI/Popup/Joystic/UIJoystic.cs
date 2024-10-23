using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIJoystic : UIScene
{
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

    private void FixedUpdate()
    {
		if (isPointDown) 
		{
            in_tr.position = Input.mousePosition;
            in_tr.localPosition = Vector3.ClampMagnitude(in_tr.localPosition, MaxRadisus);

            Managers.Observer.OnJoystic.Invoke(Direction);
        }
    }

    public enum UIImageE
    {
		Out,
		Out_In,
    }
	public enum UIButtonE
    {
		TouchBackGround,
    }
}
