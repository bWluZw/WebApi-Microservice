using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiBase.Migrations
{
    public partial class test2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "role",
                table: "user",
                newName: "角色");

            migrationBuilder.RenameColumn(
                name: "pwd",
                table: "user",
                newName: "密码");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "user",
                newName: "用户名");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "角色",
                table: "user",
                newName: "role");

            migrationBuilder.RenameColumn(
                name: "密码",
                table: "user",
                newName: "pwd");

            migrationBuilder.RenameColumn(
                name: "用户名",
                table: "user",
                newName: "name");
        }
    }
}
