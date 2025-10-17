using Devices;
using static Devices.AppVariable;

namespace Driver.Infrastructure.AppVision
{
    internal class VariableRow
    {
        public Guid Id { get; }
        public string Name { get; }
        public string ExternalId { get; }
        public char Type { get; }
        public string? Value { get; }
        public DateTimeOffset Date { get; }
        public string? ParentName { get; }



        private VariableRow(AppVariable appVariable, string? parentName=null)
        {
            Id = Guid.NewGuid();
            Name = appVariable.Name;
            ExternalId = appVariable.ExternalId;
            Value = appVariable.Value ?? "None";
            Type = appVariable.Type == AppType.State ? 'V' : 'N';
            Date = DateTimeOffset.Now;  // TODO : !!!
            ParentName = parentName;
        }

        static public IEnumerable<VariableRow> ToAppVisionModel(AppVariable appVariable, string? parentName=null)
        {
            var variables = new List<VariableRow>();

            if (appVariable.Type == AppType.State)
            {
                variables.Add(new VariableRow(appVariable));
            }
            else
            {
                foreach(var child in appVariable.Children)
                    variables.AddRange(ToAppVisionModel(child, appVariable.Name));
            }

            return variables;
        }
    }
}
