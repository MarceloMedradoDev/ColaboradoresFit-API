using FluentEmail.Core;
using FluentEmail.Smtp;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailService
{
    private readonly IFluentEmail _fluentEmail;

    public EmailService()
    {
        var smtpSender = new SmtpSender(() => new SmtpClient("smtp.mailtrap.io")
        {
            Port = 587,
            Credentials = new NetworkCredential("68135d9236bfd9", "d339254fe08442"),
            EnableSsl = true
        });

        _fluentEmail = Email.From("k_du02@hotmail.com");
        _fluentEmail.Sender = smtpSender;
    }

    public string GenerateRandomPassword(int length = 10)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public async Task SendPasswordResetEmail(string email, string newPassword)
    {
        var emailMessage = await _fluentEmail
            .To(email)
            .Subject("Redefinição de Senha")
            .Body($@"
            <html>
            <body style='font-family: Arial, sans-serif; color: #333; background-color: #f9f9f9; padding: 20px;'>
                <div style='max-width: 600px; margin: 0 auto; background-color: #fff; padding: 20px; border-radius: 8px; box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);'>
                    <h2 style='color: #444;'>Redefinição de Senha</h2>
                    <p>Olá,</p>
                    <p>Sua nova senha temporária é: <strong>{newPassword}</strong></p>
                    <p>Recomendamos que você altere esta senha após fazer login.</p>
                    <a type='submit' style='background-color: #44dc64; color: white; border-radius: 5px; padding: 5px; font-family: Arial, bold' href='http://localhost:4200/'>Seja Redirecionado</a>
                    <p>Atenciosamente,<br/> <br/>Equipe ColaboradoresFit</p>
                </div>
            </body>
            </html>", true)
            .SendAsync();

        if (!emailMessage.Successful)
        {
            throw new Exception("Falha ao enviar o e-mail de redefinição.");
        }
    }
}
