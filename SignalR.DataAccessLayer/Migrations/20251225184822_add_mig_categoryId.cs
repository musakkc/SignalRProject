using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SignalR.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class add_mig_categoryId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryID",
                table: "Discounts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_CategoryID",
                table: "Discounts",
                column: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Discounts_Categories_CategoryID",
                table: "Discounts",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "CategoryID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discounts_Categories_CategoryID",
                table: "Discounts");

            migrationBuilder.DropIndex(
                name: "IX_Discounts_CategoryID",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "Discounts");
        }
    }
}
