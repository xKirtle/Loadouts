using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader.IO;

namespace Loadouts.Utils
{
    public class Loadout : TagSerializable
    {
        public static readonly Func<TagCompound, Loadout> DESERIALIZER = Load;
        private const int maxAccessorySlots = 7;

        public List<Item> armor;
        public List<Item> vArmor;
        public List<Item> accessories;
        public List<Item> vAccessories;
        public List<Item> miscEquips;
        public List<Item> dyes;

        //Mod compatibility stuff (null if mod is unloaded)
        public List<Item> wingSlot;

        public Loadout(bool firstLoadout = false)
        {
            armor = Enumerable.Repeat(new Item(), 3).ToList();
            vArmor = Enumerable.Repeat(new Item(), 3).ToList();
            accessories = Enumerable.Repeat(new Item(), 7).ToList();
            vAccessories = Enumerable.Repeat(new Item(), 7).ToList();
            miscEquips = Enumerable.Repeat(new Item(), 5).ToList();
            dyes = Enumerable.Repeat(new Item(), 15).ToList();

            if (firstLoadout)
                SaveLoadout();
        }

        public void SaveLoadout()
        {
            Player player = Main.LocalPlayer;

            for (int i = 0; i < 3; i++)
                armor[i] = player.armor[i];

            for (int i = 10; i < 13; i++)
                vArmor[i - 10] = player.armor[i];

            for (int i = 3; i < 3 + maxAccessorySlots; i++)
                accessories[i - 3] = player.armor[i];

            for (int i = 13; i < 13 + maxAccessorySlots; i++)
                vAccessories[i - 13] = player.armor[i];

            for (int i = 0; i < 5; i++)
                miscEquips[i] = player.miscEquips[i];

            for (int i = 0; i < 15; i++)
                dyes[i] = (i < 10) ? player.dye[i] : player.miscDyes[i - 10];

            // if (Loadouts.wingSlot != null) 
            // {
            //     wingSlot.Add((Item)Loadouts.wingSlot.Call("getVanity", player.whoAmI));
            //     wingSlot.Add((Item)Loadouts.wingSlot.Call("getEquip", player.whoAmI));
            // }

            WorldGen.saveToonWhilePlaying();
            WorldGen.saveAndPlay();
        }

        public void LoadLoadout()
        {
            Player player = Main.LocalPlayer;

            for (int i = 0; i < 3; i++)
                player.armor[i] = armor[i];

            for (int i = 10; i < 13; i++)
                player.armor[i] = vArmor[i - 10];

            for (int i = 3; i < 3 + accessories.Count; i++)
                player.armor[i] = accessories[i - 3];

            for (int i = 13; i < 13 + vAccessories.Count; i++)
                player.armor[i] = vAccessories[i - 13];

            for (int i = 0; i < 5; i++)
                player.miscEquips[i] = miscEquips[i];

            for (int i = 0; i < 10; i++)
                player.dye[i] = dyes[i];

            for (int i = 0; i < 5; i++)
                player.miscDyes[i] = dyes[i + 10];

            // if (Loadouts.wingSlot != null)
            // {
            //     Loadouts.wingSlot.Call("setVanity", wingSlot[0], player.whoAmI);
            //     Loadouts.wingSlot.Call("setEquip", wingSlot[1], player.whoAmI);
            // }
        }

        public TagCompound SerializeData()
        {
            TagCompound save = new TagCompound
            {
                {"armor", armor},
                {"vArmor", vArmor},
                {"accessories", accessories},
                {"vAccessories", vAccessories},
                {"miscEquips", miscEquips},
                {"dyes", dyes}
            };

            if (Loadouts.wingSlot != null)
                save.Add("wingSlot", wingSlot);

            return save;
        }

        public static Loadout Load(TagCompound tag)
        {
            Loadout loadout = new Loadout
            {
                armor = tag.Get<List<Item>>("armor"),
                vArmor = tag.Get<List<Item>>("vArmor"),
                accessories = tag.Get<List<Item>>("accessories"),
                vAccessories = tag.Get<List<Item>>("vAccessories"),
                miscEquips = tag.Get<List<Item>>("miscEquips"),
                dyes = tag.Get<List<Item>>("dyes"),
            };

            if (tag.ContainsKey("wingSlot"))
                loadout.wingSlot = tag.Get<List<Item>>("wingSlot");

            return loadout;
        }
    }
}