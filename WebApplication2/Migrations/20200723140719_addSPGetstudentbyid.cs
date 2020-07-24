using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication2.Migrations
{
    public partial class addSPGetstudentbyid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var procedure = @"Create PROCEDURE [dbo].[GetStudentById] 
                                   @Id int
                            AS
                            BEGIN
                                SET NOCOUNT ON;
                                        Select* from Students where id = @Id;
                            END";
            migrationBuilder.Sql(procedure);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var procedure = @"drop PROCEDURE [dbo].[GetStudentById]";
            migrationBuilder.Sql(procedure);
        }
    }
}
