using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecruitmentTool.Data.Migrations
{
    public partial class ChangeInterviewCompositeKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Interviews",
                table: "Interviews");

            migrationBuilder.AddColumn<int>(
                name: "JobId",
                table: "Interviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Interviews",
                table: "Interviews",
                columns: new[] { "CandidateId", "RecruiterId", "JobId" });

            migrationBuilder.CreateIndex(
                name: "IX_Interviews_JobId",
                table: "Interviews",
                column: "JobId");

            migrationBuilder.AddForeignKey(
                name: "FK_Interviews_Jobs_JobId",
                table: "Interviews",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interviews_Jobs_JobId",
                table: "Interviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Interviews",
                table: "Interviews");

            migrationBuilder.DropIndex(
                name: "IX_Interviews_JobId",
                table: "Interviews");

            migrationBuilder.DropColumn(
                name: "JobId",
                table: "Interviews");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Interviews",
                table: "Interviews",
                columns: new[] { "CandidateId", "RecruiterId" });
        }
    }
}
