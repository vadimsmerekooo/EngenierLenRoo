using Microsoft.EntityFrameworkCore.Migrations;

namespace EngeneerLenRooAspNet.Migrations
{
    public partial class changemodelEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Cabinets_CabinetId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_CabinetId",
                table: "Employees");

            migrationBuilder.AlterColumn<string>(
                name: "Fio",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CabinetId",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CabinetId1",
                table: "Employees",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CabinetId1",
                table: "Employees",
                column: "CabinetId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Cabinets_CabinetId1",
                table: "Employees",
                column: "CabinetId1",
                principalTable: "Cabinets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Cabinets_CabinetId1",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_CabinetId1",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CabinetId1",
                table: "Employees");

            migrationBuilder.AlterColumn<string>(
                name: "Fio",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CabinetId",
                table: "Employees",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CabinetId",
                table: "Employees",
                column: "CabinetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Cabinets_CabinetId",
                table: "Employees",
                column: "CabinetId",
                principalTable: "Cabinets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
