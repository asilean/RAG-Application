using Microsoft.EntityFrameworkCore;
using ProjectManagament_WebApp.Data.Models;

namespace ProjectManagament_WebApp.Data
{
    public class PMContext : DbContext
    {
        public PMContext(DbContextOptions<PMContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<ConversationHistory> ConversationHistories { get; set; }
        public DbSet<ForgotPasswordCode> ForgotPasswordCodes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=localhost; database=Companier; integrated security=true;");
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<User>().Property(u => u.Email).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Password).IsRequired();

            modelBuilder.Entity<Module>().HasKey(m => m.Id);
            modelBuilder.Entity<Module>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Module>().Property(m => m.Name).IsRequired();
            modelBuilder.Entity<Module>().Property(m => m.Description).IsRequired();
            
            modelBuilder.Entity<ConversationHistory>().HasKey(ch => ch.Id);
            modelBuilder.Entity<ConversationHistory>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<ConversationHistory>().HasOne(ch => ch.User).WithMany(u => u.ConversationHistories).HasForeignKey(ch => ch.UserId);
            modelBuilder.Entity<ConversationHistory>().HasOne(ch => ch.Module).WithMany(m => m.ConversationHistories).HasForeignKey(ch => ch.ModuleId);
            modelBuilder.Entity<ConversationHistory>().Property(ch => ch.UserId).IsRequired();
            modelBuilder.Entity<ConversationHistory>().Property(ch => ch.ModuleId).IsRequired();
            modelBuilder.Entity<ConversationHistory>().Property(ch => ch.Context).IsRequired();
            modelBuilder.Entity<ConversationHistory>().Property(ch => ch.Role).IsRequired();

            modelBuilder.Entity<ForgotPasswordCode>().HasKey(fpc => fpc.Id);
            modelBuilder.Entity<ForgotPasswordCode>().Property(fpc => fpc.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<ForgotPasswordCode>().HasOne(fpc => fpc.User).WithMany(u => u.ForgotPasswordCodes).HasForeignKey(fpc => fpc.UserId);
            modelBuilder.Entity<ForgotPasswordCode>().Property(fpc => fpc.UserId).IsRequired();
            modelBuilder.Entity<ForgotPasswordCode>().Property(fpc => fpc.Code).IsRequired();
            modelBuilder.Entity<ForgotPasswordCode>().Property(fpc => fpc.CreatedAt).IsRequired();
            modelBuilder.Entity<ForgotPasswordCode>().Property(fpc => fpc.ExpiredAt).IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}
