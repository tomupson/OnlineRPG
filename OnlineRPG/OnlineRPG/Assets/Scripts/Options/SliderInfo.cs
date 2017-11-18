public class SliderInfo : IOptionsInfo
{
    public string Name { get; set; }
    public string Description { get; set; }
    public float? Value { get; set; }
    public float MinValue { get; set; }
    public float MaxValue { get; set; }

    public SliderInfo(string name, string description, float? value, float minValue, float maxValue)
    {
        this.Name = name;
        this.Description = description;
        this.Value = value;
        this.MinValue = minValue;
        this.MaxValue = maxValue;
    }
}