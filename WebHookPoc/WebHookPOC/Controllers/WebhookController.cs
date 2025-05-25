using Microsoft.AspNetCore.Mvc;
using WebHookPOC.Services;

namespace WebHookPOC.Controllers
{
    /// <summary>
    /// Controller for handling secure webhook POST requests.
    /// Implements IP whitelisting and HMAC signature validation.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class WebhookController : ControllerBase
    {
        private readonly IWebhookService _webhookService;

        public WebhookController(IWebhookService webhookService)
        {
            _webhookService = webhookService;
        }

        /// <summary>
        /// Receives webhook POST requests, validates IP and signature.
        /// </summary>
        /// <param name="payload">The JSON payload sent by the webhook sender.</param>
        /// <returns>200 OK if validated, 401 Unauthorized otherwise.</returns>
        [HttpPost]       
        public IActionResult Receive([FromBody] object payload)
        {
            var remoteIp = HttpContext.Connection.RemoteIpAddress?.ToString();
            if (!_webhookService.IsIpAllowed(remoteIp))
            {
                return Unauthorized($"IP {remoteIp} is not allowed.");
            }

            // Optionally, validate signature header for extra security
            if (!Request.Headers.TryGetValue("X-Signature", out var signature))
            {
                return Unauthorized("Missing signature header.");
            }

            var body = payload.ToString() ?? string.Empty;
            var computedSignature = _webhookService.ComputeHmac(body);

            // Log the computed and received signatures for debugging
            Console.WriteLine($"Received Body: {body}");
            Console.WriteLine($"Computed Signature: {computedSignature}");
            Console.WriteLine($"Received Signature: {signature}");
            
            var signatureValue = signature.Count > 0 && signature[0] != null ? signature[0] : string.Empty;
            if (!_webhookService.CryptographicEquals(signatureValue ?? string.Empty, computedSignature ?? string.Empty))
            {
                return Unauthorized("Invalid signature.");
            }

            // If we reach here, the request is authenticated and authorized. You can process the payload.
            Console.WriteLine("Webhook received and validated successfully.");
            
            // Process webhook payload here
            return Ok(new { status = "Webhook received and validated successfully." });
        }
    }
}
