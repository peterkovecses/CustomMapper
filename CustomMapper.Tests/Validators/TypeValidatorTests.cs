using CustomMapper.Tests.HelperClasses;
using CustomMapper.Validators;

namespace CustomMapper.Tests.Validators
{
    public class TypeValidatorTests
    {
        private readonly TypeValidator _sut;

        public TypeValidatorTests()
        {
            _sut = new TypeValidator();
        }

        [Fact]
        public void TypeValidator_AreDifferentReferenceTypesWhenSourceTypeIsValueType_ReturnsFalse()
        {
            // Arrange
            var sourceType = typeof(int);
            var targetType = typeof(string);

            // Act
            var actual = _sut.AreDifferentReferenceTypes(sourceType, targetType);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void TypeValidator_AreDifferentReferenceTypesWhenTargetTypeIsValueType_ReturnsFalse()
        {
            // Arrange
            var sourceType = typeof(string);
            var targetType = typeof(double);

            // Act
            var actual = _sut.AreDifferentReferenceTypes(sourceType, targetType);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void TypeValidator_AreDifferentReferenceTypesWhenArgumentsAreReferenceTypesButNotTheSameTypes_ReturnsTrue()
        {
            // Arrange
            var sourceType = typeof(User);
            var targetType = typeof(UserDto);

            // Act
            var actual = _sut.AreDifferentReferenceTypes(sourceType, targetType);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void TypeValidator_AreDifferentReferenceTypesWhenArgumentsAreSameReferenceTypess_ReturnsFalse()
        {
            // Arrange
            var sourceType = typeof(string);
            var targetType = typeof(string);

            // Act
            var actual = _sut.AreDifferentReferenceTypes(sourceType, targetType);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void TypeValidator_CheckTypesForNullWhenSourceTypeIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            Type? sourceType = null;
            var targetType = typeof(string);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _sut.CheckTypesForNull(sourceType, targetType));
        }

        [Fact]
        public void TypeValidator_CheckTypesForNullWhenTargetTypeIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var sourceType = typeof(string);
            Type? targetType = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _sut.CheckTypesForNull(sourceType, targetType));
        }

        [Fact]
        public void TypeValidator_CheckTypesForNullWhenNoneOfTheArgumentsIsNull_NotThrowsAnyException()
        {
            // Arrange
            var sourceType = typeof(string);
            var targetType = typeof(double);

            // Act
            var exception = Record.Exception(() => _sut.CheckTypesForNull(sourceType, targetType));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void TypeValidator_CanMapSourcePropertyWhenPropertyHasNoGetter_ReturnsFalse()
        {
            // Arrange
            var propertyInfo = typeof(Dog).GetProperty("Id");

            // Act
            var actual = _sut.CanMapSourceProperty(propertyInfo);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void TypeValidator_CanMapSourcePropertyWhenPropertyHasGetter_ReturnsTrue()
        {
            // Arrange
            var propertyInfo = typeof(Dog).GetProperty("Name");

            // Act
            var actual = _sut.CanMapSourceProperty(propertyInfo);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void TypeValidator_CanMapTargetPropertyWhenPropertyHasNoSetter_ReturnsFalse()
        {
            // Arrange
            var propertyInfo = typeof(Dog).GetProperty("Variety");

            // Act
            var actual = _sut.CanMapTargetProperty(propertyInfo);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void TypeValidator_CanMapTargetPropertyWhenPropertyHasSetter_ReturnsTrue()
        {
            // Arrange
            var propertyInfo = typeof(Dog).GetProperty("Name");

            // Act
            var actual = _sut.CanMapTargetProperty(propertyInfo);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void TypeValidator_CanMapByAttributeWhenAttributeNameIsTheSameAsPropertyNameButDifferentValueTypes_ReturnsFalse()
        {
            // Arrange
            var sourcePropertyInfo = typeof(User).GetProperty("TaxNumber");
            var targetPropertyInfo = typeof(UserDto).GetProperty("TaxCode");

            // Act
            var actual = _sut.CanMapByAttribute(sourcePropertyInfo, targetPropertyInfo);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void TypeValidator_CanMapByAttributeWhenAttributeNameIsTheSameAsPropertyNameAndSameValueTypes_ReturnsTrue()
        {
            // Arrange
            var sourcePropertyInfo = typeof(MembershipType).GetProperty("Id");
            var targetPropertyInfo = typeof(MembershipTypeDto).GetProperty("Id2");

            // Act
            var actual = _sut.CanMapByAttribute(sourcePropertyInfo, targetPropertyInfo);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void TypeValidator_CanMapByAttributeWhenAttributeNameIsNotTheSameAsPropertyName_ReturnsFalse()
        {
            // Arrange
            var sourcePropertyInfo = typeof(MembershipTypeDto).GetProperty("Price");
            var targetPropertyInfo = typeof(MembershipType).GetProperty("Price");

            // Act
            var actual = _sut.CanMapByAttribute(sourcePropertyInfo, targetPropertyInfo);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void TypeValidator_CanMapByPropertyNameWhenPropertyNamesAreTheSameButDifferentValueTypes_ReturnsFalse()
        {
            // Arrange
            var sourcePropertyInfo = typeof(MembershipType).GetProperty("Id");
            var targetPropertyInfo = typeof(MembershipTypeDto).GetProperty("Id");

            // Act
            var actual = _sut.CanMapByPropertyName(sourcePropertyInfo, targetPropertyInfo);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void TypeValidator_CanMapByPropertyNameWhenPropertyNamesAreTheSameAndSameValueTypes_ReturnsTrue()
        {
            // Arrange
            var sourcePropertyInfo = typeof(MembershipTypeDto).GetProperty("DiscountRate");
            var targetPropertyInfo = typeof(MembershipType).GetProperty("DiscountRate");

            // Act
            var actual = _sut.CanMapByPropertyName(sourcePropertyInfo, targetPropertyInfo);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void TypeValidator_CanMapByPropertyNameWhenPropertyNamesAreNotTheSame_ReturnsFalse()
        {
            // Arrange
            var sourcePropertyInfo = typeof(MembershipTypeDto).GetProperty("Name");
            var targetPropertyInfo = typeof(MembershipType).GetProperty("Name2");

            // Act
            var actual = _sut.CanMapByPropertyName(sourcePropertyInfo, targetPropertyInfo);

            // Assert
            Assert.False(actual);
        }
    }
}
