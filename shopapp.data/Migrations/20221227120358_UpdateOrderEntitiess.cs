using Microsoft.EntityFrameworkCore.Migrations;

namespace shopapp.data.Migrations
{
    public partial class UpdateOrderEntitiess : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConversationId",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentId",
                table: "Orders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConversationId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "Orders");
        }
    }
}
