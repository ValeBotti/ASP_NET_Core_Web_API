using Microsoft.EntityFrameworkCore;

public class UserService : IUserService
{
/// <remarks>
/// OVERVIEW: This service performs persistence operations for the user entity.
/// </remarks>

    private readonly IUserRepository _userRepository;
    private readonly IOrderRepository _orderRepository;

    public UserService(IUserRepository userRepository, IOrderRepository orderRepository)
    {
        _userRepository = userRepository;
        _orderRepository = orderRepository;
    }
    
    /// <summary>
    /// Retrieves a user by their unique identifier.
    /// </summary>
    /// <remarks>
    /// 
    /// EFFECTS:
    /// Retrieves the user entity's data by their unique identifier, along with the last order's identifier and status.
    /// </remarks>
    public async Task<UserDto?> GetUserAsync(int uid)
    {
        var user = await _userRepository.GetUserAsync(  uid);
        if (user == null) throw new KeyNotFoundException($"Utente con UID {uid} non trovato.");

        var lastOrder = await _orderRepository.GetLastOrderAsync(uid);

        return new UserDto
        {
            FirstName = user.FirstName,
            LastName = user.LastName,

            CardFullName = user.CardFullName,
            CardNumber = user.CardNumber,
            CardExpireMonth = user.CardExpireMonth,
            CardExpireYear = user.CardExpireYear,
            CardCVV = user.CardCVV,

            LastOid = lastOrder?.Id,
            OrderStatus = lastOrder?.Status
        };
    }


    /// <summary>
    /// Updates a user.
    /// </summary>
    /// <remarks>
    /// MODIFIES:
    /// The database: the user entity is updated with the provided information.
    /// 
    /// EFFECTS:
    /// Retrieves the user entity's data, order's last identifier, and order's status.
    /// </remarks>
    public async Task<UserDto?> UpdateUserAsync(int uid, UserDto dto)
    {
        var user = await _userRepository.GetUserAsync(uid);
        if (user == null)
            throw new KeyNotFoundException($"Utente con UID {uid} non trovato.");

        var now = DateTime.UtcNow;
        int currentYear = now.Year;
        int currentMonth = now.Month;

        bool isExpired =
            dto.CardExpireYear < currentYear ||
            (dto.CardExpireYear == currentYear && dto.CardExpireMonth < currentMonth);

        if (isExpired)
            throw new InvalidOperationException("CARD_EXPIRED");

        user.FirstName = dto.FirstName ?? user.FirstName;
        user.LastName = dto.LastName ?? user.LastName;
        user.CardFullName = dto.CardFullName ?? user.CardFullName;
        user.CardNumber = dto.CardNumber ?? user.CardNumber;
        user.CardExpireMonth = dto.CardExpireMonth ?? user.CardExpireMonth;
        user.CardExpireYear = dto.CardExpireYear ?? user.CardExpireYear;
        user.CardCVV = dto.CardCVV ?? user.CardCVV;

        await _userRepository.UpdateUserAsync(user);

        return new UserDto
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            CardFullName = user.CardFullName,
            CardNumber = user.CardNumber,
            CardExpireMonth = user.CardExpireMonth,
            CardExpireYear = user.CardExpireYear,
            CardCVV = user.CardCVV
        };
    }
}
