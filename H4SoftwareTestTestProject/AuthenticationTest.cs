using Bunit;
using Bunit.TestDoubles;
using H4SoftwareTest.Components.Pages;

namespace H4SoftwareTestTestProject
{
    public class AuthenticationTest
    {
        [Fact]
        public void LogedinViewTest()
        {
            //Arrange

            var ctx = new TestContext();
            var authContext = ctx.AddTestAuthorization();
            authContext.SetAuthorized("");
            //authContext.SetRoles("Admin");
            //Act
            //Code under text = cut
            var cut = ctx.RenderComponent<Home>();
            //Assert
            cut.MarkupMatches("<h1>Welcome,You are logedin!</h1>\r\n   <button >Press Me</button>");
        }

        [Fact]
        public void LogedinCodeTest()
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

        [Fact]
        public void NotLogedinViewTest()
        {
            //Arrange

            var ctx = new TestContext();
            var authContext = ctx.AddTestAuthorization();
            authContext.SetNotAuthorized();
            //authContext.SetRoles("Admin");
            //Act
            //Code under text = cut
            var cut = ctx.RenderComponent<Home>();
            //Assert
            cut.MarkupMatches("<p>You must log in to view page</p> \r\n  <button >Press Me</button>");
        }

        [Fact]
        public void NotLogedinCodeTest()
        {
            //Arrange

            var ctx = new TestContext();
            var authContext = ctx.AddTestAuthorization();
            authContext.SetNotAuthorized();
            //Act
            //Code under text = cut
            var cut = ctx.RenderComponent<Home>();
            var homeObj = cut.Instance;

            //Assert
            Assert.Equal(homeObj.isAuthenticated, false);
        }
    }
}