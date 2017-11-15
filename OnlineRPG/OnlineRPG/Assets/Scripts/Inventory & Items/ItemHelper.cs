using System;

public static class ItemHelper
{
    public static Type GetTypeFromItemCategory(string category, bool acceptInheritance = false)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies)
        {
            var assemblyTypes = assembly.GetTypes();

            foreach (Type type in assemblyTypes)
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