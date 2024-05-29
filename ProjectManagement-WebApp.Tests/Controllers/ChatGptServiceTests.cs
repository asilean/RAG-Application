using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using ProjectManagament_WebApp.Sevices;
using Xunit;

public class ChatGptServiceTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly Mock<ILogger<ChatGptService>> _mockLogger;
    private readonly JsonTemplates _jsonTemplates;
    private readonly ChatGptService _chatGptService;

    public ChatGptServiceTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _mockLogger = new Mock<ILogger<ChatGptService>>();
        _jsonTemplates = new JsonTemplates();

        var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("https://api.openai.com/v1/")
        };

        _chatGptService = new ChatGptService(httpClient, _mockLogger.Object, _jsonTemplates);
    }

    [Fact]
    public async Task GetChatCompletionAsync_ValidResponse_ReturnsExpectedContent()
    {
        // Arrange
        var moduleId = new Guid("29931809-ec70-433b-9ecf-def9c7189e39");
        var prompt = "test prompt";
        var expectedResponse = @"{""choices"":[{""message"":{""content"":""<div>Test Response</div>""}}]}";

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(expectedResponse)
            });

        // Act
        var result = await _chatGptService.GetChatCompletionAsync(prompt, moduleId);

        // Assert
        Assert.Equal("<div>Test Response</div>", result);
    }

    [Fact]
    public async Task GetChatCompletionAsync_HttpRequestException_ReturnsErrorMessage()
    {
        // Arrange
        var moduleId = new Guid("29931809-ec70-433b-9ecf-def9c7189e39");
        var prompt = "test prompt";

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Network error"));

        // Act
        var result = await _chatGptService.GetChatCompletionAsync(prompt, moduleId);

        // Assert
        Assert.Equal("Failed to connect to the GPT-4 service.", result);
    }

    [Fact]
    public async Task GetChatCompletionAsync_InvalidResponseFormat_ReturnsErrorMessage()
    {
        // Arrange
        var moduleId = new Guid("29931809-ec70-433b-9ecf-def9c7189e39");
        var prompt = "test prompt";
        var invalidResponse = @"{""invalid"":""response""}";

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(invalidResponse)
            });

        // Act
        var result = await _chatGptService.GetChatCompletionAsync(prompt, moduleId);

        // Assert
        Assert.Equal("No valid response or 'choices' is empty.", result);
    }
}
