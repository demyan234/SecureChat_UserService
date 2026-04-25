using NodaTime;

namespace SecureChatUserMicroService.Domain.Entities
{
    public class UsersEntity
    {
        public UsersEntity(Guid id, string email, string nickname, string name, string avatarUrl, Guid roleId,
            Instant? deletedAt = null)
        {
            Id = id;
            Email = email;
            Nickname = nickname;
            Name = name;
            AvatarUrl = avatarUrl;
            RoleId = roleId;
            DeletedAt = deletedAt;
        }

        /// <summary>
        /// Идентификатор пользователя с AuthService
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Почта
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// Никнейм
        /// </summary>
        public string Nickname { get; private set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Ссылка на аватар
        /// </summary>
        public string AvatarUrl { get; private set; }

        /// <summary>
        /// Идентификатор роли
        /// </summary>
        public Guid RoleId { get; private set; }

        /// <summary>
        /// Удален ли пользователь
        /// </summary>
        public Instant? DeletedAt { get; private set; }

        public void Update(string? email, string? nickname, string? avatarUrl, string? userName)
        {
            Email = !string.IsNullOrEmpty(email) ? email : Email;
            Nickname = !string.IsNullOrEmpty(nickname) ? nickname : Nickname;
            AvatarUrl = !string.IsNullOrEmpty(avatarUrl) ? avatarUrl : AvatarUrl;
            Name = !string.IsNullOrEmpty(userName) ? userName : Name;
        }
    }
}