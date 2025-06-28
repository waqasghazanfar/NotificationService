namespace NotificationService.Api.Authentication
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.Extensions.Options;
    using System.Net.Http.Headers;
    using System.Security.Claims;
    using System.Text;
    using System.Text.Encodings.Web;

    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly SecuritySettings _securitySettings;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            IOptions<SecuritySettings> securitySettings)
            : base(options, logger, encoder)
        {
            _securitySettings = securitySettings.Value;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // If authentication is disabled in config, automatically succeed.
            if (!_securitySettings.EnableAuthentication)
            {
                var claims = new[] { new Claim(ClaimTypes.Name, "System") };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }

            // Handle actual authentication if enabled.
            if (!Request.Headers.ContainsKey("Authorization"))
                return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));

            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]!);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter!);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                var clientId = credentials[0];
                var clientSecret = credentials[1];

                if (clientId == _securitySettings.ClientId && clientSecret == _securitySettings.ClientSecret)
                {
                    var claims = new[] {
                        new Claim(ClaimTypes.NameIdentifier, clientId),
                        new Claim(ClaimTypes.Name, clientId),
                    };
                    var identity = new ClaimsIdentity(claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);

                    return Task.FromResult(AuthenticateResult.Success(ticket));
                }

                return Task.FromResult(AuthenticateResult.Fail("Invalid ClientId or ClientSecret"));
            }
            catch
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
            }
        }
    }
}
