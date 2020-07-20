using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication2.Migrations
{
    public partial class seedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "Course", "Name", "Photo", "Subject" },
                values: new object[] { 1, "MCA", "Sandeep", "dan.jpg", "IT" });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "Course", "Name", "Photo", "Subject" },
                values: new object[] { 2, "MCA", "Sandeep2", "tan.jpg", "IT" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
