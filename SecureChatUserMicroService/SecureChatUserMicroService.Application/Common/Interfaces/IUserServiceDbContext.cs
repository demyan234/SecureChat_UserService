using Microsoft.EntityFrameworkCore.Infrastructure;

namespace SecureChatUserMicroService.Application.Common.Interfaces;

public interface IUserServiceDbContext
{
    DatabaseFacade Database { get; }
    
    //public DbSet<UserProfileEntity> UserProfile { get; set; }

    void Migrate();

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}