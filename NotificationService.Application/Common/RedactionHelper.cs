namespace NotificationService.Application.Common
{
    /// <summary>
    /// A helper class for redacting Personally Identifiable Information (PII).
    /// </summary>
    public static class RedactionHelper
    {
        /// <summary>
        /// Masks an email address to protect PII.
        /// Example: "jane.doe@example.com" becomes "j***.***e@e******.com"
        /// </summary>
        /// <param name="email">The email address to mask.</param>
        /// <returns>A masked email address.</returns>
        public static string MaskEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
                return email;

            try
            {
                var parts = email.Split('@');
                string localPart = parts[0];
                string domainPart = parts[1];

                string maskedLocalPart = MaskString(localPart, 1, 1);

                var domainParts = domainPart.Split('.');
                string tld = "." + domainParts.Last();
                string mainDomain = string.Join(".", domainParts.Take(domainParts.Length - 1));

                string maskedDomain = $"{MaskString(mainDomain, 1, 0)}{tld}";

                return $"{maskedLocalPart}@{maskedDomain}";
            }
            catch
            {
                // Fallback for unexpected formats
                return "email-redacted";
            }
        }

        /// <summary>
        /// Masks a phone number.
        /// Example: "+447912345678" becomes "+******5678"
        /// </summary>
        /// <param name="phone">The phone number to mask.</param>
        /// <returns>A masked phone number.</returns>
        public static string MaskPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone) || phone.Length < 4)
                return phone;

            return new string('*', phone.Length - 4) + phone.Substring(phone.Length - 4);
        }

        private static string MaskString(string value, int startVisible, int endVisible)
        {
            if (string.IsNullOrEmpty(value)) return value;

            int visibleLength = startVisible + endVisible;
            if (value.Length <= visibleLength) return new string('*', value.Length);

            var start = value.Substring(0, startVisible);
            var end = value.Substring(value.Length - endVisible, endVisible);
            var middle = new string('*', value.Length - visibleLength);

            return $"{start}{middle}{end}";
        }
    }
}