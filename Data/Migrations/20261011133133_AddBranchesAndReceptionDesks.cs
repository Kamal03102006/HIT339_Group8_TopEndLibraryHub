using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TopEndLibraryHub.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBranchesAndReceptionDesks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReceptionDesks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeskCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceptionDesks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceptionDesks_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_BranchId",
                table: "Items",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_BranchCode",
                table: "Branches",
                column: "BranchCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReceptionDesks_BranchId",
                table: "ReceptionDesks",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceptionDesks_DeskCode",
                table: "ReceptionDesks",
                column: "DeskCode",
                unique: true);
            // Create a default location for stock from the original
            // single-location application. This is a fictional demo branch.
            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[]
                {
        "Id",
        "BranchCode",
        "Name",
        "Address",
        "IsActive"
                },
                values: new object[]
                {
        1,
        "DAR",
        "Darwin Hub",
        "Fictional demonstration location in Darwin NT",
        true
                });

            // Preserve existing items by assigning them to Darwin Hub
            // before enforcing the branch foreign key.
            migrationBuilder.Sql(
                """
    UPDATE [Items]
    SET [BranchId] = (
        SELECT [Id]
        FROM [Branches]
        WHERE [BranchCode] = N'DAR'
    )
    WHERE [BranchId] = 0;
    """);
            migrationBuilder.AddForeignKey(
                name: "FK_Items_Branches_BranchId",
                table: "Items",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Branches_BranchId",
                table: "Items");

            migrationBuilder.DropTable(
                name: "ReceptionDesks");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropIndex(
                name: "IX_Items_BranchId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Items");
        }
    }
}
