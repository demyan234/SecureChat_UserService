using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecureChatUserMicroService.Domain.Entities;

namespace SecureChatUserMicroService.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UsersEntity>
    {
        public void Configure(EntityTypeBuilder<UsersEntity> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Email)
                .HasMaxLength(150)
                .IsRequired()
                .HasComment("Почта пользователя");
            
            builder.Property(x => x.Nickname)
                .HasMaxLength(50)
                .IsRequired()
                .HasComment("Ник пользователя");
            
            builder.Property(x => x.Name)
                .HasMaxLength(50)
                .IsRequired()
                .HasComment("Имя пользователя");
            
            builder.Property(x => x.AvatarUrl)
                .IsRequired()
                .HasComment("Ссылка на аватар пользователя");
            
            builder.Property(x => x.RoleId)
                .IsRequired()
                .HasComment("Идентификатор роли пользователя");
            
            builder.Property(x => x.DeletedAt)
                .IsRequired(false)
                .HasDefaultValue(null)
                .HasComment("Когда был удален пользователь");
            
            builder.HasIndex(x => x.Nickname).IsUnique();
            builder.HasIndex(x => x.Email).IsUnique();
        }
    }
}