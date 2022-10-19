using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiBase.Migrations
{
    public partial class UpdateDIDProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_data_view_db_connection_DBConnectionModelID",
                table: "data_view");

            migrationBuilder.DropColumn(
                name: "DID",
                table: "data_view");

            migrationBuilder.RenameColumn(
                name: "DBConnectionModelID",
                table: "data_view",
                newName: "DBID");

            migrationBuilder.RenameIndex(
                name: "IX_data_view_DBConnectionModelID",
                table: "data_view",
                newName: "IX_data_view_DBID");

            migrationBuilder.AddForeignKey(
                name: "FK_data_view_db_connection_DBID",
                table: "data_view",
                column: "DBID",
                principalTable: "db_connection",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_data_view_db_connection_DBID",
                table: "data_view");

            migrationBuilder.RenameColumn(
                name: "DBID",
                table: "data_view",
                newName: "DBConnectionModelID");

            migrationBuilder.RenameIndex(
                name: "IX_data_view_DBID",
                table: "data_view",
                newName: "IX_data_view_DBConnectionModelID");

            migrationBuilder.AddColumn<int>(
                name: "DID",
                table: "data_view",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "DB连接表的ID,所属数据库");

            migrationBuilder.AddForeignKey(
                name: "FK_data_view_db_connection_DBConnectionModelID",
                table: "data_view",
                column: "DBConnectionModelID",
                principalTable: "db_connection",
                principalColumn: "ID");
        }
    }
}
