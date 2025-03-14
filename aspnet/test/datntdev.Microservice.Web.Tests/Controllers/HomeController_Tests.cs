﻿using datntdev.Microservice.Models.TokenAuth;
using datntdev.Microservice.Web.Controllers;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace datntdev.Microservice.Web.Tests.Controllers;

public class HomeController_Tests : MicroserviceWebTestBase
{
    [Fact]
    public async Task Index_Test()
    {
        await AuthenticateAsync(null, new AuthenticateModel
        {
            UserNameOrEmailAddress = "admin",
            Password = "123qwe"
        });

        //Act
        var response = await GetResponseAsStringAsync(
            GetUrl<HomeController>(nameof(HomeController.Index))
        );

        //Assert
        response.ShouldNotBeNullOrEmpty();
    }
}