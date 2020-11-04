using web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;



namespace web.Data
{
    public class MaoContext : IdentityDbContext<ApplicationUser>
    {
        public MaoContext(DbContextOptions<MaoContext> options) : base(options)
        {
        }

        public DbSet<Menu> Menus { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<MenuOrder> MenuOrders { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Menu>().ToTable("Menu");
            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<MenuOrder>().ToTable("MenuOrder");

            modelBuilder.Entity<MenuOrder>()
               .HasKey(c => new { c.MenuID, c.OrderID });
        }
    }
}