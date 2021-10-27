﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Web_Api.Migrations
{
    public partial class Updateteamtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Teams_TeamId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Teams_TeamId1",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_TeamId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_TeamId1",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "TeamId1",
                table: "Matches");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "Matches",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamId1",
                table: "Matches",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_TeamId",
                table: "Matches",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_TeamId1",
                table: "Matches",
                column: "TeamId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Teams_TeamId",
                table: "Matches",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Teams_TeamId1",
                table: "Matches",
                column: "TeamId1",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
