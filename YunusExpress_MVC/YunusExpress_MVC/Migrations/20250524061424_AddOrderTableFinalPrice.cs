﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YunusExpress_MVC.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderTableFinalPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "FinalPrice",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinalPrice",
                table: "Orders");
        }
    }
}
