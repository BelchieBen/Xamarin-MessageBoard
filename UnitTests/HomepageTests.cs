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
using System.Collections.ObjectModel;
using System.Linq;

namespace UnitTests
{
    public class HomepageTests
    {
        private readonly Mock<INavigationService> _mockNavigationService;
        private readonly Mock<IMessageDataService> _mockMessageDataService;
        private readonly Mock<IDialogService> _mockDialogService;
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
            _mockDialogService = new Mock<IDialogService>();
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

        private  ObservableCollection<Message> MessageList()
        {
            ObservableCollection<Message> messages = new ObservableCollection<Message>()
            {
                new Message{Id=1, MessageTitle="Post1", Description="Hello", PostDate=DateTime.Today, User="benbelcher112@gmail.com"},
                new Message{Id=2, MessageTitle="Test Post", Description="Bye", PostDate=DateTime.Today, User="belchieben112@gmail.com"},
                new Message{Id=3, MessageTitle="New Post Test", Description="Greetings", PostDate=DateTime.Today, User="benbelcher112@gmail.com"},
            };
            return messages;
        }

        [Fact]
        public async Task NewCommandTest()
        {
            // Arrange
            HomepageViewModel vm = new HomepageViewModel(_mockMessageDataService.Object, _mockNavigationService.Object, _auth.Object, _mockDialogService.Object);

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
            HomepageViewModel vm = new HomepageViewModel(_mockMessageDataService.Object, _mockNavigationService.Object, _auth.Object, _mockDialogService.Object);

            // Act
            await vm.OnRefreshCommand();

            // Assert
            Assert.NotNull(vm.Messages);
        }

        [Fact]
        public async Task SelectedMessageTestNotAuthor()
        {
            // Arrange
            HomepageViewModel vm = new HomepageViewModel(_mockMessageDataService.Object, _mockNavigationService.Object, _auth.Object, _mockDialogService.Object);

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
            HomepageViewModel vm = new HomepageViewModel(_mockMessageDataService.Object, _mockNavigationService.Object, _auth.Object, _mockDialogService.Object);

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

        [Fact]
        public async Task LogoutCommandTest()
        {
            // Arrange 
            HomepageViewModel vm = new HomepageViewModel(_mockMessageDataService.Object, _mockNavigationService.Object, _auth.Object, _mockDialogService.Object);
            _auth.Setup(x => x.Logout());

            // Act
            await vm.OnLogoutCommand();

            // Assert
            _mockNavigationService.Verify(x => x.GoTo(ViewNames.LoginPage), Times.Once);
        }

        [Fact]
        public async Task SearchbarTest()
        {
            // Arrange
            HomepageViewModel vm = new HomepageViewModel(_mockMessageDataService.Object, _mockNavigationService.Object, _auth.Object, _mockDialogService.Object);

            var searchText = "Test";
            var messages = MessageList();
            vm.SearchText = searchText;
            vm.Messages = messages;

            // Act
            await vm.OnSearchCommand();

            // Assert
            Assert.Equal(2, vm.Messages.Count);
        }
    }
}
