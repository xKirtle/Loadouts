using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Loadouts.Commands
{
    class ConfirmDelete : ModCommand
    {
        public static bool deleteRequest;
        public override string Command => "confirm";
        public override string Description => "Deletes a loadout profile";
        public override CommandType Type => CommandType.Chat;

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (deleteRequest)
            {
                ELPlayer mp = Main.LocalPlayer.GetModPlayer<ELPlayer>();
                mp.loadouts.RemoveAt(mp.loadoutIndex);
                mp.loadoutIndex -= mp.loadoutIndex < mp.loadouts.Count ? 0 : 1;
                mp.loadouts[mp.loadoutIndex].LoadLoadout();
                deleteRequest = false;

                Main.NewText("Loadout removed succesfully.", Color.Green);
            }

        }
    }
}