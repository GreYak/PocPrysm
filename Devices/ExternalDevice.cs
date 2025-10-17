
namespace Devices
{
    public abstract class ExternalDevice
    {
        virtual public IEnumerable<AppVariable> GetAppVariables() => new List<AppVariable>();
    }
}
