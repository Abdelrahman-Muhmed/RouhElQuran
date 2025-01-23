using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class Test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoursePlans_Courses_CourseId",
                table: "CoursePlans");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "CoursePlans",
                newName: "courseId");

            migrationBuilder.RenameIndex(
                name: "IX_CoursePlans_CourseId",
                table: "CoursePlans",
                newName: "IX_CoursePlans_courseId");

            migrationBuilder.AlterColumn<int>(
                name: "courseId",
                table: "CoursePlans",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CoursePlans_Courses_courseId",
                table: "CoursePlans",
                column: "courseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoursePlans_Courses_courseId",
                table: "CoursePlans");

            migrationBuilder.RenameColumn(
                name: "courseId",
                table: "CoursePlans",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_CoursePlans_courseId",
                table: "CoursePlans",
                newName: "IX_CoursePlans_CourseId");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "CoursePlans",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_CoursePlans_Courses_CourseId",
                table: "CoursePlans",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id");
        }
    }
}
