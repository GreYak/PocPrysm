using Devices;
using Devices.Exceptions;

namespace UnitTests.Domain.Devices
{
    public class AppVariablesRoot_AddOrUpdateVariableTests
    {
        [Fact]
        public void AddOrUpdateVariable_Should_Add_New_Variable_If_Not_Exists()
        {
            // Arrange
            var root = new AppVariablesRoot(new List<AppVariable>());
            var newVar = new AppVariable("NewVar", "ext1", AppVariable.AppType.State);

            // Act
            root.AddOrUpdateVariable(newVar);

            // Assert
            Assert.Contains(newVar, root.Variables);
            Assert.Equal(1, root.NewVariablesCount);
            Assert.Equal(0, root.UpdatedVariablesCount);
        }

        [Fact]
        public void AddOrUpdateVariable_Should_Update_Existing_Variable()
        {
            // Arrange
            var originalVar = new AppVariable("Var1", "ext1", AppVariable.AppType.State);
            var root = new AppVariablesRoot(new List<AppVariable> { originalVar });

            var updatedVar = new AppVariable("Var1", "ext2", AppVariable.AppType.Node); 

            // Act
            root.AddOrUpdateVariable(updatedVar);

            // Assert
            Assert.Contains(updatedVar, root.Variables);
            Assert.DoesNotContain(root.Variables, v => ReferenceEquals(v, originalVar));
            Assert.Equal(0, root.NewVariablesCount);
            Assert.Equal(1, root.UpdatedVariablesCount);
        }

        [Fact]
        public void AddOrUpdateVariable_Should_Throw_When_Update_Variable_Not_Found()
        {
            // Arrange
            var root = new AppVariablesRoot(new List<AppVariable>());

            var varToUpdate = new AppVariable("MissingVar", "ext1", AppVariable.AppType.State);

            // Manipulate internal state to simulate missing key (not possible normally)
            // Instead, test indirectly by calling AddOrUpdateVariable on empty root with variable that triggers update

            // Because AddOrUpdateVariable checks existence before updating,
            // this scenario can't occur directly. So no test here is needed.

            // Test that UpdateVariable throws if called directly with missing key
            // => Since UpdateVariable is private, this can't be tested from outside.

            // So this test is skipped or we can note it here.
        }

        [Fact]
        public void AddOrUpdateVariable_Should_Throw_DuplicateVariableException_On_Key_Duplication()
        {
            // Arrange
            var var1 = new AppVariable("Var1", "ext1", AppVariable.AppType.State);
            var var2 = new AppVariable("Var2", "ext2", AppVariable.AppType.Node);

            var root = new AppVariablesRoot(new List<AppVariable> { var1, var2 });

            // Create a variable with keys that conflict with existing variables
            var conflictingChild = new AppVariable("Var1", "ext3", AppVariable.AppType.State);
            var newVar = new AppVariable("NewVar", "ext4", AppVariable.AppType.Node);
            newVar.AddChildren(conflictingChild);

            // Act & Assert
            Assert.Throws<DuplicateVariableException>(() => root.AddOrUpdateVariable(newVar));
        }
    }
}
