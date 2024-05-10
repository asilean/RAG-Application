using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using ProjectManagament_WebApp.Sevices;

public class ChatGptService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ChatGptService> _logger;
    private readonly JsonTemplates _jsonTemplates;

    public ChatGptService(HttpClient httpClient, ILogger<ChatGptService> logger, JsonTemplates jsonTemplates)
    {
        _httpClient = httpClient;
        _logger = logger;
        _jsonTemplates = jsonTemplates;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "your_openai_api_key");
        _httpClient.BaseAddress = new Uri("https://api.openai.com/v1/");
    }

    public async Task<string> GetChatCompletionAsync(string prompt, Guid moduleId)
    {
        var systemContent = _jsonTemplates.GetTemplate(moduleId);

        var payload = new
        {
            model = "gpt-4-0125-preview", 
            messages = new[]
            {
                new { role = "system", content = systemContent },
                new { role = "user", content = prompt }
            }
        };

        var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

        try
        {
            // Make the POST request to the OpenAI API
            var response = await _httpClient.PostAsync("chat/completions", content);
            response.EnsureSuccessStatusCode();

            // Parse the response
            var responseString = await response.Content.ReadAsStringAsync();
            var responseJson = System.Text.Json.JsonSerializer.Deserialize<dynamic>(responseString);
            return responseJson.choices[0].message.content;
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to get response from GPT-4: {0}", ex);
            return "Failed to connect to the GPT-4 service.";
        }
    }
}
