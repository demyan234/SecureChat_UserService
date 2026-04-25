using SecureChatUserMicroService.Domain.Common;

namespace SecureChatUserMicroService.Domain.Enums.User
{
    public class UserRolesEnum : Enumeration
    {
        private UserRolesEnum(Guid id, string name) : base(id, name)
        { }

        public static IEnumerable<UserRolesEnum> List()
        {
            return
            [
                User,
                Admin
            ];
        }

        /// <summary>
        /// Получение статуса пользователя по названию
        /// </summary>
        public static UserRolesEnum FromName(string typeOfCourseFromName)
        {
            var request = List()
                .SingleOrDefault(s =>
                    string.Equals(s.Name, typeOfCourseFromName, StringComparison.CurrentCultureIgnoreCase));

            if (request != null) return request;
            {
                var typeOfCourseIsExists = string.Join(",", List().Select(s => s.Name));

                /*TODO: Кастомное исключение*/
                throw new ArgumentNullException(typeOfCourseIsExists);
            }
        }

        /// <summary>
        /// Получение статуса пользователя по его Id
        /// </summary>
        public static UserRolesEnum FromId(Guid fieldTypeId)
        {
            var request = List().SingleOrDefault(s => s.Id == fieldTypeId);

            if (request != null) return request;
            {
                var typeOfCourseIsExists = string.Join(",", List().Select(s => s.Id));

                throw new Exception(typeOfCourseIsExists);
            }
        }
    
        public static readonly UserRolesEnum User = new(
            Guid.Parse("9a460a7b-600f-4662-a598-2e61cd64d171"),
            "Пользователь".ToLowerInvariant());
    
        public static readonly UserRolesEnum Admin = new(
            Guid.Parse("48240e89-0d8d-4e4f-a6df-9776df45794c"),
            "Администратор".ToLowerInvariant());
    }
}