using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SecureChatUserMicroService.Domain.Entities;

namespace SecureChatUserMicroService.Application.Common.Interfaces
{
    public interface IUserServiceDbContext
    {
        DatabaseFacade Database { get; }

        public DbSet<UserEntity> User { get; set; }
        public DbSet<UserProfileEntity> UserProfile { get; set; }
        public DbSet<BlockUserEntity> BlockUser { get; set; }

        void Migrate();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}