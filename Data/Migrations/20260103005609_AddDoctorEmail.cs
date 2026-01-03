using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthMate.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDoctorEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Doctors",
                newName: "Email");

            migrationBuilder.AlterColumn<int>(
                name: "Experience",
                table: "Doctors",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Doctors",
                newName: "UserId");

            migrationBuilder.AlterColumn<string>(
                name: "Experience",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
