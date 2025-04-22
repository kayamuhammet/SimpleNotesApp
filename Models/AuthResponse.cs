namespace SimpleNotesApp.Models
{
    public class AuthResponse
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
        public string? Email { get; set; }
        public string? UserId { get; set; }
    }
}