using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobAggregator.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class JobDeduplicationConfidenceAndIngestionFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MatchConfidence",
                table: "JobDuplicates",
                type: "decimal(5,4)",
                precision: 5,
                scale: 4,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MatchConfidence",
                table: "JobDuplicates");
        }
    }
}
