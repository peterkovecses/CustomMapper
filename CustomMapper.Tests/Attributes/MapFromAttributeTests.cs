using CustomMapper.Attributes;
using System.Reflection;

namespace CustomMapper.Tests.Attributes
{
    internal class HelperClass
    {
        [MapFrom("name")]
        public string HelperProperty { get; set; }
    }

    public class MapFromAttributeTests
    {
        [Fact]
        public void MapFromAttribute_WhenSetupNameForTheAttribute_GetBackTheSameNameFromProperty()
        {
            // Arrange
            var expected = "name";

            // Act
            var actual = typeof(HelperClass).GetProperties().First().GetCustomAttribute<MapFromAttribute>()?.Name;

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
