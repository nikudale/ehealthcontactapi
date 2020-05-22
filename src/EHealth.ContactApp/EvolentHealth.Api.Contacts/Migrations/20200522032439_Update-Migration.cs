using Microsoft.EntityFrameworkCore.Migrations;

namespace EHealth.Api.Contacts.Migrations
{
    public partial class UpdateMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Contacts",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "PhoneNumber", "Status" },
                values: new object[] { 1, "nitin@test.com", "Nitin", "Kudale", "1234567890", (short)1 });

            migrationBuilder.InsertData(
                table: "Contacts",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "PhoneNumber", "Status" },
                values: new object[] { 2, "ramesh@test.com", "Ramesh", "Shah", "1111111111", (short)1 });
        }
    }
}
