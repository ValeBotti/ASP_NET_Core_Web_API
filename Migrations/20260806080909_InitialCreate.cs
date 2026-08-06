using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP_NET_Core_Web_API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    Mid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Location_Latitude = table.Column<double>(type: "float", nullable: false),
                    Location_Longitude = table.Column<double>(type: "float", nullable: false),
                    ImageVersion = table.Column<int>(type: "int", nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LongDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliveryTime = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Mid);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Uid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardFullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardExpireMonth = table.Column<int>(type: "int", nullable: true),
                    CardExpireYear = table.Column<int>(type: "int", nullable: true),
                    CardCVV = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastOid = table.Column<int>(type: "int", nullable: true),
                    OrderStatus = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Uid);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Oid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Uid = table.Column<int>(type: "int", nullable: false),
                    Mid = table.Column<int>(type: "int", nullable: false),
                    CreationTimestamp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliveryLocation_Latitude = table.Column<double>(type: "float", nullable: false),
                    DeliveryLocation_Longitude = table.Column<double>(type: "float", nullable: false),
                    ExpectedDeliveryTimestamp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliveryTimestamp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentPosition_Latitude = table.Column<double>(type: "float", nullable: false),
                    CurrentPosition_Longitude = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_Orders_Menus_Mid",
                        column: x => x.Mid,
                        principalTable: "Menus",
                        principalColumn: "Mid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Users_Uid",
                        column: x => x.Uid,
                        principalTable: "Users",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UidSids",
                columns: table => new
                {
                    Sid = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Uid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UidSids", x => x.Sid);
                    table.ForeignKey(
                        name: "FK_UidSids_Users_Uid",
                        column: x => x.Uid,
                        principalTable: "Users",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Mid",
                table: "Orders",
                column: "Mid");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Uid",
                table: "Orders",
                column: "Uid");

            migrationBuilder.CreateIndex(
                name: "IX_UidSids_Uid",
                table: "UidSids",
                column: "Uid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "UidSids");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
