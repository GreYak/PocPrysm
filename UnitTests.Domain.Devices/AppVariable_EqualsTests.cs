using Devices;

namespace UnitTests.Domain.Devices
{
    public class AppVariable_EqualsTests
    {
        [Fact]
        public void Equals_Should_Return_True_For_Same_Name()
        {
            // Arrange
            var var1 = new AppVariable("SharedName", "ext-1", AppVariable.AppType.State);
            var var2 = new AppVariable("SharedName", "ext-2", AppVariable.AppType.Node);

            // Act
            var result = var1.Equals(var2);

            // Assert
            Assert.True(result);
            Assert.Equal(var1, var2); // Uses overridden Equals
        }

        [Fact]
        public void Equals_Should_Return_False_When_Name_Differs()
        {
            // Arrange
            var var1 = new AppVariable("Name1", "ext-1", AppVariable.AppType.State);
            var var2 = new AppVariable("Name2", "ext-2", AppVariable.AppType.State);

            // Act
            var result = var1.Equals(var2);

            // Assert
            Assert.False(result);
            Assert.NotEqual(var1, var2);
        }

        [Fact]
        public void Equals_Should_Return_False_When_Compared_To_Null()
        {
            // Arrange
            var variable = new AppVariable("SomeName", "ext-1", AppVariable.AppType.Node);

            // Act
            var result = variable.Equals(null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Equals_Should_Return_True_When_Compared_To_Self()
        {
            // Arrange
            var variable = new AppVariable("Same", "ext-1", AppVariable.AppType.Node);

            // Act & Assert
            Assert.True(variable.Equals(variable));
            Assert.True(variable == variable);
        }

        [Fact]
        public void Operator_Equality_Should_Respect_Logic()
        {
            // Arrange
            var var1 = new AppVariable("SameName", "ext-1", AppVariable.AppType.State);
            var var2 = new AppVariable("SameName", "ext-2", AppVariable.AppType.State);

            // Act & Assert
            Assert.True(var1 == var2);
            Assert.False(var1 != var2);
        }

        [Fact]
        public void Operator_Equality_Should_Handle_Nulls()
        {
            // Arrange
            AppVariable? var1 = null;
            AppVariable? var2 = null;
            var var3 = new AppVariable("X", "ext", AppVariable.AppType.State);

            // Act & Assert
            Assert.True(var1 == var2);          // Both null => equal
            Assert.False(var1 == var3);         // One null => not equal
            Assert.True(var1 != var3);          // One null => not equal
        }
    }
}