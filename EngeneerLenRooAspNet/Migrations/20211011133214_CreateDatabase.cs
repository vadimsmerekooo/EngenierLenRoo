using Microsoft.EntityFrameworkCore.Migrations;

namespace EngeneerLenRooAspNet.Migrations
{
    public partial class CreateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cabinets",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cabinets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sotrudniks",
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
                    table.PrimaryKey("PK_Sotrudniks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sotrudniks_Cabinets_CabinetId",
                        column: x => x.CabinetId,
                        principalTable: "Cabinets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Techniques",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeTechnique = table.Column<int>(type: "int", nullable: false),
                    InventoryNumber = table.Column<long>(type: "bigint", nullable: false),
                    SotrudnikId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Techniques", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Techniques_Sotrudniks_SotrudnikId",
                        column: x => x.SotrudnikId,
                        principalTable: "Sotrudniks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sotrudniks_CabinetId",
                table: "Sotrudniks",
                column: "CabinetId");

            migrationBuilder.CreateIndex(
                name: "IX_Techniques_SotrudnikId",
                table: "Techniques",
                column: "SotrudnikId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Techniques");

            migrationBuilder.DropTable(
                name: "Sotrudniks");

            migrationBuilder.DropTable(
                name: "Cabinets");
        }
    }
}
