using tecnologyApp.Data;

namespace tecnologyApp.Data;

public static class SeedData
{
    public static void Initialize(ApplicationDbContext context)
    {
        if (!context.Users.Any())
        {
            context.Users.Add(new User
            {
                Username = "admin",
                Password = "admin123",
                Role = "Seller"
            });

            context.SaveChanges();
        }
    }
}
