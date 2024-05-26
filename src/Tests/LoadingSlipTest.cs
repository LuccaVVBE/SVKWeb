using System;
using JetBrains.Annotations;
using Shouldly;
using Svk.Domain;
using Svk.Fakers.Common;
using Xunit;

namespace Svk.Tests;

[TestSubject(typeof(LoadingSlip))]
public class LoadingSlipTest
{
    [Fact]
    public void Be_created_when_valid()
    {
        //Arange
        const int laadNumber = 1;
        Address leveringsAddress = new Address("Arbeidstraat 14", "9300", "9", "BE");

        //Act
        var loadingSlip = new LoadingSlip(laadNumber, leveringsAddress);

        //Assert
        //TODO: JUIST LaadbonNr & Addres?
        loadingSlip.LaadbonNr.ShouldBe(laadNumber);
        loadingSlip.LeveringsAddress.ShouldBe(leveringsAddress);
    }
}