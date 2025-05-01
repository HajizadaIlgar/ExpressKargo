using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YunusExpress_MVC.Migrations
{
    /// <inheritdoc />
    public partial class createtables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Couriers",
                columns: table => new
                {
                    CourierId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourierName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourierPhoneNum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrdersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Couriers", x => x.CourierId);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryZoneTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryZoneTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Receivers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ReceiverAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverPhoneNum = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receivers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Senders",
                columns: table => new
                {
                    SenderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenderAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenderPhoneNum = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Senders", x => x.SenderId);
                });

            migrationBuilder.CreateTable(
                name: "ServiceTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryZones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    DeliveryZoneTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryZones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryZones_DeliveryZoneTypes_DeliveryZoneTypeId",
                        column: x => x.DeliveryZoneTypeId,
                        principalTable: "DeliveryZoneTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryZoneTypeSender",
                columns: table => new
                {
                    DeliveryZonesId = table.Column<int>(type: "int", nullable: false),
                    SendersSenderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryZoneTypeSender", x => new { x.DeliveryZonesId, x.SendersSenderId });
                    table.ForeignKey(
                        name: "FK_DeliveryZoneTypeSender_DeliveryZoneTypes_DeliveryZonesId",
                        column: x => x.DeliveryZonesId,
                        principalTable: "DeliveryZoneTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeliveryZoneTypeSender_Senders_SendersSenderId",
                        column: x => x.SendersSenderId,
                        principalTable: "Senders",
                        principalColumn: "SenderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderNo = table.Column<int>(type: "int", nullable: false),
                    InvoiceNo = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    ReceiverName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverPhoneNum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverId = table.Column<int>(type: "int", nullable: false),
                    SenderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenderPhoneNum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenderAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliveryZoneId = table.Column<int>(type: "int", nullable: false),
                    CourierId = table.Column<int>(type: "int", nullable: false),
                    OrderPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SpecialPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Discount = table.Column<int>(type: "int", nullable: true),
                    EDV = table.Column<int>(type: "int", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Couriers_CourierId",
                        column: x => x.CourierId,
                        principalTable: "Couriers",
                        principalColumn: "CourierId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_DeliveryZones_DeliveryZoneId",
                        column: x => x.DeliveryZoneId,
                        principalTable: "DeliveryZones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Receivers_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Receivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_ServiceTypes_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "ServiceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryZones_DeliveryZoneTypeId",
                table: "DeliveryZones",
                column: "DeliveryZoneTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryZoneTypeSender_SendersSenderId",
                table: "DeliveryZoneTypeSender",
                column: "SendersSenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CourierId",
                table: "Orders",
                column: "CourierId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DeliveryZoneId",
                table: "Orders",
                column: "DeliveryZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_InvoiceNo",
                table: "Orders",
                column: "InvoiceNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderNo",
                table: "Orders",
                column: "OrderNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ReceiverId",
                table: "Orders",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ServiceId",
                table: "Orders",
                column: "ServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryZoneTypeSender");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Senders");

            migrationBuilder.DropTable(
                name: "Couriers");

            migrationBuilder.DropTable(
                name: "DeliveryZones");

            migrationBuilder.DropTable(
                name: "Receivers");

            migrationBuilder.DropTable(
                name: "ServiceTypes");

            migrationBuilder.DropTable(
                name: "DeliveryZoneTypes");
        }
    }
}
