using System;
using System.Reflection;

namespace UnitTestProject2
{
    public static class PropertyInfoExtensions
    {
        public static bool HasAttribute<TAttribute>(this PropertyInfo propertyInfo)
            where TAttribute : Attribute
        {
            return propertyInfo.GetCustomAttribute<TAttribute>() != null;
        }
    }
}