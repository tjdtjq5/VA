public class PlayerItemData
{
    public string ItemCode { get; set; }
    public decimal SignValue { get; set; }
    public decimal ExpValue { get; set; }
    public BBNumber Count()
    {
        return new BBNumber((double)SignValue, (double)ExpValue);
    }
    public void Set(BBNumber value)
    {
        SignValue = (decimal)value.significand;
        ExpValue = (decimal)value.exponent;
    }
    public void Add(BBNumber value)
    {
        BBNumber c = Count();
        c += value;
        Set(c);
    }
    public void Sub(BBNumber value)
    {
        BBNumber c = Count();
        c -= value;
        Set(c);
    }
    public void Mul(BBNumber value)
    {
        BBNumber c = Count();
        c *= value;
        Set(c);
    }
    public void Div(BBNumber value)
    {
        BBNumber c = Count();
        c /= value;
        Set(c);
    }
}