#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsFragileToBox : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFragile",
                table: "Boxes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFragile",
                table: "Boxes");
        }
    }
}
