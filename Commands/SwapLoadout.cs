using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Loadouts.Commands;

internal class SwapLoadout : ModCommand
{
    public override string Command { get; } = "swaploadout";
    public override string Description { get; } = "Swaps two loadout profiles";
    public override string Usage => "<loadout slot> or <loadout slot 1> <loadout slot 2>";
    public override CommandType Type => CommandType.Chat;

    public override void Action(CommandCaller caller, string input, string[] args)
    {
        if (args.Length < 1)
        {
            Main.NewText("You must specify the index of the current loadout profile to swap with.", Color.Red);
            return;
        }

        var player = caller.Player.GetModPlayer<ELPlayer>();
        int from = player.loadoutIndex;
        
        if (!int.TryParse(args[^1], out var to))
        {
            Main.NewText($"Equipment profile index `{args[^1]}` is not a valid number.", Color.Red);
            return;
        }

        if (args.Length > 1)
        {
            if (!int.TryParse(args[0], out from))
            {
                Main.NewText($"Equipment profile index `{args[^1]}` is not a valid number.", Color.Red);
                return;
            }
        }

        var lFrom = player.loadouts[from];
        var lTo = player.loadouts[to];

        player.loadouts[from] = lTo;

        // I have no idea why we have to do this, probably something to do with Terraria and it's saving items or w/e.
        // Removing this line duplicates your profile. Let's just call it a feature :)!
        if (player.loadoutIndex == from)
        {
            player.loadoutIndex = to;
        }

        player.loadouts[to] = lFrom;

        if (from == player.loadoutIndex)
        {
            lFrom.LoadLoadout();
        }
    }
}