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
        internal static ModHotKey leftArrow;
        internal static ModHotKey rightArrow;
        public override void Load()
        {
            leftArrow = RegisterHotKey("Decrease loadouts index", "Left");
            rightArrow = RegisterHotKey("Increase loadouts index", "Right");

            UIModSystem.Load();
        }

        public override void Unload()
        {
            UIModSystem.Unload();
            leftArrow = null;
            rightArrow = null;
        }
    }
}