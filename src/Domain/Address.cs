using Ardalis.GuardClauses;
using Svk.Domain.Common;

namespace Svk.Domain
{
    public class Address : ValueObject
    {
        public Address(string street, string postCode, string huisNr, string countryIso)
        {
            Street = Guard.Against.NullOrWhiteSpace(street, nameof(Street));
            Postcode = Guard.Against.NullOrWhiteSpace(postCode, nameof(postCode));
            HouseNr = Guard.Against.NullOrWhiteSpace(huisNr, nameof(huisNr));
            CountryIsoCode = Guard.Against.NullOrWhiteSpace(countryIso, nameof(countryIso));
        }

        public Address()
        {
        }

        public string Street { get; }
        public string Postcode { get; }
        public string HouseNr { get; }
        public string CountryIsoCode { get; }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Street.ToLower();
            yield return Postcode.ToLower();
            yield return HouseNr.ToLower();
            yield return CountryIsoCode.ToLower();
        }
    }
}