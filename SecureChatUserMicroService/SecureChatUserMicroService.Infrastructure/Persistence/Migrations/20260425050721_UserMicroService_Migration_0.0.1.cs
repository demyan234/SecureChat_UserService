using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace SecureChatUserMicroService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UserMicroService_Migration_001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "UserMicroService");

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "UserMicroService",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false, comment: "Почта пользователя"),
                    Nickname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Ник пользователя"),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Имя пользователя"),
                    AvatarUrl = table.Column<string>(type: "text", nullable: false, comment: "Ссылка на аватар пользователя"),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Идентификатор роли пользователя"),
                    DeletedAt = table.Column<Instant>(type: "timestamp with time zone", nullable: true, comment: "Когда был удален пользователь")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserStatuses",
                schema: "UserMicroService",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Роль")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStatuses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                schema: "UserMicroService",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Nickname",
                schema: "UserMicroService",
                table: "Users",
                column: "Nickname",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserStatuses_Id",
                schema: "UserMicroService",
                table: "UserStatuses",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users",
                schema: "UserMicroService");

            migrationBuilder.DropTable(
                name: "UserStatuses",
                schema: "UserMicroService");
        }
    }
}
