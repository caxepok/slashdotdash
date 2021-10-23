using Microsoft.EntityFrameworkCore.Migrations;

namespace dashserver.Migrations
{
    public partial class Update4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanDays_Plans_PlanId",
                table: "PlanDays");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanDays_ResourceGroups_ResourceGroupId",
                table: "PlanDays");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanDays_Resources_ResourceId",
                table: "PlanDays");

            migrationBuilder.DropIndex(
                name: "IX_PlanDays_PlanId",
                table: "PlanDays");

            migrationBuilder.DropIndex(
                name: "IX_PlanDays_ResourceGroupId",
                table: "PlanDays");

            migrationBuilder.DropIndex(
                name: "IX_PlanDays_ResourceId",
                table: "PlanDays");

            migrationBuilder.AddColumn<bool>(
                name: "ThresholdDirection",
                table: "KPIs",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThresholdDirection",
                table: "KPIs");

            migrationBuilder.CreateIndex(
                name: "IX_PlanDays_PlanId",
                table: "PlanDays",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanDays_ResourceGroupId",
                table: "PlanDays",
                column: "ResourceGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanDays_ResourceId",
                table: "PlanDays",
                column: "ResourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanDays_Plans_PlanId",
                table: "PlanDays",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
    }
}
