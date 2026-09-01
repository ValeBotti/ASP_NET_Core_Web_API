using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP_NET_Core_Web_API.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "menu",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    price = table.Column<double>(type: "float", nullable: false),
                    location_lat = table.Column<float>(type: "real", nullable: false),
                    location_lng = table.Column<float>(type: "real", nullable: false),
                    image_version = table.Column<int>(type: "int", nullable: false),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    short_description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    long_description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    delivery_time = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_menu", x => x.id);
                    table.CheckConstraint("CK_menu_id_positive", "id > 0");
                    table.CheckConstraint("CK_menu_image_version_positive", "image_version >= 0");
                    table.CheckConstraint("CK_menu_lat_range", "location_lat BETWEEN -90 AND 90");
                    table.CheckConstraint("CK_menu_lng_range", "location_lng BETWEEN -180 AND 180");
                    table.CheckConstraint("CK_menu_price_positive", "price >= 0");
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    first_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    last_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    card_full_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    card_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    card_expire_month = table.Column<int>(type: "int", nullable: true),
                    card_expire_year = table.Column<int>(type: "int", nullable: true),
                    card_cvv = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.id);
                    table.CheckConstraint("CK_user_card_expire_month", "card_expire_month BETWEEN 1 AND 12");
                    table.CheckConstraint("CK_user_id_positive", "id > 0");
                });

            migrationBuilder.CreateTable(
                name: "order",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    menu_id = table.Column<int>(type: "int", nullable: false),
                    creation_timestamp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    delivery_timestamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    current_position_lat = table.Column<float>(type: "real", nullable: false),
                    current_position_lng = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order", x => x.id);
                    table.CheckConstraint("CK_order_delivery_after_creation", "delivery_timestamp >= creation_timestamp");
                    table.CheckConstraint("CK_order_delivery_timestamp_logic", "(status = 'ON_DELIVERY' AND delivery_timestamp IS NULL) OR (status = 'COMPLETED' AND delivery_timestamp IS NOT NULL)");
                    table.CheckConstraint("CK_order_id_positive", "id > 0");
                    table.CheckConstraint("CK_order_lat_range", "current_position_lat BETWEEN -90 AND 90");
                    table.CheckConstraint("CK_order_lng_range", "current_position_lng BETWEEN -180 AND 180");
                    table.CheckConstraint("CK_order_menu_id_positive", "menu_id > 0");
                    table.CheckConstraint("CK_order_status_valid", "status IN ('ON_DELIVERY', 'COMPLETED')");
                    table.CheckConstraint("CK_order_user_id_positive", "user_id > 0");
                    table.ForeignKey(
                        name: "FK_order_menu_menu_id",
                        column: x => x.menu_id,
                        principalTable: "menu",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_order_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "uid_sid",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uid_sid", x => x.id);
                    table.CheckConstraint("CK_uid_sid_user_id_positive", "user_id >= 0");
                    table.ForeignKey(
                        name: "FK_uid_sid_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_order_menu_id",
                table: "order",
                column: "menu_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_user_id",
                table: "order",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_uid_sid_user_id",
                table: "uid_sid",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "order");

            migrationBuilder.DropTable(
                name: "uid_sid");

            migrationBuilder.DropTable(
                name: "menu");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
