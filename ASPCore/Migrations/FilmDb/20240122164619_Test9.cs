using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPCore.Migrations.FilmDb
{
    /// <inheritdoc />
    public partial class Test9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StarRating",
                table: "Films");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "StarRating",
                table: "Films",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
