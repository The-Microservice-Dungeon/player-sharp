using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sharp.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayerDetails",
                columns: table => new
                {
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Token = table.Column<string>(type: "TEXT", nullable: false),
                    PlayerId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerDetails", x => new { x.Name, x.Email });
                });

            migrationBuilder.CreateTable(
                name: "GameRegistrations",
                columns: table => new
                {
                    GameId = table.Column<string>(type: "TEXT", nullable: false),
                    TransactionId = table.Column<string>(type: "TEXT", nullable: false),
                    PlayerDetailsName = table.Column<string>(type: "TEXT", nullable: false),
                    PlayerDetailsEmail = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameRegistrations", x => x.GameId);
                    table.ForeignKey(
                        name: "FK_GameRegistrations_PlayerDetails_PlayerDetailsName_PlayerDetailsEmail",
                        columns: x => new { x.PlayerDetailsName, x.PlayerDetailsEmail },
                        principalTable: "PlayerDetails",
                        principalColumns: new[] { "Name", "Email" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameRegistrations_PlayerDetailsName_PlayerDetailsEmail",
                table: "GameRegistrations",
                columns: new[] { "PlayerDetailsName", "PlayerDetailsEmail" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameRegistrations");

            migrationBuilder.DropTable(
                name: "PlayerDetails");
        }
    }
}
