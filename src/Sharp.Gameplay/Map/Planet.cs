namespace Sharp.Gameplay.Map;

public class Planet : IFieldLocatable
{
    public Planet(Field field)
    {
        Location = field;
    }

    public Field Location { get; }
}