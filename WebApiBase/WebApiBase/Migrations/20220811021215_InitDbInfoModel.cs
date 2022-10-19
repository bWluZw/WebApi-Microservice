using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiBase.Migrations
{
    public partial class InitDbInfoModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "db_connection",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false, comment: "ID")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DBName = table.Column<string>(type: "varchar(50)", nullable: true, comment: "数据库名")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DBIP = table.Column<string>(type: "varchar(20)", nullable: true, comment: "数据库IP")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DBPort = table.Column<string>(type: "varchar(5)", nullable: true, comment: "数据库端口")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DBType = table.Column<int>(type: "int", nullable: false, comment: "0 Mysql;1 SqlServer;2 Oracle"),
                    DBUser = table.Column<string>(type: "varchar(20)", nullable: true, comment: "数据库用户")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DBPassword = table.Column<string>(type: "varchar(255)", nullable: true, comment: "数据库密码")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间"),
                    Creator = table.Column<string>(type: "varchar(30)", nullable: true, comment: "创建人")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_db_connection", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "data_view",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false, comment: "ID")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    VNName = table.Column<string>(type: "varchar(255)", nullable: true, comment: "视图名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DID = table.Column<int>(type: "int", nullable: false, comment: "DB连接表的ID,所属数据库"),
                    SqlText = table.Column<string>(type: "longtext", nullable: true, comment: "sql语句")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间"),
                    Creator = table.Column<string>(type: "varchar(30)", nullable: true, comment: "创建人")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DBConnectionModelID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_data_view", x => x.ID);
                    table.ForeignKey(
                        name: "FK_data_view_db_connection_DBConnectionModelID",
                        column: x => x.DBConnectionModelID,
                        principalTable: "db_connection",
                        principalColumn: "ID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_data_view_DBConnectionModelID",
                table: "data_view",
                column: "DBConnectionModelID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "data_view");

            migrationBuilder.DropTable(
                name: "db_connection");
        }
    }
}
