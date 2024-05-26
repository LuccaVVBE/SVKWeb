using System;
using JetBrains.Annotations;
using Shouldly;
using Svk.Domain;
using Xunit;

namespace Svk.Tests;

[TestSubject(typeof(Routenumber))]
public class RoutenumberTest
{
    [Fact]
    public void Be_created_when_valid()
    {
        //Arange
        const string number = "123";

        //Act
        var routenumber = new Routenumber(number);

        //Assert
        routenumber.Number.ShouldBe(number);
    }
    
    [Fact]
    public void Throw_when_invalid_name_null()
    {
        //Arange
        const string name = null;

        //Act
        Action act = () =>
        {
            var routenumber = new Routenumber(name);
        };

        //Assert
        act.ShouldThrow<Exception>();
    }
    
    [Fact]
    public void Throw_when_invalid_name_empty()
    {
        //Arange
        const string name = "";

        //Act
        Action act = () =>
        {
            var routenumber = new Routenumber(name);
        };

        //Assert
        act.ShouldThrow<Exception>();
    }
}