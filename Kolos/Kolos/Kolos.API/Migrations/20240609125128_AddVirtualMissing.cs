using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kolos.API.Migrations
{
    /// <inheritdoc />
    public partial class AddVirtualMissing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Money",
                table: "Subscriptions",
                newName: "Price");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Subscriptions",
                newName: "Money");
        }
    }
}
