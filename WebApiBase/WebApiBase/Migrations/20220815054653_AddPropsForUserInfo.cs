using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiBase.Migrations
{
    public partial class AddPropsForUserInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "user_info",
                type: "varchar(30)",
                nullable: true,
                comment: "用户账号",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "用户名")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "user_info",
                type: "varchar(5)",
                nullable: true,
                comment: "角色",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "角色")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Pwd",
                table: "user_info",
                type: "varchar(30)",
                nullable: true,
                comment: "密码",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "密码")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "HeadSculptrue",
                table: "user_info",
                type: "varchar(50)",
                nullable: true,
                comment: "头像")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "user_info",
                type: "varchar(30)",
                nullable: true,
                comment: "用户名")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "user_info",
                type: "varchar(200)",
                nullable: true,
                comment: "备注")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeadSculptrue",
                table: "user_info");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "user_info");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "user_info");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "user_info",
                type: "longtext",
                nullable: true,
                comment: "用户名",
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldNullable: true,
                oldComment: "用户账号")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "user_info",
                type: "longtext",
                nullable: true,
                comment: "角色",
                oldClrType: typeof(string),
                oldType: "varchar(5)",
                oldNullable: true,
                oldComment: "角色")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Pwd",
                table: "user_info",
                type: "longtext",
                nullable: true,
                comment: "密码",
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldNullable: true,
                oldComment: "密码")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
