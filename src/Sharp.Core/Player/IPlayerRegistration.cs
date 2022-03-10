namespace Sharp.Core.Player;

public interface IPlayerRegistration
{
    PlayerCredentials Register(PlayerDetails details);
    PlayerCredentials GetCredentials(PlayerDetails details);
}