using Microsoft.EntityFrameworkCore.Migrations;

namespace BeltExam.Migrations
{
    public partial class Sixth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Associations_Funtimes_FuntimeId",
                table: "Associations");

            migrationBuilder.DropColumn(
                name: "ActivityId",
                table: "Associations");

            migrationBuilder.AlterColumn<int>(
                name: "FuntimeId",
                table: "Associations",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Associations_Funtimes_FuntimeId",
                table: "Associations",
                column: "FuntimeId",
                principalTable: "Funtimes",
                principalColumn: "FuntimeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Associations_Funtimes_FuntimeId",
                table: "Associations");

            migrationBuilder.AlterColumn<int>(
                name: "FuntimeId",
                table: "Associations",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "ActivityId",
                table: "Associations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Associations_Funtimes_FuntimeId",
                table: "Associations",
                column: "FuntimeId",
                principalTable: "Funtimes",
                principalColumn: "FuntimeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
