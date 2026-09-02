public interface ISessionRepository
{
    Task<UidSid?> GetUidBySidAsync(string sid);
    Task<(int uid, string sid)> CreateSessionAsync();
}
