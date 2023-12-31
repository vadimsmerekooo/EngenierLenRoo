using Microsoft.EntityFrameworkCore.Migrations;

namespace EngeneerLenRooAspNet.Migrations
{
    public partial class UPM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IpComputer",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "NumberPcMap",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "UserMap",
                table: "Employees");

            migrationBuilder.AddColumn<int>(
                name: "IpComputer",
                table: "Techniques",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IpComputer",
                table: "Techniques");

            migrationBuilder.AddColumn<int>(
                name: "IpComputer",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberPcMap",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserMap",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
