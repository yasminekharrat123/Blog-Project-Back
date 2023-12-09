
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Newtonsoft.Json.Linq;

namespace AI {

[Route("[controller]")]
[ApiController]
public class OpenAIController : ControllerBase
{
      private readonly HttpClient _httpClient;
      private readonly IOpenAIService _openAIService;

     public OpenAIController(IHttpClientFactory httpClientFactory, IOpenAIService openAIService)
    {
        _httpClient = httpClientFactory.CreateClient("OpenAI");
        _openAIService = openAIService;
    }

    [HttpPost("evaluate")]
  public async Task<IActionResult> EvaluateBlog([FromBody] Blog.Models.Blog blog)
{
    try
    {
        string endpoint = "chat/completions";

        string payloadJson = _openAIService.Evaluate(blog);
        var requestContent = new StringContent(payloadJson, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(endpoint, requestContent);
        if (response.IsSuccessStatusCode)
        {
            string result = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(result)?["choices"]?[0]?["message"]?["function_call"]?["arguments"];
            return Ok(jsonObject?.ToString());
        } 
        else
        {
            return BadRequest($"Error: {response.StatusCode}");
        }
    }
    catch (Exception ex)
    {
        return BadRequest($"Error: {ex.Message}");
    }
}

}

   
}