using Pawze.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Pawze.Data.Infrastructure
{
    public class PawzeDataContext : DbContext
    {
        public PawzeDataContext() : base("Pawze")
        {
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }
        public IDbSet<Box> Boxes { get; set; }
        public IDbSet<BoxItem> BoxItems { get; set; }
        public IDbSet<Inventory> Inventories { get; set; }
        public IDbSet<Subscription> Subscriptions { get; set; }
        public IDbSet<Shipment> Shipments { get; set; }
        public IDbSet<PawzeConfiguration> PawzeConfigurations { get; set; }
        public IDbSet<PawzeUser> Users { get; set; }
        public IDbSet<UserRole> UserRoles { get; set; }
        public IDbSet<Role> Roles { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PawzeUser>()
            .HasMany(p => p.Subscriptions)
            .WithRequired(o => o.PawzeUser)
            .HasForeignKey(o => o.PawzeUserId)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<PawzeUser>()
            .HasMany(u => u.Boxes)
            .WithRequired(b => b.PawzeUser)
            .HasForeignKey(b => b.PawzeUserId);



            modelBuilder.Entity<Subscription>()
            .HasMany(o => o.Boxes)
            .WithOptional(b => b.Subscription)
            .HasForeignKey(b => b.SubscriptionId);
            

            modelBuilder.Entity<Box>()
            .HasMany(b => b.BoxItems)
            .WithRequired(b => b.Box)
            .HasForeignKey(b => b.BoxId);

            modelBuilder.Entity<Inventory>()
            .HasMany(i => i.BoxItems)
            .WithRequired(b => b.Inventory)
            .HasForeignKey(b => b.InventoryId);

            //TODO: Configure relationships for Users/Roles/Etc
            modelBuilder.Entity<PawzeUser>().Property(p => p.Id).HasColumnName("PawzeUserId");

            modelBuilder.Entity<PawzeUser>()
                .HasMany(u => u.Roles)
                .WithRequired(ur => ur.User)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<Role>()
                .HasMany(r => r.Users)
                .WithRequired(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<UserRole>().HasKey(u => new { u.UserId, u.RoleId });
        }

    }
}