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
    public class SignupPageTests
    {
        private readonly Mock<INavigationService> _mockNavigationService;
        private readonly Mock<IFirebaseAuth> _auth;
        private readonly Mock<IDialogService> _mockDialogService;
        private List<User> users = new List<User>
        {
            new User()
            {
                Id = "id1",
                email = "testuser1@email.com",
                Username = "TestUser1",
            },

            new User()
            {
                Id = "id2",
                email = "testuser2@email.com",
                Username = "TestUser2",
            }
        };
        public SignupPageTests()
        {
            Xamarin.Forms.Mocks.MockForms.Init();
            _mockNavigationService = new Mock<INavigationService>();
            _auth = new Mock<IFirebaseAuth>();
            _mockDialogService = new Mock<IDialogService>();
        }

        [Fact]
        public async Task SignupUserCommandTest()
        {
            // Arrange
            SignupViewModel vm = new SignupViewModel(_mockNavigationService.Object, _auth.Object, _mockDialogService.Object );

            var user = users[0];
            var password = "Testing321";

            vm.Email = user.email;
            vm.Password = password;

            _auth.Setup(x => x.SignUpWithEmailPassword(user.email, password))
                .Returns(true);

            _mockDialogService.Setup(x => x.ShowDialogAsync(Strings.Congratulations, Strings.Account_Created_Success, Strings.Login));

            // Act
            await vm.OnSignupCommand();

            // Assert
            _auth.Verify(x => x.SignUpWithEmailPassword(user.email, password), Times.Once);
            _mockNavigationService.Verify(x => x.GoTo(ViewNames.LoginPage), Times.Once);
        }
    }
}
