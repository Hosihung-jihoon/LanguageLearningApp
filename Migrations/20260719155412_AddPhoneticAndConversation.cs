using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LanguageLearningApp.Migrations
{
    /// <inheritdoc />
    public partial class AddPhoneticAndConversation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConversationId",
                table: "Sentences",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phonetic",
                table: "Sentences",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Sentences",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConversationId",
                table: "Sentences");

            migrationBuilder.DropColumn(
                name: "Phonetic",
                table: "Sentences");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Sentences");
        }
    }
}
