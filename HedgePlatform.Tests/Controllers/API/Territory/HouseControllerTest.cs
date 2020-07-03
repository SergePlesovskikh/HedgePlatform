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
    public class HouseControllerTest
    {
        [Fact]
        public void IndexReturnHouses()
        {
            // Arrange
            var mock = new Mock<IHouseService>();
            mock.Setup(service => service.GetHouses()).Returns(GetTHouseDTO());
            var controller = new HouseController(mock.Object);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsAssignableFrom<IEnumerable<HouseViewModel>>(result);
            Assert.Equal(2, viewResult.ToList().Count());

        }
        private IEnumerable<HouseDTO> GetTHouseDTO()
        {
            var houses = new List<HouseDTO>
            {
               new HouseDTO {Id = 1, City = "Город 1", Home = "1", HouseManagerId = 1, Street = "Улица 1"},
               new HouseDTO {Id = 2, City = "Город 2", Home = "2", HouseManagerId = 1, Street = "Улица 2"}
            };
            return houses;
        }
    }
}
