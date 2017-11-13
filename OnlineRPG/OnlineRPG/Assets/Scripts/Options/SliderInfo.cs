public class SliderInfo : IOptionsInfo
{
    public string Name { get; set; }
    public float? Value { get; set; }
    public float MinValue { get; set; }
    public float MaxValue { get; set; }

    public SliderInfo(string name, float? value, float minValue, float maxValue)
    {
        this.Name = name;
        this.Value = value;
        this.MinValue = minValue;
        this.MaxValue = maxValue;
    }
}