using Microsoft.EntityFrameworkCore.Migrations;

namespace Quiz.Data.Migrations
{
    public partial class changeKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersAnswers_AspNetUsers_IdentityUserId",
                table: "UsersAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersAnswers",
                table: "UsersAnswers");

            migrationBuilder.AlterColumn<string>(
                name: "IdentityUserId",
                table: "UsersAnswers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UsersAnswers",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersAnswers",
                table: "UsersAnswers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UsersAnswers_IdentityUserId",
                table: "UsersAnswers",
                column: "IdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersAnswers_AspNetUsers_IdentityUserId",
                table: "UsersAnswers",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersAnswers_AspNetUsers_IdentityUserId",
                table: "UsersAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersAnswers",
                table: "UsersAnswers");

            migrationBuilder.DropIndex(
                name: "IX_UsersAnswers_IdentityUserId",
                table: "UsersAnswers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UsersAnswers");

            migrationBuilder.AlterColumn<string>(
                name: "IdentityUserId",
                table: "UsersAnswers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersAnswers",
                table: "UsersAnswers",
                columns: new[] { "IdentityUserId", "QuizId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UsersAnswers_AspNetUsers_IdentityUserId",
                table: "UsersAnswers",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
