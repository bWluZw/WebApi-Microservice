using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiBase.Migrations
{
    public partial class UpdateProps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_data_view_db_connection_DBID",
                table: "data_view");

            migrationBuilder.RenameColumn(
                name: "DBUser",
                table: "db_connection",
                newName: "DbUser");

            migrationBuilder.RenameColumn(
                name: "DBType",
                table: "db_connection",
                newName: "DbType");

            migrationBuilder.RenameColumn(
                name: "DBPort",
                table: "db_connection",
                newName: "DbPort");

            migrationBuilder.RenameColumn(
                name: "DBPassword",
                table: "db_connection",
                newName: "DbPassword");

            migrationBuilder.RenameColumn(
                name: "DBName",
                table: "db_connection",
                newName: "DbName");

            migrationBuilder.RenameColumn(
                name: "DBIP",
                table: "db_connection",
                newName: "DbIP");

            migrationBuilder.RenameColumn(
                name: "DBID",
                table: "data_view",
                newName: "DbID");

            migrationBuilder.RenameColumn(
                name: "VNName",
                table: "data_view",
                newName: "ViewName");

            migrationBuilder.RenameIndex(
                name: "IX_data_view_DBID",
                table: "data_view",
                newName: "IX_data_view_DbID");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "db_connection",
                type: "varchar(255)",
                nullable: true,
                comment: "注释")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "data_view",
                type: "varchar(255)",
                nullable: true,
                comment: "注释")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_data_view_db_connection_DbID",
                table: "data_view",
                column: "DbID",
                principalTable: "db_connection",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_data_view_db_connection_DbID",
                table: "data_view");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "db_connection");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "data_view");

            migrationBuilder.RenameColumn(
                name: "DbUser",
                table: "db_connection",
                newName: "DBUser");

            migrationBuilder.RenameColumn(
                name: "DbType",
                table: "db_connection",
                newName: "DBType");

            migrationBuilder.RenameColumn(
                name: "DbPort",
                table: "db_connection",
                newName: "DBPort");

            migrationBuilder.RenameColumn(
                name: "DbPassword",
                table: "db_connection",
                newName: "DBPassword");

            migrationBuilder.RenameColumn(
                name: "DbName",
                table: "db_connection",
                newName: "DBName");

            migrationBuilder.RenameColumn(
                name: "DbIP",
                table: "db_connection",
                newName: "DBIP");

            migrationBuilder.RenameColumn(
                name: "DbID",
                table: "data_view",
                newName: "DBID");

            migrationBuilder.RenameColumn(
                name: "ViewName",
                table: "data_view",
                newName: "VNName");

            migrationBuilder.RenameIndex(
                name: "IX_data_view_DbID",
                table: "data_view",
                newName: "IX_data_view_DBID");

            migrationBuilder.AddForeignKey(
                name: "FK_data_view_db_connection_DBID",
                table: "data_view",
                column: "DBID",
                principalTable: "db_connection",
                principalColumn: "ID");
        }
    }
}
