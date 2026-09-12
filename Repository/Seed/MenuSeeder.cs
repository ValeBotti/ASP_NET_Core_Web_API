using System.Text.Json;
public static class MenuSeeder
{
    public static void Seed(AppDbContext db, IWebHostEnvironment env)
    {

        var jsonPath = Path.Combine(env.ContentRootPath, "Repository", "Seed", "menus.json");
        var json = File.ReadAllText(jsonPath);

        var items = JsonSerializer.Deserialize<List<MenuSeedDto>>(json);

        foreach (var item in items)
        {

            var menu = new Menu
            {
                Name = item.Name,
                Price = item.Price,
                Location = item.Location,
                ImageVersion = item.ImageVersion,
                Image = item.Image,
                ShortDescription = item.ShortDescription,
                LongDescription = item.LongDescription,
                DeliveryTime = item.DeliveryTime
            };

            db.Menus.Add(menu);
        }

        db.SaveChanges();
    }
}
