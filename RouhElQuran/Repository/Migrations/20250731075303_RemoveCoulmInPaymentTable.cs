using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCoulmInPaymentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentIntentID",
                table: "UserPayments");

            migrationBuilder.RenameColumn(
                name: "Plan",
                table: "CoursePlans",
                newName: "PlanNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlanNumber",
                table: "CoursePlans",
                newName: "Plan");

            migrationBuilder.AddColumn<string>(
                name: "PaymentIntentID",
                table: "UserPayments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
