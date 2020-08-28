using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Loadouts.Utils;

namespace Loadouts
{
    class ELPlayer : ModPlayer
    {
        public List<Loadout> loadouts = null;
        public int loadoutIndex;
        public override void Initialize()
        {
            loadouts = new List<Loadout>();
            loadoutIndex = 0;
        }

        public override void OnEnterWorld(Player player)
        {
            if (loadouts.Count == 0)
            {
                Loadout loadout = new Loadout(true);
                loadout.SaveLoadout();
                loadouts.Add(loadout);
            }
        }

        int timer = 0;
        public override void PostUpdate()
        {
            //Saves every 3min
            if (timer++ == 1800)
            {
                if (loadouts != null)
                    loadouts[loadoutIndex].SaveLoadout();
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
