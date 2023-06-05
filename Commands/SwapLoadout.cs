using Loadouts.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Loadouts.Commands;

internal class SwapLoadout : ModCommand
{
    public override string Command { get; } = "swaploadout";
    public override string Description { get; } = "Swaps two loadout profiles";
    public override string Usage => "Usage: /swaploadout <loadout slot> or /swaploadout <loadout slot 1> <loadout slot 2>";
    public override CommandType Type => CommandType.Chat;

    public override void Action(CommandCaller caller, string input, string[] args)
    {
        if (args.Length < 1 || args.Length > 2) {
            Main.NewText(Usage);
            return;
        }
        
        bool validFirstArg = int.TryParse(args[0], out int firstArg);
        int secondArg = -1;
        bool validSecondArg = args.Length == 2 ? int.TryParse(args[1], out secondArg) : false;
        
        switch ((validFirstArg, validSecondArg, args.Length)) {
            case (false, _, _):
            case (true, false, 2):
                Main.NewText("Invalid Format: Loadout indexes must be numbers", Color.Red);
                return;
        }
        
        ELPlayer mp = Main.LocalPlayer.GetModPlayer<ELPlayer>();
        bool IsWithinRange(int value, int inclusiveLowerBound, int exclusiveUpperBound) => value >= inclusiveLowerBound && value < exclusiveUpperBound;
        
        if (!IsWithinRange(firstArg, 0, mp.loadouts.Count) || 
            (args.Length == 2 && !IsWithinRange(secondArg, 0, mp.loadouts.Count))) {
            Main.NewText($"Invalid Format: Loadout indexes cannot be smaller than 0 or bigger than {mp.loadouts.Count - 1}", Color.Red);
            return;
        }
        
        // No point in swapping if both are equal
        if (firstArg == secondArg)
            return;
        
        // Save whatever's the current loadout
        mp.loadouts[mp.loadoutIndex].SaveLoadout();
        
        if (args.Length == 2) {
            var temp = mp.loadouts[firstArg];
            mp.loadouts[firstArg] = mp.loadouts[secondArg];
            mp.loadouts[secondArg] = temp;
            
            Main.NewText($"Loadouts {firstArg} and {secondArg} were successfully swapped", Color.Green);
        } else {
            var temp = mp.loadouts[mp.loadoutIndex];
            mp.loadouts[mp.loadoutIndex] = mp.loadouts[firstArg];
            mp.loadouts[firstArg] = temp;
            
            Main.NewText($"Loadouts {mp.loadoutIndex} and {firstArg} were successfully swapped", Color.Green);
        }
        
        // Load current loadout again, in case it has changed
        mp.loadouts[mp.loadoutIndex].LoadLoadout();
    }
}