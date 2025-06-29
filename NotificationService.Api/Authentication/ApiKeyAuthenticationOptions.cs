using Microsoft.AspNetCore.Authentication;

namespace NotificationService.Api.Authentication;

public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
{
    public const string DefaultScheme = "ApiKey";
    public const string PolicyName = "ApiKeyPolicy";
}