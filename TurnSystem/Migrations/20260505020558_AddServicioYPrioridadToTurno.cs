using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TurnSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddServicioYPrioridadToTurno : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Prioridad",
                table: "Turnos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Servicio",
                table: "Turnos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Prioridad",
                table: "Turnos");

            migrationBuilder.DropColumn(
                name: "Servicio",
                table: "Turnos");
        }
    }
}
