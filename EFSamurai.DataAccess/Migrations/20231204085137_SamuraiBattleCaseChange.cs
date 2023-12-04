using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFSamurai.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SamuraiBattleCaseChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_samuraiBattles_Battle_BattleId",
                table: "samuraiBattles");

            migrationBuilder.DropForeignKey(
                name: "FK_samuraiBattles_Samurai_SamuraiId",
                table: "samuraiBattles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_samuraiBattles",
                table: "samuraiBattles");

            migrationBuilder.RenameTable(
                name: "samuraiBattles",
                newName: "SamuraiBattles");

            migrationBuilder.RenameIndex(
                name: "IX_samuraiBattles_BattleId",
                table: "SamuraiBattles",
                newName: "IX_SamuraiBattles_BattleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SamuraiBattles",
                table: "SamuraiBattles",
                columns: new[] { "SamuraiId", "BattleId" });

            migrationBuilder.AddForeignKey(
                name: "FK_SamuraiBattles_Battle_BattleId",
                table: "SamuraiBattles",
                column: "BattleId",
                principalTable: "Battle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SamuraiBattles_Samurai_SamuraiId",
                table: "SamuraiBattles",
                column: "SamuraiId",
                principalTable: "Samurai",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SamuraiBattles_Battle_BattleId",
                table: "SamuraiBattles");

            migrationBuilder.DropForeignKey(
                name: "FK_SamuraiBattles_Samurai_SamuraiId",
                table: "SamuraiBattles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SamuraiBattles",
                table: "SamuraiBattles");

            migrationBuilder.RenameTable(
                name: "SamuraiBattles",
                newName: "samuraiBattles");

            migrationBuilder.RenameIndex(
                name: "IX_SamuraiBattles_BattleId",
                table: "samuraiBattles",
                newName: "IX_samuraiBattles_BattleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_samuraiBattles",
                table: "samuraiBattles",
                columns: new[] { "SamuraiId", "BattleId" });

            migrationBuilder.AddForeignKey(
                name: "FK_samuraiBattles_Battle_BattleId",
                table: "samuraiBattles",
                column: "BattleId",
                principalTable: "Battle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_samuraiBattles_Samurai_SamuraiId",
                table: "samuraiBattles",
                column: "SamuraiId",
                principalTable: "Samurai",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
