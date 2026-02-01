
using Microsoft.EntityFrameworkCore;

namespace Services.Coupon.API.Data;

public class AppDbContext : DbContext
{   
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Models.Coupon> Coupons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Models.Coupon>().HasData(
            new Models.Coupon
            {
                CouponId = 4,
                CouponCode = "WELCOME10",
                DiscountAmount = 10.0,
                MinAmount = 50
            },
            new Models.Coupon
            {
                CouponId = 5,
                CouponCode = "SUMMER15",
                DiscountAmount = 15.0,
                MinAmount = 100
            }
        );
    }
}
