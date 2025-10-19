
namespace Driver.Application.Exceptions
{
    public class UseCaseException : ApplicationException
    {
        public string UseCase { get; }

        public UseCaseException(string useCase, ApplicationException domainException)
            : base(FormatMessage(useCase), domainException)
        {
            UseCase = useCase;
        }

        private static string FormatMessage(string useCase)
            => $"A functional error has been thrown for the use case '{useCase}'.";
    }
}
