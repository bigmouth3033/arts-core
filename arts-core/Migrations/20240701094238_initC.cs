using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace arts_core.Migrations
{
    /// <inheritdoc />
    public partial class initC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Variants_VariantId",
                table: "Orders");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Variants_VariantId",
                table: "Orders",
                column: "VariantId",
                principalTable: "Variants",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Variants_VariantId",
                table: "Orders");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Variants_VariantId",
                table: "Orders",
                column: "VariantId",
                principalTable: "Variants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
