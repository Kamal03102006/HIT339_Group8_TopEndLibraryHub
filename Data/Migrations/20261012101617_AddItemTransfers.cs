using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TopEndLibraryHub.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddItemTransfers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemTransfers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    OriginBranchId = table.Column<int>(type: "int", nullable: false),
                    DestinationBranchId = table.Column<int>(type: "int", nullable: false),
                    DispatchDeskId = table.Column<int>(type: "int", nullable: false),
                    ReceivingDeskId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DispatchedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReceivedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DispatchedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ReceivedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTransfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemTransfers_Branches_DestinationBranchId",
                        column: x => x.DestinationBranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemTransfers_Branches_OriginBranchId",
                        column: x => x.OriginBranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemTransfers_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemTransfers_ReceptionDesks_DispatchDeskId",
                        column: x => x.DispatchDeskId,
                        principalTable: "ReceptionDesks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemTransfers_ReceptionDesks_ReceivingDeskId",
                        column: x => x.ReceivingDeskId,
                        principalTable: "ReceptionDesks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemTransfers_DestinationBranchId",
                table: "ItemTransfers",
                column: "DestinationBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTransfers_DispatchDeskId",
                table: "ItemTransfers",
                column: "DispatchDeskId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTransfers_ItemId",
                table: "ItemTransfers",
                column: "ItemId",
                unique: true,
                filter: "[Status] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTransfers_OriginBranchId",
                table: "ItemTransfers",
                column: "OriginBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTransfers_ReceivingDeskId",
                table: "ItemTransfers",
                column: "ReceivingDeskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemTransfers");
        }
    }
}
