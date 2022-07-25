using CustomMapper.Tests.HelperClasses;

namespace CustomMapper.Tests
{
    public class MapperTests
    {
        private readonly Mapper _sut;

        public MapperTests()
        {
            _sut = new Mapper();
        }

        private static MembershipType GetMembershipType()
        {
            return new MembershipType { Id = 1, Name = "Gold", DiscountRate = 20 };
        }

        private static MembershipTypeDto GetMembershipTypeDto()
        {
            return new MembershipTypeDto { Id = 2, Name = "Silver", DiscountRate = 15 };
        }

        private User GetUser()
        {
            var membershipType = GetMembershipType();
            return new User { Name = "Jane Smith", BirthDate = new DateTime(1990, 05, 07), UserName = "janesmith89", Email = "janesmith89@gmail.com", MembershipType = membershipType, Secret = "secret" };
        }

        private UserDto GetUserDto()
        {
            var membershipType = GetMembershipTypeDto();
            return new UserDto { Name = "Jane Smith", DateOfBirth = new DateTime(1990, 05, 07), UserName = "janesmith89", Email = "janesmith89@gmail.com", WorkplaceEmail = "jane.smith6@myworspace.com", MembershipTypeDto = membershipType, Secret = "secret" };
        }

        [Fact]
        public void Mapper_MapWhenPropertyHasMapIgnoreAttribute_NotMapped()
        {
            // Arrange
            var source = GetUser();
            string? expected = null;

            // Act
            var target = _sut.Map<User, UserDto>(source);

            // Assert
            Assert.Equal(expected, target.Secret);
        }

        [Fact]
        public void MyCustomMapper_MapWhenSourceIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var source = new User();
            source = null;

            // Act & Assert
#pragma warning disable CS8604
            Assert.Throws<ArgumentNullException>(() => _sut.Map<User, UserDto>(source));
#pragma warning restore CS8604
        }

        [Fact]
        public void Mapper_MapByNameWhenPropertiesAreStrings_PropertyIsMapped()
        {
            // Arrange
            var source = GetMembershipType();

            // Act
            var target = _sut.Map<MembershipType, MembershipTypeDto>(source);

            // Assert
            Assert.Equal(source.Name, target.Name);
        }

        [Fact]
        public void Mapper_MapByNameWhenPropertiesAreSameValueTypes_PropertyIsMapped()
        {
            // Arrange
            var source = GetMembershipType();

            // Act
            var target = _sut.Map<MembershipType, MembershipTypeDto>(source);

            // Assert
            Assert.Equal(source.DiscountRate, target.DiscountRate);
        }

        [Fact]
        public void Mapper_MapByAttributeWhenPropertiesAreSameValueTypes_PropertyIsMapped()
        {
            // Arrange
            var source = GetMembershipType();

            // Act
            var target = _sut.Map<MembershipType, MembershipTypeDto>(source);

            // Assert
            Assert.Equal(source.Id, target.Id2);
        }

        [Fact]
        public void Mapper_MapByNameWhenPropertiesAreDifferentValueTypes_PropertyIsNotMapped()
        {
            // Arrange
            var source = GetMembershipType();
            var expected = 0;

            // Act
            var target = _sut.Map<MembershipType, MembershipTypeDto>(source);

            // Assert
            Assert.Equal(expected, target.Id);
        }

        [Fact]
        public void Mapper_MapByAttributeWhenPropertiesAreDifferentValueTypes_PropertyIsNotMapped()
        {
            // Arrange
            var source = GetMembershipType();
            var expected = 0;

            // Act
            var target = _sut.Map<MembershipType, MembershipTypeDto>(source);

            // Assert
            Assert.Equal(expected, target.Id3);
        }

        [Fact]
        public void Mapper_MapWhenSourceHasNestedObject_PropertyCanBeMapped()
        {
            // Arrange
            var source = GetUser();

            // Act
            var target = _sut.Map<User, UserDto>(source);

            // Assert
            Assert.Equal(source.MembershipType.Id, target.MembershipTypeDto.Id2);
            Assert.Equal(source.MembershipType.Name, target.MembershipTypeDto.Name);
            Assert.Equal(source.MembershipType.DiscountRate, target.MembershipTypeDto.DiscountRate);
        }

        [Fact]
        public void Mapper_MapWhenCanMapByAttribute_NotMapByName()
        {
            // Arrange
            var source = GetUserDto();

            // Act
            var target = _sut.Map<UserDto, User>(source);

            // Assert
            Assert.Equal(source.WorkplaceEmail, target.Email);
        }

        [Fact]
        public void Mapper_MapWhenPossibleToMapOnePropertyByAttributeAndAnotherByName_BothMapped()
        {
            // Arrange
            var source = GetUser();

            // Act
            var target = _sut.Map<User, UserDto>(source);

            // Assert
            Assert.Equal(source.Name, target.Name);
            Assert.Equal(source.Name, target.FullName);
        }

        [Fact]
        public void Mapper_MapWhenThereIsAnAttributeButThereIsNoMatchForIt_NotMappedBasedOnAttributeNam()
        {
            // Arrange
            var source = GetUser();

            // Act
            var target = _sut.Map<User, UserDto>(source);

            // Assert
            Assert.Null(target.UserName);
        }
    }
}
 