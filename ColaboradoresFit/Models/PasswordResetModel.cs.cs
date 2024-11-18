using System;

namespace ColaboradoresFit.Models
{
    public class PasswordResetModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
