using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/ai")]
[ApiController]
public class AiController : ControllerBase
{
    private readonly OpenAIClientService _openAiClientService;

    public AiController(OpenAIClientService openAiClientService)
    {
        _openAiClientService = openAiClientService;
    }

    [HttpPost("chat")]
    public async Task<IActionResult> ChatWithAI([FromBody] ChatRequest request)
    {
        if (string.IsNullOrEmpty(request.Message))
            return BadRequest("Message cannot be empty.");

        var response = await _openAiClientService.GetAIResponseAsync(request.Message);
        return Ok(new { response });
    }
}

public class ChatRequest
{
    public string Message { get; set; }
}
