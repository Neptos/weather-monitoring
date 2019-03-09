using Microsoft.EntityFrameworkCore.Migrations;

namespace Ingester.Persistence.Migrations
{
    public partial class ChangedValueToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "DataPoints",
                nullable: true,
                oldClrType: typeof(float));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Value",
                table: "DataPoints",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
