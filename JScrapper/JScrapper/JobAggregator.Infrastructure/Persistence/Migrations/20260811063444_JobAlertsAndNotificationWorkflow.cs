using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobAggregator.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class JobAlertsAndNotificationWorkflow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AlertId",
                table: "Notifications",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Channel",
                table: "Notifications",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "JobId",
                table: "Notifications",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "SentAtUtc",
                table: "Notifications",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Notifications",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EmploymentType",
                table: "JobAlerts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Experience",
                table: "JobAlerts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Keywords",
                table: "JobAlerts",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "JobAlerts",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MaxSalary",
                table: "JobAlerts",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MinSalary",
                table: "JobAlerts",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Remote",
                table: "JobAlerts",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SkillsCsv",
                table: "JobAlerts",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourcesCsv",
                table: "JobAlerts",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_AlertId",
                table: "Notifications",
                column: "AlertId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_JobId",
                table: "Notifications",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_Status_Channel_CreatedAtUtc",
                table: "Notifications",
                columns: new[] { "Status", "Channel", "CreatedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId_JobId_AlertId_Channel",
                table: "Notifications",
                columns: new[] { "UserId", "JobId", "AlertId", "Channel" },
                unique: true,
                filter: "[JobId] IS NOT NULL AND [AlertId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_JobAlerts_AlertId",
                table: "Notifications",
                column: "AlertId",
                principalTable: "JobAlerts",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Jobs_JobId",
                table: "Notifications",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_JobAlerts_AlertId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Jobs_JobId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_AlertId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_JobId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_Status_Channel_CreatedAtUtc",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_UserId_JobId_AlertId_Channel",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "AlertId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Channel",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "JobId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "SentAtUtc",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "EmploymentType",
                table: "JobAlerts");

            migrationBuilder.DropColumn(
                name: "Experience",
                table: "JobAlerts");

            migrationBuilder.DropColumn(
                name: "Keywords",
                table: "JobAlerts");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "JobAlerts");

            migrationBuilder.DropColumn(
                name: "MaxSalary",
                table: "JobAlerts");

            migrationBuilder.DropColumn(
                name: "MinSalary",
                table: "JobAlerts");

            migrationBuilder.DropColumn(
                name: "Remote",
                table: "JobAlerts");

            migrationBuilder.DropColumn(
                name: "SkillsCsv",
                table: "JobAlerts");

            migrationBuilder.DropColumn(
                name: "SourcesCsv",
                table: "JobAlerts");
        }
    }
}
