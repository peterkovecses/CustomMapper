using CustomMapper.Attributes;
using CustomMapper.Interfaces;
using System.Collections;
using System.Reflection;

namespace CustomMapper.Validators
{
    public class TypeValidator : ITypeValidator
    {
        public bool AreDifferentReferenceTypes(Type sourceType, Type targetType)
        {
            if (!sourceType.IsClass || !targetType.IsClass)
            {
                return false;
            }

            return sourceType != targetType;
        }

        public void CheckTypesForNull(Type sourceType, Type targetType)
        {
            if (sourceType == null || targetType == null)
            {
                throw new ArgumentNullException(nameof(sourceType));
            }
        }

        public bool CanMapSourceProperty(PropertyInfo propertyInfo)
        {
            return propertyInfo.CanRead && ValidateType(propertyInfo);
        }

        public bool CanMapTargetProperty(PropertyInfo propertyInfo)
        {
            return propertyInfo.CanWrite && !HasMapIgnoreAttribute(propertyInfo) && ValidateType(propertyInfo);
        }

        private static bool ValidateType(PropertyInfo propertyInfo)
        {
            var type = propertyInfo.PropertyType;
            return type.IsValueType || IsString(type) || IsCompatibleClass(type);
        }

        private static bool IsString(Type type)
        {
            return type == typeof(string);
        }

        private static bool IsCompatibleClass(Type type)
        {
            if (!type.IsClass)
            {
                return false;
            }

            if (type.GetInterfaces().Any(i => i == typeof(ICollection)) || type.GetInterfaces().Any(i => i.GetGenericTypeDefinition() == typeof(ICollection<>)))
            {
                return false;
            }

            return true;
        }

        private static bool HasMapIgnoreAttribute(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttributes(typeof(MapIgnoreAttribute), true).Any();
        }

        public bool CanMapByAttribute(PropertyInfo sourcePropertyInfo, PropertyInfo targetPropertyInfo)
        {
            return targetPropertyInfo.GetCustomAttribute<MapFromAttribute>()?.Name == sourcePropertyInfo.Name && !IsValueTypeConflict(targetPropertyInfo, sourcePropertyInfo);
        }

        public bool CanMapByPropertyName(PropertyInfo sourcePropertyInfo, PropertyInfo targetPropertyInfo)
        {
            return targetPropertyInfo.GetCustomAttribute<MapFromAttribute>() == null && targetPropertyInfo.Name == sourcePropertyInfo.Name && !IsValueTypeConflict(sourcePropertyInfo, targetPropertyInfo);
        }

        private static bool IsValueTypeConflict(PropertyInfo sourcePropertyInfo, PropertyInfo targetPropertyInfo)
        {
            if (!targetPropertyInfo.PropertyType.IsValueType)
            {
                return false;
            }

            return targetPropertyInfo.PropertyType != sourcePropertyInfo.PropertyType;
        }
    }
}
