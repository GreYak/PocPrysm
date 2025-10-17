using Devices;

namespace Driver.Infrastructure.OpcUa
{
    internal class OpcUnite : ExternalDevice
    {
        private int _id;
        private Guid _value;
      
        public OpcUnite(int id, Guid value)
        {
            _id = id;
            _value = value;
        }

        public override IEnumerable<AppVariable> GetAppVariables()
        {
            var appVariable = new AppVariable($"OpcUnite-{_id}", _id.ToString(), AppVariable.AppType.State);
            appVariable.SetValue(_value.ToString());

            return new List<AppVariable> { appVariable };

        }
    }
}
