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

namespace UnitTests
{
    public class HomepageTests
    {
        private readonly Mock<INavigationService> _mockNavigationService;
        private readonly Mock<IMessageDataService> _mockMessageDataService;
        private readonly Mock<IFirebaseAuth> _auth;
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

        public HomepageTests()
        {
            Xamarin.Forms.Mocks.MockForms.Init();
            _mockMessageDataService = new Mock<IMessageDataService>();
            _mockNavigationService = new Mock<INavigationService>();
            _auth = new Mock<IFirebaseAuth>();
        }

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
        

        [Fact]
        public async Task NewCommandTest()
        {
            // Arrange
            HomepageViewModel vm = new HomepageViewModel(_mockMessageDataService.Object, _mockNavigationService.Object, _auth.Object);

            // Act
            await vm.OnNewCommand();

            // Assert
            Assert.NotNull(vm.NewCommand);
            _mockNavigationService.Verify(x => x.NavigateTo(ViewNames.NewMessagePage, null), Times.Once);
        }

        [Fact]
        public async Task RefreshMessagesTest()
        {
            // Arrange
            HomepageViewModel vm = new HomepageViewModel(_mockMessageDataService.Object, _mockNavigationService.Object, _auth.Object);

            // Act
            await vm.OnRefreshCommand();

            // Assert
            Assert.NotNull(vm.Messages);
        }

        [Fact]
        public async Task SelectedMessageTestNotAuthor()
        {
            // Arrange
            HomepageViewModel vm = new HomepageViewModel(_mockMessageDataService.Object, _mockNavigationService.Object, _auth.Object);

            var message = GetMessage();

            var user = users[0];
            _auth.Setup(x => x.GetCurrentUser())
                .ReturnsAsync(user);

            // Act
            await vm.OnMessageSelectedCommand(message);

            // Assert
            Assert.Equal(message.User, user.email);
            _mockNavigationService.Verify(x => x.NavigateTo(ViewNames.MessageDetailView, message), Times.Once);
        }

        [Fact]
        public async Task SelectedMessageTestAuthor()
        {
            // Arrange
            HomepageViewModel vm = new HomepageViewModel(_mockMessageDataService.Object, _mockNavigationService.Object, _auth.Object);

            var message = GetMessage();

            var user = users[1];
            _auth.Setup(x => x.GetCurrentUser())
                .ReturnsAsync(user);

            // Act
            await vm.OnMessageSelectedCommand(message);

            // Assert
            Assert.NotEqual(message.User, user.email);
            _mockNavigationService.Verify(x => x.NavigateTo(ViewNames.MessageDetailReadonlyPage, message), Times.Once);
        }
    }
}
