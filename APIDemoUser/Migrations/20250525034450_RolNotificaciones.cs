using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIDemoUser.Migrations
{
    /// <inheritdoc />
    public partial class RolNotificaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rol",
                table: "Notificaciones",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rol",
                table: "Notificaciones");
        }
    }
}
