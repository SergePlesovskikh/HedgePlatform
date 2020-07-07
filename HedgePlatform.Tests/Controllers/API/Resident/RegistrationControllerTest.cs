using Xunit;
using Moq;
using HedgePlatform.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using HedgePlatform.Controllers.API;
using HedgePlatform.BLL.Infr;
using HedgePlatform.BLL.DTO;
using HedgePlatform.ViewModel.API;
using Microsoft.AspNetCore.Http;
using System;

namespace HedgePlatform.Tests.Controllers.API.Resident
{
    public class RegistrationControllerTest
    {
        private static ControllerContext _context;

        public RegistrationControllerTest()
        {
            _context = new ControllerContext();
            _context.HttpContext = new DefaultHttpContext();
            _context.HttpContext.Items["ResidentId"] = 1;
        }

        [Fact]
        public void RegistrationSuccess()
        {
            // Arrange
            var mock = new Mock<IResidentService>();
            var controller = new RegistrationController(mock.Object);
            controller.ControllerContext = _context;

            ResidentViewModel resident = new ResidentViewModel { FlatId = 1, FIO = "TEST", BirthDate = DateTime.Now, PhoneId = 1 };

            // Act
            var result = controller.Registration(resident, "123");

            // Assert
            Assert.IsType<OkResult>(result);
        }
        [Fact]
        public void RegistrationBLLException()
        {
            // Arrange
            var mock = new Mock<IResidentService>();
            mock.Setup(service => service.RegistrationResident(It.IsAny<string>(), It.IsAny<ResidentDTO>())).Throws(new ValidationException("Exception", "Test"));
            var controller = new RegistrationController(mock.Object);
            controller.ControllerContext = _context;
            ResidentViewModel resident = new ResidentViewModel { };

            // Act
            var result = controller.Registration(resident, "123");

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
