using Microsoft.EntityFrameworkCore.Migrations;

namespace dashserver.Migrations
{
    public partial class Update7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KPIs_KPIs_KPIId",
                table: "KPIs");

            migrationBuilder.DropIndex(
                name: "IX_KPIs_KPIId",
                table: "KPIs");

            migrationBuilder.DropColumn(
                name: "KPIId",
                table: "KPIs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KPIId",
                table: "KPIs",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_KPIs_KPIId",
                table: "KPIs",
                column: "KPIId");

            migrationBuilder.AddForeignKey(
                name: "FK_KPIs_KPIs_KPIId",
                table: "KPIs",
                column: "KPIId",
                principalTable: "KPIs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
