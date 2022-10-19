using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiBase.Migrations
{
    public partial class UpdateTimeType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "user_info",
                type: "datetime(0)",
                nullable: false,
                comment: "创建时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "db_connection",
                type: "datetime(0)",
                nullable: false,
                comment: "创建时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "data_view",
                type: "datetime(0)",
                nullable: false,
                comment: "创建时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldComment: "创建时间");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "user_info",
                type: "datetime(6)",
                nullable: false,
                comment: "创建时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime(0)",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "db_connection",
                type: "datetime(6)",
                nullable: false,
                comment: "创建时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime(0)",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "data_view",
                type: "datetime(6)",
                nullable: false,
                comment: "创建时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime(0)",
                oldComment: "创建时间");
        }
    }
}
