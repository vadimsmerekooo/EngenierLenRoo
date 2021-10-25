using Microsoft.EntityFrameworkCore.Migrations;

namespace EngeneerLenRooAspNet.Migrations
{
    public partial class EmployeeUdpate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Techniques_Sotrudniks_SotrudnikId",
                table: "Techniques");

            migrationBuilder.DropTable(
                name: "Sotrudniks");

            migrationBuilder.RenameColumn(
                name: "SotrudnikId",
                table: "Techniques",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Techniques_SotrudnikId",
                table: "Techniques",
                newName: "IX_Techniques_EmployeeId");

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Fio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IpComputer = table.Column<int>(type: "int", nullable: true),
                    UserMap = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CabinetId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Cabinets_CabinetId",
                        column: x => x.CabinetId,
                        principalTable: "Cabinets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CabinetId",
                table: "Employees",
                column: "CabinetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Techniques_Employees_EmployeeId",
                table: "Techniques",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Techniques_Employees_EmployeeId",
                table: "Techniques");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Techniques",
                newName: "SotrudnikId");

            migrationBuilder.RenameIndex(
                name: "IX_Techniques_EmployeeId",
                table: "Techniques",
                newName: "IX_Techniques_SotrudnikId");

            migrationBuilder.CreateTable(
                name: "Sotrudniks",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CabinetId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Fio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IpComputer = table.Column<int>(type: "int", nullable: true),
                    UserMap = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sotrudniks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sotrudniks_Cabinets_CabinetId",
                        column: x => x.CabinetId,
                        principalTable: "Cabinets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sotrudniks_CabinetId",
                table: "Sotrudniks",
                column: "CabinetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Techniques_Sotrudniks_SotrudnikId",
                table: "Techniques",
                column: "SotrudnikId",
                principalTable: "Sotrudniks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
