using System;
using System.Reflection;

namespace MSBuildTaskParameterLogger.Extensions
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