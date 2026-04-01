using System.ComponentModel.DataAnnotations;
using SecureChatUserMicroService.Domain.Common;

namespace SecureChatUserMicroService.Domain.Entities
{
    public class UserProfileEntity : Entity
    {
        public UserProfileEntity()
        {
            BlockUsers = new HashSet<BlockUserEntity>();
        }

        public UserProfileEntity(string name, string nickname, string avatarUrl, string? statusQuote, bool isBlocked,
            bool isDeleted, Guid status, Guid userId) : this()
        {
            Name = name;
            Nickname = nickname;
            AvatarUrl = avatarUrl;
            StatusQuote = statusQuote;
            IsBlocked = isBlocked;
            IsDeleted = isDeleted;
            Status = status;
            UserId = userId;
        }

        /// <summary>
        /// Имя
        /// </summary>
        [MaxLength(150)]
        public string Name { get; private set; }

        /// <summary>
        /// Ник
        /// </summary>
        [MaxLength(150)]
        public string Nickname { get; private set; }

        /// <summary>
        /// Ссылка на аватар
        /// </summary>
        public string AvatarUrl { get; private set; }

        /// <summary>
        /// Цитата(статус)
        /// </summary>
        [MaxLength(140)]
        public string? StatusQuote { get; private set; }

        /// <summary>
        /// Признак блокировки
        /// </summary>
        public bool IsBlocked { get; private set; }

        /// <summary>
        /// Признак удаления
        /// </summary>
        public bool IsDeleted { get; private set; }

        /// <summary>
        /// TODO: Выяснить!!!!
        /// </summary>
        public Guid Status { get; private set; }

        /// <summary>
        /// Принадлежность к пользователю
        /// </summary>
        public Guid UserId { get; private set; }

        public virtual UserEntity User { get; set; }

        public virtual ICollection<BlockUserEntity> BlockUsers { get; private set; }
    
        public void Update(string? name, string? nickname, string? avatarUrl, string? statusQuote, bool? isBlocked,
            bool? isDeleted)
        {
            Name = name ?? Name;
            Nickname = nickname ?? Nickname;
            AvatarUrl = avatarUrl ?? AvatarUrl;
            StatusQuote = statusQuote ?? StatusQuote;
            IsBlocked = isBlocked ?? IsBlocked;
            IsDeleted = isDeleted ?? IsDeleted;
        }
    }
}