namespace Comp584Server.Data.DTO
{
    public class LoginResponse
    {
        public required bool Success { get; set; }
        public required string Token { get; set; }
        public required string Message { get; set; }

    }
}
