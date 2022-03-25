namespace Sharp.Gameplay.Game;

public abstract class CommandBuilderFacade
{
    protected BaseCommand Command;

    protected CommandBuilderFacade(BaseCommand command)
    {
        Command = command;
    }

    protected abstract bool IsValid();

    public BaseCommand Build()
    {
        if (!IsValid())
            throw new ApplicationException("Validation of command failed");
        return Command;
    }
}