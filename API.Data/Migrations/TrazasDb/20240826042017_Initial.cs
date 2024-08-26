using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations.TrazasDb
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PonerNombreSistema",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TablaBD = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FechaCreado = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaActualizado = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualizadoPor = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PonerNombreSistema", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PonerNombreSistema_Id",
                table: "PonerNombreSistema",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PonerNombreSistema");
        }
    }
}
