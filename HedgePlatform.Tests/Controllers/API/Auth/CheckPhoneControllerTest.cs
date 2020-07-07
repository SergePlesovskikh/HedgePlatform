using Xunit;
using Moq;
using HedgePlatform.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using HedgePlatform.Controllers.API;
using HedgePlatform.BLL.Infr;
using HedgePlatform.ViewModel.API;


namespace HedgePlatform.Tests.Controllers.API.Auth
{
    public class CheckPhoneControllerTest
    {        
        [Fact]
        public async void CheckPhoneSuccess()
        {
            // Arrange
            var mock = new Mock<ISMSSendService>();
          
            var mock2 = new Mock<IPhoneService>();
            mock2.Setup(service => service.CheckPhone("79999999999")).Returns(true);

            var controller = new CheckPhoneController(mock.Object, mock2.Object);       

            // Act
            var result = await controller.Get("79999999999");

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async void CheckPhoneAlready()
        {
            // Arrange
            var mock = new Mock<ISMSSendService>();

            var mock2 = new Mock<IPhoneService>();
            mock2.Setup(service => service.CheckPhone("79999999999")).Returns(false);

            var controller = new CheckPhoneController(mock.Object, mock2.Object);

            // Act
            var result = await controller.Get("79999999999");
            var okResult =  result as OkObjectResult;
            var actualres = okResult.Value as string;

            // Assert
            Assert.Equal("ALREADY", actualres);
        }
        [Fact]
        public async void CheckPhoneBLLException()
        {
            // Arrange
            var mock = new Mock<ISMSSendService>();            

            var mock2 = new Mock<IPhoneService>();
            mock2.Setup(service => service.CheckPhone("79999999999")).Throws(new ValidationException("Exception", "Test"));

            var controller = new CheckPhoneController(mock.Object, mock2.Object);          
            ResidentViewModel resident = new ResidentViewModel { };

            // Act
            var result = await controller.Get("79999999999");

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
