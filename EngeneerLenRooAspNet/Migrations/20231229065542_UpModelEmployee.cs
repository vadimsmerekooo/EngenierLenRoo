using Microsoft.EntityFrameworkCore.Migrations;

namespace EngeneerLenRooAspNet.Migrations
{
    public partial class UpModelEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Employees");
        }
    }
}
