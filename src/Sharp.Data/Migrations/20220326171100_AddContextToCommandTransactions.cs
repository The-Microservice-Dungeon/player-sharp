using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sharp.Data.Migrations
{
    public partial class AddContextToCommandTransactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "GameId",
                table: "CommandTransactions",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlanetId",
                table: "CommandTransactions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RobotId",
                table: "CommandTransactions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TargetId",
                table: "CommandTransactions",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlanetId",
                table: "CommandTransactions");

            migrationBuilder.DropColumn(
                name: "RobotId",
                table: "CommandTransactions");

            migrationBuilder.DropColumn(
                name: "TargetId",
                table: "CommandTransactions");

            migrationBuilder.AlterColumn<string>(
                name: "GameId",
                table: "CommandTransactions",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}
