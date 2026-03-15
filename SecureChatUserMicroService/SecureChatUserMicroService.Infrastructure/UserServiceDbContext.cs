using Microsoft.EntityFrameworkCore;
using SecureChatUserMicroService.Application.Common.Interfaces;

namespace SecureChatUserMicroService.Infrastructure;

public sealed class UserServiceDbContext : DbContext, IUserServiceDbContext
    {
        private readonly string _defaultSchema = "UserMicroService";

        public UserServiceDbContext(DbContextOptions<UserServiceDbContext> options)
            : base(options)
        {
        }
        
        //public DbSet<UserProfileEntity> UserProfile { get; set; }

        public void Migrate()
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(_defaultSchema);

            #region user

            //modelBuilder.ApplyConfiguration(new UserProfileConfiguration());

            #endregion

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserServiceDbContext).Assembly);
        }

        public UserServiceDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseNpgsql("Server=109.205.58.47;User Id=persProjectUser;Password=7FEpX_wl6g;Port=5432;Database=testDb;", npgsqlOptions => { npgsqlOptions.UseNodaTime(); }).UseLazyLoadingProxies();
            optionsBuilder.UseNpgsql("Server=localhost;User Id=postgres;Password=0000;Port=5432;Database=postgres;", npgsqlOptions => { npgsqlOptions.UseNodaTime(); }).UseLazyLoadingProxies();
        }

        private static DbContextOptions<T> ChangeOptionsType<T>(DbContextOptions options) where T : DbContext
        {
            return new DbContextOptionsBuilder<T>()
                .Options;
        }
    }