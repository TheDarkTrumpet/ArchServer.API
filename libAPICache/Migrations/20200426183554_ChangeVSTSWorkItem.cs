using Microsoft.EntityFrameworkCore.Migrations;

namespace libAPICache.Migrations
{
    public partial class ChangeVSTSWorkItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkItemComment_VSTSWorkItems_WorkItemid",
                schema: "VSTS",
                table: "WorkItemComment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VSTSWorkItems",
                table: "VSTSWorkItems");

            migrationBuilder.RenameTable(
                name: "VSTSWorkItems",
                newName: "WorkItems",
                newSchema: "VSTS");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkItems",
                schema: "VSTS",
                table: "WorkItems",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItemComment_WorkItems_WorkItemid",
                schema: "VSTS",
                table: "WorkItemComment",
                column: "WorkItemid",
                principalSchema: "VSTS",
                principalTable: "WorkItems",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkItemComment_WorkItems_WorkItemid",
                schema: "VSTS",
                table: "WorkItemComment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkItems",
                schema: "VSTS",
                table: "WorkItems");

            migrationBuilder.RenameTable(
                name: "WorkItems",
                schema: "VSTS",
                newName: "VSTSWorkItems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VSTSWorkItems",
                table: "VSTSWorkItems",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItemComment_VSTSWorkItems_WorkItemid",
                schema: "VSTS",
                table: "WorkItemComment",
                column: "WorkItemid",
                principalTable: "VSTSWorkItems",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
