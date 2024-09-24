using System;

[Serializable]
public class PlayerItemData
{
    public string ItemCode { get; set; }
    public double CountSign { get; set; }
    public double CountExp { get; set; }

    public BBNumber Count
    {
        get
        {
            return new BBNumber(CountSign, CountExp);
        }
        set
        {
            BBNumber current = new BBNumber(CountSign, CountExp);
            BBNumber next = current + value;

            CountSign = next.significand;
            CountExp = next.exponent;
        }
    }
}