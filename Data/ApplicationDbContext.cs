using Jaahub.Models;
using Microsoft.EntityFrameworkCore;


namespace Jaahub.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<PropertyView> PropertyViews { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ✅ حل مشکل Multiple Cascade Paths در Messages
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.SetNull); // اگر فرستنده حذف شد، مقدار NULL شود

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany()
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction); // اگر گیرنده حذف شد، پیام باقی بماند

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Property)
                .WithMany()
                .HasForeignKey(m => m.PropertyId)
                .OnDelete(DeleteBehavior.NoAction); // حذف ملک باعث حذف پیام‌ها نشود

            // ✅ حل مشکل Multiple Cascade Paths در Favorites
            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Property)
                .WithMany()
                .HasForeignKey(f => f.PropertyId)
                .OnDelete(DeleteBehavior.NoAction); // اگر ملک حذف شد، تاثیری روی علاقه‌مندی‌ها نداشته باشد

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade); // حذف کاربر باعث حذف علاقه‌مندی‌هایش می‌شود

            // ✅ حل مشکل Multiple Cascade Paths در Rentals
            modelBuilder.Entity<Rental>()
                .HasOne(r => r.Property)
                .WithMany()
                .HasForeignKey(r => r.PropertyId)
                .OnDelete(DeleteBehavior.SetNull); // اگر ملک حذف شد، مقدار PropertyId به NULL شود

            modelBuilder.Entity<Rental>()
                .HasOne(r => r.Renter)
                .WithMany()
                .HasForeignKey(r => r.RenterId)
                .OnDelete(DeleteBehavior.NoAction); // حذف کاربر تاثیری روی رزروهای او ندارد

            // ✅ حل مشکل حذف ملک در PropertyViews
            modelBuilder.Entity<PropertyView>()
                .HasOne(pv => pv.Property)
                .WithMany()
                .HasForeignKey(pv => pv.PropertyId)
                .OnDelete(DeleteBehavior.SetNull);

            // ✅ حل مشکل Multiple Cascade Paths در Reviews
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Property)
                .WithMany()
                .HasForeignKey(r => r.PropertyId)
                .OnDelete(DeleteBehavior.SetNull); // اگر ملک حذف شد، مقدار PropertyId به NULL شود

            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction); // حذف کاربر تاثیری روی نظراتش ندارد

            // ✅ حل مشکل حذف ملک در Transactions
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Property)
                .WithMany()
                .HasForeignKey(t => t.PropertyId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // ✅ مشخص کردن دقت و مقیاس برای فیلدهای decimal
            modelBuilder.Entity<Property>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Rental>()
                .Property(r => r.TotalPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasColumnType("decimal(18,2)");
        }


    }
}
