using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NodaTime;
using SecureChatUserMicroService.Domain.Entities;

namespace SecureChatUserMicroService.Infrastructure.Configurations
{
    public class BlockUserConfiguration : IEntityTypeConfiguration<BlockUserEntity>
    {
        public void Configure(EntityTypeBuilder<BlockUserEntity> builder)
        {
            builder.ToTable("BlockUsers");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.IsActive)
                .HasColumnName("IsActive")
                .HasDefaultValue(true)
                .HasComment("Активна ли блокировка");
            
            builder.Property(x => x.StartDate)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("StartDate")
                .HasComment("Дата начала");

            builder.Property(x => x.EndDate)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("EndDate")
                .HasComment("Дата окончания");
            
            builder.HasOne(x => x.UserProfile)
                .WithMany(x => x.BlockUsers)
                .HasForeignKey(x => x.UserProfileId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}