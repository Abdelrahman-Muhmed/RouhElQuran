using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class addandfixeddatawithBaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Time_Work",
                table: "Instructors");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Instructors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Instructors",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Instructors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "Time_From",
                table: "Instructors",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "Time_To",
                table: "Instructors",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Instructors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "Time_From",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "Time_To",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Instructors");

            migrationBuilder.AddColumn<string>(
                name: "Time_Work",
                table: "Instructors",
                type: "varchar(200)",
                unicode: false,
                maxLength: 200,
                nullable: true);
        }
    }
}
