using Devices;
using Devices.Exceptions;

namespace UnitTests.Domain.Devices
{
    public class AppVariable_AddChildrenTests
    {
        [Fact]
        public void AddChildren_Should_Add_Child_When_Type_Is_Node_And_Key_Is_Unique()
        {
            // Arrange
            var parent = new AppVariable("Parent", "ext-1", AppVariable.AppType.Node);
            var child = new AppVariable("Child", "ext-2", AppVariable.AppType.State);

            // Act
            parent.AddChildren(child);

            // Assert
            Assert.Single(parent.Children);
            Assert.Equal("Child", parent.Children[0].Name);
        }

        [Theory]
        [InlineData(AppVariable.AppType.State)]
        public void AddChildren_Should_Throw_When_Type_Is_Not_Node(AppVariable.AppType type)
        {
            // Arrange
            var parent = new AppVariable("InvalidParent", "ext-1", type);
            var child = new AppVariable("Child", "ext-2", AppVariable.AppType.State);

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => parent.AddChildren(child));
            Assert.Contains("AddChildren not allowed for type", ex.Message);
        }

        [Fact]
        public void AddChildren_Should_Throw_When_Child_Has_Duplicate_Key()
        {
            // Arrange
            var parent = new AppVariable("Node", "ext-1", AppVariable.AppType.Node);
            var child1 = new AppVariable("Child", "ext-2", AppVariable.AppType.State);
            var child2 = new AppVariable("Child", "ext-3", AppVariable.AppType.State); // same name => same key

            parent.AddChildren(child1);

            // Act & Assert
            var ex = Assert.Throws<DuplicateVariableException>(() => parent.AddChildren(child2));
            Assert.Contains("Child", ex.Message); // The duplicate key name should appear
        }

        [Fact]
        public void AddChildren_Should_Throw_When_Childs_SubChildren_Have_Duplicate_Key()
        {
            // Arrange
            var parent = new AppVariable("Node", "ext-1", AppVariable.AppType.Node);

            var child1 = new AppVariable("Level1", "ext-2", AppVariable.AppType.Node);
            var child2 = new AppVariable("Level1", "ext-3", AppVariable.AppType.Node); // Duplicate key in root

            var subChild = new AppVariable("SubChild", "ext-4", AppVariable.AppType.State);
            child1.AddChildren(subChild);

            parent.AddChildren(child1);

            // Act & Assert
            var ex = Assert.Throws<DuplicateVariableException>(() => parent.AddChildren(child2));
            Assert.Contains("Level1", ex.Message);
        }
    }
}