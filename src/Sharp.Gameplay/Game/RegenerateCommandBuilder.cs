using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp.Gameplay.Game
{
    public class RegenerateCommandBuilder : CommandBuilderFacade
    {

        public RegenerateCommandBuilder(BaseCommand command) : base(command)
        {
        }
        
        protected override bool IsValid() => Command.RobotId != null;

        public RegenerateCommandBuilder SetRobotId(string id)
        {
            Command.RobotId = id;
            return this;
        }
    }
}
