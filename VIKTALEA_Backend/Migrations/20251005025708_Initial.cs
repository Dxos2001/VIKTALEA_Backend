using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VIKTALEA_Backend.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "VIKTALEA");

            migrationBuilder.CreateTable(
                name: "Clientes",
                schema: "VIKTALEA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Ruc = table.Column<string>(type: "NVARCHAR2(11)", maxLength: 11, nullable: false),
                    RazonSocial = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    TelefonoContacto = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: true),
                    CorreoContacto = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: true),
                    Direccion = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: true),
                    activate = table.Column<string>(type: "CHAR(1)", nullable: false, defaultValueSql: "1"),
                    createdAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false, defaultValueSql: "SYS_EXTRACT_UTC(SYSTIMESTAMP)"),
                    updatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_Ruc",
                schema: "VIKTALEA",
                table: "Clientes",
                column: "Ruc",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clientes",
                schema: "VIKTALEA");
        }
    }
}
