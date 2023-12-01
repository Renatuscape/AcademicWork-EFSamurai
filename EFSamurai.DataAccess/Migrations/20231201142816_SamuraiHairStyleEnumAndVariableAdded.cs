using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFSamurai.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SamuraiHairStyleEnumAndVariableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HairStyle",
                table: "Samurai",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HairStyle",
                table: "Samurai");
        }
    }
}
