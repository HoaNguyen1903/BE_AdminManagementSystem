BEGIN TRANSACTION;
GO

-- Drop existing tables that we know are there and will be recreated
-- We check for existence before dropping to avoid errors if some don't exist
IF OBJECT_ID(N'[UserItems]') IS NOT NULL DROP TABLE [UserItems];
IF OBJECT_ID(N'[ShopOrderDetails]') IS NOT NULL DROP TABLE [ShopOrderDetails];
IF OBJECT_ID(N'[ShopOrders]') IS NOT NULL DROP TABLE [ShopOrders];
IF OBJECT_ID(N'[Reports]') IS NOT NULL DROP TABLE [Reports];
IF OBJECT_ID(N'[Notifications]') IS NOT NULL DROP TABLE [Notifications];
IF OBJECT_ID(N'[Announcements]') IS NOT NULL DROP TABLE [Announcements];
IF OBJECT_ID(N'[BundleItems]') IS NOT NULL DROP TABLE [BundleItems];
IF OBJECT_ID(N'[UserBundles]') IS NOT NULL DROP TABLE [UserBundles];
IF OBJECT_ID(N'[TopUpHistories]') IS NOT NULL DROP TABLE [TopUpHistories];
IF OBJECT_ID(N'[Items]') IS NOT NULL DROP TABLE [Items];
IF OBJECT_ID(N'[Staffs]') IS NOT NULL DROP TABLE [Staffs];
IF OBJECT_ID(N'[Users]') IS NOT NULL DROP TABLE [Users];
IF OBJECT_ID(N'[GemBundles]') IS NOT NULL DROP TABLE [GemBundles];
IF OBJECT_ID(N'[ItemBundles]') IS NOT NULL DROP TABLE [ItemBundles];
IF OBJECT_ID(N'[AbilitiesSets]') IS NOT NULL DROP TABLE [AbilitiesSets];
IF OBJECT_ID(N'[CharacterPvPs]') IS NOT NULL DROP TABLE [CharacterPvPs];
IF OBJECT_ID(N'[CharacterAttacks]') IS NOT NULL DROP TABLE [CharacterAttacks];
IF OBJECT_ID(N'[CharacterPassives]') IS NOT NULL DROP TABLE [CharacterPassives];
IF OBJECT_ID(N'[CharacterSkills]') IS NOT NULL DROP TABLE [CharacterSkills];
IF OBJECT_ID(N'[CharacterStats]') IS NOT NULL DROP TABLE [CharacterStats];
-- Also drop old tables from the previous schema if they still exist
IF OBJECT_ID(N'[OrderDetail]') IS NOT NULL DROP TABLE [OrderDetail];
IF OBJECT_ID(N'[Order]') IS NOT NULL DROP TABLE [Order];
IF OBJECT_ID(N'[BannerItem]') IS NOT NULL DROP TABLE [BannerItem];
IF OBJECT_ID(N'[Banner]') IS NOT NULL DROP TABLE [Banner];
IF OBJECT_ID(N'[Staff]') IS NOT NULL DROP TABLE [Staff];
IF OBJECT_ID(N'[User]') IS NOT NULL DROP TABLE [User];
IF OBJECT_ID(N'[Notification]') IS NOT NULL DROP TABLE [Notification];
IF OBJECT_ID(N'[Report]') IS NOT NULL DROP TABLE [Report];
IF OBJECT_ID(N'[Item]') IS NOT NULL DROP TABLE [Item];
IF OBJECT_ID(N'[UserItem]') IS NOT NULL DROP TABLE [UserItem];

GO

-- Re-create the tables according to the new schema

CREATE TABLE [CharacterAttacks] (
    [CharacterAttackId] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [Damage] int NOT NULL,
    [AP] int NOT NULL,
    [YuanPressure] int NOT NULL,
    [CritDmg] int NOT NULL,
    [CritRate] int NOT NULL,
    [LockedState] bit NOT NULL,
    CONSTRAINT [PK_CharacterAttacks] PRIMARY KEY ([CharacterAttackId])
);
GO

CREATE TABLE [CharacterPassives] (
    [CharacterPassiveId] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [LockedState] bit NOT NULL,
    CONSTRAINT [PK_CharacterPassives] PRIMARY KEY ([CharacterPassiveId])
);
GO

