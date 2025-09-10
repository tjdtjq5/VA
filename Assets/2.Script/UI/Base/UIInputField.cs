using UnityEngine;
using UnityEngine.UI;
public class UIInputField : UIFrame
{
    InputField InputField
    {
        get
        {
            return GetComponent<InputField>();
        }
    }

    public string text
    {
        get
        {
            return InputField.text;
        }
        set
        {
            InputField.text = value;
        }
    }
    public string placeHolder
    {
        get
        {
            Graphic graphic = InputField.placeholder;
            return ((Text)(graphic)).text;
        }
        set 
        {
            Graphic graphic = InputField.placeholder;
            ((Text)(graphic)).text = value;
        }
    }
}
