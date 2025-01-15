using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
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
            entity.Property(i => i.FileName).IsRequired();
            entity.Property(i => i.FileSize).IsRequired();
            entity.Property(i => i.ContentType).IsRequired();
            entity.HasOne(i => i.User)
                .WithMany(u => u.Images)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<UserFriend>(entity =>
        {
            entity.HasKey(uf => new { uf.UserId, uf.FriendId });
            
            entity.HasOne(uf => uf.User)
                .WithMany(u => u.Friends)
                .HasForeignKey(uf => uf.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(uf => uf.Friend)
                .WithMany(uf => uf.AddedByFriends)
                .HasForeignKey(uf => uf.FriendId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}