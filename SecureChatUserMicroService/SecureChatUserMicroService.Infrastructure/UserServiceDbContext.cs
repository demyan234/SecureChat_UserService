using Microsoft.EntityFrameworkCore;
using SecureChatUserMicroService.Application.Common.Interfaces;
using SecureChatUserMicroService.Domain.Entities;
using SecureChatUserMicroService.Infrastructure.Configurations;

namespace SecureChatUserMicroService.Infrastructure
{
    public sealed class UserServiceDbContext : DbContext, IUserServiceDbContext
    {
        private readonly string _defaultSchema = "UserMicroService";

        public UserServiceDbContext(DbContextOptions<UserServiceDbContext> options)
            : base(options)
        {
        }

        public DbSet<UsersEntity> Users{ get; set; }

        public void Migrate()
        {
            if (Database.IsRelational())
            {
                Database.EnsureCreated();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(_defaultSchema);

            #region user

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());

            #endregion

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserServiceDbContext).Assembly);
        }

        public UserServiceDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server=193.42.115.251;User Id=nikita;Password=qwertyaib12345678;Port=5432;Database=chat;",
                npgsqlOptions => { npgsqlOptions.UseNodaTime(); }).UseLazyLoadingProxies();
            /*optionsBuilder.UseNpgsql("Server=radiomgn.ru;User Id=nikita;Password=qwertyaib12345678;Port=4444;Database=chat;",
                npgsqlOptions => { npgsqlOptions.UseNodaTime(); }).UseLazyLoadingProxies();*/
            /*optionsBuilder.UseNpgsql("Server=localhost;User Id=postgres2;Password=0000;Port=5432;Database=securechat_dev;",
                npgsqlOptions => { npgsqlOptions.UseNodaTime(); }).UseLazyLoadingProxies();*/
        }

        private static DbContextOptions<T> ChangeOptionsType<T>(DbContextOptions options) where T : DbContext
        {
            return new DbContextOptionsBuilder<T>()
                .Options;
        }
    }
}