namespace Sharp.Domain.Game
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
