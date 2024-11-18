using ColaboradoresFit.DataContext;
using ColaboradoresFit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ColaboradoresFit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordResetController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;

        public PasswordResetController(ApplicationDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpPost("request-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] EmailRequest emailRequest)
        {
            if (string.IsNullOrEmpty(emailRequest.Email))
            {
                return BadRequest(new { message = "O e-mail não pode estar vazio." });
            }

            var user = await _context.Colaboradores
                .FirstOrDefaultAsync(u => u.Email.ToLower() == emailRequest.Email.ToLower());

            if (user == null)
            {
                return BadRequest(new { message = "Usuário não encontrado." });
            }

            // Gera uma nova senha temporária
            var newPassword = _emailService.GenerateRandomPassword();

            // Atualiza a senha do usuário no banco de dados
            user.Senha = newPassword;
            await _context.SaveChangesAsync();

            try
            {
                // Envia o e-mail com a nova senha temporária
                await _emailService.SendPasswordResetEmail(emailRequest.Email, newPassword);
                return Ok(new { message = "Nova senha enviada ao e-mail." });
            }
            catch (Exception ex)
            {
                // Loga o erro
                Console.WriteLine("Erro ao enviar o e-mail: " + ex.Message);
                return StatusCode(500, new { message = "Erro ao enviar o e-mail: " + ex.Message });
            }
        }
    }
}

