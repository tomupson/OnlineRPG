public class DropdownInfo : IOptionsInfo
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int? Index { get; set; }

    public DropdownInfo(string name, string description, int? index)
    {
        this.Name = name;
        this.Description = description;
        this.Index = index;
    }
}