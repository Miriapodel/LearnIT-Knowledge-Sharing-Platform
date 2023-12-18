using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnIt.Data.Migrations
{
    public partial class m1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CatgoryId",
                table: "Topics");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CatgoryId",
                table: "Topics",
                type: "int",
                nullable: true);
        }
    }
}
