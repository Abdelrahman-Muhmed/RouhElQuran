using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddcoulnminInstructorTableForWorkExAndDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Current_Course",
                table: "Instructors");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Instructors",
                type: "nvarchar(800)",
                maxLength: 800,
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "WorkExperienceFrom",
                table: "Instructors",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "WorkExperienceTo",
                table: "Instructors",
                type: "date",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "WorkExperienceFrom",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "WorkExperienceTo",
                table: "Instructors");

            migrationBuilder.AddColumn<string>(
                name: "Current_Course",
                table: "Instructors",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true);
        }
    }
}
