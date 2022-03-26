using Sharp.Gameplay.Trading;

namespace Sharp.Gameplay.Game;

/// <summary>
///     Fluent API Builder for a Buy command
/// </summary>
public class BuyCommandBuilder : CommandBuilderFacade
{
    public BuyCommandBuilder(BaseCommand command) : base(command)
    {
    }

    public BuyCommandBuilder SetItem(Item item)
    {
        Command.CommandObject.ItemName = item;
        return this;
    }

    public BuyCommandBuilder SetQuantity(uint qty)
    {
        Command.CommandObject.ItemQuantity = qty;
        return this;
    }

    protected override bool IsValid()
    {
        return Command.CommandObject.ItemName != null &&
               Command.CommandObject.ItemQuantity != null;
    }
}