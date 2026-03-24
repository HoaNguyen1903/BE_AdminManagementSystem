using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Unalive_WebManagement.Models;

namespace Unalive_WebManagement.Data;

public partial class UnaliveDbContext : DbContext
{
    public UnaliveDbContext()
    {
    }

    public UnaliveDbContext(DbContextOptions<UnaliveDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Banner> Banners { get; set; }

    public virtual DbSet<BannerItem> BannerItems { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<Staff> Staffs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserItem> UserItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // fallback for development if not configured via dependency injection
            // It is recommended to always configure this via Program.cs
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Banner>(entity =>
        {
            entity.HasKey(e => e.BannerId).HasName("PK__Banner__32E86A3105BD4F22");

            entity.ToTable("Banner");

            entity.Property(e => e.BannerId).HasColumnName("BannerID");
            entity.Property(e => e.BannerImage)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<BannerItem>(entity =>
        {
            entity.HasKey(e => new { e.BannerId, e.ItemId });

            entity.ToTable("BannerItem");

            entity.Property(e => e.BannerId).HasColumnName("BannerID");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");

            entity.HasOne(d => d.Banner).WithMany(p => p.BannerItems)
                .HasForeignKey(d => d.BannerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BannerItem_Banner");

            entity.HasOne(d => d.Item).WithMany(p => p.BannerItems)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BannerItem_Item");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__Item__727E83EBC67B7FD7");

            entity.ToTable("Item");

            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValue("No description available");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Type).HasDefaultValue(1);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E327824AC33");

            entity.ToTable("Notification");

            entity.Property(e => e.NotificationId).HasColumnName("NotificationID");
            entity.Property(e => e.NotificationMessage)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ReceiverId).HasColumnName("ReceiverID");

            entity.HasOne(d => d.Receiver).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.ReceiverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notification_Staff");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__C3905BAF74018F6A");

            entity.ToTable("Order");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_User");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductId });

            entity.ToTable("OrderDetail");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Subtotal).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetail_Order");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetail_Item");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK__Report__D5BD48E564E770A9");

            entity.ToTable("Report");

            entity.Property(e => e.ReportId).HasColumnName("ReportID");
            entity.Property(e => e.AccusedId).HasColumnName("AccusedID");
            entity.Property(e => e.AdditionalInfo).HasMaxLength(1000);
            entity.Property(e => e.Reason)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.SendDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SenderId).HasColumnName("SenderID");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Unapproved");
            entity.Property(e => e.ApprovedBy).HasColumnName("ApprovedBy");
            entity.Property(e => e.ApprovedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Accused).WithMany(p => p.ReportAccuseds)
                .HasForeignKey(d => d.AccusedId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Report_Accused");

            entity.HasOne(d => d.Sender).WithMany(p => p.ReportSenders)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Report_Sender");

            entity.HasOne(d => d.ApprovedByNavigation).WithMany()
                .HasForeignKey(d => d.ApprovedBy)
                .HasConstraintName("FK_Report_Staff");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK__Staffs__96D4AAF784D9B092");

            entity.Property(e => e.StaffId).HasColumnName("StaffID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CCACD5C34135");

            entity.ToTable("User");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Banned).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserItem>(entity =>
        {
            entity.HasKey(e => e.UserItemId).HasName("PK__UserItem__B44C03C3DF3D8F14");

            entity.ToTable("UserItem");

            entity.HasIndex(e => new { e.UserId, e.ItemId }, "UQ_UserItem_UserID_ItemID").IsUnique();

            entity.Property(e => e.UserItemId).HasColumnName("UserItemID");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.ObtainedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Quantity).HasDefaultValue(1);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Item).WithMany(p => p.UserItems)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserItem_Item");

            entity.HasOne(d => d.User).WithMany(p => p.UserItems)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserItem_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
