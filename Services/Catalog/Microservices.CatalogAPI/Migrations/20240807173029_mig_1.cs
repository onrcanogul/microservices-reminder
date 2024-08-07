using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microservices.CatalogAPI.Migrations
{
    /// <inheritdoc />
    public partial class mig_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CatalogOutboxes",
                columns: table => new
                {
                    IdempotentToken = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProcessedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OccuredOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Payload = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogOutboxes", x => x.IdempotentToken);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatalogOutboxes");
        }
    }
}
