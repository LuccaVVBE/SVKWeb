using System;
using JetBrains.Annotations;
using Shouldly;
using Svk.Domain;
using Xunit;

namespace Svk.Tests;

[TestSubject(typeof(Address))]
public class AddressTest
{
    [Fact]
    public void Be_created_when_valid()
    {
        //Arrange
        const string street = "Arbeidstraat 14";
        const string postalCode = "9300";
        const string houseNumber = "9";
        const string country = "BE";

        //Act
        var address = new Address(street, postalCode, houseNumber, country);

        address.Street.ShouldBe(street);
        address.Postcode.ShouldBe(postalCode);
        address.HouseNr.ShouldBe(houseNumber);
        address.CountryIsoCode.ShouldBe(country);
    }

    [Fact]
    public void Throw_when_invalid_street()
    {
        //Arrange
        const string street = "Arbeidstraat 14";
        const string postalCode = "9300";
        const string houseNumber = "";
        const string country = "BE";

        //Act
        Action act = () =>
        {
            var address = new Address(street, postalCode, houseNumber, country);
        };

        //Assert
        act.ShouldThrow<Exception>();
    }
}