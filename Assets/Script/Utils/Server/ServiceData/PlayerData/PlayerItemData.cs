public class PlayerItemData
{
    public string ItemCode { get; set; }
    public double SignValue { get; set; }
    public double ExpValue { get; set; }
    public BBNumber Count()
    {
        return new BBNumber(SignValue, ExpValue);
    }
    public void Set(BBNumber value)
    {
        SignValue = value.significand;
        ExpValue = value.exponent;
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