﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tank.Financing.Migrations
{
    public partial class Updated_Apply_22031013314660 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Allowance",
                table: "AppApplies",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "APR",
                table: "AppApplies",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppApplies",
                keyColumn: "Allowance",
                keyValue: null,
                column: "Allowance",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Allowance",
                table: "AppApplies",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AppApplies",
                keyColumn: "APR",
                keyValue: null,
                column: "APR",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "APR",
                table: "AppApplies",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
