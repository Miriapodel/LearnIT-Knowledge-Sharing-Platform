using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnIt.Data.Migrations
{
    public partial class likes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalLikes",
                table: "Comments",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalLikes",
                table: "Comments");
        }
    }
}
