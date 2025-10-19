using Devices;
using Moq;

namespace UnitTests.Driver.Application.DomainHelpers
{
    internal class MockExternalDevices : ExternalDevice
    {
        private readonly AppVariable _variableToReturn;

        public MockExternalDevices(AppVariable refAppVariable)
        {
            _variableToReturn = refAppVariable;
        }

        override public IEnumerable<AppVariable> GetAppVariables()
        {
            yield return _variableToReturn;
        }
    }
}
