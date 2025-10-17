namespace Devices.Exceptions
{
    public class DuplicateVariableException : ApplicationException
    {
        public readonly IReadOnlyList<string> VariableKeys;

        public DuplicateVariableException(IEnumerable<string> variableKeys) 
            : base($"Some variables ({string.Join(", ", variableKeys)}) are duplicate : a variable must be unique.")
        {
            VariableKeys = variableKeys.ToList().AsReadOnly();
        }
    }
}
