using FluentValidation;
using SolarCharge.ChatBot.Models;

namespace SolarCharge.ChatBot.Validators;

public class SendMessageRequestValidator : AbstractValidator<SendMessageRequest>
{
    public SendMessageRequestValidator()
    {
        RuleFor(request => request.MessageText).NotEmpty();
    }
}