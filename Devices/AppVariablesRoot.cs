
using Devices.Exceptions;

namespace Devices
{
    public class AppVariablesRoot
    {
        private Dictionary<string, AppVariable> _variablesByName;
        private HashSet<string> _addedVariables;
        private HashSet<string> _updatedVariables;

        public AppVariablesRoot(IEnumerable<AppVariable> variables)
        {
            _variablesByName = new Dictionary<string, AppVariable>();
            _addedVariables = new HashSet<string>();
            _updatedVariables = new HashSet<string>();

            foreach (var variable in variables)
            {
                AddVariable(variable);
            }
            _addedVariables.Clear();
        }

        public IReadOnlyList<AppVariable> Variables => _variablesByName.Values.ToList().AsReadOnly();

        public void AddOrUpdateVariable(AppVariable variable)
        {
            if (_variablesByName.ContainsKey(variable.Key))
            {
                UpdateVariable(variable);
            }
            else
            {
                AddVariable(variable);
            }
        }

        public int NewVariablesCount => _addedVariables.Count;
        public int UpdatedVariablesCount => _updatedVariables.Count;

        private void UpdateVariable(AppVariable variableToUpdate)
        {
            if (!_variablesByName.ContainsKey(variableToUpdate.Key))
                throw new KeyNotFoundException(variableToUpdate.Key);

            EnsureVariableKeyUnicity(variableToUpdate);

            _variablesByName[variableToUpdate.Key] = variableToUpdate;
            _updatedVariables.Add(variableToUpdate.Key);
        }

        private void AddVariable(AppVariable variableToAdd)
        {
            EnsureVariableKeyUnicity(variableToAdd);

            _variablesByName.Add(variableToAdd.Key, variableToAdd);
            _addedVariables.Add(variableToAdd.Key);
        }

        private void EnsureVariableKeyUnicity(AppVariable variableToCheck)
        {
            IEnumerable<string> keysToCheck = variableToCheck.AllKeys;

            foreach (AppVariable var in _variablesByName.Values)
                var.EnsureKeyUnicity(keysToCheck);
        }
    }
}
