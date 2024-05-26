using System;
using JetBrains.Annotations;
using Shouldly;
using Svk.Domain;
using Xunit;

namespace Svk.Tests;

[TestSubject(typeof(Transporter))]
public class TransporterTest
{
    [Fact]
    public void Be_created_when_valid()
    {
        //Arange
        const string name = "Jef";

        //Act
        var transporter = new Transporter(name);

        //Assert
        transporter.Name.ShouldBe(name);
    }
    
    [Fact]
    public void Throw_when_invalid_name_null()
    {
        //Arange
        const string name = null;

        //Act
        Action act = () =>
        {
            var transporter = new Transporter(name);
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
            var transporter = new Transporter(name);
        };

        //Assert
        act.ShouldThrow<Exception>();
    } 
}