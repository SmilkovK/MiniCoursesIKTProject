using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniCoursesIKTProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class test3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Index",
                table: "AspNetUsers",
                newName: "Indeks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Indeks",
                table: "AspNetUsers",
                newName: "Index");
        }
    }
}
