﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StackOverflow.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedCascadePathForQuestionTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QestionTags_Questions_QuestionId",
                table: "QestionTags");

            migrationBuilder.AddForeignKey(
                name: "FK_QestionTags_Questions_QuestionId",
                table: "QestionTags",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QestionTags_Questions_QuestionId",
                table: "QestionTags");

            migrationBuilder.AddForeignKey(
                name: "FK_QestionTags_Questions_QuestionId",
                table: "QestionTags",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id");
        }
    }
}
