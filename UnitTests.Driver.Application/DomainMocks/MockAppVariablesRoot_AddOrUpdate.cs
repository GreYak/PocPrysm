using Devices;

namespace UnitTests.Driver.Application.DomainMocks
{
    internal class MockAppVariablesRoot_AddOrUpdate : AppVariablesRoot
    {
        private readonly bool _withException;

        public MockAppVariablesRoot_AddOrUpdate(bool withException) : base(new List<AppVariable>())
        {
            _withException = withException;
        }

        public new void AddOrUpdateVariable(AppVariable variable)
        {
            if (_withException)
                throw new ApplicationException("Boom");
        }
    }
}
