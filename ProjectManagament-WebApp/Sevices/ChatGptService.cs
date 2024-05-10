﻿using System.Diagnostics;
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
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "sk-proj-yZVIF8KJShyVIM9u21NhT3BlbkFJkZR6xcAsEnmNS0d3QtLB");
        _httpClient.BaseAddress = new Uri("https://api.openai.com/v1/");
    }

    public async Task<string> GetChatCompletionAsync(string prompt, Guid moduleId)
    {
        var systemContent = _jsonTemplates.GetTemplate(moduleId);

        Debug.WriteLine(systemContent);
        Debug.WriteLine(prompt);
        Debug.WriteLine(moduleId);

        var payload = new
        {
            model = "gpt-4-turbo", 
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
            
            try
            {
                var doc = System.Text.Json.JsonDocument.Parse(responseString);
                var root = doc.RootElement;

                if (root.TryGetProperty("choices", out var choicesElement) && choicesElement.ValueKind == JsonValueKind.Array && choicesElement.GetArrayLength() > 0)
                {
                    var firstChoice = choicesElement[0];
                    if (firstChoice.TryGetProperty("message", out var messageElement) && messageElement.TryGetProperty("content", out var contentElement) && contentElement.ValueKind == JsonValueKind.String)
                    {
                        return contentElement.GetString();
                    }
                }
                return "No valid response or 'choices' is empty.";
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to process the response: {0}", ex);
                return "Error processing the API response.";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to get response from GPT-4: {0}", ex);
            return "Failed to connect to the GPT-4 service.";
        }
    }
}
