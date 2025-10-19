using Devices;

namespace UnitTests.Domain.Devices
{
    public class AppVariable_SetValueTests
    {
        [Fact]
        public void SetValue_Should_Set_Value_When_Type_Is_State()
        {
            // Arrange
            var variable = new AppVariable("Temperature", "ext-1", AppVariable.AppType.State);
            var expectedValue = "25°C";

            // Act
            variable.SetValue(expectedValue);

            // Assert
            Assert.Equal(expectedValue, variable.Value);
        }

        [Theory]
        [InlineData(AppVariable.AppType.Node)]
        public void SetValue_Should_Throw_When_Type_Is_Not_State(AppVariable.AppType type)
        {
            // Arrange
            var variable = new AppVariable("Node1", "ext-2", type);

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => variable.SetValue("test"));
            Assert.Contains("SetValue not allowed for type", ex.Message);
        }
    }
}