using CustomMapper.Attributes;

namespace CustomMapper.Tests.HelperClasses
{
    internal class MembershipTypeDto
    {
        public byte Id { get; set; }

        [MapFrom("Id")]
        public int Id2 { get; set; }

        [MapFrom("Id")]
        public byte Id3 { get; set; }

        public string Name { get; set; }

        [MapFrom("Cost")]
        public decimal Price { get; set; }
        public byte DiscountRate { get; set; }
    }
}
