using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StackOverflow.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadePathWithQuestionVote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionVote_Questions_QuestionId",
                table: "QuestionVote");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionVote_Questions_QuestionId",
                table: "QuestionVote",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionVote_Questions_QuestionId",
                table: "QuestionVote");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionVote_Questions_QuestionId",
                table: "QuestionVote",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id");
        }
    }
}
