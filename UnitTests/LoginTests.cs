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
    public class LoginTests
    {
        private readonly Mock<INavigationService> _mockNavigationService;
        private readonly Mock<IFirebaseAuth> _auth;
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

        public LoginTests()
        {
            Xamarin.Forms.Mocks.MockForms.Init();
            _mockNavigationService = new Mock<INavigationService>();
            _auth = new Mock<IFirebaseAuth>();
        }

        [Fact]
        public async Task LoginCommandTest()
        {
            // Arrange 
            LoginViewModel vm = new LoginViewModel(_mockNavigationService.Object, _auth.Object);

            var user = users[0];
            var password = "Testing321";
            var token = Guid.NewGuid().ToString();

            vm.Email = user.email;
            vm.Password = password;

            _auth.Setup(x => x.LoginWithEmailPassword(user.email, password))
                .ReturnsAsync(token);

            _auth.Setup(x => x.GetCurrentUser())
                .ReturnsAsync(user);

            // Act 
            await vm.OnLoginCommand();

            // Asssert
            _auth.Verify(x => x.LoginWithEmailPassword(user.email, password), Times.Once);
            _mockNavigationService.Verify(x => x.GoTo(ViewNames.HomepageView));
        }
    }
}
