using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Moq;
using ProjectManagament_WebApp.Data;
using ProjectManagament_WebApp.Sevices;
using Xunit;

public class EmailServiceTests
{
    private readonly IConfiguration _configuration;
    private readonly PMContext _context;
    private readonly EmailService _emailService;

    public EmailServiceTests()
    {
        var inMemorySettings = new Dictionary<string, string> {
            {"Smtp:Host", "smtp.test.com"},
            {"Smtp:Port", "587"},
            {"Smtp:EnableSsl", "true"},
            {"Smtp:UseDefaultCredentials", "false"},
            {"Smtp:Email", "test@test.com"},
            {"Smtp:Password", "password"}
        };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        var options = new DbContextOptionsBuilder<PMContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new PMContext(options);

        _emailService = new EmailService(_context, _configuration);
    }

    [Fact]
    public void SendCodeEmail_InvalidEmail_ThrowsFormatException()
    {
        // Arrange
        var invalidEmail = "invalid-email";
        var code = "123456";

        // Act & Assert
        Assert.Throws<FormatException>(() => _emailService.SendCodeEmail(invalidEmail, code));
    }
}
