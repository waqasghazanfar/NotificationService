namespace NotificationService.Api.Authentication
{
    public class SecuritySettings
    {
        public bool EnableAuthentication { get; set; } = false;
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
    }
}