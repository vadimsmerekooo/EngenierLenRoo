using Microsoft.EntityFrameworkCore.Migrations;

namespace EngeneerLenRooAspNet.Migrations
{
    public partial class UpModeltechnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cartridges_Employees_EmployeeId1",
                table: "Cartridges");

            migrationBuilder.DropIndex(
                name: "IX_Cartridges_EmployeeId1",
                table: "Cartridges");

            migrationBuilder.DropColumn(
                name: "EmployeeId1",
                table: "Cartridges");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Techniques",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MapNubmer",
                table: "Techniques",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "Cartridges",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Cartridges_EmployeeId",
                table: "Cartridges",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cartridges_Employees_EmployeeId",
                table: "Cartridges",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cartridges_Employees_EmployeeId",
                table: "Cartridges");

            migrationBuilder.DropIndex(
                name: "IX_Cartridges_EmployeeId",
                table: "Cartridges");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Techniques");

            migrationBuilder.DropColumn(
                name: "MapNubmer",
                table: "Techniques");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "Cartridges",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeId1",
                table: "Cartridges",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cartridges_EmployeeId1",
                table: "Cartridges",
                column: "EmployeeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Cartridges_Employees_EmployeeId1",
                table: "Cartridges",
                column: "EmployeeId1",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
