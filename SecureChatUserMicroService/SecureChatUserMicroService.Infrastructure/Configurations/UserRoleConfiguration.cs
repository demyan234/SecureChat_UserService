using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecureChatUserMicroService.Domain.Enums.User;

namespace SecureChatUserMicroService.Infrastructure.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRolesEnum>
    {
        public void Configure(EntityTypeBuilder<UserRolesEnum> builder)
        {
            builder.ToTable("UserStatuses");

            builder.Property(o => o.Id)
                .ValueGeneratedNever();

            builder.Property(o => o.Name)
                .HasMaxLength(200)
                .HasComment("Роль");

            builder.HasIndex(x => x.Id);
        }
    }
}