using System;

public static class ItemHelper
{
    public static Type GetTypeFromItemCategory(string category, bool acceptInheritance = false)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies)
        {
            var assembly_types = assembly.GetTypes();

            foreach (Type type in assembly_types)
            {
                if (type.IsDefined(typeof(ItemCategoryAttribute), acceptInheritance))
                {
                    ItemCategoryAttribute categoryAttribute = Attribute.GetCustomAttribute(type, typeof(ItemCategoryAttribute)) as ItemCategoryAttribute;
                    if (categoryAttribute.category.ToLower() == category.ToLower())
                    {
                        return type;
                    }
                }
            }
        }

        return null;
    }
}