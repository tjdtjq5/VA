using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class UISlider : UIFrame
{
    public Slider Slider
    {
        get
        {
            return GetComponent<Slider>();
        }
    }
    public float value
    {
        set
        {
            Slider.value = value;
        }
    }

   
}
