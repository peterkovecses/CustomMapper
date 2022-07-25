using CustomMapper.Attributes;
using CustomMapper.Interfaces;
using CustomMapper.Models;
using CustomMapper.Validators;
using System.Collections;
using System.Reflection;

namespace CustomMapper
{
    /// <summary>
    /// It is an object to object mapper for .NET. Not for all types, just for practicing Reflection.
    /// </summary>
    public class Mapper : ICustomMapper
    {
        private readonly Dictionary<string, PropertyMap[]> _maps;
        private readonly ITypeValidator _typeValidator;

        public Mapper(ITypeValidator typeValidator)
        {
            _maps = new Dictionary<string, PropertyMap[]>();
            _typeValidator = typeValidator;
        }

        public Mapper() : 
            this(new TypeValidator())
        {

        }

        /// Execute mapping from the source object to a new target object.
        /// </summary>
        /// <typeparam name="T1">Source type</typeparam>
        /// <typeparam name="T2">Target type</typeparam>
        /// <param name="source">Source object to map from</param>
        /// <returns>Mapped target object</returns>
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
                    if (!_typeValidator.AreDifferentReferenceTypes(propertyMap.SourcePropertyInfo.PropertyType, propertyMap.TargetPropertyInfo.PropertyType))
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

        private IEnumerable<PropertyMap> GetPropertyMaps(Type sourceType, Type targetType)
        {
            _typeValidator.CheckTypesForNull(sourceType, targetType);

            var sourcePropertyInfos = sourceType.GetProperties().Where(p => _typeValidator.CanMapSourceProperty(p));
            var targetPropertyInfos = targetType.GetProperties().Where(p => _typeValidator.CanMapTargetProperty(p));

            return CreatePropertyMaps(sourcePropertyInfos, targetPropertyInfos);
        }

        private IEnumerable<PropertyInfo> CheckForSuitableTargetProperty(PropertyInfo sourcePropertyInfo, IEnumerable<PropertyInfo> targetPropertyInfos)
        {
            return targetPropertyInfos.Where(p => _typeValidator.CanMapByAttribute(sourcePropertyInfo, p) || _typeValidator.CanMapByPropertyName(sourcePropertyInfo, p));
        }

        private IEnumerable<PropertyMap> CreatePropertyMaps(IEnumerable<PropertyInfo> sourcePropertyInfos, IEnumerable<PropertyInfo> targetPropertyInfos)
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
