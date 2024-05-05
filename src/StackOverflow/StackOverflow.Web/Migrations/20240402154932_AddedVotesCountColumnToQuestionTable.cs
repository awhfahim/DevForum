using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StackOverflow.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddedVotesCountColumnToQuestionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VotesCount",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VotesCount",
                table: "Questions");
        }
    }
}
