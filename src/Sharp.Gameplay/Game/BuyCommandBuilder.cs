using Sharp.Gameplay.Trading;

namespace Sharp.Gameplay.Game;

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

    protected override bool Validate() => Command.CommandObject.ItemName != null &&
                                          Command.CommandObject.ItemQuantity != null;
}