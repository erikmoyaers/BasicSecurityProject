using Microsoft.EntityFrameworkCore.Migrations;

namespace BasicSecurityProject.Migrations
{
    public partial class seeddata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "ID", "Hash", "Salt", "Username" },
                values: new object[] { 1, "A8CFCD74832004951B4408CDB0A5DBCD8C7E52D43F7FE244BF720582E05241DA", "ef6e5f49d8ssd8v", "John" });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "ID", "Hash", "Salt", "Username" },
                values: new object[] { 2, "9AE4BC0E32DB0E3484CD398459D20F9B4F79CCE36667428181BF037131A3C987", "ef6e5f49d8ssd8v", "Chris" });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "ID", "Hash", "Salt", "Username" },
                values: new object[] { 3, "E2674AA2162A3225B5B51DD0A796C32F17679642593347BD6A24EB90EEBF912B", "ef6e5f49d8ssd8v", "Mukesh" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "ID",
                keyValue: 3);
        }
    }
}
