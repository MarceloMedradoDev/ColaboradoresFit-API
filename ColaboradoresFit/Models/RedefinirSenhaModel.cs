using System.ComponentModel.DataAnnotations;

namespace ColaboradoresFit.Models
{
    public class RedefinirSenhaModel
    {
        [Required(ErrorMessage = "Digite o login")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Digite a e-mail")]
        public string Email { get; set; }
    }
}
