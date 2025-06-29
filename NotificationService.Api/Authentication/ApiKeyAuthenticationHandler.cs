using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using NotificationService.Api.Authentication;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace NotificationService.Api.Authentication;

public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
{
    public const string ClientIdHeaderName = "X-Client-Id";
    public const string ClientSecretHeaderName = "X-Client-Secret";

    private readonly IConfiguration _configuration;

    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<ApiKeyAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IConfiguration configuration)
        : base(options, logger, encoder)
    {
        _configuration = configuration;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(ClientIdHeaderName, out var clientIdHeaderValues))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        if (!Request.Headers.TryGetValue(ClientSecretHeaderName, out var clientSecretHeaderValues))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var providedClientId = clientIdHeaderValues.FirstOrDefault();
        var providedClientSecret = clientSecretHeaderValues.FirstOrDefault();

        var configClientId = _configuration["ApiKeyAuthentication:ClientId"];
        var configClientSecret = _configuration["ApiKeyAuthentication:ClientSecret"];

        if (providedClientId != configClientId || providedClientSecret != configClientSecret)
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid ClientId or ClientSecret."));
        }

        var claims = new[] { new Claim(ClaimTypes.Name, providedClientId) };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}