using CustomMapper.Attributes;

namespace CustomMapper.Tests.HelperClasses
{
    internal class MembershipType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Name2 { get; set; }
        public decimal Price { get; set; }
        public byte DiscountRate { get; set; }
    }
}
