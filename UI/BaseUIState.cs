using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Loadouts.Commands;
using Loadouts.Utils;
using Terraria.Audio;
using Terraria.ID;

namespace Loadouts.UI
{
    class BaseUIState : UIState
    {
        private UIImage background;
        private UIText loadoutText;
        private UIImage[] elements;
        private string[] names = new string[] {"Minus", "Left", "Right", "Plus"};
        private int[] offset = new int[] {4, 38, 96, 130};

        //Might want to make this a ModConfig variable?
        public static readonly int maxLoadouts = 10;
        public static int uiPosConfig;
        public override void OnInitialize() => CreateLayout();

        void CreateLayout()
        {
            elements = new UIImage[4];
            const string texturePath = "Loadouts/Textures/";

            background = new UIImage(ModContent.GetTexture(texturePath + "Background"))
            {
                Width = {Pixels = 162},
                Height = {Pixels = 36},
                Left = {Pixels = Main.screenWidth - 165f},
                Top = {Pixels = Main.screenHeight / 2f + 350f}
            };

            for (int i = 0; i < elements.Length; i++)
            {
                int index = i;
                UIImage temp = new UIImage(ModContent.GetTexture(texturePath + names[index]))
                {
                    Width = {Pixels = 28},
                    Height = {Pixels = 28},
                    Left = {Pixels = offset[index]},
                    Top = {Pixels = 4}
                };
                temp.OnClick += (__, _) => Click(index);
                temp.OnMouseOver += (__, _) => Hover(index, true);
                temp.OnMouseOut += (__, _) => Hover(index, false);
                background.Append(temp);

                elements[index] = temp;
            }

            loadoutText = new UIText("")
            {
                Width = {Pixels = 12},
                Height = {Pixels = 18},
                Left = {Pixels = 75},
                Top = {Pixels = 9}
            };
            background.Append(loadoutText);
            Append(background);
        }

        private void Hover(int index, bool mouseOver)
        {
            string texturePath = "Loadouts/Textures/" + names[index];
            if (mouseOver)
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
                texturePath += "Hover";
            }

            elements[index].SetImage(ModContent.GetTexture(texturePath));
        }

        public static void Click(int index)
        {
            ELPlayer mp = Main.LocalPlayer.GetModPlayer<ELPlayer>();
            //Don't want the delete request to be on hold if player clicks somewhere else
            if (index != 0) ConfirmDelete.deleteRequest = false;

            switch (index)
            {
                case 0: //Minus
                    if (!ConfirmDelete.deleteRequest && mp.loadouts.Count > 1)
                    {
                        SoundEngine.PlaySound(SoundID.MenuTick);
                        Main.NewText(
                            "Are you sure you want to remove this loadout? Every item in the loadout will be deleted. Type \"/confirm\" to remove it.",
                            Color.Red);
                        ConfirmDelete.deleteRequest = true;
                    }

                    break;
                case 1: //Left
                    if (mp.loadoutIndex > 0)
                    {
                        SoundEngine.PlaySound(SoundID.MenuTick);
                        mp.loadouts[mp.loadoutIndex].SaveLoadout();
                        mp.loadouts[--mp.loadoutIndex].LoadLoadout();
                    }

                    break;
                case 2: //Right
                    if (mp.loadoutIndex < mp.loadouts.Count - 1)
                    {
                        SoundEngine.PlaySound(SoundID.MenuTick);
                        mp.loadouts[mp.loadoutIndex].SaveLoadout();
                        mp.loadouts[++mp.loadoutIndex].LoadLoadout();
                    }

                    break;
                case 3: //Plus
                    if (mp.loadouts.Count < maxLoadouts)
                    {
                        SoundEngine.PlaySound(SoundID.MenuTick);
                        mp.loadouts[mp.loadoutIndex].SaveLoadout();

                        Loadout loadout = new Loadout();
                        mp.loadouts.Add(loadout);
                        mp.loadoutIndex = mp.loadouts.Count - 1;

                        mp.loadouts[mp.loadoutIndex].LoadLoadout();
                    }
                    
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Main.playerInventory)
            {
                if (background == null) CreateLayout();
                else if (background.IsMouseHovering) Main.LocalPlayer.mouseInterface = true;

                loadoutText.SetText(Main.LocalPlayer.GetModPlayer<ELPlayer>().loadoutIndex.ToString());

                if (uiPosConfig == 0)
                {
                    background.Left.Set(Main.screenWidth - 165f, 0);
                    float offsetY = Main.LocalPlayer.extraAccessory && Main.expertMode ? 155f : 110f;

                    if (Main.mapStyle == 1) offsetY += 255f;
                    background.Top.Set(offsetY, 0);
                }
                else if (uiPosConfig == 1)
                {
                    background.Left.Set(280f, 0);
                    background.Top.Set(265f, 0);
                }
            }
            else
            {
                background?.Remove();
                background = null;
            }
        }
    }
}