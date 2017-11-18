public class ToggleInfo : IOptionsInfo
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool? IsChecked { get; set; }

    public ToggleInfo(string name, string description, bool? isChecked)
    {
        this.Name = name;
        this.Description = description;
        this.IsChecked = isChecked;
    }
}