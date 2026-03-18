using System.ComponentModel.DataAnnotations;
using NodaTime;
using SecureChatUserMicroService.Domain.Common;

namespace SecureChatUserMicroService.Domain.Entities
{
    public class UserEntity : Entity
    {
        public UserEntity()
        {
            UserProfiles = new HashSet<UserProfileEntity>();
        }

        public UserEntity(string email, Instant createdTime, Instant lastUpdateTime, Instant? deleteTime) : this()
        {
            Email = email;
            CreatedTime = createdTime;
            LastUpdateTime = lastUpdateTime;
            DeleteTime = deleteTime;
        }

        /// <summary>
        /// Почта
        /// </summary>
        [MaxLength(150)]
        public string Email { get; private set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public Instant CreatedTime { get; private set; }

        /// <summary>
        /// Дата изменения
        /// </summary>
        public Instant LastUpdateTime { get; private set; }

        /// <summary>
        /// Дата удаления
        /// </summary>
        public Instant? DeleteTime { get; private set; }

        public virtual ICollection<UserProfileEntity> UserProfiles { get; private set; }

        public void Update(string? email, Instant? lastUpdateTime, Instant? deleteTime)
        {
            Email = email ?? Email;
            LastUpdateTime = lastUpdateTime ?? LastUpdateTime;
            DeleteTime = deleteTime ?? DeleteTime;
        }
    }
}