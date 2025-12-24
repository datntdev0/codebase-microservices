using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace datntdev.Microservices.Migrator.Migrations.Identity
{
    /// <inheritdoc />
    public partial class UpdateAppUserEntityAddPermissionsColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Permissions",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Permissions",
                table: "AppUsers");
        }
    }
}
