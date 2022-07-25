using CustomMapper.Attributes;

namespace CustomMapper.Tests.HelperClasses
{
    internal class UserDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [MapFrom("Name")]
        public string FullName { get; set; }

        [MapFrom("BirthDate")]
        public DateTime DateOfBirth { get; set; }

        [MapFrom("Username")]
        public string UserName { get; set; }

        [MapFrom("Email")]
        public string WorkplaceEmail { get; set; }

        public string Email { get; set; }

        [MapFrom("MembershipType")]
        public MembershipTypeDto MembershipTypeDto { get; set; }

        [MapIgnore]
        public string Secret { get; set; }

    }
}
