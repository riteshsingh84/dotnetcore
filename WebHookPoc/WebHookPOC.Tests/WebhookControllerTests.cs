using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebHookPOC.Controllers;
using WebHookPOC.Services;
using Xunit;

namespace WebHookPOC.Tests
{
    public class WebhookControllerTests
    {
        [Fact]
        public void Receive_ReturnsUnauthorized_WhenIpNotAllowed()
        {
            var mockService = new Mock<IWebhookService>();
            mockService.Setup(s => s.IsIpAllowed(It.IsAny<string>())).Returns(false);
            var controller = new WebhookController(mockService.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            var result = controller.Receive(new { });
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public void Receive_ReturnsUnauthorized_WhenSignatureMissing()
        {
            var mockService = new Mock<IWebhookService>();
            mockService.Setup(s => s.IsIpAllowed(It.IsAny<string>())).Returns(true);
            var controller = new WebhookController(mockService.Object);
            var context = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext { HttpContext = context };
            var result = controller.Receive(new { });
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public void Receive_ReturnsUnauthorized_WhenSignatureIsEmpty()
        {
            var mockService = new Mock<IWebhookService>();
            mockService.Setup(s => s.IsIpAllowed(It.IsAny<string>())).Returns(true);
            var controller = new WebhookController(mockService.Object);
            var context = new DefaultHttpContext();
            context.Request.Headers["X-Signature"] = ""; // Empty signature
            controller.ControllerContext = new ControllerContext { HttpContext = context };
            var result = controller.Receive(new { });
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public void Receive_ReturnsUnauthorized_WhenSignatureIsInvalid()
        {
            var mockService = new Mock<IWebhookService>();
            mockService.Setup(s => s.IsIpAllowed(It.IsAny<string>())).Returns(true);
            mockService.Setup(s => s.ComputeHmac(It.IsAny<string>())).Returns("valid");
            mockService.Setup(s => s.CryptographicEquals(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            var controller = new WebhookController(mockService.Object);
            var context = new DefaultHttpContext();
            context.Request.Headers["X-Signature"] = "invalid";
            controller.ControllerContext = new ControllerContext { HttpContext = context };
            var result = controller.Receive(new { });
            Assert.IsType<UnauthorizedObjectResult>(result);
        }
    }
}
