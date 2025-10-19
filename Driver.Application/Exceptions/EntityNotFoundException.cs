namespace Driver.Application.Exceptions
{
    public class EntityNotFoundException : ApplicationException
    {
        public EntityNotFoundException(string entityName) 
            : base($"No {entityName} have been found.")
        {
        }
    }
}
