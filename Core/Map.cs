namespace Player.Sharp.Core;

public class Planet
{
    public readonly string ID;

    public Planet(string id)
    {
        ID = id;
    }
}

public class Spacestation
{
    public readonly string ID;

    public Spacestation(string id)
    {
        ID = id;
    }
}

public class Map
{
    public readonly string ID;
    public readonly HashSet<Planet> planets = new();
    public readonly HashSet<Spacestation> spacestations = new();

    public Map(string id)
    {
        ID = id;
    }
}