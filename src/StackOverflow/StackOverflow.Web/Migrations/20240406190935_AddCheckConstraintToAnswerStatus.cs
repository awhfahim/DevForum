using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StackOverflow.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddCheckConstraintToAnswerStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE Answers ADD CONSTRAINT CK_Answer_AnswerStatus CHECK (AnswerStatus IN (0, 1, 2))");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE Answers DROP CONSTRAINT CK_Answer_AnswerStatus");
        }
    }
}
