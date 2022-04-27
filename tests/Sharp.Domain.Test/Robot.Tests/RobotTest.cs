using NUnit.Framework;
using Sharp.Domain.Map;

namespace Sharp.Domain.Robot.Tests;

[TestFixture]
public class RobotTest
{
    private readonly RobotAttributes _zeroedRobotAttributes = new(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
    private readonly Field _dummyField = new("mycoolField", new Map.Map("mycoolmap"));
    
    [Test]
    public void ShouldUpdateEnergy()
    {
        var robot = new Robot("mycooltestrobot", true, _zeroedRobotAttributes, _dummyField);
        
        robot.UpdateEnergy(40);
        Assert.AreEqual(40, robot.Attributes.Energy);
    }
    
    [Test]
    public void ShouldThrowOnUpdateEnergyIfRobotIsDead()
    {
        var robot = new Robot("mycooltestrobot", false, _zeroedRobotAttributes, _dummyField);
        
        Assert.Throws<DeadRobotActionException>(() => robot.UpdateEnergy(40));
        Assert.AreEqual(0, robot.Attributes.Energy);
    }

    [Test]
    public void ShouldKillRobot()
    {
        var robot = new Robot("mycooltestrobot", true, _zeroedRobotAttributes, _dummyField);
        
        robot.Kill();
        Assert.AreEqual(false, robot.Alive);
    }
    
    [Test]
    public void ShouldThrowOnMoveIfRobotIsDead()
    {
        var map = new Map.Map("mycoolmap");
        var field = new Field("mycoolfield", map);
        map.AddField(field);
        var robot = new Robot("mycooltestrobot", false, _zeroedRobotAttributes, _dummyField);
        
        Assert.Throws<DeadRobotActionException>(() => robot.Move(field));
    }

    [Test]
    public void ShouldThrowIfFieldIsNotANeighbour()
    {
        var map = new Map.Map("mycoolmap");
        var field = new Field("mycoolfield", map);
        map.AddField(field);
        _dummyField.Map.AddField(_dummyField); // Don't ask... Need to refactor the map
        var robot = new Robot("mycooltestrobot", true, _zeroedRobotAttributes, _dummyField);
        
        Assert.Throws<IllegalRobotMovementException>(() => robot.Move(field));
    }
    
    [Test]
    public void ShouldMoveIfFieldIsANeighbour()
    {
        var map = new Map.Map("mycoolmap");
        var startField = new Field("mycoolfield", map);
        var destField = new Field("mycoolfield2", map);
        var connection = new Connection(destField);
        map.AddField(startField);
        map.AddField(destField);
        map.AddConnection(startField, connection);
        var robot = new Robot("mycooltestrobot", true, _zeroedRobotAttributes, startField);
        
        robot.Move(destField);
        
        Assert.AreEqual(destField, robot.Field);
    }
}