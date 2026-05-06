using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TurnSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAsesorFieldsToTurno : Migration
    {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "AsesorNombre",
            table: "Turnos",
            type: "longtext",
            nullable: true)
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.AddColumn<string>(
            name: "Ventanilla",
            table: "Turnos",
            type: "longtext",
            nullable: true)
            .Annotation("MySql:CharSet", "utf8mb4");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "AsesorNombre",
            table: "Turnos");

        migrationBuilder.DropColumn(
            name: "Ventanilla",
            table: "Turnos");
    }
    }
}
