using FileSystemVisitor;
using Xunit;

namespace FileSystemVisitorTests
{
    public class InputValidatorTests
    {
        [Theory]
        [InlineData(@"C:\Users\Veronika_Korzhevykh\Pictures")]
        [InlineData(@"C:\Users\Veronika123\Pictures")]
        [InlineData(@"C:\Users\Вероника_Коржевых\Pictures")]
        [InlineData(@"C:\Users")]
        [InlineData(@"C:")]
        public void IsPathValid_ValidPath_TrueReturned(string validPath)
        {
            // Arrange
            var validator = new InputValidator();

            // Act
            var result = validator.IsPathValid(validPath);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(@"C:\Users\Veronika_Korzhevykh\Pictures.txt")]
        [InlineData(@"C:\Users\\Pictures")]
        [InlineData(@"C:\Users\Вероника_Коржевых?\Pictures")]
        [InlineData(@"C1:\Users")]
        [InlineData(@"C:\")]
        public void IsPathValid_InvalidPath_FalseReturned(string invalidPath)
        {
            // Arrange
            var validator = new InputValidator();

            // Act
            var result = validator.IsPathValid(invalidPath);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(".txt")]
        [InlineData("")]
        [InlineData(".aaa")]
        public void IsExtensionValid_ValidFileExtension_TrueReturned(string fileExtension)
        {
            // Arrange
            var validator = new InputValidator();

            // Act
            var result = validator.IsExtensionValid(fileExtension);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("qwerty.")]
        [InlineData(null)]
        public void IsExtensionValid_InvalidFileExtension_FalseReturned(string fileExtension)
        {
            // Arrange
            var validator = new InputValidator();

            // Act
            var result = validator.IsExtensionValid(fileExtension);

            // Assert
            Assert.False(result);
        }

    }
}
