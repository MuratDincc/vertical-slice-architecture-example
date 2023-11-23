using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Application.Migrations
{
    /// <inheritdoc />
    public partial class v100 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "customer",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    surname = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    email = table.Column<string>(type: "character varying(350)", maxLength: 350, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customer", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "order",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    customer_id = table.Column<int>(type: "integer", nullable: false),
                    total = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "order_item",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    order_id = table.Column<int>(type: "integer", nullable: false),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    quantity = table.Column<decimal>(type: "numeric", nullable: false),
                    total = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_item", x => x.id);
                    table.ForeignKey(
                        name: "fk_order_item_order_order_id",
                        column: x => x.order_id,
                        principalSchema: "public",
                        principalTable: "order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_order_item_product_product_id",
                        column: x => x.product_id,
                        principalSchema: "public",
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "customer",
                columns: new[] { "id", "email", "name", "surname" },
                values: new object[] { 1, "info@muratdinc.dev", "Murat", "Dinc" });

            migrationBuilder.InsertData(
                schema: "public",
                table: "order",
                columns: new[] { "id", "customer_id", "total" },
                values: new object[] { 1, 1, 6100m });

            migrationBuilder.InsertData(
                schema: "public",
                table: "product",
                columns: new[] { "id", "price", "title" },
                values: new object[,]
                {
                    { 1, 1000m, "iPhone 8" },
                    { 2, 1300m, "iPhone X" },
                    { 3, 1500m, "iPhone 11" }
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "order_item",
                columns: new[] { "id", "order_id", "price", "product_id", "quantity", "title", "total" },
                values: new object[,]
                {
                    { 1, 1, 1000m, 1, 2m, "iPhone 8", 2000m },
                    { 2, 1, 1300m, 2, 2m, "iPhone X", 2600m },
                    { 3, 1, 1500m, 3, 1m, "iPhone 11", 1500m }
                });

            migrationBuilder.CreateIndex(
                name: "ix_order_item_order_id",
                schema: "public",
                table: "order_item",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_item_product_id",
                schema: "public",
                table: "order_item",
                column: "product_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "customer",
                schema: "public");

            migrationBuilder.DropTable(
                name: "order_item",
                schema: "public");

            migrationBuilder.DropTable(
                name: "order",
                schema: "public");

            migrationBuilder.DropTable(
                name: "product",
                schema: "public");
        }
    }
}
