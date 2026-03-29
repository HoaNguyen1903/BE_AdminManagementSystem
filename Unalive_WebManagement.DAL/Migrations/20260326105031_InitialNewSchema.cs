using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unalive_WebManagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialNewSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CharacterAttacks",
                columns: table => new
                {
                    CharacterAttackId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Damage = table.Column<int>(type: "int", nullable: false),
                    AP = table.Column<int>(type: "int", nullable: false),
                    YuanPressure = table.Column<int>(type: "int", nullable: false),
                    CritDmg = table.Column<int>(type: "int", nullable: false),
                    CritRate = table.Column<int>(type: "int", nullable: false),
                    LockedState = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterAttacks", x => x.CharacterAttackId);
                });

            migrationBuilder.CreateTable(
                name: "CharacterPassives",
                columns: table => new
                {
                    CharacterPassiveId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LockedState = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterPassives", x => x.CharacterPassiveId);
                });

            migrationBuilder.CreateTable(
                name: "CharacterSkills",
                columns: table => new
                {
                    CharacterSkillId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Damage = table.Column<int>(type: "int", nullable: false),
                    SP = table.Column<int>(type: "int", nullable: false),
                    YuanPressure = table.Column<int>(type: "int", nullable: false),
                    CritDmg = table.Column<int>(type: "int", nullable: false),
                    CritRate = table.Column<int>(type: "int", nullable: false),
                    LockedState = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterSkills", x => x.CharacterSkillId);
                });

            migrationBuilder.CreateTable(
                name: "CharacterStats",
                columns: table => new
                {
                    CharacterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MoveRange = table.Column<int>(type: "int", nullable: false),
                    MaxHealth = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterStats", x => x.CharacterId);
                });

            migrationBuilder.CreateTable(
                name: "GemBundles",
                columns: table => new
                {
                    GemBundleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BundleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BundlePrice = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GemBundles", x => x.GemBundleId);
                });

            migrationBuilder.CreateTable(
                name: "ItemBundles",
                columns: table => new
                {
                    ItemBundleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BundleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BundlePrice = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemBundles", x => x.ItemBundleId);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ItemId);
                });

            migrationBuilder.CreateTable(
                name: "Staffs",
                columns: table => new
                {
                    StaffId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staffs", x => x.StaffId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Banned = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "CharacterPvPs",
                columns: table => new
                {
                    CharacterTacticId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterPvPs", x => x.CharacterTacticId);
                    table.ForeignKey(
                        name: "FK_CharacterPvPs_CharacterStats_CharacterTacticId",
                        column: x => x.CharacterTacticId,
                        principalTable: "CharacterStats",
                        principalColumn: "CharacterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BundleItems",
                columns: table => new
                {
                    BundleId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BundleItems", x => new { x.BundleId, x.ItemId });
                    table.ForeignKey(
                        name: "FK_BundleItems_ItemBundles_BundleId",
                        column: x => x.BundleId,
                        principalTable: "ItemBundles",
                        principalColumn: "ItemBundleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BundleItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Announcements",
                columns: table => new
                {
                    AnnouncementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcements", x => x.AnnouncementId);
                    table.ForeignKey(
                        name: "FK_Announcements_Staffs_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Staffs",
                        principalColumn: "StaffId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Announcements_Staffs_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Staffs",
                        principalColumn: "StaffId");
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverId = table.Column<int>(type: "int", nullable: false),
                    Read = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    ReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    AccusedId = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdditionalInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SendDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Approved = table.Column<bool>(type: "bit", nullable: false),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_Reports_Staffs_ApprovedBy",
                        column: x => x.ApprovedBy,
                        principalTable: "Staffs",
                        principalColumn: "StaffId");
                    table.ForeignKey(
                        name: "FK_Reports_Users_AccusedId",
                        column: x => x.AccusedId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reports_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShopOrders",
                columns: table => new
                {
                    ShopOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<float>(type: "real", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopOrders", x => x.ShopOrderId);
                    table.ForeignKey(
                        name: "FK_ShopOrders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TopUpHistories",
                columns: table => new
                {
                    TopUpId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    GemBundleId = table.Column<int>(type: "int", nullable: false),
                    GemsAmount = table.Column<int>(type: "int", nullable: false),
                    RealMoneyAmount = table.Column<float>(type: "real", nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentGateway = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopUpHistories", x => x.TopUpId);
                    table.ForeignKey(
                        name: "FK_TopUpHistories_GemBundles_GemBundleId",
                        column: x => x.GemBundleId,
                        principalTable: "GemBundles",
                        principalColumn: "GemBundleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TopUpHistories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserBundles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ItemBundleId = table.Column<int>(type: "int", nullable: false),
                    GemBundleId = table.Column<int>(type: "int", nullable: false),
                    Remaining = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBundles", x => new { x.UserId, x.ItemBundleId, x.GemBundleId });
                    table.ForeignKey(
                        name: "FK_UserBundles_GemBundles_GemBundleId",
                        column: x => x.GemBundleId,
                        principalTable: "GemBundles",
                        principalColumn: "GemBundleId");
                    table.ForeignKey(
                        name: "FK_UserBundles_ItemBundles_ItemBundleId",
                        column: x => x.ItemBundleId,
                        principalTable: "ItemBundles",
                        principalColumn: "ItemBundleId");
                    table.ForeignKey(
                        name: "FK_UserBundles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbilitiesSets",
                columns: table => new
                {
                    CharacterTacticId = table.Column<int>(type: "int", nullable: false),
                    CharacterPassiveId = table.Column<int>(type: "int", nullable: false),
                    CharacterSkillId = table.Column<int>(type: "int", nullable: false),
                    CharacterAttackId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbilitiesSets", x => x.CharacterTacticId);
                    table.ForeignKey(
                        name: "FK_AbilitiesSets_CharacterAttacks_CharacterAttackId",
                        column: x => x.CharacterAttackId,
                        principalTable: "CharacterAttacks",
                        principalColumn: "CharacterAttackId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AbilitiesSets_CharacterPassives_CharacterPassiveId",
                        column: x => x.CharacterPassiveId,
                        principalTable: "CharacterPassives",
                        principalColumn: "CharacterPassiveId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AbilitiesSets_CharacterPvPs_CharacterTacticId",
                        column: x => x.CharacterTacticId,
                        principalTable: "CharacterPvPs",
                        principalColumn: "CharacterTacticId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AbilitiesSets_CharacterSkills_CharacterSkillId",
                        column: x => x.CharacterSkillId,
                        principalTable: "CharacterSkills",
                        principalColumn: "CharacterSkillId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShopOrderDetails",
                columns: table => new
                {
                    ShopOrderDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShopOrderId = table.Column<int>(type: "int", nullable: false),
                    ItemBundleId = table.Column<int>(type: "int", nullable: true),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopOrderDetails", x => x.ShopOrderDetailId);
                    table.ForeignKey(
                        name: "FK_ShopOrderDetails_ItemBundles_ItemBundleId",
                        column: x => x.ItemBundleId,
                        principalTable: "ItemBundles",
                        principalColumn: "ItemBundleId");
                    table.ForeignKey(
                        name: "FK_ShopOrderDetails_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShopOrderDetails_ShopOrders_ShopOrderId",
                        column: x => x.ShopOrderId,
                        principalTable: "ShopOrders",
                        principalColumn: "ShopOrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserItems",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ShopOrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserItems", x => new { x.UserId, x.ItemId });
                    table.ForeignKey(
                        name: "FK_UserItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserItems_ShopOrders_ShopOrderId",
                        column: x => x.ShopOrderId,
                        principalTable: "ShopOrders",
                        principalColumn: "ShopOrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserItems_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbilitiesSets_CharacterAttackId",
                table: "AbilitiesSets",
                column: "CharacterAttackId");

            migrationBuilder.CreateIndex(
                name: "IX_AbilitiesSets_CharacterPassiveId",
                table: "AbilitiesSets",
                column: "CharacterPassiveId");

            migrationBuilder.CreateIndex(
                name: "IX_AbilitiesSets_CharacterSkillId",
                table: "AbilitiesSets",
                column: "CharacterSkillId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_CreatedBy",
                table: "Announcements",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_UpdatedBy",
                table: "Announcements",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_BundleItems_ItemId",
                table: "BundleItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ReceiverId",
                table: "Notifications",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_AccusedId",
                table: "Reports",
                column: "AccusedId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ApprovedBy",
                table: "Reports",
                column: "ApprovedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_SenderId",
                table: "Reports",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopOrderDetails_ItemBundleId",
                table: "ShopOrderDetails",
                column: "ItemBundleId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopOrderDetails_ItemId",
                table: "ShopOrderDetails",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopOrderDetails_ShopOrderId",
                table: "ShopOrderDetails",
                column: "ShopOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopOrders_UserId",
                table: "ShopOrders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TopUpHistories_GemBundleId",
                table: "TopUpHistories",
                column: "GemBundleId");

            migrationBuilder.CreateIndex(
                name: "IX_TopUpHistories_UserId",
                table: "TopUpHistories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBundles_GemBundleId",
                table: "UserBundles",
                column: "GemBundleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBundles_ItemBundleId",
                table: "UserBundles",
                column: "ItemBundleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserItems_ItemId",
                table: "UserItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_UserItems_ShopOrderId",
                table: "UserItems",
                column: "ShopOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbilitiesSets");

            migrationBuilder.DropTable(
                name: "Announcements");

            migrationBuilder.DropTable(
                name: "BundleItems");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "ShopOrderDetails");

            migrationBuilder.DropTable(
                name: "TopUpHistories");

            migrationBuilder.DropTable(
                name: "UserBundles");

            migrationBuilder.DropTable(
                name: "UserItems");

            migrationBuilder.DropTable(
                name: "CharacterAttacks");

            migrationBuilder.DropTable(
                name: "CharacterPassives");

            migrationBuilder.DropTable(
                name: "CharacterPvPs");

            migrationBuilder.DropTable(
                name: "CharacterSkills");

            migrationBuilder.DropTable(
                name: "Staffs");

            migrationBuilder.DropTable(
                name: "GemBundles");

            migrationBuilder.DropTable(
                name: "ItemBundles");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "ShopOrders");

            migrationBuilder.DropTable(
                name: "CharacterStats");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
