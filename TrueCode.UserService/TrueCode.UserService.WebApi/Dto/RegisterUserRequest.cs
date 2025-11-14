namespace TrueCode.UserService.WebApi.Dto
{
    public record RegisterUserRequest
    {
        public string? UserName { get; init; }
        public string? Password { get; init; }
    }
}
