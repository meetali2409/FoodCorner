using FoodCornerAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class FoodCornerDbContext
    : IdentityDbContext<User>
{
    public FoodCornerDbContext(DbContextOptions<FoodCornerDbContext> options)
        : base(options)
    {
    }
    public DbSet<FoodItem> FoodItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Cart> Carts { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Order>()
            .Property(o => o.Status)
            .HasConversion<string>();

        builder.Entity<Category>().HasData(
            new Category { CategoryId = 1, CategoryName = "Beverages" },
            new Category { CategoryId = 2, CategoryName = "Snacks" },
            new Category { CategoryId = 3, CategoryName = "Main Course" },
            new Category { CategoryId = 4, CategoryName = "Dessert" }
        );

      
        builder.Entity<FoodItem>().HasData(
            new FoodItem
            {
                FoodItemId = 1,
                FoodItemName = "Masala Tea",
                Price = 30,
                CategoryId = 1,
                Description = "Hot Indian masala tea brewed with spices.",
                Image = "https://ikneadtoeat.com/wp-content/uploads/2019/01/Masala-Chai-Recipe-1.jpg"
            },
            new FoodItem
            {
                FoodItemId = 2,
                FoodItemName = "Cold Coffee",
                Price = 120,
                CategoryId = 1,
                Description = "Chilled coffee topped with cream.",
                Image = "https://deliciousmadeeasy.com/wp-content/uploads/2018/04/chocoholic-cold-brew-coffee-1-of-1-7-scaled.jpg"
            },
            new FoodItem
            {
                FoodItemId = 3,
                FoodItemName = "Veg Sandwich",
                Price = 80,
                CategoryId = 2,
                Description = "Fresh vegetable sandwich with buttered bread.",
                Image = "https://www.yumcurry.com/wp-content/uploads/2021/05/club-sandwich-recipe.jpg"
            },
            new FoodItem
            {
                FoodItemId = 4,
                FoodItemName = "Paneer Butter Masala",
                Price = 220,
                CategoryId = 3,
                Description = "Creamy paneer curry cooked in rich tomato gravy.",
                Image = "https://www.vegrecipesofindia.com/wp-content/uploads/2020/01/paneer-butter-masala-5-500x375.jpg"
            },
            new FoodItem
            {
                FoodItemId = 5,
                FoodItemName = "Veg Biryani",
                Price = 180,
                CategoryId = 3,
                Description = "Aromatic basmati rice cooked with vegetables and spices.",
                Image = "https://www.madhuseverydayindian.com/wp-content/uploads/2022/11/easy-vegetable-biryani.jpg"
            },
            new FoodItem
            {
                FoodItemId = 6,
                FoodItemName = "Gulab Jamun",
                Price = 60,
                CategoryId = 4,
                Description = "Soft milk-solid balls soaked in sugar syrup.",
                Image = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQDoDvFX_4AvlKsYRBc0Ku0T--XFDb5tYCayQ&s"
            }
        );
    }


}

