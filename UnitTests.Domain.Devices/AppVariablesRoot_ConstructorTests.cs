using Devices;

namespace UnitTests.Domain.Devices
{
    public class AppVariablesRoot_ConstructorTests
    {
        [Fact]
        public void Constructor_Should_Add_All_Variables()
        {
            // Arrange
            var var1 = new AppVariable("A", "ext1", AppVariable.AppType.State);
            var var2 = new AppVariable("B", "ext2", AppVariable.AppType.Node);
            var variables = new List<AppVariable> { var1, var2 };

            // Act
            var root = new AppVariablesRoot(variables);

            // Assert
            Assert.Equal(2, root.Variables.Count);
            Assert.Contains(var1, root.Variables);
            Assert.Contains(var2, root.Variables);
        }

        [Fact]
        public void Constructor_Should_Clear_AddedVariables_After_Adding()
        {
            // Arrange
            var var1 = new AppVariable("A", "ext1", AppVariable.AppType.State);
            var root = new AppVariablesRoot(new[] { var1 });

            // Act & Assert
            Assert.Equal(0, root.NewVariablesCount);
        }
    }
}
