using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniCoursesRepository.Migrations
{
    /// <inheritdoc />
    public partial class subjects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubject_Subject_SubjectId",
                table: "StudentSubject");

            migrationBuilder.DropForeignKey(
                name: "FK_Subject_AspNetUsers_ProfessorId",
                table: "Subject");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subject",
                table: "Subject");

            migrationBuilder.RenameTable(
                name: "Subject",
                newName: "Subjects");

            migrationBuilder.RenameIndex(
                name: "IX_Subject_ProfessorId",
                table: "Subjects",
                newName: "IX_Subjects_ProfessorId");

            migrationBuilder.AlterColumn<string>(
                name: "ProfessorId",
                table: "Subjects",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subjects",
                table: "Subjects",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubject_Subjects_SubjectId",
                table: "StudentSubject",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_AspNetUsers_ProfessorId",
                table: "Subjects",
                column: "ProfessorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubject_Subjects_SubjectId",
                table: "StudentSubject");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_AspNetUsers_ProfessorId",
                table: "Subjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subjects",
                table: "Subjects");

            migrationBuilder.RenameTable(
                name: "Subjects",
                newName: "Subject");

            migrationBuilder.RenameIndex(
                name: "IX_Subjects_ProfessorId",
                table: "Subject",
                newName: "IX_Subject_ProfessorId");

            migrationBuilder.AlterColumn<string>(
                name: "ProfessorId",
                table: "Subject",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subject",
                table: "Subject",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubject_Subject_SubjectId",
                table: "StudentSubject",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subject_AspNetUsers_ProfessorId",
                table: "Subject",
                column: "ProfessorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
