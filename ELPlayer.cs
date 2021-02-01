using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Loadouts.Utils;
using Terraria.GameInput;
using Loadouts.UI;
using Terraria.ObjectData;

namespace Loadouts
{
    class ELPlayer : ModPlayer
    {
        public List<Loadout> loadouts = null;
        public int loadoutIndex;
        //Might want to make this a ModConfig variable?
        public int saveInterval; //in minutes
        public override void Initialize()
        {
            loadouts = new List<Loadout>();
            loadoutIndex = 0;
            saveInterval = 3;
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Loadouts.leftArrow.JustPressed) BaseUIState.Click(1);
            if (Loadouts.rightArrow.JustPressed) BaseUIState.Click(2);
        }

        public override void OnEnterWorld(Player player)
        {
            if (loadouts.Count == 0)
                loadouts.Add(new Loadout(true));
        }

        int timer = 0;
        public override void PostUpdate()
        {
            if (timer++ == saveInterval * 60 * 60)
            {
                loadouts?[loadoutIndex].SaveLoadout();
                timer = 0;
            }
        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                { "loadouts", loadouts },
                { "loadoutIndex", loadoutIndex }
            };
        }

        public override void Load(TagCompound tag)
        {
            if (tag.ContainsKey("loadouts"))
                loadouts = tag.Get<List<Loadout>>("loadouts");

            if (tag.ContainsKey("loadoutIndex"))
                loadoutIndex = tag.GetInt("loadoutIndex");
        }
    }
}
