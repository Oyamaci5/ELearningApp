using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace elearningapp.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseDuration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseDuration",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseDuration",
                table: "Courses");
        }
    }
}
