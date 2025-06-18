using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ShoeStore.Migrations
{
    /// <inheritdoc />
    public partial class ExtendedShoeCollection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Brand = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Size = table.Column<int>(type: "INTEGER", nullable: false),
                    Color = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    Category = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    StockQuantity = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    City = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    PostalCode = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Country = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    IsAdmin = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    ShoeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Shoes_ShoeId",
                        column: x => x.ShoeId,
                        principalTable: "Shoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderNumber = table.Column<string>(type: "TEXT", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    ShippingAddress = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    ShippingCity = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ShippingPostalCode = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    ShippingCountry = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    TrackingNumber = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false),
                    ShoeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Shoes_ShoeId",
                        column: x => x.ShoeId,
                        principalTable: "Shoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Shoes",
                columns: new[] { "Id", "Brand", "Category", "Color", "CreatedAt", "Description", "IsActive", "Name", "Price", "Size", "StockQuantity", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Nike", "Спортивне взуття", "Білий", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Класичні кросівки Nike Air Max 90 з видимою підошвою Air", true, "Air Max 90", 120.00m, 42, 15, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, "Adidas", "Повсякденне взуття", "Білий", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Легендарні білі кросівки Adidas Stan Smith", true, "Stan Smith", 85.00m, 41, 20, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, "Converse", "Повсякденне взуття", "Чорний", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Іконічні високі кеди Converse Chuck Taylor", true, "Chuck Taylor All Star", 65.00m, 40, 25, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, "Nike", "Спортивне взуття", "Білий", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Легендарні баскетбольні кросівки Nike Air Force 1", true, "Air Force 1", 110.00m, 43, 25, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, "Adidas", "Спортивне взуття", "Чорний", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Високотехнологічні кросівки для бігу з технологією Boost", true, "Ultraboost 22", 180.00m, 42, 15, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { 6, "Puma", "Повсякденне взуття", "Синій", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Класичні замшеві кросівки Puma у винтажному стилі", true, "Suede Classic", 75.00m, 41, 30, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, "Vans", "Скейтерське взуття", "Чорний/Білий", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Іконічні скейтерські кросівки з боковою смужкою", true, "Old Skool", 65.00m, 40, 20, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { 8, "New Balance", "Спортивне взуття", "Сірий", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Комфортні ретро кросівки з класичним дизайном", true, "574 Core", 95.00m, 44, 18, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, "Nike", "Баскетбольне взуття", "Червоний/Чорний/Білий", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Легендарні баскетбольні кросівки Jordan у середній висоті", true, "Air Jordan 1 Mid", 125.00m, 43, 12, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { 10, "Adidas", "Повсякденне взуття", "Зелений", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Класичні замшеві кросівки з винтажним шармом", true, "Gazelle", 90.00m, 42, 22, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { 11, "Reebok", "Повсякденне взуття", "Білий", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Шкіряні кросівки у класичному стилі", true, "Classic Leather", 80.00m, 41, 16, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { 12, "Nike", "Спортивне взуття", "Коричневий/Білий", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Стильні кросівки для скейтбордингу та повсякденного носіння", true, "Dunk Low", 100.00m, 42, 14, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { 13, "Converse", "Повсякденне взуття", "Жовтий", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Преміум версія класичних високих кедів", true, "Chuck 70 High Top", 85.00m, 40, 28, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { 14, "Nike", "Жіноче взуття", "Рожевий", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Жіночі кросівки з максимальною амортизацією", true, "Air Max 270", 140.00m, 38, 10, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { 15, "Nike", "Чоловіче взуття", "Чорний", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Чоловічі кросівки з максимальною амортизацією", true, "Air Max 270", 140.00m, 45, 8, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "City", "Country", "CreatedAt", "Email", "FirstName", "IsAdmin", "LastName", "Phone", "PostalCode" },
                values: new object[,]
                {
                    { 1, "вул. Хрещатик, 1", "Київ", "Україна", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "test@example.com", "Тест", false, "Користувач", "+380501234567", "01001" },
                    { 2, "вул. Хрещатик, 2", "Київ", "Україна", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "admin@example.com", "Адмін", true, "Користувач", "+380501234568", "01001" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ShoeId",
                table: "CartItems",
                column: "ShoeId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_UserId_ShoeId",
                table: "CartItems",
                columns: new[] { "UserId", "ShoeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ShoeId",
                table: "OrderItems",
                column: "ShoeId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderNumber",
                table: "Orders",
                column: "OrderNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Status",
                table: "Orders",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Shoes_Brand",
                table: "Shoes",
                column: "Brand");

            migrationBuilder.CreateIndex(
                name: "IX_Shoes_Category",
                table: "Shoes",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Shoes_Name",
                table: "Shoes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Shoes");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
