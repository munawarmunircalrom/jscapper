using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobAggregator.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class JobSearchPerformanceIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_JobSourcePostings_JobSourceId_IsActive",
                table: "JobSourcePostings",
                columns: new[] { "JobSourceId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_JobSkills_Name",
                table: "JobSkills",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_EmploymentType_WorkMode_Seniority",
                table: "Jobs",
                columns: new[] { "EmploymentType", "WorkMode", "Seniority" });

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_IsDeleted",
                table: "Jobs",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_IsDeleted_PostedAtUtc",
                table: "Jobs",
                columns: new[] { "IsDeleted", "PostedAtUtc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_JobSourcePostings_JobSourceId_IsActive",
                table: "JobSourcePostings");

            migrationBuilder.DropIndex(
                name: "IX_JobSkills_Name",
                table: "JobSkills");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_EmploymentType_WorkMode_Seniority",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_IsDeleted",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_IsDeleted_PostedAtUtc",
                table: "Jobs");
        }
    }
}
