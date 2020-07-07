using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using HedgePlatform.BLL.Interfaces;
using HedgePlatform.BLL.DTO;
using HedgePlatform.BLL.Services;
using HedgePlatform.DAL.Interfaces;
using HedgePlatform.DAL.Entities;
using System.Collections.Generic;
using System.Linq;
using System;
using HedgePlatform.BLL.Infr;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace HedgePlatform.Tests.BLL
{
    public class MessageServiceTest
    {
        private IEnumerable<Message> GetMessagesTestData()
        {
            var messages = new List<Message>
            {
              new Message {Id = 1, Content = "Контент 1", Header = "Новость 1", DateMessage = DateTime.Now },
              new Message {Id = 2, Content = "Контент 2", Header = "Новость 2", DateMessage = DateTime.Now }
            };
            return messages;
        }

        [Fact]
        public void GetMessagesOk()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            // Arrange
            var mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(service => service.Messages.Find(It.IsAny<Func<Message, bool>>())).Returns(GetMessagesTestData());

            var mockVoteService = new Mock<IVoteService>();
            var mockResidentService = new Mock<IResidentService>();
            var mockVoteResultService = new Mock<IVoteResultService>();
            var mockLogger = new Mock<ILogger<MessageService>>();

            IMessageService messageService = new MessageService(mockUow.Object, mockVoteService.Object, mockResidentService.Object, 
                mockVoteResultService.Object, mockLogger.Object);

            // Act
            var result = messageService.GetMessages();

            // Assert
            var viewResult = Assert.IsAssignableFrom<IEnumerable<MessageDTO>>(result);
            Assert.Equal(2, viewResult.ToList().Count());

        }

        [Fact]
        public void GetMessagesAndVotesOk()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            // Arrange
            var mockUow = new Mock<IUnitOfWork>(); 
            var mockRepository = new Mock<IRepository<Message>>();
            mockUow.Setup(service => service.Messages.Find(It.IsAny<Func<Message, bool>>())).Returns(GetMessagesTestData());

            var mockVoteService = new Mock<IVoteService>();
            var mockResidentService = new Mock<IResidentService>();
            var mockVoteResultService = new Mock<IVoteResultService>();
            var mockLogger = new Mock<ILogger<MessageService>>();

            IMessageService messageService = new MessageService(mockUow.Object, mockVoteService.Object, mockResidentService.Object,
                mockVoteResultService.Object, mockLogger.Object);

            // Act
            var result = messageService.GetMessagesAndVotes(1);

            // Assert
            var viewResult = Assert.IsAssignableFrom<IEnumerable<VoteDTO>>(result);
            Assert.Equal(2, viewResult.ToList().Count());

        }

        [Fact]
        public void GetMessagesAndVotesNoParam()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            // Arrange
            var mockUow = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IRepository<Message>>();
            mockUow.Setup(x => x.Messages).Returns(mockRepository.Object);

            var mockVoteService = new Mock<IVoteService>();
            var mockResidentService = new Mock<IResidentService>();
            var mockVoteResultService = new Mock<IVoteResultService>();
            var mockLogger = new Mock<ILogger<MessageService>>();

            IMessageService messageService = new MessageService(mockUow.Object, mockVoteService.Object, mockResidentService.Object,
                mockVoteResultService.Object, mockLogger.Object);

            // Act
            Action act = () => messageService.GetMessagesAndVotes(null);

            // Assert
            var exception = Assert.Throws<ValidationException>(act);
            Assert.Equal("No Resident Id", exception.Message);
        }


        [Fact]
        public void CreateMessagesOk()
        {
            //Arrange
            var mockUow = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IRepository<Message>>();
            mockUow.Setup(x => x.Messages).Returns(mockRepository.Object);

            var mockVoteService = new Mock<IVoteService>();
            var mockResidentService = new Mock<IResidentService>();
            var mockVoteResultService = new Mock<IVoteResultService>();
            var mockLogger = new Mock<ILogger<MessageService>>();

            IMessageService messageService = new MessageService(mockUow.Object, mockVoteService.Object, mockResidentService.Object,
              mockVoteResultService.Object, mockLogger.Object);

            // Act
            messageService.CreateMessage(new MessageDTO { });

            // Assert
            mockUow.Verify(a => a.Messages.Create(It.IsAny<Message>()));
            mockUow.Verify(a => a.Save());
        }

        [Fact]
        public void CreateMessagesDBError()
        {
            //Arrange
            var mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(service => service.Messages.Create(It.IsAny<Message>())).Throws(new DbUpdateException("Exception"));

            var mockVoteService = new Mock<IVoteService>();
            var mockResidentService = new Mock<IResidentService>();
            var mockVoteResultService = new Mock<IVoteResultService>();            
            var mockLogger = new Mock<ILogger<MessageService>>();            

            IMessageService messageService = new MessageService(mockUow.Object, mockVoteService.Object, mockResidentService.Object,
              mockVoteResultService.Object, mockLogger.Object);

            // Act
            Action act = () => messageService.CreateMessage(new MessageDTO { });

            // Assert
            var exception = Assert.Throws<ValidationException>(act);
            Assert.Equal("DB_ERROR", exception.Message);
        }

        [Fact]
        public void EditMessagesOk()
        {
            //Arrange
            var mockUow = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IRepository<Message>>();
            mockUow.Setup(x => x.Messages).Returns(mockRepository.Object);

            var mockVoteService = new Mock<IVoteService>();
            var mockResidentService = new Mock<IResidentService>();
            var mockVoteResultService = new Mock<IVoteResultService>();
            var mockLogger = new Mock<ILogger<MessageService>>();

            IMessageService messageService = new MessageService(mockUow.Object, mockVoteService.Object, mockResidentService.Object,
              mockVoteResultService.Object, mockLogger.Object);

            // Act
            messageService.EditMessage(new MessageDTO { });

            // Assert
            mockUow.Verify(a => a.Messages.Update(It.IsAny<Message>()));
            mockUow.Verify(a => a.Save());
        }

        [Fact]
        public void EditMessagesNoObj()
        {
            //Arrange
            var mockUow = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IRepository<Message>>();
            mockUow.Setup(x => x.Messages).Returns(mockRepository.Object);

            var mockVoteService = new Mock<IVoteService>();
            var mockResidentService = new Mock<IResidentService>();
            var mockVoteResultService = new Mock<IVoteResultService>();
            var mockLogger = new Mock<ILogger<MessageService>>();

            IMessageService messageService = new MessageService(mockUow.Object, mockVoteService.Object, mockResidentService.Object,
              mockVoteResultService.Object, mockLogger.Object);

            // Act
            Action act = () => messageService.EditMessage(null);

            // Assert
            var exception = Assert.Throws<ValidationException>(act);
            Assert.Equal("No Message object", exception.Message);
        }

        [Fact]
        public void EditMessagesDBError()
        {
            //Arrange
            var mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(service => service.Messages.Update(It.IsAny<Message>())).Throws(new DbUpdateException("Exception"));

            var mockVoteService = new Mock<IVoteService>();
            var mockResidentService = new Mock<IResidentService>();
            var mockVoteResultService = new Mock<IVoteResultService>();
            var mockLogger = new Mock<ILogger<MessageService>>();

            IMessageService messageService = new MessageService(mockUow.Object, mockVoteService.Object, mockResidentService.Object,
              mockVoteResultService.Object, mockLogger.Object);

            // Act
            Action act = () => messageService.EditMessage(new MessageDTO { });

            // Assert
            var exception = Assert.Throws<ValidationException>(act);
            Assert.Equal("DB_ERROR", exception.Message);
        }

        [Fact]
        public void DeleteMessagesOk()
        {
            //Arrange
            var mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(service => service.Messages.Get(1)).Returns(new Message { Id = 1 });

            var mockVoteService = new Mock<IVoteService>();
            var mockResidentService = new Mock<IResidentService>();
            var mockVoteResultService = new Mock<IVoteResultService>();
            var mockLogger = new Mock<ILogger<MessageService>>();

            IMessageService messageService = new MessageService(mockUow.Object, mockVoteService.Object, mockResidentService.Object,
              mockVoteResultService.Object, mockLogger.Object);

            // Act
            messageService.DeleteMessage(1);

            // Assert
            mockUow.Verify(a => a.Messages.Delete(1));
            mockUow.Verify(a => a.Save());
        }

        [Fact]
        public void DeleteMessagesNoParam()
        {
            //Arrange
            var mockUow = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IRepository<Message>>();
            mockUow.Setup(x => x.Messages).Returns(mockRepository.Object);

            var mockVoteService = new Mock<IVoteService>();
            var mockResidentService = new Mock<IResidentService>();
            var mockVoteResultService = new Mock<IVoteResultService>();
            var mockLogger = new Mock<ILogger<MessageService>>();

            IMessageService messageService = new MessageService(mockUow.Object, mockVoteService.Object, mockResidentService.Object,
              mockVoteResultService.Object, mockLogger.Object);

            // Act
            Action act = () => messageService.DeleteMessage(null);

            // Assert
            var exception = Assert.Throws<ValidationException>(act);
            Assert.Equal("NULL", exception.Message);

        }

        [Fact]
        public void DeleteMessagesNotFound()
        {
            //Arrange
            var mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(service => service.Messages.Get(It.IsAny<int>())).Returns((Message)null);

            var mockVoteService = new Mock<IVoteService>();
            var mockResidentService = new Mock<IResidentService>();
            var mockVoteResultService = new Mock<IVoteResultService>();
            var mockLogger = new Mock<ILogger<MessageService>>();

            IMessageService messageService = new MessageService(mockUow.Object, mockVoteService.Object, mockResidentService.Object,
              mockVoteResultService.Object, mockLogger.Object);

            // Act
            Action act = () => messageService.DeleteMessage(1);

            // Assert
            var exception = Assert.Throws<ValidationException>(act);
            Assert.Equal("NOT_FOUND", exception.Message);

        }

        [Fact]
        public void DeleteMessagesDBError()
        {
            //Arrange
            var mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(service => service.Messages.Get(1)).Returns(new Message { Id = 1 });
            mockUow.Setup(service => service.Messages.Delete(It.IsAny<int>())).Throws(new DbUpdateException("Exception"));

            var mockVoteService = new Mock<IVoteService>();
            var mockResidentService = new Mock<IResidentService>();
            var mockVoteResultService = new Mock<IVoteResultService>();
            var mockLogger = new Mock<ILogger<MessageService>>();

            IMessageService messageService = new MessageService(mockUow.Object, mockVoteService.Object, mockResidentService.Object,
              mockVoteResultService.Object, mockLogger.Object);

            // Act
            Action act = () => messageService.DeleteMessage(1);

            // Assert
            var exception = Assert.Throws<ValidationException>(act);
            Assert.Equal("DB_ERROR", exception.Message);
        }
    }
}
