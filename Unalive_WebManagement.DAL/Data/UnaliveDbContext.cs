using Microsoft.EntityFrameworkCore;
using Unalive_WebManagement.Models;

namespace Unalive_WebManagement.Data;

public partial class UnaliveDbContext : DbContext
{
    public UnaliveDbContext(DbContextOptions<UnaliveDbContext> options) : base(options) { }

    #region DbSets
    public virtual DbSet<AbilitiesSet> AbilitiesSets { get; set; }
    public virtual DbSet<Announcement> Announcements { get; set; }
    public virtual DbSet<BundleItem> BundleItems { get; set; }
    public virtual DbSet<CharacterAttack> CharacterAttacks { get; set; }
    public virtual DbSet<CharacterPassive> CharacterPassives { get; set; }
    public virtual DbSet<CharacterPvP> CharacterPvPs { get; set; }
    public virtual DbSet<CharacterSkill> CharacterSkills { get; set; }
    public virtual DbSet<CharacterStat> CharacterStats { get; set; }
    public virtual DbSet<GemBundle> GemBundles { get; set; }
    public virtual DbSet<Item> Items { get; set; }
    public virtual DbSet<SkinAndCharacterBundle> SkinAndCharacterBundles { get; set; }
    public virtual DbSet<Notification> Notifications { get; set; }
    public virtual DbSet<Report> Reports { get; set; }
    public virtual DbSet<ShopOrder> ShopOrders { get; set; }
    public virtual DbSet<ShopOrderDetail> ShopOrderDetails { get; set; }
    public virtual DbSet<Staff> Staffs { get; set; }
    public virtual DbSet<TopUpHistory> TopUpHistories { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserBundle> UserBundles { get; set; }
    public virtual DbSet<UserItem> UserItems { get; set; }
    public virtual DbSet<UserBanLog> UserBanLogs { get; set; }
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        #region Entity Configurations
        modelBuilder.Entity<UserBanLog>(entity =>
        {
            entity.HasKey(e => e.UserBanLogId);
            entity.HasOne(d => d.User).WithMany().HasForeignKey(d => d.UserId);
            entity.HasOne(d => d.BannedByNavigation).WithMany().HasForeignKey(d => d.BannedBy);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Password).IsRequired().HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Banned)
                .HasComputedColumnSql("CASE WHEN [BannedUntil] > GETUTCDATE() THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Role).IsRequired().HasMaxLength(50);
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.ReportId);
            entity.HasOne<User>().WithMany().HasForeignKey(e => e.SenderId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<User>().WithMany().HasForeignKey(e => e.AccusedId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<Staff>().WithMany().HasForeignKey(e => e.ApprovedBy).IsRequired(false);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId);
            entity.HasOne<User>().WithMany().HasForeignKey(e => e.ReceiverId);
        });

        modelBuilder.Entity<Announcement>(entity =>
        {
            entity.HasKey(e => e.AnnouncementId);
            entity.HasOne<Staff>().WithMany().HasForeignKey(e => e.CreatedBy).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<Staff>().WithMany().HasForeignKey(e => e.UpdatedBy).IsRequired(false);
        });

        modelBuilder.Entity<TopUpHistory>(entity =>
        {
            entity.HasKey(e => e.TopUpId);
            entity.HasOne<User>().WithMany().HasForeignKey(e => e.UserId);
            entity.HasOne<GemBundle>().WithMany().HasForeignKey(e => e.GemBundleId);
        });

        modelBuilder.Entity<ShopOrder>(entity =>
        {
            entity.HasKey(e => e.ShopOrderId);
            entity.HasOne<User>().WithMany().HasForeignKey(e => e.UserId);
        });

        modelBuilder.Entity<ShopOrderDetail>(entity =>
        {
            entity.HasKey(e => e.ShopOrderDetailId);
            entity.HasOne<ShopOrder>().WithMany().HasForeignKey(e => e.ShopOrderId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<SkinAndCharacterBundle>().WithMany().HasForeignKey(e => e.SkinAndCharacterBundleId).IsRequired(false);
            entity.HasOne<Item>().WithMany().HasForeignKey(e => e.ItemId);
        });

        modelBuilder.Entity<UserItem>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.ItemId });
            entity.HasOne<User>().WithMany().HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<Item>().WithMany().HasForeignKey(e => e.ItemId);
            entity.HasOne<ShopOrder>().WithMany().HasForeignKey(e => e.ShopOrderId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ItemId);
        });

        modelBuilder.Entity<SkinAndCharacterBundle>(entity =>
        {
            entity.HasKey(e => e.SkinAndCharacterBundleId);
        });

        modelBuilder.Entity<GemBundle>(entity =>
        {
            entity.HasKey(e => e.GemBundleId);
        });

        modelBuilder.Entity<UserBundle>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.SkinAndCharacterBundleId, e.GemBundleId });
            entity.HasOne<User>().WithMany().HasForeignKey(e => e.UserId);
            entity.HasOne<SkinAndCharacterBundle>().WithMany().HasForeignKey(e => e.SkinAndCharacterBundleId).IsRequired(false);
            entity.HasOne<GemBundle>().WithMany().HasForeignKey(e => e.GemBundleId).IsRequired(false);
        });

        modelBuilder.Entity<BundleItem>(entity =>
        {
            entity.HasKey(e => new { e.SkinAndCharacterBundleId, e.ItemId });
            entity.HasOne<SkinAndCharacterBundle>().WithMany().HasForeignKey(e => e.SkinAndCharacterBundleId);
            entity.HasOne<Item>().WithMany().HasForeignKey(e => e.ItemId);
        });

        modelBuilder.Entity<CharacterStat>(entity =>
        {
            entity.HasKey(e => e.CharacterId);
        });

        modelBuilder.Entity<CharacterPvP>(entity =>
        {
            entity.HasKey(e => e.CharacterTacticId);
            entity.HasOne<CharacterStat>().WithMany().HasForeignKey(e => e.CharacterTacticId);
        });

        modelBuilder.Entity<AbilitiesSet>(entity =>
        {
            entity.HasKey(e => e.CharacterTacticId);
            entity.HasOne<CharacterPvP>().WithMany().HasForeignKey(e => e.CharacterTacticId);
            entity.HasOne<CharacterPassive>().WithMany().HasForeignKey(e => e.CharacterPassiveId);
            entity.HasOne<CharacterSkill>().WithMany().HasForeignKey(e => e.CharacterSkillId);
            entity.HasOne<CharacterAttack>().WithMany().HasForeignKey(e => e.CharacterAttackId);
        });

        modelBuilder.Entity<CharacterPassive>(entity =>
        {
            entity.HasKey(e => e.CharacterPassiveId);
        });

        modelBuilder.Entity<CharacterSkill>(entity =>
        {
            entity.HasKey(e => e.CharacterSkillId);
        });

        modelBuilder.Entity<CharacterAttack>(entity =>
        {
            entity.HasKey(e => e.CharacterAttackId);
        });

        #endregion
    }
}
