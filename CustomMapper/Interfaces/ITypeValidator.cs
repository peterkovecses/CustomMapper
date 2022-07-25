using System.Reflection;

namespace CustomMapper.Interfaces
{
    public interface ITypeValidator
    {
        bool AreDifferentReferenceTypes(Type sourceType, Type targetType);
        void CheckTypesForNull(Type sourceType, Type targetType);
        bool CanMapSourceProperty(PropertyInfo propertyInfo);
        bool CanMapTargetProperty(PropertyInfo propertyInfo);
        bool CanMapByAttribute(PropertyInfo p, PropertyInfo sourcePropertyInfo);
        bool CanMapByPropertyName(PropertyInfo p, PropertyInfo sourcePropertyInfo);
    }
}