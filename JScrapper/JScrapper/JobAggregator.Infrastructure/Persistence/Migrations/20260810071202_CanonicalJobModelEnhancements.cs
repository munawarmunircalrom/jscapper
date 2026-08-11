using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobAggregator.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CanonicalJobModelEnhancements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_JobSourcePostings_JobId_JobSourceId",
                table: "JobSourcePostings",
                columns: new[] { "JobId", "JobSourceId" },
                unique: true,
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_JobSourcePostings_RawPayloadHash",
                table: "JobSourcePostings",
                column: "RawPayloadHash");

            migrationBuilder.AddCheckConstraint(
                name: "CK_JobSourcePostings_LastSeenAfterFirstSeen",
                table: "JobSourcePostings",
                sql: "[LastSeenAtUtc] >= [FirstSeenAtUtc]");

            migrationBuilder.AddCheckConstraint(
                name: "CK_JobSalaries_MinLessOrEqualMax",
                table: "JobSalaries",
                sql: "[MinAmount] IS NULL OR [MaxAmount] IS NULL OR [MinAmount] <= [MaxAmount]");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Jobs_ExpiresAfterPosted",
                table: "Jobs",
                sql: "[PostedAtUtc] IS NULL OR [ExpiresAtUtc] IS NULL OR [ExpiresAtUtc] >= [PostedAtUtc]");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_Name_IsDeleted",
                table: "Companies",
                columns: new[] { "Name", "IsDeleted" },
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_JobSourcePostings_JobId_JobSourceId",
                table: "JobSourcePostings");

            migrationBuilder.DropIndex(
                name: "IX_JobSourcePostings_RawPayloadHash",
                table: "JobSourcePostings");

            migrationBuilder.DropCheckConstraint(
                name: "CK_JobSourcePostings_LastSeenAfterFirstSeen",
                table: "JobSourcePostings");

            migrationBuilder.DropCheckConstraint(
                name: "CK_JobSalaries_MinLessOrEqualMax",
                table: "JobSalaries");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Jobs_ExpiresAfterPosted",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Companies_Name_IsDeleted",
                table: "Companies");
        }
    }
}
