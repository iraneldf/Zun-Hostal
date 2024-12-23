#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations.TrazasDb;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "PonerNombreSistema",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                Descripcion = table.Column<string>("nvarchar(max)", nullable: false),
                TablaBD = table.Column<string>("nvarchar(100)", maxLength: 100, nullable: false),
                FechaCreado = table.Column<DateTime>("datetime2", nullable: false),
                CreadoPor = table.Column<string>("nvarchar(max)", nullable: false),
                FechaActualizado = table.Column<DateTime>("datetime2", nullable: false),
                ActualizadoPor = table.Column<string>("nvarchar(max)", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_PonerNombreSistema", x => x.Id); });

        migrationBuilder.CreateIndex(
            "IX_PonerNombreSistema_Id",
            "PonerNombreSistema",
            "Id",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "PonerNombreSistema");
    }
}