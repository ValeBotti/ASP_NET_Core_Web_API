public interface ISessionRepository
{
    Task<(int uid, string sid)> CreateSessionAsync();
}
