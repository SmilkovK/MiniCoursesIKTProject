using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using MiniCoursesDomain.DTO;
using MiniCoursesService.Interface;

namespace MiniCoursesService.Implementation;

public class AICheckerService : IAICheckerService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public AICheckerService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<double> IsAIWrittenAsync(string text)
    {
        var apiKey = _config["Sapling:ApiKey"];
        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.sapling.ai/api/v1/aidetect");

        var requestData = new
        {
            key = apiKey,
            text = text,
            sent_scores = true,
            score_string = false
        };

        request.Content = JsonContent.Create(requestData);

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error: {errorContent}");
            throw new Exception($"AI detection API call failed. Status code: {response.StatusCode}. Error: {errorContent}");
        }

        var resultJson = await response.Content.ReadFromJsonAsync<SaplingResponse>();
    
        if (resultJson == null)
        {
            throw new Exception("Error: AI detection API response is null.");
        }
        
        return resultJson.Score;
    }
}