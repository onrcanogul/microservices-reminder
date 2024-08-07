using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microservices.CatalogInboxPublisherService.Migrations
{
    /// <inheritdoc />
    public partial class mig_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CatalogInboxes",
                columns: table => new
                {
                    IdempotentToken = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Process = table.Column<bool>(type: "bit", nullable: false),
                    Payload = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogInboxes", x => x.IdempotentToken);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatalogInboxes");
        }
    }
}
