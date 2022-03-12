namespace Sharp.Gameplay.Map;

public class SpaceStation : IFieldLocatable
{
    public SpaceStation(Field field)
    {
        Location = field;
    }

    public Field Location { get; }
}