CREATE TABLE [CharacterSkills] (
    [CharacterSkillId] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [Damage] int NOT NULL,
    [SP] int NOT NULL,
    [YuanPressure] int NOT NULL,
    [CritDmg] int NOT NULL,
    [CritRate] int NOT NULL,
    [LockedState] bit NOT NULL,
    CONSTRAINT [PK_CharacterSkills] PRIMARY KEY ([CharacterSkillId])
);
GO

CREATE TABLE [CharacterStats] (
    [CharacterId] int NOT NULL IDENTITY,
    [MoveRange] int NOT NULL,
    [MaxHealth] int NOT NULL,
    CONSTRAINT [PK_CharacterStats] PRIMARY KEY ([CharacterId])
);
GO

CREATE TABLE [GemBundles] (
    [GemBundleId] int NOT NULL IDENTITY,
    [BundleName] nvarchar(max) NOT NULL,
    [BundlePrice] real NOT NULL,
    CONSTRAINT [PK_GemBundles] PRIMARY KEY ([GemBundleId])
);
GO

CREATE TABLE [ItemBundles] (
    [ItemBundleId] int NOT NULL IDENTITY,
    [BundleName] nvarchar(max) NOT NULL,
    [BundlePrice] real NOT NULL,
    CONSTRAINT [PK_ItemBundles] PRIMARY KEY ([ItemBundleId])
);
GO

CREATE TABLE [Items] (
    [ItemId] int NOT NULL IDENTITY,
    [ItemName] nvarchar(max) NOT NULL,
    [ItemDescription] nvarchar(max) NOT NULL,
    [ItemType] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Items] PRIMARY KEY ([ItemId])
);
GO

CREATE TABLE [Staffs] (
    [StaffId] int NOT NULL IDENTITY,
    [Email] nvarchar(255) NOT NULL,
    [Password] nvarchar(max) NOT NULL,
    [Role] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Staffs] PRIMARY KEY ([StaffId])
);
GO

CREATE TABLE [Users] (
    [UserId] int NOT NULL IDENTITY,
    [Email] nvarchar(255) NOT NULL,
    [FirstName] nvarchar(100) NOT NULL,
    [LastName] nvarchar(100) NOT NULL,
    [Banned] bit NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([UserId])
);
GO

CREATE TABLE [CharacterPvPs] (
    [CharacterTacticId] int NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_CharacterPvPs] PRIMARY KEY ([CharacterTacticId]),
    CONSTRAINT [FK_CharacterPvPs_CharacterStats_CharacterTacticId] FOREIGN KEY ([CharacterTacticId]) REFERENCES [CharacterStats] ([CharacterId]) ON DELETE CASCADE
);
GO

CREATE TABLE [BundleItems] (
    [BundleId] int NOT NULL,
    [ItemId] int NOT NULL,
    [Quantity] int NOT NULL,
    CONSTRAINT [PK_BundleItems] PRIMARY KEY ([BundleId], [ItemId]),
    CONSTRAINT [FK_BundleItems_ItemBundles_BundleId] FOREIGN KEY ([BundleId]) REFERENCES [ItemBundles] ([ItemBundleId]) ON DELETE CASCADE,
    CONSTRAINT [FK_BundleItems_Items_ItemId] FOREIGN KEY ([ItemId]) REFERENCES [Items] ([ItemId]) ON DELETE CASCADE
);
GO

