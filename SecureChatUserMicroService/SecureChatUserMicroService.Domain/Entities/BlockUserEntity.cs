using NodaTime;
using SecureChatUserMicroService.Domain.Common;

namespace SecureChatUserMicroService.Domain.Entities
{
    public class BlockUserEntity : Entity
    {
        public BlockUserEntity(Instant startDate, Instant endDate, Guid userProfileId)
        {
            StartDate = startDate;
            EndDate = endDate;
            UserProfileId = userProfileId;
            IsActive = true;
        }

        /// <summary>
        /// Действительна ли блокировка
        /// TODO: св-во нужно, если блокировка была выдана по ошибке и ее надо снять без удаления записи
        /// </summary>
        public bool IsActive { get; private set; }
        
        /// <summary>
        /// Дата начала
        /// </summary>
        public Instant StartDate { get; private set; }

        /// <summary>
        /// Дата окончания блокировки
        /// </summary>
        public Instant EndDate { get; private set; }

        /// <summary>
        /// Принадлежность к пользователю
        /// </summary>
        public Guid UserProfileId { get; private set; }
        public virtual UserProfileEntity UserProfile { get; set; }

        public void Update(Instant? startDate, Instant? endDate, bool? isActive)
        {
            StartDate = startDate ?? StartDate;
            EndDate = endDate ?? EndDate;
            IsActive = isActive ?? IsActive;
        }
    }
}