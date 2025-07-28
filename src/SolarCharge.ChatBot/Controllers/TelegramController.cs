using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SolarCharge.ChatBot.Models;
using SolarCharge.ChatBot.Telegram;

namespace SolarCharge.ChatBot.Controllers;

[ApiController]
[Route("[controller]")]
public class TelegramController(
    ILogger<TelegramController> logger,
    IValidator<SendMessageRequest> validator,
    ITelegramService telegramService)
    : ControllerBase
{
    [HttpPost]
    [Route("send")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
    {
        logger.LogDebug("Received request to send message to the chat bot. Trace: {TraceIdentifier}",
            HttpContext.TraceIdentifier);
        
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var formattedErrors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            logger.LogError("Validation failed. Trace {TraceIdentifier}. Errors: {ErrorMessage}",
                HttpContext.TraceIdentifier,
                formattedErrors);
            return BadRequest(formattedErrors);
        }

        await telegramService.SendMessageAsync(request.MessageText);
        return Ok();
    }
}