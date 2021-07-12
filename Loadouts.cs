using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.UI;
using Terraria.ModLoader;
using Loadouts.UI;
using Microsoft.Xna.Framework;

namespace Loadouts
{
    public class Loadouts : Mod
    {
        internal static ModKeybind leftArrow;
        internal static ModKeybind rightArrow;
        public override void Load()
        {
            leftArrow = KeybindLoader.RegisterKeybind(this, "Decrease loadouts index", "Left");
            rightArrow = KeybindLoader.RegisterKeybind(this, "Increase loadouts index", "Right");
        }

        public override void Unload()
        {
            leftArrow = null;
            rightArrow = null;
        }
    }
}