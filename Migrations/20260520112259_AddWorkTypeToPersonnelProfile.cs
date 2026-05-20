using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace okumatakibisanalkutuphane.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkTypeToPersonnelProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WorkType",
                table: "PersonnelProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkType",
                table: "PersonnelProfiles");
        }
    }
}
