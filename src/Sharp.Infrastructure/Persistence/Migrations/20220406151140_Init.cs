using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sharp.Data.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommandTransactions",
                columns: table => new
                {
                    TransactionId = table.Column<string>(type: "TEXT", nullable: false),
                    GameId = table.Column<string>(type: "TEXT", nullable: false),
                    CommandType = table.Column<int>(type: "INTEGER", nullable: false),
                    RobotId = table.Column<string>(type: "TEXT", nullable: true),
                    PlanetId = table.Column<string>(type: "TEXT", nullable: true),
                    TargetId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandTransactions", x => x.TransactionId);
                });

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
                    PlayerName = table.Column<string>(type: "TEXT", nullable: false),
                    PlayerEmail = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameRegistrations", x => x.GameId);
                    table.ForeignKey(
                        name: "FK_GameRegistrations_PlayerDetails_PlayerName_PlayerEmail",
                        columns: x => new { x.PlayerName, x.PlayerEmail },
                        principalTable: "PlayerDetails",
                        principalColumns: new[] { "Name", "Email" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameRegistrations_PlayerName_PlayerEmail",
                table: "GameRegistrations",
                columns: new[] { "PlayerName", "PlayerEmail" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommandTransactions");

            migrationBuilder.DropTable(
                name: "GameRegistrations");

            migrationBuilder.DropTable(
                name: "PlayerDetails");
        }
    }
}
