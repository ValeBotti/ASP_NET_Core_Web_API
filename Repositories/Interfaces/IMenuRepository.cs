public interface IMenuRepository
{
    List<MenuListDto> GetMenu();

    string GetMenuImage(int mid);

    MenuListDto GetMenuDetails(int mid);
}
