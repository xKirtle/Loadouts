using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader.IO;

namespace Loadouts.Utils
{
    class Loadout : TagSerializable
    {
        public static readonly Func<TagCompound, Loadout> DESERIALIZER = Load;

        public List<Item> armor;
        public List<Item> vArmor;
        public List<Item> accessories;
        public List<Item> vAccessories;
        public List<Item> miscEquips;
        public List<Item> dyes;

        void Init()
        {
            armor = new List<Item>(3);
            vArmor = new List<Item>(3);
            accessories = new List<Item>(7);
            vAccessories = new List<Item>(7);
            miscEquips = new List<Item>(5);
            dyes = new List<Item>(15);
        }

        public Loadout(bool firstLoadout = false)
        {
            Init();

            ArrayOfEmptyItems(armor);
            ArrayOfEmptyItems(vArmor);
            ArrayOfEmptyItems(accessories);
            ArrayOfEmptyItems(vAccessories);
            ArrayOfEmptyItems(miscEquips);
            ArrayOfEmptyItems(dyes);

            if (firstLoadout)
                SaveLoadout();

            void ArrayOfEmptyItems(List<Item> list)
            {
                for (int i = 0; i < list.Capacity; i++)
                    list.Add(new Item());
            }
        }

        public void SaveLoadout()
        {
            Player player = Main.LocalPlayer;

            int maxAccessoryIndex = 5 + player.GetAmountOfExtraAccessorySlotsToShow();

            for (int i = 0; i < 3; i++)
                armor[i] = player.armor[i];

            for (int i = 10; i < 13; i++)
                vArmor[i - 10] = player.armor[i];

            for (int i = 3; i < 3 + maxAccessoryIndex; i++)
                accessories[i - 3] = player.armor[i];

            for (int i = 13; i < 13 + maxAccessoryIndex; i++)
                vAccessories[i - 13] = player.armor[i];

            for (int i = 0; i < 5; i++)
                miscEquips[i] = player.miscEquips[i];

            for (int i = 0; i < 15; i++)
                dyes[i] = (i < 10) ? player.dye[i] : player.miscDyes[i - 10];
        }

        public void LoadLoadout()
        {
            Player player = Main.LocalPlayer;

            int maxAccessoryIndex = 5 + player.GetAmountOfExtraAccessorySlotsToShow();

            for (int i = 0; i < 3; i++)
                player.armor[i] = armor[i];

            for (int i = 10; i < 13; i++)
                player.armor[i] = vArmor[i - 10];

            for (int i = 3; i < 3 + maxAccessoryIndex; i++)
                player.armor[i] = accessories[i - 3];

            for (int i = 13; i < 13 + maxAccessoryIndex; i++)
                player.armor[i] = vAccessories[i - 13];

            for (int i = 0; i < 5; i++)
                player.miscEquips[i] = miscEquips[i];

            for (int i = 0; i < 10; i++)
                player.dye[i] = dyes[i];

            for (int i = 0; i < 5; i++)
                player.miscDyes[i] = dyes[i + 10];
        }

        public TagCompound SerializeData()
        {
            return new TagCompound
            {
                ["armor"] = armor,
                ["vArmor"] = vArmor,
                ["accessories"] = accessories,
                ["vAccessories"] = vAccessories,
                ["miscEquips"] = miscEquips,
                ["dyes"] = dyes
            };
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
            return loadout;
        }
    }
}
