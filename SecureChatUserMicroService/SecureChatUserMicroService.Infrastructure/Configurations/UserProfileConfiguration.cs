using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecureChatUserMicroService.Domain.Entities;

namespace SecureChatUserMicroService.Infrastructure.Configurations
{
    public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfileEntity>
    {
        public void Configure(EntityTypeBuilder<UserProfileEntity> builder)
        {
            builder.ToTable("UserProfiles");
            builder.HasKey(x => x.Id);

            builder.Property<string>(x => x.Name)
                .HasColumnName("Name")
                .IsRequired()
                .HasMaxLength(150)
                .HasComment("Имя");

            builder.Property<string>(x => x.Nickname)
                .HasColumnName("Nickname")
                .IsRequired()
                .HasMaxLength(150)
                .HasComment("Ник");

            builder.Property<string>(x => x.AvatarUrl)
                .HasColumnName("AvatarUrl")
                .IsRequired()
                .HasComment("Ссылка на аватар");

            builder.Property<string?>(x => x.StatusQuote)
                .HasColumnName("StatusQuote")
                .IsRequired()
                .HasMaxLength(140)
                .HasComment("Цитата(статус)");

            builder.Property(x => x.IsBlocked)
                .HasColumnName("IsBlocked")
                .HasDefaultValue(false)
                .HasComment("Признак блокировки");

            builder.Property(x => x.IsDeleted)
                .HasColumnName("IsDeleted")
                .HasDefaultValue(false)
                .HasComment("Признак удаления");

            builder.Property(x => x.Status)
                .HasColumnName("Status")
                .IsRequired()
                .HasComment("Статус");

            builder.HasOne(x => x.User)
                .WithMany(x => x.UserProfiles)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(x => x.Nickname).IsUnique();
        }
    }
}