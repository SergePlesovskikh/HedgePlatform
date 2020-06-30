using Xunit;
using Moq;
using HedgePlatform.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using HedgePlatform.Controllers.API;
using HedgePlatform.BLL.Infr;
using HedgePlatform.BLL.DTO;
using HedgePlatform.ViewModel.API;
using Microsoft.AspNetCore.Http;


namespace HedgePlatform.Tests.Controllers.API
{
    public class VoteResultControllerTest
    {
        private static ControllerContext _context;

        public VoteResultControllerTest ()
        {
            _context = new ControllerContext();
            _context.HttpContext = new DefaultHttpContext();
            _context.HttpContext.Items["ResidentId"] = 1;
        }

        [Fact]
        public void CreateVoteResultSuccess()
        {
            // Arrange
            var mock = new Mock<IVoteResultService>();
            var controller = new VoteResultController(mock.Object);
            controller.ControllerContext = _context;

            VoteResultViewModel voteResult = new VoteResultViewModel { Id = 1, VoteOptionId = 1 };

            // Act
            var result = controller.Create(voteResult);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public void CreateVoteResultBLLException()
        {
            // Arrange
            var mock = new Mock<IVoteResultService>();
            mock.Setup(service => service.CreateVoteResult(It.IsAny<VoteResultDTO>(), It.IsAny<int>())).Throws(new ValidationException("Exception","Test"));
            var controller = new VoteResultController(mock.Object);
            controller.ControllerContext = _context;
            VoteResultViewModel voteResult = new VoteResultViewModel {  };

            // Act
            var result = controller.Create(voteResult);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
