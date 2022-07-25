using CustomMapper.Attributes;

namespace CustomMapper.Tests.HelperClasses
{
    internal class User
    {
        public int Id { get; set; }

        [MapFrom("FullName")]
        public string Name { get; set; }

        [MapFrom("BirthDate")]
        public DateTime BirthDate { get; set; }
        
        public string UserName { get; set; }

        [MapFrom("WorkplaceEmail")]
        public string Email { get; set; }

        [MapFrom("MembershipTypeDto")]
        public MembershipType MembershipType { get; set; }

        [MapIgnore]
        public string Secret { get; set; }
    }
}
