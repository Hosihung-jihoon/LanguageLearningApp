using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LanguageLearningApp.Migrations
{
    /// <inheritdoc />
    public partial class RenameCreateAtToCreatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "UserAttempts",
                newName: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "UserAttempts",
                newName: "CreateAt");
        }
    }
}
