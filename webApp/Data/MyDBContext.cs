using System;
using API.Models;
using Microsoft.EntityFrameworkCore;
using webApp.Models;

namespace webApp.Data
{
    public class MyDBContext : DbContext
    {

        public MyDBContext(DbContextOptions<MyDBContext> options) : base(options) {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        builder.Entity<CartGroupOption>() 
       .ToTable("cart_group_options");


            builder.Entity<OptionGroup>()
           .ToTable("option_groups");

            builder.Entity<UserNotification>()
          .ToTable("user_notifications");
        }
        public DbSet<ApplicationUser> users { get; set; }
        public DbSet<Market> markets { get; set; }
        public DbSet<Food> foods { get; set; }
        public DbSet<Driver> drivers { get; set; }
        public DbSet<Cart> carts { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Slider> sliders { get; set; }
        public DbSet<Field> fields { get; set; }
        public DbSet<City> cites { get; set; }
        public DbSet<Option> options { get; set; }
        public DbSet<OptionGroup> OptionGroups { get; set; }
        public DbSet<CartGroupOption> CartGroupoptions { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<Address> addresses { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }

    }
}

