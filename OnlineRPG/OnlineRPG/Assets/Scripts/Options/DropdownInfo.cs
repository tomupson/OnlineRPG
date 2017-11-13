public class DropdownInfo : IOptionsInfo
{
    public string Name { get; set; }
    public int? Index { get; set; }

    public DropdownInfo(string name, int? index)
    {
        this.Name = name;
        this.Index = index;
    }
}