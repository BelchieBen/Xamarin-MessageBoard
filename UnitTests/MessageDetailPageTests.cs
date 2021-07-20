using MessageBoard.Services;
using MessageBoard.ViewModels;
using MessageBoard.Utility;
using Moq;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xunit;
using Xunit.Abstractions;
using MessageBoard.Models;
using System.Collections.Generic;
using MessageBoard.Styles;

namespace UnitTests
{
    public class MessageDetailPageTests
    {
        private readonly Mock<INavigationService> _mockNavigationService;
        private readonly Mock<IMessageDataService> _mockMessageDataService;
        private readonly Mock<IDialogService> _mockDialogService;
        private List<User> users = new List<User>
        {
            new User()
            {
                email = "testuser1@email.com",
                Username = "TestUser1",
            },

            new User()
            {
                email = "testuser2@email.com",
                Username = "TestUser2",
            }
        };

        private Message GetMessage()
        {
            return new Message()
            {
                MessageTitle = "Test message 1",
                Description = "Test description",
                PostDate = DateTime.Now,
                User = "testuser1@email.com",
            };
        }

        public MessageDetailPageTests()
        {
            Xamarin.Forms.Mocks.MockForms.Init();
            _mockMessageDataService = new Mock<IMessageDataService>();
            _mockNavigationService = new Mock<INavigationService>();
            _mockDialogService = new Mock<IDialogService>();
        }

        [Fact]
        public async Task SaveCommandTestNew()
        {
            // Arrange
            MessageDetailViewModel vm = new MessageDetailViewModel(_mockMessageDataService.Object, _mockNavigationService.Object, _mockDialogService.Object);
            var newMessage = GetMessage();
            newMessage.User = null;
            vm.SelectedMessage = newMessage;


            // Act
            await vm.OnSaveCommand();

            // Assert
            _mockMessageDataService.Verify(x => x.NewFirebaseMessage(newMessage.MessageTitle, newMessage.Description), Times.Once);
        }

        [Fact]
        public async Task SaveCommandTestUpdate()
        {
            // Arrange
            MessageDetailViewModel vm = new MessageDetailViewModel(_mockMessageDataService.Object, _mockNavigationService.Object, _mockDialogService.Object);
            var newMessage = GetMessage();
            vm.SelectedMessage = newMessage;


            // Act
            await vm.OnSaveCommand();

            // Assert
            _mockMessageDataService.Verify(x => x.UpdatedFirebaseMessage(newMessage.MessageTitle, newMessage.Description, newMessage.Id, newMessage.User), Times.Once);
        }

        [Fact]
        public void CancelCommandtest()
        {
            // Arrange 
            MessageDetailViewModel vm = new MessageDetailViewModel(_mockMessageDataService.Object, _mockNavigationService.Object, _mockDialogService.Object);

            // Act
            vm.OnCancelCommand();

            // Assert
            _mockNavigationService.Verify(x => x.PopPage());
        }

        [Fact]
        public async Task DeleteCommandTest()
        {
            // Arrange
            MessageDetailViewModel vm = new MessageDetailViewModel(_mockMessageDataService.Object, _mockNavigationService.Object, _mockDialogService.Object);
            var newMessage = GetMessage();
            vm.SelectedMessage = newMessage;

            // Act
            await vm.OnDeleteCommand();

            // Assert
            _mockDialogService.Verify(x => x.ShowToastMessage(Strings.Message_Deleted));
            _mockMessageDataService.Verify(x => x.DeleteFirebaseMessage(newMessage.MessageTitle, newMessage.Description, newMessage.Id));
            _mockNavigationService.Verify(x => x.PopPage());
        }
    }
}
