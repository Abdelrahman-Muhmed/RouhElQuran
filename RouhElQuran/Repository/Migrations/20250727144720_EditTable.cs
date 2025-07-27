using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class EditTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoursePlans_Courses_courseId",
                table: "CoursePlans");

            migrationBuilder.DropColumn(
                name: "planName",
                table: "CoursePlans");

            migrationBuilder.RenameColumn(
                name: "sessionCount",
                table: "CoursePlans",
                newName: "SessionCount");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "CoursePlans",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "courseId",
                table: "CoursePlans",
                newName: "CourseId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "CoursePlans",
                newName: "ID");

            migrationBuilder.RenameIndex(
                name: "IX_CoursePlans_courseId",
                table: "CoursePlans",
                newName: "IX_CoursePlans_CourseId");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "UserPayments",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Plan",
                table: "CoursePlans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_CoursePlans_Courses_CourseId",
                table: "CoursePlans",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoursePlans_Courses_CourseId",
                table: "CoursePlans");

            migrationBuilder.DropColumn(
                name: "Plan",
                table: "CoursePlans");

            migrationBuilder.RenameColumn(
                name: "SessionCount",
                table: "CoursePlans",
                newName: "sessionCount");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "CoursePlans",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "CoursePlans",
                newName: "courseId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "CoursePlans",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_CoursePlans_CourseId",
                table: "CoursePlans",
                newName: "IX_CoursePlans_courseId");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "UserPayments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "planName",
                table: "CoursePlans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_CoursePlans_Courses_courseId",
                table: "CoursePlans",
                column: "courseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
