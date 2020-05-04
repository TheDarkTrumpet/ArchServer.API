using Microsoft.EntityFrameworkCore.Migrations;

namespace libAPICache.Migrations
{
    public partial class KimaiDuration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Duration",
                schema: "Kimai",
                table: "TimeEntries",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                schema: "Kimai",
                table: "TimeEntries");
        }
    }
}
