using System.Security.Cryptography;
using System.Text;

namespace WebHookPOC.Services
{
    /// <summary>
    /// Service for handling webhook security logic, including IP whitelisting and HMAC signature validation.
    /// </summary>
    public class WebhookService : IWebhookService
    {
        private readonly string[] allowedIps = new[] { "127.0.0.1", "::1" }; // Add your allowed IPs here
                
        private const string secret = "Ritesh"; // Change this to your actual secret

        /// <summary>
        /// Initializes a new instance of the <see cref="WebhookService"/> class.
        /// </summary>
        public WebhookService()
        {
        }

        /// <summary>
        /// Checks if the provided remote IP address is allowed to access the webhook endpoint.
        /// </summary>
        /// <param name="remoteIp">The remote IP address of the incoming request.</param>
        /// <returns>True if the IP is allowed; otherwise, false.</returns>
        public bool IsIpAllowed(string? remoteIp)
        {
            Console.WriteLine($"Received request from IP: {remoteIp}");
            Console.WriteLine($"Allowed IPs: {string.Join(", ", allowedIps)}");
            return !string.IsNullOrEmpty(remoteIp) && allowedIps.Contains(remoteIp);
        }

        /// <summary>
        /// Computes the HMAC SHA256 signature for the given data using the configured secret.
        /// </summary>
        /// <param name="data">The data to sign.</param>
        /// <returns>The computed HMAC signature as a hexadecimal string.</returns>
        public string ComputeHmac(string data)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(hash).Replace("-", "");
        }

        /// <summary>
        /// Compares two strings in a way that protects against timing attacks.
        /// </summary>
        /// <param name="a">The first string to compare.</param>
        /// <param name="b">The second string to compare.</param>
        /// <returns>True if the strings are equal; otherwise, false.</returns>
        public bool CryptographicEquals(string a, string b)
        {
            var aBytes = Encoding.UTF8.GetBytes(a ?? "");
            var bBytes = Encoding.UTF8.GetBytes(b ?? "");
            return CryptographicOperations.FixedTimeEquals(aBytes, bBytes);
        }
    }
}