CREATE TABLE [Announcements] (
    [AnnouncementId] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NOT NULL,
    [Content] nvarchar(max) NOT NULL,
    [Type] nvarchar(max) NOT NULL,
    [Status] nvarchar(max) NOT NULL,
    [StartDate] datetime2 NOT NULL,
    [EndDate] datetime2 NOT NULL,
    [CreatedBy] int NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedBy] int NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_Announcements] PRIMARY KEY ([AnnouncementId]),
    CONSTRAINT [FK_Announcements_Staffs_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Staffs] ([StaffId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Announcements_Staffs_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [Staffs] ([StaffId])
);
GO

CREATE TABLE [Notifications] (
    [NotificationId] int NOT NULL IDENTITY,
    [NotificationMessage] nvarchar(max) NOT NULL,
    [ReceiverId] int NOT NULL,
    [Read] bit NOT NULL,
    CONSTRAINT [PK_Notifications] PRIMARY KEY ([NotificationId]),
    CONSTRAINT [FK_Notifications_Users_ReceiverId] FOREIGN KEY ([ReceiverId]) REFERENCES [Users] ([UserId]) ON DELETE CASCADE
);
GO

CREATE TABLE [Reports] (
    [ReportId] int NOT NULL IDENTITY,
    [SenderId] int NOT NULL,
    [AccusedId] int NOT NULL,
    [Reason] nvarchar(max) NOT NULL,
    [AdditionalInfo] nvarchar(max) NULL,
    [SendDate] datetime2 NOT NULL,
    [Approved] bit NOT NULL,
    [ApprovedDate] datetime2 NULL,
    [ApprovedBy] int NULL,
    CONSTRAINT [PK_Reports] PRIMARY KEY ([ReportId]),
    CONSTRAINT [FK_Reports_Staffs_ApprovedBy] FOREIGN KEY ([ApprovedBy]) REFERENCES [Staffs] ([StaffId]),
    CONSTRAINT [FK_Reports_Users_AccusedId] FOREIGN KEY ([AccusedId]) REFERENCES [Users] ([UserId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Reports_Users_SenderId] FOREIGN KEY ([SenderId]) REFERENCES [Users] ([UserId]) ON DELETE NO ACTION
);
GO

CREATE TABLE [ShopOrders] (
    [ShopOrderId] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [TotalAmount] real NOT NULL,
    [OrderDate] datetime2 NOT NULL,
    CONSTRAINT [PK_ShopOrders] PRIMARY KEY ([ShopOrderId]),
    CONSTRAINT [FK_ShopOrders_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE CASCADE
);
GO

CREATE TABLE [TopUpHistories] (
    [TopUpId] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [GemBundleId] int NOT NULL,
    [GemsAmount] int NOT NULL,
    [RealMoneyAmount] real NOT NULL,
    [CurrencyCode] nvarchar(max) NOT NULL,
    [PaymentGateway] nvarchar(max) NOT NULL,
    [TransactionId] nvarchar(max) NOT NULL,
    [Status] nvarchar(max) NOT NULL,
    [Date] datetime2 NOT NULL,
    CONSTRAINT [PK_TopUpHistories] PRIMARY KEY ([TopUpId]),
    CONSTRAINT [FK_TopUpHistories_GemBundles_GemBundleId] FOREIGN KEY ([GemBundleId]) REFERENCES [GemBundles] ([GemBundleId]) ON DELETE CASCADE,
    CONSTRAINT [FK_TopUpHistories_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE CASCADE
);
GO

CREATE TABLE [UserBundles] (
    [UserId] int NOT NULL,
    [ItemBundleId] int NOT NULL,
    [GemBundleId] int NOT NULL,
    [Remaining] int NOT NULL,
    CONSTRAINT [PK_UserBundles] PRIMARY KEY ([UserId], [ItemBundleId], [GemBundleId]),
    CONSTRAINT [FK_UserBundles_GemBundles_GemBundleId] FOREIGN KEY ([GemBundleId]) REFERENCES [GemBundles] ([GemBundleId]),
    CONSTRAINT [FK_UserBundles_ItemBundles_ItemBundleId] FOREIGN KEY ([ItemBundleId]) REFERENCES [ItemBundles] ([ItemBundleId]),
    CONSTRAINT [FK_UserBundles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE CASCADE
);
GO

CREATE TABLE [AbilitiesSets] (
    [CharacterTacticId] int NOT NULL,
    [CharacterPassiveId] int NOT NULL,
    [CharacterSkillId] int NOT NULL,
    [CharacterAttackId] int NOT NULL,
    CONSTRAINT [PK_AbilitiesSets] PRIMARY KEY ([CharacterTacticId]),
    CONSTRAINT [FK_AbilitiesSets_CharacterAttacks_CharacterAttackId] FOREIGN KEY ([CharacterAttackId]) REFERENCES [CharacterAttacks] ([CharacterAttackId]) ON DELETE CASCADE,
    CONSTRAINT [FK_AbilitiesSets_CharacterPassives_CharacterPassiveId] FOREIGN KEY ([CharacterPassiveId]) REFERENCES [CharacterPassives] ([CharacterPassiveId]) ON DELETE CASCADE,
    CONSTRAINT [FK_AbilitiesSets_CharacterPvPs_CharacterTacticId] FOREIGN KEY ([CharacterTacticId]) REFERENCES [CharacterPvPs] ([CharacterTacticId]) ON DELETE CASCADE,
    CONSTRAINT [FK_AbilitiesSets_CharacterSkills_CharacterSkillId] FOREIGN KEY ([CharacterSkillId]) REFERENCES [CharacterSkills] ([CharacterSkillId]) ON DELETE CASCADE
);
GO

CREATE TABLE [ShopOrderDetails] (
    [ShopOrderDetailId] int NOT NULL IDENTITY,
    [ShopOrderId] int NOT NULL,
    [ItemBundleId] int NULL,
    [ItemId] int NOT NULL,
    [Quantity] int NOT NULL,
    [UnitPrice] real NOT NULL,
    CONSTRAINT [PK_ShopOrderDetails] PRIMARY KEY ([ShopOrderDetailId]),
    CONSTRAINT [FK_ShopOrderDetails_ItemBundles_ItemBundleId] FOREIGN KEY ([ItemBundleId]) REFERENCES [ItemBundles] ([ItemBundleId]),
    CONSTRAINT [FK_ShopOrderDetails_Items_ItemId] FOREIGN KEY ([ItemId]) REFERENCES [Items] ([ItemId]) ON DELETE CASCADE,
    CONSTRAINT [FK_ShopOrderDetails_ShopOrders_ShopOrderId] FOREIGN KEY ([ShopOrderId]) REFERENCES [ShopOrders] ([ShopOrderId]) ON DELETE CASCADE
);
GO

CREATE TABLE [UserItems] (
    [UserId] int NOT NULL,
    [ItemId] int NOT NULL,
    [Quantity] int NOT NULL,
    [ShopOrderId] int NOT NULL,
    CONSTRAINT [PK_UserItems] PRIMARY KEY ([UserId], [ItemId]),
    CONSTRAINT [FK_UserItems_Items_ItemId] FOREIGN KEY ([ItemId]) REFERENCES [Items] ([ItemId]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserItems_ShopOrders_ShopOrderId] FOREIGN KEY ([ShopOrderId]) REFERENCES [ShopOrders] ([ShopOrderId]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserItems_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_AbilitiesSets_CharacterAttackId] ON [AbilitiesSets] ([CharacterAttackId]);
GO

CREATE INDEX [IX_AbilitiesSets_CharacterPassiveId] ON [AbilitiesSets] ([CharacterPassiveId]);
GO

CREATE INDEX [IX_AbilitiesSets_CharacterSkillId] ON [AbilitiesSets] ([CharacterSkillId]);
GO

CREATE INDEX [IX_Announcements_CreatedBy] ON [Announcements] ([CreatedBy]);
GO

CREATE INDEX [IX_Announcements_UpdatedBy] ON [Announcements] ([UpdatedBy]);
GO

CREATE INDEX [IX_BundleItems_ItemId] ON [BundleItems] ([ItemId]);
GO

CREATE INDEX [IX_Notifications_ReceiverId] ON [Notifications] ([ReceiverId]);
GO

CREATE INDEX [IX_Reports_AccusedId] ON [Reports] ([AccusedId]);
GO

CREATE INDEX [IX_Reports_ApprovedBy] ON [Reports] ([ApprovedBy]);
GO

CREATE INDEX [IX_Reports_SenderId] ON [Reports] ([SenderId]);
GO

CREATE INDEX [IX_ShopOrderDetails_ItemBundleId] ON [ShopOrderDetails] ([ItemBundleId]);
GO

CREATE INDEX [IX_ShopOrderDetails_ItemId] ON [ShopOrderDetails] ([ItemId]);
GO

CREATE INDEX [IX_ShopOrderDetails_ShopOrderId] ON [ShopOrderDetails] ([ShopOrderId]);
GO

CREATE INDEX [IX_ShopOrders_UserId] ON [ShopOrders] ([UserId]);
GO

CREATE INDEX [IX_TopUpHistories_GemBundleId] ON [TopUpHistories] ([GemBundleId]);
GO

CREATE INDEX [IX_TopUpHistories_UserId] ON [TopUpHistories] ([UserId]);
GO

CREATE INDEX [IX_UserBundles_GemBundleId] ON [UserBundles] ([GemBundleId]);
GO

CREATE INDEX [IX_UserBundles_ItemBundleId] ON [UserBundles] ([ItemBundleId]);
GO

CREATE INDEX [IX_UserItems_ItemId] ON [UserItems] ([ItemId]);
GO

CREATE INDEX [IX_UserItems_ShopOrderId] ON [UserItems] ([ShopOrderId]);
GO

-- Re-initialize migrations history if needed
IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

DELETE FROM [__EFMigrationsHistory];
INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260326105031_InitialNewSchema', N'8.0.2');
GO

COMMIT;
GO
