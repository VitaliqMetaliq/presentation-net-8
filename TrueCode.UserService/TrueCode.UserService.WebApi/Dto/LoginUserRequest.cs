namespace TrueCode.UserService.WebApi.Dto
{
    public record LoginUserRequest
    {
        public string? UserName { get; init; }
        public string? Password { get; init; }
    }
}
