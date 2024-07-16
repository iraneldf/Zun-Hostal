using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AgregadoRolPermiso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RolId",
                table: "Usuarios",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Permisos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FechaCreado = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaActualizado = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualizadoPor = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permisos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FechaCreado = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaActualizado = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualizadoPor = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolPermiso",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermisoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FechaCreado = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaActualizado = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualizadoPor = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolPermiso", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolPermiso_Permisos_PermisoId",
                        column: x => x.PermisoId,
                        principalTable: "Permisos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolPermiso_Roles_RolId",
                        column: x => x.RolId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Permisos",
                columns: new[] { "Id", "ActualizadoPor", "CreadoPor", "Descripcion", "FechaActualizado", "FechaCreado", "Nombre" },
                values: new object[,]
                {
                    { new Guid("4129cf49-cc22-46a1-9625-501855f2da8b"), "", "", "Permite ver, crear, modificar y eliminar usuarios en el sistema.", new DateTime(2024, 6, 28, 23, 18, 26, 102, DateTimeKind.Local).AddTicks(7067), new DateTime(2024, 6, 28, 23, 18, 26, 102, DateTimeKind.Local).AddTicks(7058), "Gestionar usuarios" },
                    { new Guid("56b3924b-209b-40fb-9f31-ad75c12f4528"), "", "", "Permite ver los usuarios existentes en el sistema y sus datos.", new DateTime(2024, 6, 28, 23, 18, 26, 102, DateTimeKind.Local).AddTicks(7038), new DateTime(2024, 6, 28, 23, 18, 26, 102, DateTimeKind.Local).AddTicks(7030), "Listar usuarios" },
                    { new Guid("90abf232-a641-478d-8720-f0ae49e8a306"), "", "", "Permite ver, crear, modificar y eliminar roles en el sistema.", new DateTime(2024, 6, 28, 23, 18, 26, 102, DateTimeKind.Local).AddTicks(7094), new DateTime(2024, 6, 28, 23, 18, 26, 102, DateTimeKind.Local).AddTicks(7091), "Gestionar rol" },
                    { new Guid("e36d283c-8b25-42b6-83bd-56edd953e770"), "", "", "Permite ver los roles existentes en el sistema y sus datos.", new DateTime(2024, 6, 28, 23, 18, 26, 102, DateTimeKind.Local).AddTicks(7086), new DateTime(2024, 6, 28, 23, 18, 26, 102, DateTimeKind.Local).AddTicks(7083), "Listar roles" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ActualizadoPor", "CreadoPor", "FechaActualizado", "FechaCreado", "Nombre" },
                values: new object[] { new Guid("c0b7e3b3-a06e-4580-b985-bb2fc4336522"), "", "", new DateTime(2024, 6, 28, 23, 18, 26, 102, DateTimeKind.Local).AddTicks(6759), new DateTime(2024, 6, 28, 23, 18, 26, 102, DateTimeKind.Local).AddTicks(6702), "Administrador" });

            migrationBuilder.InsertData(
                table: "RolPermiso",
                columns: new[] { "Id", "ActualizadoPor", "CreadoPor", "FechaActualizado", "FechaCreado", "PermisoId", "RolId" },
                values: new object[,]
                {
                    { new Guid("4129cf49-cc22-46a1-9625-501855f2da8b"), "", "", new DateTime(2024, 6, 28, 23, 18, 26, 102, DateTimeKind.Local).AddTicks(7396), new DateTime(2024, 6, 28, 23, 18, 26, 102, DateTimeKind.Local).AddTicks(7394), new Guid("4129cf49-cc22-46a1-9625-501855f2da8b"), new Guid("c0b7e3b3-a06e-4580-b985-bb2fc4336522") },
                    { new Guid("56b3924b-209b-40fb-9f31-ad75c12f4528"), "", "", new DateTime(2024, 6, 28, 23, 18, 26, 102, DateTimeKind.Local).AddTicks(7383), new DateTime(2024, 6, 28, 23, 18, 26, 102, DateTimeKind.Local).AddTicks(7375), new Guid("56b3924b-209b-40fb-9f31-ad75c12f4528"), new Guid("c0b7e3b3-a06e-4580-b985-bb2fc4336522") },
                    { new Guid("90abf232-a641-478d-8720-f0ae49e8a306"), "", "", new DateTime(2024, 6, 28, 23, 18, 26, 102, DateTimeKind.Local).AddTicks(7425), new DateTime(2024, 6, 28, 23, 18, 26, 102, DateTimeKind.Local).AddTicks(7423), new Guid("90abf232-a641-478d-8720-f0ae49e8a306"), new Guid("c0b7e3b3-a06e-4580-b985-bb2fc4336522") },
                    { new Guid("e36d283c-8b25-42b6-83bd-56edd953e770"), "", "", new DateTime(2024, 6, 28, 23, 18, 26, 102, DateTimeKind.Local).AddTicks(7417), new DateTime(2024, 6, 28, 23, 18, 26, 102, DateTimeKind.Local).AddTicks(7401), new Guid("e36d283c-8b25-42b6-83bd-56edd953e770"), new Guid("c0b7e3b3-a06e-4580-b985-bb2fc4336522") }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "ActualizadoPor", "Apellidos", "Contrasenna", "Correo", "CreadoPor", "DebeCambiarContrasenna", "FechaActualizado", "FechaCreado", "Nombre", "RolId", "Username" },
                values: new object[] { new Guid("42717fb8-6e3f-4c94-b6b1-a88e8718d0a6"), "", "System", "poner hash", "admin.system@api.cu", "", false, new DateTime(2024, 6, 28, 23, 18, 26, 103, DateTimeKind.Local).AddTicks(5331), new DateTime(2024, 6, 28, 23, 18, 26, 103, DateTimeKind.Local).AddTicks(5311), "Admin", new Guid("c0b7e3b3-a06e-4580-b985-bb2fc4336522"), "admin.system" });

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_RolId",
                table: "Usuarios",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_Permisos_Id",
                table: "Permisos",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Id",
                table: "Roles",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolPermiso_Id",
                table: "RolPermiso",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolPermiso_PermisoId",
                table: "RolPermiso",
                column: "PermisoId");

            migrationBuilder.CreateIndex(
                name: "IX_RolPermiso_RolId_PermisoId",
                table: "RolPermiso",
                columns: new[] { "RolId", "PermisoId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Roles_RolId",
                table: "Usuarios",
                column: "RolId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Roles_RolId",
                table: "Usuarios");

            migrationBuilder.DropTable(
                name: "RolPermiso");

            migrationBuilder.DropTable(
                name: "Permisos");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_RolId",
                table: "Usuarios");

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: new Guid("42717fb8-6e3f-4c94-b6b1-a88e8718d0a6"));

            migrationBuilder.DropColumn(
                name: "RolId",
                table: "Usuarios");
        }
    }
}
