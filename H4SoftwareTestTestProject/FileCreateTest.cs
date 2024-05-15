using Bunit;
using Bunit.TestDoubles;
using H4SoftwareTest.Components.Pages;

namespace H4SoftwareTestTestProject
{
    public class FileCreateTest
    {
        [Fact]
        public async Task SaveToFile_Successfully()
        {
            // Arrange
            var component = new Home();
            component.isSaveToFile = false;

            // Act

            await component.SaveToFile();

            // Assert
            Assert.True(component.isSaveToFile); 
        }

        //[Fact]
        //public async Task SaveToFile_Exception()
        //{
        //    // Arrange
        //    var component = new Home();
        //    component.isSaveToFile = false;

        //    // Act & Assert
        //    await Assert.ThrowsAsync<Exception>(async () => await component.SaveToFile()); // Verify that an exception is thrown if an error occurs during file saving
        //}
    }
}
