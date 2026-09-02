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
    /// Retrieves menu information by its identifier.
    /// </summary>
    /// <remarks>
    /// EFFECTS:
    /// Queries the database and returns the menu associated with the specified identifier.
    /// </remarks>
    public async Task<Menu?> GetMenuByIdAsync(int mid)
    {
        return await _db.Menus
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == mid);
    }

    /// <summary>
    /// Retrieves all menus.
    /// </summary>
    /// <remarks>
    /// EFFECTS:
    /// Queries the database and returns all menus mapped to MenuListDto.
    /// </remarks>
    public async Task<List<MenuListDto>> GetMenuAsync()
    {
        return await _db.Menus
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
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves the menu image.
    /// </summary>
    /// <remarks>
    /// EFFECTS:
    /// Queries the database and returns the image associated with the specified menu identifier.
    /// </remarks>
    public async Task<string> GetMenuImageAsync(int mid)
    {
        var exists = await _db.Menus
            .AsNoTracking()
            .AnyAsync(m => m.Id == mid);

        if (!exists)
            throw new KeyNotFoundException($"Menu con MID {mid} non trovato.");

        return await _db.Menus
            .AsNoTracking()
            .Where(m => m.Id == mid)
            .Select(m => m.Image)
            .FirstAsync();
    }

    /// <summary>
    /// Retrieves the menu details.
    /// </summary>
    /// <remarks>
    /// EFFECTS:
    /// Queries the database and returns the details associated with the specified menu identifier.
    /// </remarks>
    public async Task<MenuListDto> GetMenuDetailsAsync(int mid)
    {
        var menu = await _db.Menus
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
            .FirstOrDefaultAsync();

        if (menu == null)
            throw new KeyNotFoundException($"Menu con MID {mid} non trovato.");

        return menu;
    }

}
