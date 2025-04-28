using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsDoneColumnFromToDo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDone",
                table: "Todos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDone",
                table: "Todos",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
