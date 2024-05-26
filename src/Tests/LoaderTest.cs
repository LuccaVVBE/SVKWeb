using System;
using JetBrains.Annotations;
using Shouldly;
using Svk.Domain;
using Xunit;

namespace Svk.Tests;

[TestSubject(typeof(Loader))]
public class LoaderTest
{
    [Fact]
    public void Be_created_when_valid()
    {
        //Arange
        const string email = "jef.email@email.com";
        const string name = "Jef";
        const string auth0Id = "authId123";

        //Act
        var loader = new Loader(email, name, auth0Id);

        //Assert
        loader.Email.ShouldBe(email);
        loader.Name.ShouldBe(name);
        loader.Auth0Id.ShouldBe(auth0Id);
    }
    
    [Fact]
    public void Throw_when_invalid_email()
    {
        //Arange
        const string email = null;
        const string name = "";
        const string auth0Id = "";

        //Act
        Action act = () =>
        {
            var loader = new Loader(email, name, auth0Id);
        };

        //Assert
        act.ShouldThrow<Exception>();
    }
    
    [Fact]
    public void Throw_when_invalid_name_null()
    {
        //Arange
        const string email = "jef.email@email.com";
        const string name = null;
        const string auth0Id = "authId123";

        //Act
        Action act = () =>
        {
            var loader = new Loader(email, name, auth0Id);
        };

        //Assert
        act.ShouldThrow<Exception>();
    }
    
    [Fact]
    public void Throw_when_invalid_name_empty()
    {
        //Arange
        const string email = "jef.email@email.com";
        const string name = "";
        const string auth0Id = "authId123";

        //Act
        Action act = () =>
        {
            var loader = new Loader(email, name, auth0Id);
        };

        //Assert
        act.ShouldThrow<Exception>();
    }
    
}