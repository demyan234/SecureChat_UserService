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
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false, comment: "Почта"),
                    CreatedTime = table.Column<Instant>(type: "timestamp with time zone", nullable: false, comment: "Дата создания"),
                    LastUpdateTime = table.Column<Instant>(type: "timestamp with time zone", nullable: false, comment: "Дата обновления"),
                    DeleteTime = table.Column<Instant>(type: "timestamp with time zone", nullable: true, comment: "Дата удаления")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                schema: "UserMicroService",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false, comment: "Имя"),
                    Nickname = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false, comment: "Ник"),
                    AvatarUrl = table.Column<string>(type: "text", nullable: false, comment: "Ссылка на аватар"),
                    StatusQuote = table.Column<string>(type: "character varying(140)", maxLength: 140, nullable: false, comment: "Цитата(статус)"),
                    IsBlocked = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Признак блокировки"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Признак удаления"),
                    Status = table.Column<Guid>(type: "uuid", nullable: false, comment: "Статус"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "UserMicroService",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BlockUsers",
                schema: "UserMicroService",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Активна ли блокировка"),
                    StartDate = table.Column<Instant>(type: "timestamp with time zone", nullable: false, comment: "Дата начала"),
                    EndDate = table.Column<Instant>(type: "timestamp with time zone", nullable: false, comment: "Дата окончания"),
                    UserProfileId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlockUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlockUsers_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalSchema: "UserMicroService",
                        principalTable: "UserProfiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlockUsers_UserProfileId",
                schema: "UserMicroService",
                table: "BlockUsers",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_Nickname",
                schema: "UserMicroService",
                table: "UserProfiles",
                column: "Nickname",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_UserId",
                schema: "UserMicroService",
                table: "UserProfiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                schema: "UserMicroService",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlockUsers",
                schema: "UserMicroService");

            migrationBuilder.DropTable(
                name: "UserProfiles",
                schema: "UserMicroService");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "UserMicroService");
        }
    }
}
