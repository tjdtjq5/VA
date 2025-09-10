using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class DirectionMapLine : UIFrame
{
    public void UISet(Transform start, Transform end)
    {
        this.transform.position = (start.position + end.position) / 2f;

        Vector3 startPos = start.position;
        Vector3 endPos = end.position;

        Vector3 direction = (endPos - startPos).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;

        float distance = Vector3.Distance(startPos, endPos);
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, distance);

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
