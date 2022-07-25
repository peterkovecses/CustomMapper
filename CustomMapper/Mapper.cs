using CustomMapper.Attributes;
using CustomMapper.Interfaces;
using CustomMapper.Models;
using System.Collections;
using System.Reflection;

namespace CustomMapper
{
    public class Mapper : ICustomMapper
    {
        private readonly Dictionary<string, PropertyMap[]> _maps = new();

        public T2 Map<T1, T2>(T1 source) where T1 : class where T2 : class, new()
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return GetTargetItem<T2>(source);
        }

        private T2 GetTargetItem<T2>(object source) where T2 : class, new()
        {
            var target = new T2();

            var propertyMaps = GetPropertyMaps(source, target);

            Copy(source, target, propertyMaps);

            return target;
        }

        private PropertyMap[] GetPropertyMaps(object source, object target)
        {
            var sourceType = source.GetType();
            var targetType = target.GetType();

            var key = GetMapKey(sourceType, targetType);

            if (!_maps.ContainsKey(key))
            {
                MapTypes(sourceType, targetType, key);
            }

            return _maps[key];
        }

        private static string GetMapKey(Type sourceType, Type targetType)
        {
            return $"{sourceType.FullName}_To_{targetType.FullName}";
        }

        private void MapTypes(Type source, Type target, string key)
        {
            if (_maps.ContainsKey(key))
            {
                return;
            }

            var properties = GetPropertyMaps(source, target);
            _maps.Add(key, properties.ToArray());
        }

        private void Copy(object source, object target, PropertyMap[] propertyMaps)
        {
            for (var i = 0; i < propertyMaps.Length; i++)
            {
                var propertyMap = propertyMaps[i];

                var sourceValue = propertyMap.SourcePropertyInfo.GetValue(source, null);

                if (sourceValue != null)
                {
                    if (!AreDifferentReferenceTypes(propertyMap.SourcePropertyInfo.PropertyType, propertyMap.TargetPropertyInfo.PropertyType))
                    {
                        propertyMap.TargetPropertyInfo.SetValue(target, sourceValue, null);
                    }

                    else
                    {
                        var sType = propertyMap.SourcePropertyInfo.PropertyType;
                        var tType = propertyMap.TargetPropertyInfo.PropertyType;

                        var methodInfo = typeof(Mapper).GetMethod("Map")?.MakeGenericMethod(sType, tType);
                        var transformedValue = methodInfo?.Invoke(this, new object[] { sourceValue });

                        propertyMap.TargetPropertyInfo.SetValue(target, transformedValue, null);
                    }
                }
            }
        }

        private static bool AreDifferentReferenceTypes(Type sourceType, Type targetType)
        {
            if (!sourceType.IsClass)
            {
                return false;
            }

            return sourceType != targetType;
        }

        private static IEnumerable<PropertyMap> GetPropertyMaps(Type sourceType, Type targetType)
        {
            CheckTypesForNull(sourceType, targetType);

            var sourcePropertyInfos = sourceType.GetProperties().Where(p => CanMapSourceProperty(p));
            var targetPropertyInfos = targetType.GetProperties().Where(p => CanMapTargetProperty(p));

            return CreatePropertyMaps(sourcePropertyInfos, targetPropertyInfos);
        }

        private static void CheckTypesForNull(Type sourceType, Type targetType)
        {
            if (sourceType == null || targetType == null)
            {
                throw new ArgumentNullException(nameof(sourceType));
            }
        }

        private static bool CanMapSourceProperty(PropertyInfo propertyInfo)
        {
            return propertyInfo.CanRead && ValidateType(propertyInfo);
        }

        private static bool CanMapTargetProperty(PropertyInfo propertyInfo)
        {
            return propertyInfo.CanWrite && !HasMapIgnoreAttribute(propertyInfo) && ValidateType(propertyInfo);
        }

        private static bool ValidateType(PropertyInfo propertyInfo)
        {
            var type = propertyInfo.PropertyType;
            return type.IsValueType || IsString(type) || IsCompatibleClass(type);
        }

        private static bool HasMapIgnoreAttribute(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttributes(typeof(MapIgnoreAttribute), true).Any();
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

        private static IEnumerable<PropertyInfo> CheckForSuitableTargetProperty(PropertyInfo sourcePropertyInfo, IEnumerable<PropertyInfo> targetPropertyInfos)
        {
            return targetPropertyInfos.Where(p => CanMapByAttribute(p, sourcePropertyInfo) || CanMapByPropertyName(p, sourcePropertyInfo));
        }

        private static bool CanMapByAttribute(PropertyInfo p, PropertyInfo sourcePropertyInfo)
        {
            return p.GetCustomAttribute<MapFromAttribute>()?.Name == sourcePropertyInfo.Name && !IsValueTypeConflict(p, sourcePropertyInfo);
        }

        private static bool CanMapByPropertyName(PropertyInfo p, PropertyInfo sourcePropertyInfo)
        {
            return p.GetCustomAttribute<MapFromAttribute>() == null && p.Name == sourcePropertyInfo.Name && !IsValueTypeConflict(p, sourcePropertyInfo);
        }

        private static bool IsValueTypeConflict(PropertyInfo p, PropertyInfo sourcePropertyInfo)
        {
            if (!p.PropertyType.IsValueType)
            {
                return false;
            }

            return p.PropertyType != sourcePropertyInfo.PropertyType;
        }

        private static IEnumerable<PropertyMap> CreatePropertyMaps(IEnumerable<PropertyInfo> sourcePropertyInfos, IEnumerable<PropertyInfo> targetPropertyInfos)
        {
            foreach (var sourcePropertyInfo in sourcePropertyInfos)
            {
                var suitableTargetPropertyInfos = CheckForSuitableTargetProperty(sourcePropertyInfo, targetPropertyInfos);

                foreach (var propertyInfo in suitableTargetPropertyInfos)
                {
                    yield return new PropertyMap { SourcePropertyInfo = sourcePropertyInfo, TargetPropertyInfo = propertyInfo };
                }
            }
        }
    }
}
