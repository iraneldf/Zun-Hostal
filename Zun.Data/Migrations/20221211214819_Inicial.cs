using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zun.Datos.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntidadEjemplo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Edad = table.Column<int>(type: "int", maxLength: 3, nullable: false),
                    Intereses = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntidadEjemplo", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntidadEjemplo_Id",
                table: "EntidadEjemplo",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EntidadEjemplo_Nombre",
                table: "EntidadEjemplo",
                column: "Nombre",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntidadEjemplo");
        }
    }
}
