using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class OpenAIClientService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://api.zukijourney.com/v1";
    private const string ApiKey = "zu-d434665ba0070a9f3e3435227053e87b";

    public OpenAIClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiKey}");
    }

    public async Task<string> GetAIResponseAsync(string userMessage)
    {
        var requestBody = new
        {
            model = "gpt-3.5-turbo",
            messages = new[]
            {
                new { role = "user", content = userMessage }
            }
        };

        var jsonContent = JsonConvert.SerializeObject(requestBody);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PostAsync($"{BaseUrl}/chat/completions", content);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"API call failed: {response.StatusCode}");
        }

        var responseString = await response.Content.ReadAsStringAsync();
        var responseData = JsonConvert.DeserializeObject<dynamic>(responseString);

        return responseData?.choices[0]?.message?.content?.ToString() ?? "No response from AI";
    }
}
