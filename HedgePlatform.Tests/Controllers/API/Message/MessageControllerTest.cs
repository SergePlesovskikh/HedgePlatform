using Xunit;
using Moq;
using HedgePlatform.BLL.Interfaces;
using HedgePlatform.BLL.DTO;
using HedgePlatform.Controllers.API;
using System.Collections.Generic;
using HedgePlatform.ViewModel.API;
using System.Linq;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace HedgePlatform.Tests.Controllers.API
{
    public class MessageControllerTest
    {
        private static ControllerContext _context;
        public MessageControllerTest()
        {
            _context = new ControllerContext();
            _context.HttpContext = new DefaultHttpContext();
            _context.HttpContext.Items["ResidentId"] = 1;
        }

        [Fact]
        public void IndexReturnMessageAndVotes()
        {
            // Arrange
            var mock = new Mock<IMessageService>();
            mock.Setup(service => service.GetMessagesAndVotes(1)).Returns(GetVoteDTO());
            var controller = new MessageController(mock.Object);
            controller.ControllerContext = _context;

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsAssignableFrom<IEnumerable<VoteViewModel>>(result);
            Assert.Equal(3, viewResult.ToList().Count());

        }
        private IEnumerable<VoteDTO> GetVoteDTO()
        {
            var votes = new List<VoteDTO>
            {
               new VoteDTO {Id = 1, Content = "Контент 1", Header = "Новость 1", DateMessage = DateTime.Now },
               new VoteDTO {Id = 2, Content = "Контент 2", Header = "Новость 2", DateMessage = DateTime.Now },
               new VoteDTO {Id = 3, Content = "Контент 3", Header = "Голосование 1", DateMessage = DateTime.Now }
            };
            return votes;
        }
    }
}
