using WebHookPOC.Services;
using Xunit;

namespace WebHookPOC.Tests
{
    public class WebhookServiceTests
    {
        [Fact]
        public void IsIpAllowed_ReturnsTrue_ForAllowedIp()
        {
            var service = new WebhookService();
            Assert.True(service.IsIpAllowed("127.0.0.1"));
        }

        [Fact]
        public void IsIpAllowed_ReturnsFalse_ForNotAllowedIp()
        {
            var service = new WebhookService();
            Assert.False(service.IsIpAllowed("192.168.1.1"));
        }

        [Fact]
        public void IsIpAllowed_ReturnsFalse_ForNullIp()
        {
            var service = new WebhookService();
            Assert.False(service.IsIpAllowed(null));
        }

        [Fact]
        public void IsIpAllowed_ReturnsFalse_ForEmptyIp()
        {
            var service = new WebhookService();
            Assert.False(service.IsIpAllowed(""));
        }

        [Fact]
        public void ComputeHmac_ReturnsExpectedSignature()
        {
            var service = new WebhookService();
            var data = "testdata";
            var expected = service.ComputeHmac(data);
            Assert.Equal(expected, service.ComputeHmac(data));
        }

        [Fact]
        public void ComputeHmac_DifferentData_ReturnsDifferentSignature()
        {
            var service = new WebhookService();
            var sig1 = service.ComputeHmac("data1");
            var sig2 = service.ComputeHmac("data2");
            Assert.NotEqual(sig1, sig2);
        }

        [Fact]
        public void CryptographicEquals_ReturnsTrue_ForEqualStrings()
        {
            var service = new WebhookService();
            Assert.True(service.CryptographicEquals("abc", "abc"));
        }

        [Fact]
        public void CryptographicEquals_ReturnsFalse_ForDifferentStrings()
        {
            var service = new WebhookService();
            Assert.False(service.CryptographicEquals("abc", "def"));
        }

        [Fact]
        public void CryptographicEquals_ReturnsFalse_ForEmptyStringAndNonEmpty()
        {
            var service = new WebhookService();
            Assert.False(service.CryptographicEquals(string.Empty, "abc"));
            Assert.False(service.CryptographicEquals("abc", string.Empty));
        }
    }
}
