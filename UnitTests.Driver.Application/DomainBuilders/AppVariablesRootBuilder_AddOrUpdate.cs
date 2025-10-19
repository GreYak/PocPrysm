using Devices;

namespace UnitTests.Driver.Application.DomainHelpers
{
    internal class AppVariablesRootBuilder_AddOrUpdate
    {
        private readonly bool _withException;

        public AppVariablesRootBuilder_AddOrUpdate(bool withException)
        {
            _withException = withException;
        }

        public AppVariablesRoot GenerateAppVariableRoot(AppVariable refAppVariable)
        {
            var variables = new List<AppVariable>();

            if (_withException)
            {
                var variable = new AppVariable("Mere", Guid.NewGuid().ToString(), AppVariable.AppType.Node);
                variable.AddChildren(refAppVariable);

                variables.Add(variable);
            }

            return new AppVariablesRoot(variables);
        }
    }
}
