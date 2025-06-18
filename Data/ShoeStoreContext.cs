using Microsoft.EntityFrameworkCore;
using ShoeStore.Models;

namespace ShoeStore.Data
{
    public class ShoeStoreContext : DbContext
    {
        public ShoeStoreContext(DbContextOptions<ShoeStoreContext> options) : base(options)
        {
        }

        public DbSet<Shoe> Shoes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure entities (same as before)
            modelBuilder.Entity<Shoe>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.HasIndex(e => e.Name);
                entity.HasIndex(e => e.Brand);
                entity.HasIndex(e => e.Category);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).IsRequired();
            });

            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.User).WithMany(u => u.CartItems).HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Shoe).WithMany().HasForeignKey(e => e.ShoeId).OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(e => new { e.UserId, e.ShoeId }).IsUnique();
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");
                entity.HasIndex(e => e.OrderNumber).IsUnique();
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.Status);
                entity.HasOne(e => e.User).WithMany(u => u.Orders).HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.HasOne(e => e.Order).WithMany(o => o.OrderItems).HasForeignKey(e => e.OrderId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Shoe).WithMany().HasForeignKey(e => e.ShoeId).OnDelete(DeleteBehavior.Restrict);
            });

            var fixedDate = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc);
            
            modelBuilder.Entity<Shoe>().HasData(
                new Shoe
                {
                    Id = 1,
                    Name = "Air Max 90",
                    Brand = "Nike",
                    Price = 120.00m,
                    Size = 42,
                    Color = "Білий",
                    Category = "Спортивне взуття",
                    StockQuantity = 15,
                    Description = "Класичні кросівки Nike Air Max 90 з видимою підошвою Air",
                    IsActive = true,
                    CreatedAt = fixedDate,
                    UpdatedAt = fixedDate
                },
                new Shoe
                {
                    Id = 2,
                    Name = "Stan Smith",
                    Brand = "Adidas",
                    Price = 85.00m,
                    Size = 41,
                    Color = "Білий",
                    Category = "Повсякденне взуття",
                    StockQuantity = 20,
                    Description = "Легендарні білі кросівки Adidas Stan Smith",
                    IsActive = true,
                    CreatedAt = fixedDate,
                    UpdatedAt = fixedDate
                },
                new Shoe
                {
                    Id = 3,
                    Name = "Chuck Taylor All Star",
                    Brand = "Converse",
                    Price = 65.00m,
                    Size = 40,
                    Color = "Чорний",
                    Category = "Повсякденне взуття",
                    StockQuantity = 25,
                    Description = "Іконічні високі кеди Converse Chuck Taylor",
                    IsActive = true,
                    CreatedAt = fixedDate,
                    UpdatedAt = fixedDate
                },
                
                // НОВІ КРОСІВКИ
                new Shoe
                {
                    Id = 4,
                    Name = "Air Force 1",
                    Brand = "Nike",
                    Price = 110.00m,
                    Size = 43,
                    Color = "Білий",
                    Category = "Спортивне взуття",
                    StockQuantity = 25,
                    Description = "Легендарні баскетбольні кросівки Nike Air Force 1",
                    IsActive = true,
                    CreatedAt = fixedDate,
                    UpdatedAt = fixedDate
                },
                new Shoe
                {
                    Id = 5,
                    Name = "Ultraboost 22",
                    Brand = "Adidas",
                    Price = 180.00m,
                    Size = 42,
                    Color = "Чорний",
                    Category = "Спортивне взуття",
                    StockQuantity = 15,
                    Description = "Високотехнологічні кросівки для бігу з технологією Boost",
                    IsActive = true,
                    CreatedAt = fixedDate,
                    UpdatedAt = fixedDate
                },
                new Shoe
                {
                    Id = 6,
                    Name = "Suede Classic",
                    Brand = "Puma",
                    Price = 75.00m,
                    Size = 41,
                    Color = "Синій",
                    Category = "Повсякденне взуття",
                    StockQuantity = 30,
                    Description = "Класичні замшеві кросівки Puma у винтажному стилі",
                    IsActive = true,
                    CreatedAt = fixedDate,
                    UpdatedAt = fixedDate
                },
                new Shoe
                {
                    Id = 7,
                    Name = "Old Skool",
                    Brand = "Vans",
                    Price = 65.00m,
                    Size = 40,
                    Color = "Чорний/Білий",
                    Category = "Скейтерське взуття",
                    StockQuantity = 20,
                    Description = "Іконічні скейтерські кросівки з боковою смужкою",
                    IsActive = true,
                    CreatedAt = fixedDate,
                    UpdatedAt = fixedDate
                },
                new Shoe
                {
                    Id = 8,
                    Name = "574 Core",
                    Brand = "New Balance",
                    Price = 95.00m,
                    Size = 44,
                    Color = "Сірий",
                    Category = "Спортивне взуття",
                    StockQuantity = 18,
                    Description = "Комфортні ретро кросівки з класичним дизайном",
                    IsActive = true,
                    CreatedAt = fixedDate,
                    UpdatedAt = fixedDate
                },
                new Shoe
                {
                    Id = 9,
                    Name = "Air Jordan 1 Mid",
                    Brand = "Nike",
                    Price = 125.00m,
                    Size = 43,
                    Color = "Червоний/Чорний/Білий",
                    Category = "Баскетбольне взуття",
                    StockQuantity = 12,
                    Description = "Легендарні баскетбольні кросівки Jordan у середній висоті",
                    IsActive = true,
                    CreatedAt = fixedDate,
                    UpdatedAt = fixedDate
                },
                new Shoe
                {
                    Id = 10,
                    Name = "Gazelle",
                    Brand = "Adidas",
                    Price = 90.00m,
                    Size = 42,
                    Color = "Зелений",
                    Category = "Повсякденне взуття",
                    StockQuantity = 22,
                    Description = "Класичні замшеві кросівки з винтажним шармом",
                    IsActive = true,
                    CreatedAt = fixedDate,
                    UpdatedAt = fixedDate
                },
                new Shoe
                {
                    Id = 11,
                    Name = "Classic Leather",
                    Brand = "Reebok",
                    Price = 80.00m,
                    Size = 41,
                    Color = "Білий",
                    Category = "Повсякденне взуття",
                    StockQuantity = 16,
                    Description = "Шкіряні кросівки у класичному стилі",
                    IsActive = true,
                    CreatedAt = fixedDate,
                    UpdatedAt = fixedDate
                },
                new Shoe
                {
                    Id = 12,
                    Name = "Dunk Low",
                    Brand = "Nike",
                    Price = 100.00m,
                    Size = 42,
                    Color = "Коричневий/Білий",
                    Category = "Спортивне взуття",
                    StockQuantity = 14,
                    Description = "Стильні кросівки для скейтбордингу та повсякденного носіння",
                    IsActive = true,
                    CreatedAt = fixedDate,
                    UpdatedAt = fixedDate
                },
                new Shoe
                {
                    Id = 13,
                    Name = "Chuck 70 High Top",
                    Brand = "Converse",
                    Price = 85.00m,
                    Size = 40,
                    Color = "Жовтий",
                    Category = "Повсякденне взуття",
                    StockQuantity = 28,
                    Description = "Преміум версія класичних високих кедів",
                    IsActive = true,
                    CreatedAt = fixedDate,
                    UpdatedAt = fixedDate
                },
                new Shoe
                {
                    Id = 14,
                    Name = "Air Max 270",
                    Brand = "Nike",
                    Price = 140.00m,
                    Size = 38,
                    Color = "Рожевий",
                    Category = "Жіноче взуття",
                    StockQuantity = 10,
                    Description = "Жіночі кросівки з максимальною амортизацією",
                    IsActive = true,
                    CreatedAt = fixedDate,
                    UpdatedAt = fixedDate
                },
                new Shoe
                {
                    Id = 15,
                    Name = "Air Max 270",
                    Brand = "Nike",
                    Price = 140.00m,
                    Size = 45,
                    Color = "Чорний",
                    Category = "Чоловіче взуття",
                    StockQuantity = 8,
                    Description = "Чоловічі кросівки з максимальною амортизацією",
                    IsActive = true,
                    CreatedAt = fixedDate,
                    UpdatedAt = fixedDate
                }
            );

            // Seed test users
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FirstName = "Тест",
                    LastName = "Користувач",
                    Email = "test@example.com",
                    Phone = "+380501234567",
                    Address = "вул. Хрещатик, 1",
                    City = "Київ",
                    PostalCode = "01001",
                    Country = "Україна",
                    IsAdmin = false,
                    CreatedAt = fixedDate
                },
                new User
                {
                    Id = 2,
                    FirstName = "Адмін",
                    LastName = "Користувач",
                    Email = "admin@example.com",
                    Phone = "+380501234568",
                    Address = "вул. Хрещатик, 2",
                    City = "Київ",
                    PostalCode = "01001",
                    Country = "Україна",
                    IsAdmin = true,
                    CreatedAt = fixedDate
                }
            );
        }
    }
}