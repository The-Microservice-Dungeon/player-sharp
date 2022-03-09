using System;
using NUnit.Framework;
using Sharp.Gameplay.Map;

namespace Sharp.GameplayTests.Map;

public class FieldTests
{
    [Test]
    public void DuplicateNeighbourShouldThrow()
    {
        Field field = new(System.Guid.NewGuid().ToString(), 0, 0);
        Field neighbourField = new(System.Guid.NewGuid().ToString(), 0, 0);
        neighbourField.AddNeighbour(Direction.EAST, field);

        Assert.Throws<ArgumentException>(() => field.AddNeighbour(Direction.SOUTH, field));
    }
    
    [Test]
    public void OnOverwriteNeighbourShouldThrow()
    {
        Field field = new(System.Guid.NewGuid().ToString(), 0, 0);
        Field neighbourField = new(System.Guid.NewGuid().ToString(), 0, 0);
        field.AddNeighbour(Direction.EAST, neighbourField);

        Assert.Throws<ArgumentException>(() => field.AddNeighbour(Direction.EAST, neighbourField));
    }
    
    [Test]
    public void NeighbourToItselfShouldThrow()
    {
        Field field = new(System.Guid.NewGuid().ToString(), 0, 0);
        Field neighbourField = new(System.Guid.NewGuid().ToString(), 0, 0);
        neighbourField.AddNeighbour(Direction.EAST, field);

        Assert.Throws<ArgumentException>(() => field.AddNeighbour(Direction.SOUTH, field));
    }
}