namespace NStore.Shared.Exceptions
{
    public enum ExceptionLevel
    {
        Info,
        Warning,
        Error,
        Fatal
    }

    public class DomainException : Exception
    {
        public ExceptionLevel Level { get; protected set; }

        public DomainException(string message, ExceptionLevel level, Exception? innerException = null) : base(message, innerException)
        {
            Level = level;
        }
    }
}
