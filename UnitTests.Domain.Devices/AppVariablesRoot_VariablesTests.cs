using Devices;

namespace UnitTests.Domain.Devices
{
    public class AppVariablesRoot_VariablesTests
    {
        [Fact]
        public void Variables_Should_Return_All_Added_Variables()
        {
            // Arrange
            var var1 = new AppVariable("Var1", "ext1", AppVariable.AppType.State);
            var var2 = new AppVariable("Var2", "ext2", AppVariable.AppType.Node);

            var root = new AppVariablesRoot(new[] { var1, var2 });

            // Act
            var variables = root.Variables;

            // Assert
            Assert.Equal(2, variables.Count);
            Assert.Contains(var1, variables);
            Assert.Contains(var2, variables);
        }

        [Fact]
        public void Variables_Should_Be_ReadOnly()
        {
            // Arrange
            var var1 = new AppVariable("Var1", "ext1", AppVariable.AppType.State);
            var root = new AppVariablesRoot(new[] { var1 });

            // Act
            var variables = root.Variables;

            // Assert
            Assert.IsAssignableFrom<IReadOnlyList<AppVariable>>(variables);
            Assert.ThrowsAny<System.NotSupportedException>(() =>
            {
                // Try to cast to IList to modify (will throw)
                ((IList<AppVariable>)variables).Add(new AppVariable("VarX", "extX", AppVariable.AppType.State));
            });
        }
    }
}
