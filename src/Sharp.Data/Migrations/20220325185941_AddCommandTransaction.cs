using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sharp.Data.Migrations
{
    public partial class AddCommandTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommandTransactions",
                columns: table => new
                {
                    GameId = table.Column<string>(type: "TEXT", nullable: false),
                    TransactionId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandTransactions", x => new { x.TransactionId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommandTransactions");
        }
    }
}
