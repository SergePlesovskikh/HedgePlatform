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
using System;

namespace HedgePlatform.Tests.Controllers.API
{
    public class CounterValueControllerTest
    {
        private static ControllerContext _context;
        public CounterValueControllerTest()
        {
            _context = new ControllerContext();
            _context.HttpContext = new DefaultHttpContext();
            _context.HttpContext.Items["FlatId"] = 1;
        }

        [Fact]
        public void IndexReturnCounterValues()
        {
            // Arrange
            var mock = new Mock<ICounterValueService>();
            mock.Setup(service => service.GetCounterValuesByCounter(1)).Returns(GetCounterValueDTO());
            var controller = new CounterValueController(mock.Object);       

            // Act
            var result = controller.Index(1);

            // Assert
            var viewResult = Assert.IsAssignableFrom<IEnumerable<CounterValueViewModel>>(result);
            Assert.Equal(2, viewResult.ToList().Count());

        }
        private IEnumerable<CounterValueDTO> GetCounterValueDTO()
        {
            var counterValues = new List<CounterValueDTO>
            {
              new CounterValueDTO {Id = 1, CounterId = 1, DateValue = DateTime.Now, Value = 12.5 },
              new CounterValueDTO {Id = 2, CounterId = 1, DateValue = DateTime.Now, Value = 23.5 }
            };
            return counterValues;
        }

    }
}
