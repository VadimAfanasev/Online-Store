using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineStoreEdu.Migrations
{
    /// <inheritdoc />
    public partial class AddProductToDbFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "Product",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Product",
                newName: "id");
        }
    }
}
