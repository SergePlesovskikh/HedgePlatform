using Xunit;
using Moq;
using HedgePlatform.BLL.Interfaces;
using HedgePlatform.BLL.DTO;
using HedgePlatform.Controllers.API;
using System.Collections.Generic;
using HedgePlatform.ViewModel.API;
using System.Linq;

namespace HedgePlatform.Tests.Controllers.API
{
    public class CounterTypesControllerTest
    {
        [Fact]
        public void IndexReturnCounterTypes()
        {
            // Arrange
            var mock = new Mock<ICounterTypeService>();
            mock.Setup(service => service.GetCounterTypes()).Returns(GetTestCounterTypesDTO());
            var controller = new CounterTypeController(mock.Object);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsAssignableFrom<IEnumerable<CounterTypeViewModel>>(result);
            Assert.Equal(2, viewResult.ToList().Count());

        }
        private IEnumerable<CounterTypeDTO> GetTestCounterTypesDTO()
        {
            var counterTypes = new List<CounterTypeDTO>
            {
                new CounterTypeDTO { Id=1, Type = "Горячая водицка" },
                new CounterTypeDTO { Id=2, Type="Холодная водицка"}             
            };
            return counterTypes;
        }
    }
}
