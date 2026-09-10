using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP_NET_Core_Web_API.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCurrentPositionAddStartPosition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "current_position_lat",
                table: "order");

            migrationBuilder.DropColumn(
                name: "current_position_lng",
                table: "order");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "current_position_lat",
                table: "order",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "current_position_lng",
                table: "order",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
