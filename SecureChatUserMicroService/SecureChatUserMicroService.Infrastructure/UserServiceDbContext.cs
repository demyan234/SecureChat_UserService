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
        
        public DbSet<UserEntity> User { get; set; }
        public DbSet<UserProfileEntity> UserProfile { get; set; }
        public DbSet<BlockUserEntity> BlockUser { get; set; }

        public void Migrate()
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(_defaultSchema);

            #region user

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserProfileConfiguration());
            modelBuilder.ApplyConfiguration(new BlockUserConfiguration());

            #endregion

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserServiceDbContext).Assembly);
        }

        public UserServiceDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server=localhost;User Id=postgres;Password=0000;Port=5432;Database=postgres;", npgsqlOptions => { npgsqlOptions.UseNodaTime(); }).UseLazyLoadingProxies();
        }

        private static DbContextOptions<T> ChangeOptionsType<T>(DbContextOptions options) where T : DbContext
        {
            return new DbContextOptionsBuilder<T>()
                .Options;
        }
    }
}