using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NodaTime;
using SecureChatUserMicroService.Domain.Entities;

namespace SecureChatUserMicroService.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);

            builder.Property<string>(x => x.Email)
                .HasColumnName("Email")
                .IsRequired()
                .HasMaxLength(150)
                .HasComment("Почта");

            builder.Property<Instant>(x => x.CreatedTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("CreatedTime")
                .HasComment("Дата создания");

            builder.Property<Instant>(x => x.LastUpdateTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("LastUpdateTime")
                .HasComment("Дата обновления");

            builder.Property<Instant?>(x => x.DeleteTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("DeleteTime")
                .IsRequired(false)
                .HasComment("Дата удаления");

            builder.HasIndex(x => x.Email).IsUnique();
        }
    }
}