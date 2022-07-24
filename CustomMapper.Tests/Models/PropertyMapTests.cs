using CustomMapper.Models;
using CustomMapper.Tests.Models.Helpers;

namespace CustomMapper.Tests.Models
{
    public class PropertyMapTests
    {
        [Fact]
        public void PropertyMap_WhenPropertiesSetOnInstantiation_PropertiesOfTheInstanceAreTheSame()
        {
            // Arrange
            var sourceProperty = typeof(SourceModel).GetProperties().First();
            var targetProperty = typeof(TargetModel).GetProperties().First();

            // Act
            var propertyMap = new PropertyMap { SourcePropertyInfo = sourceProperty, TargetPropertyInfo = targetProperty };

            // Assert
            Assert.Equal(sourceProperty, propertyMap.SourcePropertyInfo);
            Assert.Equal(targetProperty, propertyMap.TargetPropertyInfo);
        }
    }
}
