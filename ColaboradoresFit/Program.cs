using FluentEmail.Core;
using FluentEmail.Smtp;
using Microsoft.Extensions.DependencyInjection;
using ColaboradoresFit.DataContext;
using Microsoft.EntityFrameworkCore;
using ColaboradoresFit.Service.ColaboradorService;
using System.Net;
using System.Net.Mail;

var builder = WebApplication.CreateBuilder(args);

// Adicionando o serviço FluentEmail com SMTP
builder.Services.AddFluentEmail("k_du02@hotmail.com") // Remetente
    .AddSmtpSender(new SmtpClient("sandbox.smtp.mailtrap.io") // O servidor SMTP do Mailtrap
    {
        Port = 587,
        Credentials = new NetworkCredential("68135d9236bfd9", "d339254fe08442"),
        EnableSsl = true
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IColaboradorInterface, ColaboradorService>();
builder.Services.AddTransient<EmailService>();

var app = builder.Build();

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();