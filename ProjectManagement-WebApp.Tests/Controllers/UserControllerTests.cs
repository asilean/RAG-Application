using Xunit;
using Moq;
using ProjectManagament_WebApp.Controllers;
using ProjectManagament_WebApp.Data;
using ProjectManagament_WebApp.Sevices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class UserControllerTests
{
    [Fact]
    public void Index_ReturnsViewResult()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<PMContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        var mockContext = new PMContext(options);

        var mockEmailService = new Mock<IEmailService>();
        var controller = new UserController(mockContext, mockEmailService.Object);

        // Act
        var result = controller.Index();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void ForgotPassword_ReturnsViewResult()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<PMContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        var mockContext = new PMContext(options);

        var mockEmailService = new Mock<IEmailService>();
        var controller = new UserController(mockContext, mockEmailService.Object);

        // Act
        var result = controller.ForgotPassword();

        // Assert
        Assert.IsType<ViewResult>(result);
    }
}
