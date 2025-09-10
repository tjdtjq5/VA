using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Indicator : MonoBehaviour
{
    [SerializeField]
    private RectTransform canvas;

    [Header("Main")]
    // Default Image
    [SerializeField]
    private Image mainImage;
    // Default Image의 Fill
    [SerializeField]
    private Image mainImageFill;
    // Charge에 쓰이는 Fill
    [SerializeField]
    private Image fillImage;

    [Header("Border")]
    [SerializeField]
    private RectTransform leftBorder;
    [SerializeField]
    private RectTransform rightBorder;

    private float radius;
    private float angle = 360f;
    private float fillAmount;

    public float Radius
    {
        get => radius;
        set
        {
            radius = Mathf.Max(value, 0f);
            // 기본 Scale 0.01 * 2 * radius = 0.01 * 2r = 지름
            canvas.localScale = Vector2.one * 0.02f * radius;
        }
    }

    public float Angle
    {
        get => angle;
        set
        {
            angle = Mathf.Clamp(value, 0f, 360f);
            mainImage.fillAmount = angle / 360f;
            mainImageFill.fillAmount = mainImage.fillAmount; 
            fillImage.fillAmount = mainImage.fillAmount;

            canvas.transform.eulerAngles = new Vector3(90f, -angle * 0.5f, 0f);

            if (Mathf.Approximately(mainImage.fillAmount, 1f))
            {
                leftBorder.gameObject.SetActive(false);
                rightBorder.gameObject.SetActive(false);
            }
            else
            {
                leftBorder.gameObject.SetActive(true);
                rightBorder.gameObject.SetActive(true);
                rightBorder.transform.localEulerAngles = new Vector3(0f, 0f, 180f - angle);
            }
        }
    }

    public float FillAmount
    {
        get => fillAmount;
        set
        {
            fillAmount = Mathf.Clamp01(value);
            fillImage.transform.localScale = Vector3.one * fillAmount;
        }
    }

    public Transform TraceTarget
    {
        get => transform.parent;
        set
        {
            transform.parent = value;
            transform.localPosition = new Vector3(0f, 0.01f, 0f);
            transform.localRotation = Quaternion.identity;
        }
    }

    public void Setup(float angle, float radius, float fillAmount, Transform traceTarget = null)
    {
        Angle = angle;
        Radius = radius;
        TraceTarget = traceTarget;
        FillAmount = fillAmount;
    }
    public void Setup(float angle, float radius, float fillAmount, Vector3 pos)
    {
        Angle = angle;
        Radius = radius;
        TraceTarget = null;
        FillAmount = fillAmount;
        this.transform.position = pos;
    }
}
