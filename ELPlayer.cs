using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Loadouts.Utils;
using Terraria.GameInput;
using Loadouts.UI;
using Microsoft.Xna.Framework;
using Terraria.ObjectData;

namespace Loadouts
{
    class ELPlayer : ModPlayer
    {
        public List<Loadout> loadouts = null;
        public int loadoutIndex;
        public Vector2 menuOffset;
        public static int saveInterval; //in minutes
        public override void Initialize()
        {
            loadouts = new List<Loadout>() { new Loadout(true) };
            loadoutIndex = 0;
            menuOffset = new Vector2(275f, 255f);
            saveInterval = 3;
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Loadouts.leftArrow.JustPressed) Menu.ButtonsClick(1);
            if (Loadouts.rightArrow.JustPressed) Menu.ButtonsClick(2);
        }

        public override void OnEnterWorld(Player player)
        {
            // if (loadouts.Count == 0)
            //     loadouts.Add(new Loadout(true));
            
            BaseUIState.menu.Left.Set(menuOffset.X, 0);
            BaseUIState.menu.Top.Set(menuOffset.Y, 0);
        }

        // int timer = 0;
        public override void PostUpdate()
        {
            // if (timer++ == saveInterval * 60 * 60)
            // {
            //     loadouts?[loadoutIndex].SaveLoadout();
            //     timer = 0;
            // }
        }

        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey("loadouts"))
                loadouts = tag.Get<List<Loadout>>("loadouts");

            if (tag.ContainsKey("loadoutIndex"))
                loadoutIndex = tag.GetInt("loadoutIndex");
            
            if (tag.ContainsKey("menuOffset"))
                menuOffset = tag.Get<Vector2>("menuOffset");
        }

        public override void SaveData(TagCompound tag)
        {
            tag["loadouts"] = loadouts;
            tag["loadoutIndex"] = loadoutIndex;
            tag["menuOffset"] = menuOffset;
        }
    }
}
