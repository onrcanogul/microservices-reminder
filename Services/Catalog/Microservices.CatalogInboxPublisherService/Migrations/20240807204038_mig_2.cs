using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microservices.CatalogInboxPublisherService.Migrations
{
    /// <inheritdoc />
    public partial class mig_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Process",
                table: "CatalogInboxes",
                newName: "Processed");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Processed",
                table: "CatalogInboxes",
                newName: "Process");
        }
    }
}
