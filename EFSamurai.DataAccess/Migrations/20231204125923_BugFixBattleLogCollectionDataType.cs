using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFSamurai.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class BugFixBattleLogCollectionDataType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BattleLog_BattleLog_BattleLogId",
                table: "BattleLog");

            migrationBuilder.DropIndex(
                name: "IX_BattleLog_BattleLogId",
                table: "BattleLog");

            migrationBuilder.DropColumn(
                name: "BattleLogId",
                table: "BattleLog");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BattleLogId",
                table: "BattleLog",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BattleLog_BattleLogId",
                table: "BattleLog",
                column: "BattleLogId");

            migrationBuilder.AddForeignKey(
                name: "FK_BattleLog_BattleLog_BattleLogId",
                table: "BattleLog",
                column: "BattleLogId",
                principalTable: "BattleLog",
                principalColumn: "Id");
        }
    }
}
