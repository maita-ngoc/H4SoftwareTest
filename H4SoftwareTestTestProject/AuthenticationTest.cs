using Bunit;
using Bunit.TestDoubles;
using H4SoftwareTest.Components.Pages;

namespace H4SoftwareTestTestProject
{
    public class AuthenticationTest
    {
        [Fact]
        public void AuthenticationView()
        {
            //Arrange

            var ctx = new TestContext();
            var authContext = ctx.AddTestAuthorization();
            //authContext.SetAuthorized("");
            authContext.SetRoles("Admin");
            //Act
            //Code under text = cut
            var cut = ctx.RenderComponent<Home>();
            //Assert
            cut.MarkupMatches("<h1>Hello, world!</h1>\r\n    <p>You are Admin</p>");
        }

        [Fact]
        public void AuthenticationCode()
        {
            //Arrange

            var ctx = new TestContext();
            var authContext = ctx.AddTestAuthorization();
            authContext.SetAuthorized("");
            //Act
            //Code under text = cut
            var cut = ctx.RenderComponent<Home>();
            var homeObj = cut.Instance;

            //Assert
            Assert.Equal(homeObj.isAuthenticated, true);
        }
    }
}