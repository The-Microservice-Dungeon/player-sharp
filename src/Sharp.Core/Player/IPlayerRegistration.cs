namespace Sharp.Core.Player;

public interface IPlayerRegistration
{
    /// <summary>
    ///     Registers a new player
    /// </summary>
    /// <param name="details">details</param>
    /// <returns>Token</returns>
    /// <exception cref="DuplicatePlayerException">If player is already registered / A player with the same details exists</exception>
    PlayerCredentials Register(PlayerDetails details);

    /// <summary>
    ///     Get Player Credentials
    /// </summary>
    /// <param name="details">details</param>
    /// <returns>Token</returns>
    /// <exception cref="PlayerNotFoundException">If player does not exist</exception>
    PlayerCredentials GetCredentials(PlayerDetails details);
}