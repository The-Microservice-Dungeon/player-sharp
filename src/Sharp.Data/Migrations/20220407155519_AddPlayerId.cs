using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sharp.Data.Migrations
{
    public partial class AddPlayerId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerDetails",
                table: "PlayerDetails");

            migrationBuilder.AlterColumn<string>(
                name: "PlayerId",
                table: "PlayerDetails",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "PlayerDetails",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PlayerDetails",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT")
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerDetails",
                table: "PlayerDetails",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerDetails_Email_Name",
                table: "PlayerDetails",
                columns: new[] { "Email", "Name" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerDetails",
                table: "PlayerDetails");

            migrationBuilder.DropIndex(
                name: "IX_PlayerDetails_Email_Name",
                table: "PlayerDetails");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PlayerDetails",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT")
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "PlayerDetails",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<string>(
                name: "PlayerId",
                table: "PlayerDetails",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerDetails",
                table: "PlayerDetails",
                columns: new[] { "Name", "Email" });
        }
    }
}
