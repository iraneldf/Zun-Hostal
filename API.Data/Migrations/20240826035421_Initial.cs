using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Apellidos = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Contrasenna = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DebeCambiarContrasenna = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreado = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaActualizado = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualizadoPor = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Roles_RolId",
                        column: x => x.RolId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Permisos",
                columns: new[] { "Id", "ActualizadoPor", "CreadoPor", "Descripcion", "FechaActualizado", "FechaCreado", "Nombre" },
                values: new object[,]
                {
                    { new Guid("4129cf49-cc22-46a1-9625-501855f2da8b"), "", "", "Permite ver, crear, modificar y eliminar usuarios en el sistema.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gestionar usuarios" },
                    { new Guid("56b3924b-209b-40fb-9f31-ad75c12f4528"), "", "", "Permite ver los usuarios existentes en el sistema y sus datos.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Listar usuarios" },
                    { new Guid("90abf232-a641-478d-8720-f0ae49e8a306"), "", "", "Permite ver, crear, modificar y eliminar roles en el sistema.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gestionar rol" },
                    { new Guid("e36d283c-8b25-42b6-83bd-56edd953e770"), "", "", "Permite ver los roles existentes en el sistema y sus datos.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Listar roles" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ActualizadoPor", "CreadoPor", "FechaActualizado", "FechaCreado", "Nombre" },
                values: new object[] { new Guid("c0b7e3b3-a06e-4580-b985-bb2fc4336522"), "", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Administrador" });

            migrationBuilder.InsertData(
                table: "RolPermiso",
                columns: new[] { "Id", "ActualizadoPor", "CreadoPor", "FechaActualizado", "FechaCreado", "PermisoId", "RolId" },
                values: new object[,]
                {
                    { new Guid("4129cf49-cc22-46a1-9625-501855f2da8b"), "", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("4129cf49-cc22-46a1-9625-501855f2da8b"), new Guid("c0b7e3b3-a06e-4580-b985-bb2fc4336522") },
                    { new Guid("56b3924b-209b-40fb-9f31-ad75c12f4528"), "", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("56b3924b-209b-40fb-9f31-ad75c12f4528"), new Guid("c0b7e3b3-a06e-4580-b985-bb2fc4336522") },
                    { new Guid("90abf232-a641-478d-8720-f0ae49e8a306"), "", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("90abf232-a641-478d-8720-f0ae49e8a306"), new Guid("c0b7e3b3-a06e-4580-b985-bb2fc4336522") },
                    { new Guid("e36d283c-8b25-42b6-83bd-56edd953e770"), "", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("e36d283c-8b25-42b6-83bd-56edd953e770"), new Guid("c0b7e3b3-a06e-4580-b985-bb2fc4336522") }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "ActualizadoPor", "Apellidos", "Contrasenna", "Correo", "CreadoPor", "DebeCambiarContrasenna", "FechaActualizado", "FechaCreado", "Nombre", "RolId", "Username" },
                values: new object[] { new Guid("42717fb8-6e3f-4c94-b6b1-a88e8718d0a6"), "", "System", "poner hash", "admin.system@api.cu", "", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", new Guid("c0b7e3b3-a06e-4580-b985-bb2fc4336522"), "admin.system" });

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

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Id",
                table: "Usuarios",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Nombre_Apellidos",
                table: "Usuarios",
                columns: new[] { "Nombre", "Apellidos" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_RolId",
                table: "Usuarios",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Username",
                table: "Usuarios",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolPermiso");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Permisos");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
