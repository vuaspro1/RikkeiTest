using Microsoft.EntityFrameworkCore;

namespace RikkeiTest.Models
{
    public class MyDbContext :DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options) 
        {

        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<Category> Categorys { get; set; }
        public virtual DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .HasKey(p => new { p.RoleId, p.UserId });
            modelBuilder.Entity<UserRole>()
                .HasOne(p => p.Role)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(p => p.RoleId);
            modelBuilder.Entity<UserRole>()
                .HasOne(p => p.User)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(p => p.UserId);
            modelBuilder.Entity<RolePermission>()
                .HasKey(p => new { p.RoleId, p.PermissionId });
            modelBuilder.Entity<RolePermission>()
                .HasOne(p => p.Role)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(p => p.RoleId);
            modelBuilder.Entity<RolePermission>()
                .HasOne(p => p.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(p => p.PermissionId);
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Price)
                      .HasColumnType("decimal(18,2)");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
