namespace Sharp.Domain.Game;

/// <summary>
///     Facade to create a fluent API on building commands
/// </summary>
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