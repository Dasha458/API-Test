using System.Net;
using ApiTestingFramework.Business.Models;
using ApiTestingFramework.Business.Services;
using ApiTestingFramework.Core.Clients;
using ApiTestingFramework.Core.Configuration;
using ApiTestingFramework.Core.Logging;
using FluentAssertions;
using NLog;
using NUnit.Framework;
using RestSharp;

namespace ApiTestingFramework.Tests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
[Category("API")]
public class UserApiTests
{
    private ApiClient _client;
    private UserService _userService;
    private ITestLogger _logger;

    [OneTimeSetUp]
    public void Setup()
    {
        var config = FrameworkConfig.LoadDefault();

        LogManager.Setup().LoadConfigurationFromFile(
            Path.Combine(AppContext.BaseDirectory, "nlog.config"));
        NLogTestLogger.SetMinLevel(config.MinLogLevel);

        _logger = TestLoggerFactory.Create<UserApiTests>();
        _client = new ApiClient(config, _logger);
        _userService = new UserService(_client);

        _logger.Info("Setup done. BaseUrl: " + config.BaseUrl);
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _client.Dispose();
        LogManager.Shutdown();
    }

    [Test]
    public async Task Task1_GetUsersList()
    {
        _logger.Info("Task 1 - get users list");

        var response = await _userService.GetUsersAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.IsSuccessful.Should().BeTrue();
        response.ErrorMessage.Should().BeNullOrEmpty();
        response.Data.Should().NotBeNullOrEmpty();

        var user = response.Data[0];
        user.Id.Should().BeGreaterThan(0);
        user.Name.Should().NotBeNullOrWhiteSpace();
        user.Username.Should().NotBeNullOrWhiteSpace();
        user.Email.Should().NotBeNullOrWhiteSpace();
        user.Address.Should().NotBeNull();
        user.Phone.Should().NotBeNullOrWhiteSpace();
        user.Website.Should().NotBeNullOrWhiteSpace();
        user.Company.Should().NotBeNull();
    }

    [Test]
    public async Task Task2_ContentTypeHeader()
    {
        _logger.Info("Task 2 - check Content-Type header");

        var response = await _userService.GetUsersRawAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.IsSuccessful.Should().BeTrue();
        response.ErrorMessage.Should().BeNullOrEmpty();

        var contentType = response.ContentHeaders
            .FirstOrDefault(h => h.Name.Equals("Content-Type", StringComparison.OrdinalIgnoreCase))
            ?.Value?.ToString();

        contentType.Should().NotBeNull();
        contentType.Should().Be("application/json; charset=utf-8");
    }

    [Test]
    public async Task Task3_UsersArray()
    {
        _logger.Info("Task 3 - validate users array");

        var response = await _userService.GetUsersAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.IsSuccessful.Should().BeTrue();
        response.ErrorMessage.Should().BeNullOrEmpty();

        var users = response.Data;
        users.Should().HaveCount(10);
        users.Select(u => u.Id).Should().OnlyHaveUniqueItems();

        foreach (var user in users)
        {
            user.Name.Should().NotBeNullOrWhiteSpace();
            user.Username.Should().NotBeNullOrWhiteSpace();
            user.Company.Should().NotBeNull();
            user.Company.Name.Should().NotBeNullOrWhiteSpace();
        }
    }

    [Test]
    public async Task Task4_CreateUser()
    {
        _logger.Info("Task 4 - create new user");

        var newUser = new CreateUserRequest
        {
            Name = "John Doe",
            Username = "johndoe"
        };

        var response = await _userService.CreateUserAsync(newUser);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.IsSuccessful.Should().BeTrue();
        response.ErrorMessage.Should().BeNullOrEmpty();
        response.Data.Should().NotBeNull();
        response.Data.Id.Should().BeGreaterThan(0);
        response.Data.Name.Should().Be(newUser.Name);
        response.Data.Username.Should().Be(newUser.Username);
    }

    [Test]
    public async Task Task5_NotFound()
    {
        _logger.Info("Task 5 - check 404 for invalid endpoint");

        var response = await _userService.GetInvalidEndpointAsync();

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.ResponseStatus.Should().Be(ResponseStatus.Completed);
        response.ErrorMessage.Should().BeNullOrEmpty();
    }
}
