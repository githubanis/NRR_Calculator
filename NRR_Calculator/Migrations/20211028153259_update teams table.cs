using Microsoft.EntityFrameworkCore.Migrations;

namespace Web_Api.Migrations
{
    public partial class updateteamstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LosesCount",
                table: "Teams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MatchsCount",
                table: "Teams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PointsCount",
                table: "Teams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WinsCount",
                table: "Teams",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LosesCount",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "MatchsCount",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "PointsCount",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "WinsCount",
                table: "Teams");
        }
    }
}
