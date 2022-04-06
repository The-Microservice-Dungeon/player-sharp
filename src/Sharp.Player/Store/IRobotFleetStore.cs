﻿using Sharp.Gameplay.Robot;

namespace Sharp.Player.Repository;

public interface IRobotFleetStore : ICollectionStateStore<Robot>
{
    Robot? Get(string id);
}