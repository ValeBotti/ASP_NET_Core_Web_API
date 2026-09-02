public interface IUserService
{
    Task<UserDto?> GetUserAsync(int uid);
    Task<UserDto?> UpdateUserAsync(int uid, UserDto dto);

}
