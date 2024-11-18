using ColaboradoresFit.Models;
using Microsoft.EntityFrameworkCore;

namespace ColaboradoresFit.DataContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ColaboradorModel> Colaboradores { get; set; }
        public DbSet<PasswordResetModel> PasswordResets { get; set; } 
    }
}
