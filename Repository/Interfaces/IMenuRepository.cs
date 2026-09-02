public interface IMenuRepository
{
    Task<Menu?> GetMenuByIdAsync(int mid);
    
    Task<List<MenuListDto>> GetMenuAsync();

    Task<string> GetMenuImageAsync(int mid);

    Task<MenuListDto> GetMenuDetailsAsync(int mid);
}
