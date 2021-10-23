using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace dashserver.Migrations
{
    public partial class Update2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "KPIType",
                table: "KPIRecords",
                newName: "KPIId");

            migrationBuilder.AddColumn<int>(
                name: "KPIId",
                table: "KPIs",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "KPIRecords",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.CreateIndex(
                name: "IX_PlanDays_PlanId",
                table: "PlanDays",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_KPIs_KPIId",
                table: "KPIs",
                column: "KPIId");

            migrationBuilder.CreateIndex(
                name: "IX_KPIRecords_KPIId",
                table: "KPIRecords",
                column: "KPIId");

            migrationBuilder.AddForeignKey(
                name: "FK_KPIRecords_KPIs_KPIId",
                table: "KPIRecords",
                column: "KPIId",
                principalTable: "KPIs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KPIs_KPIs_KPIId",
                table: "KPIs",
                column: "KPIId",
                principalTable: "KPIs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanDays_Plans_PlanId",
                table: "PlanDays",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KPIRecords_KPIs_KPIId",
                table: "KPIRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_KPIs_KPIs_KPIId",
                table: "KPIs");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanDays_Plans_PlanId",
                table: "PlanDays");

            migrationBuilder.DropIndex(
                name: "IX_PlanDays_PlanId",
                table: "PlanDays");

            migrationBuilder.DropIndex(
                name: "IX_KPIs_KPIId",
                table: "KPIs");

            migrationBuilder.DropIndex(
                name: "IX_KPIRecords_KPIId",
                table: "KPIRecords");

            migrationBuilder.DropColumn(
                name: "KPIId",
                table: "KPIs");

            migrationBuilder.RenameColumn(
                name: "KPIId",
                table: "KPIRecords",
                newName: "KPIType");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "KPIRecords",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }
    }
}
