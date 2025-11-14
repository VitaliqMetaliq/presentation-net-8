namespace TrueCode.UserService.Application.Exceptions
{
    public sealed class AccessDeniedException : Exception
    {
        public AccessDeniedException(string message) : base(message) { }
    }
}
