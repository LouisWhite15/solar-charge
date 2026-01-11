using System.Text.Json;
using System.Web;
using SolarCharge.API.Application.Features.TeslaAuth.Domain;
using SolarCharge.API.Application.Features.TeslaAuth.Events;
using SolarCharge.API.Application.Features.TeslaAuth.Infrastructure;
using Wolverine;

namespace SolarCharge.API.Application.Features.TeslaAuth.Commands;

public sealed class AuthenticateTeslaCommand(string urlWithCode, Dictionary<string, string> authenticationParameters)
{
    public string UrlWithCode { get; } = urlWithCode;
    public Dictionary<string, string> AuthenticationParameters { get; } = authenticationParameters;
    
    public class Handler(
        ILogger<Handler> logger,
        IMessageBus eventBus,
        ITeslaAuthenticationRepository teslaAuthenticationRepository,
        ITeslaAuthenticationClient teslaAuthenticationClient)
        : IWolverineHandler
    {
        public async ValueTask<bool> HandleAsync(AuthenticateTeslaCommand command, CancellationToken cancellationToken)
        {
            var finalUri = new Uri(command.UrlWithCode);
            var queryParams = HttpUtility.ParseQueryString(finalUri.Query);
            var code = queryParams["code"];
        
            var requestParameters = new Dictionary<string, string>()
            {
                { "grant_type", "authorization_code" },
                { "client_id", command.AuthenticationParameters["client_id"] },
                { "code", code ?? string.Empty },
                { "code_verifier", command.AuthenticationParameters["code_verifier"] },
                { "redirect_uri", command.AuthenticationParameters["redirect_uri"] }
            };
            var jsonRequest = JsonSerializer.Serialize(requestParameters);

            var tokens = await teslaAuthenticationClient.GetTokensAsync(jsonRequest, cancellationToken);
            if (tokens is null)
            {
                logger.LogError("Tesla Authentication Tokens could not be retrieved");
                return false;
            }
        
            logger.LogDebug("Persisting Tesla Authentication Tokens");
            await teslaAuthenticationRepository.SetAsync(new TeslaAuthentication(tokens.AccessToken, tokens.RefreshToken), cancellationToken);
        
            await eventBus.InvokeAsync(new TeslaAuthenticatedEvent(), cancellationToken);
            return true;
        }
    }
}