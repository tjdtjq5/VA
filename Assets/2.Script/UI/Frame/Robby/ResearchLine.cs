using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ResearchLine : UIFrame
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));

        base.Initialize();
    }

    [Button]
    public void UISet(Transform start, Transform end)
    {
        float x = end.position.x - start.position.x;
        float y = start.position.y - end.position.y;
        bool isLeft = x < 0;

        GetImage(UIImageE.Horizontal).gameObject.SetActive(true);

        Vector2 horizontalSizeDelta = GetImage(UIImageE.Horizontal).RectTransform.sizeDelta;
        GetImage(UIImageE.Horizontal).RectTransform.sizeDelta = new Vector2(Mathf.Abs(x), horizontalSizeDelta.y);
        GetImage(UIImageE.Horizontal).RectTransform.pivot = new Vector2(isLeft ? 1 : 0, 0.5f);
        GetImage(UIImageE.Horizontal).transform.localPosition = Vector3.zero;

        Vector2 verticalSizeDelta = GetImage(UIImageE.Vertical).RectTransform.sizeDelta;
        GetImage(UIImageE.Vertical).RectTransform.sizeDelta = new Vector2(verticalSizeDelta.x, Mathf.Abs(y) * 0.5f);
        GetImage(UIImageE.Vertical).transform.localPosition = Vector3.zero;
    }

    public void UISetLine(Transform start, Transform end)
    {
        GetImage(UIImageE.Horizontal).gameObject.SetActive(false);

        float y = start.position.y - end.position.y;

        Vector2 verticalSizeDelta = GetImage(UIImageE.Vertical).RectTransform.sizeDelta;
        GetImage(UIImageE.Vertical).RectTransform.sizeDelta = new Vector2(verticalSizeDelta.x, Mathf.Abs(y) * 0.5f);
        GetImage(UIImageE.Vertical).transform.localPosition = Vector3.zero;
    }

	public enum UIImageE
    {
		Horizontal,
		Vertical,
    }
}