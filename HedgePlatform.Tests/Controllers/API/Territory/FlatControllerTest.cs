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
    public class FlatControllerTest
    {
        [Fact]
        public void IndexReturnFlats()
        {
            // Arrange
            var mock = new Mock<IFlatService>();
            mock.Setup(service => service.GetFlats(1)).Returns(GetFlatDTO());
            var controller = new FlatController(mock.Object);

            // Act
            var result = controller.Index(1);

            // Assert
            var viewResult = Assert.IsAssignableFrom<IEnumerable<FlatViewModel>>(result);
            Assert.Equal(3, viewResult.ToList().Count());

        }
        private IEnumerable<FlatDTO> GetFlatDTO()
        {
            var houses = new List<FlatDTO>
            {
               new FlatDTO {Id = 1, HouseId = 1, MaxCounters = 3, Number = 1},
               new FlatDTO {Id = 2, HouseId = 1, MaxCounters = 3, Number = 2},
               new FlatDTO {Id = 3, HouseId = 1, MaxCounters = 3, Number = 3}
            };
            return houses;
        }
    }
}
