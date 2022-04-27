using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sharp.Data.Migrations
{
    public partial class RemovePlayerDetailsFromGameRegistration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameRegistrations_PlayerDetails_PlayerName_PlayerEmail",
                table: "GameRegistrations");

            migrationBuilder.DropIndex(
                name: "IX_GameRegistrations_PlayerName_PlayerEmail",
                table: "GameRegistrations");

            migrationBuilder.DropColumn(
                name: "PlayerEmail",
                table: "GameRegistrations");

            migrationBuilder.DropColumn(
                name: "PlayerName",
                table: "GameRegistrations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PlayerEmail",
                table: "GameRegistrations",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PlayerName",
                table: "GameRegistrations",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_GameRegistrations_PlayerName_PlayerEmail",
                table: "GameRegistrations",
                columns: new[] { "PlayerName", "PlayerEmail" });

            migrationBuilder.AddForeignKey(
                name: "FK_GameRegistrations_PlayerDetails_PlayerName_PlayerEmail",
                table: "GameRegistrations",
                columns: new[] { "PlayerName", "PlayerEmail" },
                principalTable: "PlayerDetails",
                principalColumns: new[] { "Name", "Email" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
