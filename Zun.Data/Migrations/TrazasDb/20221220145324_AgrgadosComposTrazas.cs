using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zun.Datos.Migrations.TrazasDb
{
    public partial class AgrgadosComposTrazas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Elemento",
                table: "Trazas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ElementoModificado",
                table: "Trazas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TablaBD",
                table: "Trazas",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Elemento",
                table: "Trazas");

            migrationBuilder.DropColumn(
                name: "ElementoModificado",
                table: "Trazas");

            migrationBuilder.DropColumn(
                name: "TablaBD",
                table: "Trazas");
        }
    }
}
