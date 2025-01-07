﻿using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<UserFriend> UserFriends { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.UserId);
            entity.HasIndex(u => u.Username).IsUnique();
            entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
            
            entity.Navigation(u => u.Images).UsePropertyAccessMode(PropertyAccessMode.Field);
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(i => i.PhotoId);
            entity.Property(i => i.FilePath).IsRequired();
            entity.HasOne(i => i.User)
                .WithMany(u => u.Images)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserFriend>(entity =>
        {
            entity.HasKey(uf => new { uf.UserId, uf.FriendId });
            
            entity.HasOne(uf => uf.User)
                .WithMany(u => u.Friends)
                .HasForeignKey(uf => uf.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(uf => uf.Friend)
                .WithMany(uf => uf.AddedByFriends)
                .HasForeignKey(uf => uf.FriendId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}