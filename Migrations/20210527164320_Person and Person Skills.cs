using Microsoft.EntityFrameworkCore.Migrations;

namespace CrudAPIWithRepositoryPattern.Migrations
{
    public partial class PersonandPersonSkills : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PersonID",
                table: "Skills",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YearsOfExperience = table.Column<short>(type: "smallint", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PersonSkills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    SkillId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonSkills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonSkills_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonSkills_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Skills_PersonID",
                table: "Skills",
                column: "PersonID");

            migrationBuilder.CreateIndex(
                name: "IX_PersonSkills_PersonId",
                table: "PersonSkills",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonSkills_SkillId",
                table: "PersonSkills",
                column: "SkillId");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Person_PersonID",
                table: "Skills",
                column: "PersonID",
                principalTable: "Person",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Person_PersonID",
                table: "Skills");

            migrationBuilder.DropTable(
                name: "PersonSkills");

            migrationBuilder.DropTable(
                name: "Person");

            migrationBuilder.DropIndex(
                name: "IX_Skills_PersonID",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "PersonID",
                table: "Skills");
        }
    }
}
