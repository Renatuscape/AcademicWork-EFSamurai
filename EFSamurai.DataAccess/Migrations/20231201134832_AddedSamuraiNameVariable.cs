using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFSamurai.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedSamuraiNameVariable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Samurai",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Samurai");
        }
    }
}
