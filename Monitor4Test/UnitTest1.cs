using Bunit;

namespace Monitor4Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {

            // Arrange
            using var context = new Bunit.TestContext();

            // Act
            var cut = context.RenderComponent<Monitor4WebServer.Pages.Index>();
            cut.Find("button").Click();

            // Assert
            cut.Find("h5").MarkupMatches("<h5>Blazor component is clicked.</h5>");

            //Assert.Pass();
        }
    }
}