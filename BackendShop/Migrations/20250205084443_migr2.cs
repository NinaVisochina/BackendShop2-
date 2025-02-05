using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendShop.Migrations
{
    /// <inheritdoc />
    public partial class migr2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblOrderItems_tblProducts_ProductId",
                table: "tblOrderItems");

            migrationBuilder.AddForeignKey(
                name: "FK_tblOrderItems_tblProducts_ProductId",
                table: "tblOrderItems",
                column: "ProductId",
                principalTable: "tblProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblOrderItems_tblProducts_ProductId",
                table: "tblOrderItems");

            migrationBuilder.AddForeignKey(
                name: "FK_tblOrderItems_tblProducts_ProductId",
                table: "tblOrderItems",
                column: "ProductId",
                principalTable: "tblProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
