public class ToggleInfo : IOptionsInfo
{
    public string Name { get; set; }
    public bool? IsChecked { get; set; }

    public ToggleInfo(string name, bool? isChecked)
    {
        this.Name = name;
        this.IsChecked = isChecked;
    }
}