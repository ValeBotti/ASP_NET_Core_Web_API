using Microsoft.EntityFrameworkCore;

public class MenuRepository : IMenuRepository
{
/// <remarks>
/// OVERVIEW: This repository performs persistence operations for the menu entity.
/// </remarks>

    private readonly AppDbContext _db;

    public MenuRepository(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Retrieves all menus.
    /// </summary>
    /// <remarks>
    /// EFFECTS:
    /// Queries the database and returns all menus mapped to MenuListDto.
    /// </remarks>
    public List<MenuListDto> GetMenu()
    {
        return _db.Menus
            .AsNoTracking()
            .Select(m => new MenuListDto
            {
                Mid = m.Id,
                Name = m.Name,
                Price = m.Price,
                Location = m.Location,
                ImageVersion = m.ImageVersion,
                ShortDescription = m.ShortDescription,
                LongDescription = m.LongDescription,
                DeliveryTime = m.DeliveryTime
            })
            .ToList();
    }

    /// <summary>
    /// Retrieves the menu image.
    /// </summary>
    /// <remarks>
    /// EFFECTS:
    /// Queries the database and returns the image associated with the specified menu identifier.
    /// </remarks>
    public string GetMenuImage(int mid)
    {
        return _db.Menus
            .AsNoTracking()
            .Where(m => m.Id == mid)
            .Select(m => m.Image)
            .First();
    }

    /// <summary>
    /// Retrieves the menu details.
    /// </summary>
    /// <remarks>
    /// EFFECTS:
    /// Queries the database and returns the details associated with the specified menu identifier.
    /// </remarks>
    public MenuListDto GetMenuDetails(int mid)
    {
        return _db.Menus
            .AsNoTracking()
            .Where(m => m.Id == mid)
            .Select(m => new MenuListDto
            {
                Mid = m.Id,
                Name = m.Name,
                Price = m.Price,
                Location = m.Location,
                ImageVersion = m.ImageVersion,
                ShortDescription = m.ShortDescription,
                LongDescription = m.LongDescription,
                DeliveryTime = m.DeliveryTime
            })
            .First();
    }

}
