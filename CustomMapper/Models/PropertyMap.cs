using System.Reflection;

namespace CustomMapper.Models
{
    public class PropertyMap
    {
        public PropertyInfo SourcePropertyInfo { get; set; }
        public PropertyInfo TargetPropertyInfo { get; set; }
    }
}
