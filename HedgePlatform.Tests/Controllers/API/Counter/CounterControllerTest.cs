using Xunit;
using Moq;
using HedgePlatform.BLL.Interfaces;
using HedgePlatform.BLL.DTO;
using HedgePlatform.Controllers.API;
using System.Collections.Generic;
using HedgePlatform.ViewModel.API;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace HedgePlatform.Tests.Controllers.API
{
    public class CounterControllerTest
    {
        private static ControllerContext _context;
        public CounterControllerTest()
        {
            _context = new ControllerContext();
            _context.HttpContext = new DefaultHttpContext();
            _context.HttpContext.Items["FlatId"] = 1;
        }

        [Fact]
        public void IndexReturnCounters()
        {
            // Arrange
            var mock = new Mock<ICounterService>();
            mock.Setup(service => service.GetCountersByFlat(1)).Returns(GetCounterDTO());
            var controller = new CounterController(mock.Object);
            controller.ControllerContext = _context;

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsAssignableFrom<IEnumerable<CounterViewModel>>(result);
            Assert.Equal(2, viewResult.ToList().Count());

        }
        private IEnumerable<CounterDTO> GetCounterDTO()
        {
            var counters = new List<CounterDTO>
            {
              new CounterDTO {Id = 1, FlatId = 1, CounterStatusId = 1, CounterTypeId = 1, Location = "Место 1", Number = "123"},
              new CounterDTO {Id = 2, FlatId = 1, CounterStatusId = 1, CounterTypeId = 2, Location = "Место 2", Number = "124"},
            };
            return counters;
        }
    }
}
