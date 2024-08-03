using Microservices.OrderDomainCore;

namespace Microservices.OrderDomain.OrderAggregates
{
    public class Address : ValueObject
    {
        public string Province { get; private set; } = null!;
        public string District { get; private set; } = null!;
        public string Street { get; private set; } = null!;
        public string ZipCode { get; private set; } = null!;
        public string Line { get; private set; } = null!;
        public Address()
        {
            
        }
        public Address(string province, string district, string street, string zipCode, string line)
        {
            Province = province;
            District = district;
            Street = street;
            ZipCode = zipCode;
            Line = line;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Province;
            yield return District;
            yield return Street;
            yield return ZipCode;
            yield return Line;
        }
    }
}
