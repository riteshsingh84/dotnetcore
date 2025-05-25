namespace WebHookPOC.Services
{
    public interface IWebhookService
    {
        bool IsIpAllowed(string? remoteIp);
        string ComputeHmac(string data);
        bool CryptographicEquals(string a, string b);
    }
}
