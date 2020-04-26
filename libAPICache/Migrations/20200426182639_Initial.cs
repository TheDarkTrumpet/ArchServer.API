using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace libAPICache.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Kimai");

            migrationBuilder.EnsureSchema(
                name: "TeamWork");

            migrationBuilder.EnsureSchema(
                name: "Teamwork");

            migrationBuilder.EnsureSchema(
                name: "Toggl");

            migrationBuilder.EnsureSchema(
                name: "VSTS");

            migrationBuilder.CreateTable(
                name: "VSTSWorkItems",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    url = table.Column<string>(maxLength: 255, nullable: true),
                    Type = table.Column<string>(maxLength: 50, nullable: true),
                    State = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    AssignedTo = table.Column<string>(maxLength: 100, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ChangedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VSTSWorkItems", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TimeEntries",
                schema: "Kimai",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityName = table.Column<string>(maxLength: 255, nullable: true),
                    ActivityComment = table.Column<string>(nullable: true),
                    ProjectName = table.Column<string>(maxLength: 255, nullable: true),
                    ProjectComment = table.Column<string>(nullable: true),
                    Customer = table.Column<string>(maxLength: 255, nullable: true),
                    HourlyRate = table.Column<double>(nullable: true),
                    StartTime = table.Column<DateTime>(nullable: true),
                    EndTime = table.Column<DateTime>(nullable: true),
                    TimeNotes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                schema: "Teamwork",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectName = table.Column<string>(maxLength: 255, nullable: true),
                    CompanyName = table.Column<string>(maxLength: 255, nullable: true),
                    Title = table.Column<string>(maxLength: 255, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Priority = table.Column<string>(maxLength: 20, nullable: true),
                    AssignedTo = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    DueDate = table.Column<DateTime>(nullable: true),
                    Completed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "People",
                schema: "TeamWork",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(maxLength: 100, nullable: true),
                    LastActive = table.Column<DateTime>(nullable: true),
                    FullName = table.Column<string>(maxLength: 100, nullable: true),
                    EmailAddress = table.Column<string>(maxLength: 100, nullable: true),
                    CompanyName = table.Column<string>(maxLength: 255, nullable: true),
                    Administrator = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimeEntries",
                schema: "Toggl",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    Duration = table.Column<int>(nullable: false),
                    User = table.Column<string>(maxLength: 50, nullable: true),
                    Client = table.Column<string>(maxLength: 100, nullable: true),
                    Project = table.Column<string>(maxLength: 100, nullable: true),
                    Billable = table.Column<double>(nullable: false),
                    IsBillable = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkItemComment",
                schema: "VSTS",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(maxLength: 200, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    WorkItemid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkItemComment", x => x.id);
                    table.ForeignKey(
                        name: "FK_WorkItemComment_VSTSWorkItems_WorkItemid",
                        column: x => x.WorkItemid,
                        principalTable: "VSTSWorkItems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkItemComment_WorkItemid",
                schema: "VSTS",
                table: "WorkItemComment",
                column: "WorkItemid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeEntries",
                schema: "Kimai");

            migrationBuilder.DropTable(
                name: "Tasks",
                schema: "Teamwork");

            migrationBuilder.DropTable(
                name: "People",
                schema: "TeamWork");

            migrationBuilder.DropTable(
                name: "TimeEntries",
                schema: "Toggl");

            migrationBuilder.DropTable(
                name: "WorkItemComment",
                schema: "VSTS");

            migrationBuilder.DropTable(
                name: "VSTSWorkItems");
        }
    }
}
