using Database;
using ITShop.API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ITShop.API.Database
{
    public class ITShop_DBContext : IdentityDbContext<User, Role, Guid>, IITShop_DBContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductPicture> ProductPictures { get; set; }
        public DbSet<ProductProducer> ProductProducers { get; set; }
       
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductInventory> ProductInventories { get; set; }
        public DbSet<PaymentDetails> PaymentDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<CartItems> CartItems { get; set; }
      

        public ITShop_DBContext(DbContextOptions<ITShop_DBContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("asp_net_users", "identity");
            modelBuilder.Entity<Role>().ToTable("asp_net_roles", "identity");
            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("asp_net_user_claims", "identity");
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("asp_net_user_roles", "identity");
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("asp_net_user_logins", "identity");
            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("asp_net_role_claims", "identity");
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("asp_net_user_tokens", "identity");

        }
    }
}
