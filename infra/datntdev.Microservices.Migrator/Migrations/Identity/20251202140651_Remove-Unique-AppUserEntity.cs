using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace datntdev.Microservices.Migrator.Migrations.Identity
{
    /// <inheritdoc />
    public partial class RemoveUniqueAppUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppUsers_EmailAddress",
                table: "AppUsers");

            migrationBuilder.DropIndex(
                name: "IX_AppUsers_Username",
                table: "AppUsers");

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_EmailAddress",
                table: "AppUsers",
                column: "EmailAddress");

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_Username",
                table: "AppUsers",
                column: "Username");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppUsers_EmailAddress",
                table: "AppUsers");

            migrationBuilder.DropIndex(
                name: "IX_AppUsers_Username",
                table: "AppUsers");

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_EmailAddress",
                table: "AppUsers",
                column: "EmailAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_Username",
                table: "AppUsers",
                column: "Username",
                unique: true);
        }
    }
}
