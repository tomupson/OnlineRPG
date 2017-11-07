using System;

[AttributeUsage(AttributeTargets.Class)]
public class ItemCategoryAttribute : Attribute
{
    public string category { get; set; }

    public ItemCategoryAttribute(string category)
    {
        this.category = category;
    }
}