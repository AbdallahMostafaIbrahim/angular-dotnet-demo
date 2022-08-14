using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    public partial class Seeding1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Class",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Class", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    classId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Student_Class_classId",
                        column: x => x.classId,
                        principalTable: "Class",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Class",
                columns: new[] { "Id", "name" },
                values: new object[] { 1, "Big" });

            migrationBuilder.InsertData(
                table: "Class",
                columns: new[] { "Id", "name" },
                values: new object[] { 2, "Small" });

            migrationBuilder.InsertData(
                table: "Student",
                columns: new[] { "Id", "classId", "email", "username" },
                values: new object[] { 1, 1, "abdo@gmail.com", "Abdallah" });

            migrationBuilder.InsertData(
                table: "Student",
                columns: new[] { "Id", "classId", "email", "username" },
                values: new object[] { 2, 1, "salma@gmail.com", "Salma" });

            migrationBuilder.InsertData(
                table: "Student",
                columns: new[] { "Id", "classId", "email", "username" },
                values: new object[] { 3, 2, "hamza@gmail.com", "Hamza" });

            migrationBuilder.CreateIndex(
                name: "IX_Student_classId",
                table: "Student",
                column: "classId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "Class");
        }
    }
}
