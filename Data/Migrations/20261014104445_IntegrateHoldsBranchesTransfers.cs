
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TopEndLibraryHub.Data.Migrations
{
    public partial class IntegrateHoldsBranchesTransfers : Migration
    {
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
                    Id = table.Column<int>(
                        type: "int",
                        nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),

                    BranchCode = table.Column<string>(
                        type: "nvarchar(10)",
                        maxLength: 10,
                        nullable: false),

                    Name = table.Column<string>(
                        type: "nvarchar(100)",
                        maxLength: 100,
                        nullable: false),

                    Address = table.Column<string>(
                        type: "nvarchar(200)",
                        maxLength: 200,
                        nullable: false),

                    IsActive = table.Column<bool>(
                        type: "bit",
                        nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                });

            // Create Darwin City and assign existing items.
            // Existing item names, statuses and loans are preserved.
            migrationBuilder.Sql(@"
                INSERT INTO [Branches]
                    ([BranchCode], [Name], [Address], [IsActive])
                VALUES
                    (N'DRW-CITY', N'Darwin City', N'Darwin, NT', 1);

                UPDATE [Items]
                SET [BranchId] = (
                    SELECT [Id]
                    FROM [Branches]
                    WHERE [BranchCode] = N'DRW-CITY'
                )
                WHERE [BranchId] = 0;
            ");

            migrationBuilder.CreateTable(
                name: "Holds",
                columns: table => new
                {
                    Id = table.Column<int>(
                        type: "int",
                        nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),

                    ItemId = table.Column<int>(
                        type: "int",
                        nullable: false),

                    BorrowerId = table.Column<int>(
                        type: "int",
                        nullable: false),

                    PickupBranchId = table.Column<int>(
                        type: "int",
                        nullable: false),

                    CreatedAtUtc = table.Column<DateTime>(
                        type: "datetime2",
                        nullable: false),

                    Status = table.Column<int>(
                        type: "int",
                        nullable: false),

                    AllocatedAtUtc = table.Column<DateTime>(
                        type: "datetime2",
                        nullable: true),

                    ReadyForPickupAtUtc = table.Column<DateTime>(
                        type: "datetime2",
                        nullable: true),

                    FulfilledAtUtc = table.Column<DateTime>(
                        type: "datetime2",
                        nullable: true),

                    CancelledAtUtc = table.Column<DateTime>(
                        type: "datetime2",
                        nullable: true),

                    RowVersion = table.Column<byte[]>(
                        type: "rowversion",
                        rowVersion: true,
                        nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holds", x => x.Id);

                    table.ForeignKey(
                        name: "FK_Holds_Borrowers_BorrowerId",
                        column: x => x.BorrowerId,
                        principalTable: "Borrowers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);

                    table.ForeignKey(
                        name: "FK_Holds_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReceptionDesks",
                columns: table => new
                {
                    Id = table.Column<int>(
                        type: "int",
                        nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),

                    DeskCode = table.Column<string>(
                        type: "nvarchar(20)",
                        maxLength: 20,
                        nullable: false),

                    Name = table.Column<string>(
                        type: "nvarchar(100)",
                        maxLength: 100,
                        nullable: false),

                    BranchId = table.Column<int>(
                        type: "int",
                        nullable: false),

                    IsActive = table.Column<bool>(
                        type: "bit",
                        nullable: false)
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

            migrationBuilder.CreateTable(
                name: "NotificationLogs",
                columns: table => new
                {
                    Id = table.Column<int>(
                        type: "int",
                        nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),

                    BorrowerId = table.Column<int>(
                        type: "int",
                        nullable: false),

                    LoanId = table.Column<int>(
                        type: "int",
                        nullable: true),

                    HoldId = table.Column<int>(
                        type: "int",
                        nullable: true),

                    Channel = table.Column<int>(
                        type: "int",
                        nullable: false),

                    Kind = table.Column<int>(
                        type: "int",
                        nullable: false),

                    EventKey = table.Column<string>(
                        type: "nvarchar(180)",
                        maxLength: 180,
                        nullable: false),

                    Recipient = table.Column<string>(
                        type: "nvarchar(254)",
                        maxLength: 254,
                        nullable: false),

                    Message = table.Column<string>(
                        type: "nvarchar(1000)",
                        maxLength: 1000,
                        nullable: false),

                    CreatedAtUtc = table.Column<DateTime>(
                        type: "datetime2",
                        nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationLogs", x => x.Id);

                    table.ForeignKey(
                        name: "FK_NotificationLogs_Borrowers_BorrowerId",
                        column: x => x.BorrowerId,
                        principalTable: "Borrowers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);

                    table.ForeignKey(
                        name: "FK_NotificationLogs_Holds_HoldId",
                        column: x => x.HoldId,
                        principalTable: "Holds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);

                    table.ForeignKey(
                        name: "FK_NotificationLogs_Loans_LoanId",
                        column: x => x.LoanId,
                        principalTable: "Loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItemTransfers",
                columns: table => new
                {
                    Id = table.Column<int>(
                        type: "int",
                        nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),

                    ItemId = table.Column<int>(
                        type: "int",
                        nullable: false),

                    OriginBranchId = table.Column<int>(
                        type: "int",
                        nullable: false),

                    DestinationBranchId = table.Column<int>(
                        type: "int",
                        nullable: false),

                    DispatchDeskId = table.Column<int>(
                        type: "int",
                        nullable: false),

                    ReceivingDeskId = table.Column<int>(
                        type: "int",
                        nullable: true),

                    Status = table.Column<int>(
                        type: "int",
                        nullable: false),

                    DispatchedAtUtc = table.Column<DateTime>(
                        type: "datetime2",
                        nullable: false),

                    ReceivedAtUtc = table.Column<DateTime>(
                        type: "datetime2",
                        nullable: true),

                    DispatchedByUserId = table.Column<string>(
                        type: "nvarchar(450)",
                        maxLength: 450,
                        nullable: false),

                    ReceivedByUserId = table.Column<string>(
                        type: "nvarchar(450)",
                        maxLength: 450,
                        nullable: true),

                    RowVersion = table.Column<byte[]>(
                        type: "rowversion",
                        rowVersion: true,
                        nullable: false)
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
                name: "IX_Items_BranchId",
                table: "Items",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_BranchCode",
                table: "Branches",
                column: "BranchCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Holds_BorrowerId",
                table: "Holds",
                column: "BorrowerId");

            migrationBuilder.CreateIndex(
                name: "IX_Holds_OneActiveRequestPerBorrower",
                table: "Holds",
                columns: new[] { "ItemId", "BorrowerId" },
                unique: true,
                filter: "[Status] IN (0, 1, 2)");

            migrationBuilder.CreateIndex(
                name: "IX_Holds_OneAllocationPerItem",
                table: "Holds",
                column: "ItemId",
                unique: true,
                filter: "[Status] IN (1, 2)");

            migrationBuilder.CreateIndex(
                name: "IX_Holds_QueueOrder",
                table: "Holds",
                columns: new[]
                {
                    "ItemId",
                    "Status",
                    "CreatedAtUtc",
                    "Id"
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

            migrationBuilder.CreateIndex(
                name: "IX_NotificationLogs_BorrowerHistory",
                table: "NotificationLogs",
                columns: new[]
                {
                    "BorrowerId",
                    "CreatedAtUtc"
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationLogs_EventChannel",
                table: "NotificationLogs",
                columns: new[]
                {
                    "EventKey",
                    "Channel"
                },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationLogs_HoldId",
                table: "NotificationLogs",
                column: "HoldId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationLogs_LoanId",
                table: "NotificationLogs",
                column: "LoanId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceptionDesks_BranchId",
                table: "ReceptionDesks",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceptionDesks_DeskCode",
                table: "ReceptionDesks",
                column: "DeskCode",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Branches_BranchId",
                table: "Items",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Branches_BranchId",
                table: "Items");

            migrationBuilder.DropTable(
                name: "ItemTransfers");

            migrationBuilder.DropTable(
                name: "NotificationLogs");

            migrationBuilder.DropTable(
                name: "ReceptionDesks");

            migrationBuilder.DropTable(
                name: "Holds");

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