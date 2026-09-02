public interface IUserRepository
{
    Task<User?> GetUserAsync(int uid);
    Task UpdateUserAsync(User user);
}