using System.Text.Json;

public static class MenuSeeder
{
    public static void Seed(AppDbContext db)
    {
        
        if (db.Menus.Any())
            return;

        var jsonPath = Path.Combine("Data", "menus.json");

        var json = File.ReadAllText(jsonPath);

        var items = JsonSerializer.Deserialize<List<MenuSeedDto>>(json);

        if (items is null || items.Count == 0)
        {
            Console.WriteLine("MenuSeeder: nessun elemento trovato nel JSON.");
            return;
        }

        foreach (var item in items)
        {
            
            var imagePath = Path.Combine("Data", "images", item.ImageFile);

            var bytes = File.ReadAllBytes(imagePath);

            var base64 = Convert.ToBase64String(bytes);

            var menu = new Menu
            {
                Name = item.Name,
                Price = item.Price,
                Location = item.Location,
                ImageVersion = item.ImageVersion,
                Image = base64,
                ShortDescription = item.ShortDescription,
                LongDescription = item.LongDescription,
                DeliveryTime = item.DeliveryTime
            };

            db.Menus.Add(menu);
        }

        db.SaveChanges();
    }
}