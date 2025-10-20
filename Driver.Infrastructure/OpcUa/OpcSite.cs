using Devices;

namespace Driver.Infrastructure.OpcUa
{
    internal class OpcSite : ExternalDevice
    {
        private int _id;
        private string _code;
        private int _subSitesCount;
        private string[] _values;

        public OpcSite(int id, string code, string[] values, int subSitesCount)
        {
            _id = id;   
            _code = code;
            _subSitesCount = subSitesCount;
            _values = values;

        }

        public override IEnumerable<AppVariable> GetAppVariables()
        {
            var appVariable = new AppVariable($"OpcSite-{_id}", _code, AppVariable.AppType.Node);
            for(int i=0; i < _subSitesCount; i++)
            {
                var child = new AppVariable($"OpcSubSite-{_id}-{i}",_code, AppVariable.AppType.State);
                child.SetValue($"{_code}-{_values[i]}");

                appVariable.AddChildren(child);
            }

            var countVariable = new AppVariable($"OpcSite-{_id}-Count", _code, AppVariable.AppType.State);
            countVariable.SetValue(_values.Count().ToString());

            return new List<AppVariable> { appVariable, countVariable };

        }
    }
}
