using System;
using NUnit.Framework;
using Sharp.Gameplay.Map;

namespace Sharp.GameplayTests.Map;

[TestFixture]
public class MapFixture
{
    [Test]
    public void AddDuplicateFieldShouldThrow()
    {
        Gameplay.Map.Map map = new(System.Guid.NewGuid().ToString());        
        Field field = new(System.Guid.NewGuid().ToString(), 0, 0);
        map.AddField(field);

        Assert.Throws<ArgumentException>(() => map.AddField(field));
    }
    
    [Test]
    public void AddDuplicateFieldIdShouldThrow()
    {
        Gameplay.Map.Map map = new(System.Guid.NewGuid().ToString());        
        Field field = new(System.Guid.NewGuid().ToString(), 0, 0);
        map.AddField(field);

        Field fieldWithSameId = new(field.ID, 2, 10);
        Assert.Throws<ArgumentException>(() => map.AddField(fieldWithSameId));
    }
}