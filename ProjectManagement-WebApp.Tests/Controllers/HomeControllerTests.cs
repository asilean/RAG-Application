using Xunit;
using Moq;
using ProjectManagament_WebApp.Controllers;
using ProjectManagament_WebApp.Data;
using ProjectManagament_WebApp.Sevices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net.Http;

public class HomeControllerTests
{
    [Fact]
    public void Index_ReturnsViewResult()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<PMContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        var mockContext = new PMContext(options);

        var mockLogger = new Mock<ILogger<HomeController>>();
        var mockHttpClient = new Mock<HttpClient>();
        var mockChatGptLogger = new Mock<ILogger<ChatGptService>>();
        var mockJsonTemplates = new Mock<JsonTemplates>();
        var chatGptService = new ChatGptService(mockHttpClient.Object, mockChatGptLogger.Object, mockJsonTemplates.Object);
        var controller = new HomeController(mockLogger.Object, mockContext, chatGptService);

        // Act
        var result = controller.Index();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Privacy_ReturnsViewResult()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<PMContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        var mockContext = new PMContext(options);

        var mockLogger = new Mock<ILogger<HomeController>>();
        var mockHttpClient = new Mock<HttpClient>();
        var mockChatGptLogger = new Mock<ILogger<ChatGptService>>();
        var mockJsonTemplates = new Mock<JsonTemplates>();
        var chatGptService = new ChatGptService(mockHttpClient.Object, mockChatGptLogger.Object, mockJsonTemplates.Object);
        var controller = new HomeController(mockLogger.Object, mockContext, chatGptService);

        // Act
        var result = controller.Privacy();

        // Assert
        Assert.IsType<ViewResult>(result);
    }
}
