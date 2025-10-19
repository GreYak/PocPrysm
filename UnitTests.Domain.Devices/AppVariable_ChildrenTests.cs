using Devices;

namespace UnitTests.Domain.Devices
{
    public class AppVariable_ChildrenTests
    {
        [Fact]
        public void Children_Should_Be_Empty_By_Default()
        {
            // Arrange
            var parent = new AppVariable("Parent", "ext-1", AppVariable.AppType.Node);

            // Act
            var children = parent.Children;

            // Assert
            Assert.Empty(children);
        }

        [Fact]
        public void Children_Should_Contain_Added_Children()
        {
            // Arrange
            var parent = new AppVariable("Parent", "ext-1", AppVariable.AppType.Node);
            var child1 = new AppVariable("Child1", "ext-2", AppVariable.AppType.State);
            var child2 = new AppVariable("Child2", "ext-3", AppVariable.AppType.State);

            // Act
            parent.AddChildren(child1);
            parent.AddChildren(child2);
            var children = parent.Children;

            // Assert
            Assert.Equal(2, children.Count);
            Assert.Contains(child1, children);
            Assert.Contains(child2, children);
        }

        [Fact]
        public void Children_Should_Be_ReadOnly()
        {
            // Arrange
            var parent = new AppVariable("Parent", "ext-1", AppVariable.AppType.Node);
            var children = parent.Children;

            // Act & Assert
            Assert.IsAssignableFrom<IReadOnlyList<AppVariable>>(children);
            Assert.Throws<NotSupportedException>(() => ((IList<AppVariable>)children).Add(new AppVariable("Hacker", "ext", AppVariable.AppType.State)));
        }
    }
}
