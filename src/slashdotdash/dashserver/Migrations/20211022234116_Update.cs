﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace dashserver.Migrations
{
    public partial class Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PlanDays_ResourceGroupId",
                table: "PlanDays",
                column: "ResourceGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanDays_ResourceId",
                table: "PlanDays",
                column: "ResourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanDays_ResourceGroups_ResourceGroupId",
                table: "PlanDays",
                column: "ResourceGroupId",
                principalTable: "ResourceGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanDays_Resources_ResourceId",
                table: "PlanDays",
                column: "ResourceId",
                principalTable: "Resources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanDays_ResourceGroups_ResourceGroupId",
                table: "PlanDays");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanDays_Resources_ResourceId",
                table: "PlanDays");

            migrationBuilder.DropIndex(
                name: "IX_PlanDays_ResourceGroupId",
                table: "PlanDays");

            migrationBuilder.DropIndex(
                name: "IX_PlanDays_ResourceId",
                table: "PlanDays");
        }
    }
}
