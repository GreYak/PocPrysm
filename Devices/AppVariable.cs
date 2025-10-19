using Devices.Exceptions;

namespace Devices
{
    public class AppVariable : IEquatable<AppVariable>
    {
        public string Name { get; }
        public string ExternalId { get; }
        public AppType Type { get; }
        public string? Value { get; private set; }
        private readonly List<AppVariable> _children;

        public AppVariable(string name, string externalId, AppType type)
        {
            Name = name;
            ExternalId = externalId;
            Type = type;
            _children = new List<AppVariable>();
        }

        public void SetValue(string value)
        {
            if (Type != AppType.State)
                throw new InvalidOperationException($"{nameof(SetValue)} not allowed for type : {Type}");

            Value = value;
        }

        public void AddChildren(AppVariable variableToAdd)
        {
            if (Type != AppType.Node)
                throw new InvalidOperationException($"{nameof(AddChildren)} not allowed for type : {Type}");

            EnsureKeyUnicity(variableToAdd.ChildrenKeys);

            _children.Add(variableToAdd);
        }

        internal string Key => Name;

        internal void EnsureKeyUnicity(IEnumerable<string> keysToCheck)
        {
            IEnumerable<string> duplicateKeys = ChildrenKeys.Intersect(keysToCheck);
            if (duplicateKeys.Any())
            {
                throw new DuplicateVariableException(duplicateKeys);
            }
        }

        internal IEnumerable<string> ChildrenKeys
        {
            get
            {
                yield return Key;

                foreach (var child in _children)
                {
                    foreach (var key in child.ChildrenKeys)
                    {
                        yield return key;
                    }
                }
            }
         }

        public enum AppType
        {
            Node,
            State
        }

        public IReadOnlyList<AppVariable> Children => _children.ToList().AsReadOnly();

        #region IEquatable

        public bool Equals(AppVariable? other)
        {
            if (other is null) 
                return false;

            return Key == other.Key;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as AppVariable);
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }

        public static bool operator ==(AppVariable? left, AppVariable? right)
        {
            if (ReferenceEquals(left, right))
                return true;

            if (left is null) 
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(AppVariable? left, AppVariable? right)
        {
            return !(left == right);
        }

        #endregion
    }
}