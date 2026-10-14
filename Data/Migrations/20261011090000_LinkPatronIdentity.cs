using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using TopEndLibraryHub.Data;

#nullable disable
namespace TopEndLibraryHub.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20261011090000_LinkPatronIdentity")]
    public partial class LinkPatronIdentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(name: "IdentityUserId", table: "Borrowers", type: "nvarchar(450)", nullable: true);
            migrationBuilder.CreateIndex(name: "IX_Borrowers_IdentityUserId", table: "Borrowers", column: "IdentityUserId", unique: true, filter: "[IdentityUserId] IS NOT NULL");
            migrationBuilder.AddForeignKey(name: "FK_Borrowers_AspNetUsers_IdentityUserId", table: "Borrowers", column: "IdentityUserId", principalTable: "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Restrict);
            // Existing data with duplicate active loans must be repaired before this index can be applied.
            migrationBuilder.DropIndex(name: "IX_Loans_ItemId", table: "Loans");
            migrationBuilder.CreateIndex(name: "IX_Loans_ItemId", table: "Loans", column: "ItemId", unique: true, filter: "[ReturnedDate] IS NULL");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "IX_Loans_ItemId", table: "Loans");
            migrationBuilder.CreateIndex(name: "IX_Loans_ItemId", table: "Loans", column: "ItemId");
            migrationBuilder.DropForeignKey(name: "FK_Borrowers_AspNetUsers_IdentityUserId", table: "Borrowers");
            migrationBuilder.DropIndex(name: "IX_Borrowers_IdentityUserId", table: "Borrowers");
            migrationBuilder.DropColumn(name: "IdentityUserId", table: "Borrowers");
        }
    }
}
