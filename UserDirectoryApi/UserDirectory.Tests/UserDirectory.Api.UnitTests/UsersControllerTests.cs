#nullable enable
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserDirectory.Api.Controllers;
using UserDirectory.Application.DTOs;
using UserDirectory.Application.Interfaces;
using UserDirectory.Application.Mappings;
using UserDirectory.Domain.Entities;

namespace UserDirectory.Api.UnitTests;

[TestFixture]
public class UsersControllerTests
{
    private static IMapper CreateMapper()
    {
        var cfg = new MapperConfiguration(cfg => cfg.AddProfile(new UserProfile()));
        return cfg.CreateMapper();
    }

    [Test]
    public async Task GetUsers_ReturnsOk_WithMappedDtos()
    {
        var users = new List<User>
        {
            new() { Id = 1, Name = "A", Age = 20, City = "C1", State = "S1", Pincode = "1000" },
            new() { Id = 2, Name = "B", Age = 30, City = "C2", State = "S2", Pincode = "2000" }
        };

        var repoMock = new Mock<IUserRepository>();
        repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(users);

        var mapper = CreateMapper();
        var controller = new UsersController(repoMock.Object, mapper);

        var actionResult = await controller.GetUsers();

        actionResult.Result.Should().BeOfType<OkObjectResult>();
        var ok = actionResult.Result as OkObjectResult;
        ok.Should().NotBeNull();
        var dtos = ok!.Value as IEnumerable<UserDto>;
        dtos.Should().NotBeNull();
        dtos!.Count().Should().Be(2);
        dtos.Select(d => d.Name).Should().Contain(new[] { "A", "B" });
    }

    [Test]
    public async Task GetUser_NotFound_ReturnsNotFound()
    {
        var repoMock = new Mock<IUserRepository>();
        repoMock.Setup(r => r.GetByIdAsync(99, default)).ReturnsAsync((User?)null);

        var mapper = CreateMapper();
        var controller = new UsersController(repoMock.Object, mapper);

        var result = await controller.GetUser(99);

        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Test]
    public async Task CreateUser_ReturnsCreatedAtAction()
    {
        var inputDto = new UserDto(0, "New", 22, "City", "State", "4000");
        var repoMock = new Mock<IUserRepository>();
        repoMock.Setup(r => r.AddAsync(It.IsAny<User>(), default))
            .Returns(Task.CompletedTask)
            .Callback<User, System.Threading.CancellationToken>((u, ct) => u.Id = 11);

        var mapper = CreateMapper();
        var controller = new UsersController(repoMock.Object, mapper);

        var action = await controller.CreateUser(inputDto);

        action.Result.Should().BeOfType<CreatedAtActionResult>();
        var created = action.Result as CreatedAtActionResult;
        created!.RouteValues!["id"].Should().Be(11);
    }
}