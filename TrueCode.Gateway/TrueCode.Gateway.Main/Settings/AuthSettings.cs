namespace TrueCode.Gateway.Main.Settings
{
    internal record AuthSettings
    {
        public string Issuer { get; init; } = default!;
        public string Audience { get; init; } = default!;
        public string Secret { get; init; } = default!;
    }
}
