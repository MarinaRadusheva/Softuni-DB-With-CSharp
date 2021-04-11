using Microsoft.EntityFrameworkCore.Migrations;

namespace Quiz.Data.Migrations
{
    public partial class ModelUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersAnswers_Answers_AnswerId",
                table: "UsersAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersAnswers_Quizes_QuizId",
                table: "UsersAnswers");

            migrationBuilder.DropIndex(
                name: "IX_UsersAnswers_QuizId",
                table: "UsersAnswers");

            migrationBuilder.DropColumn(
                name: "QuizId",
                table: "UsersAnswers");

            migrationBuilder.AlterColumn<int>(
                name: "AnswerId",
                table: "UsersAnswers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersAnswers_Answers_AnswerId",
                table: "UsersAnswers",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersAnswers_Answers_AnswerId",
                table: "UsersAnswers");

            migrationBuilder.AlterColumn<int>(
                name: "AnswerId",
                table: "UsersAnswers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuizId",
                table: "UsersAnswers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UsersAnswers_QuizId",
                table: "UsersAnswers",
                column: "QuizId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersAnswers_Answers_AnswerId",
                table: "UsersAnswers",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersAnswers_Quizes_QuizId",
                table: "UsersAnswers",
                column: "QuizId",
                principalTable: "Quizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
