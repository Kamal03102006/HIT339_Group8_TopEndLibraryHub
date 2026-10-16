using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TopEndLibraryHub.Data.Migrations
{
    /// <inheritdoc />
    public partial class LinkHoldsToPickupBranches : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Holds_PickupBranchId",
                table: "Holds",
                column: "PickupBranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Holds_Branches_PickupBranchId",
                table: "Holds",
                column: "PickupBranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Holds_Branches_PickupBranchId",
                table: "Holds");

            migrationBuilder.DropIndex(
                name: "IX_Holds_PickupBranchId",
                table: "Holds");
        }
    }
}
