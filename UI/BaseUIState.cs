using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Loadouts.Commands;
using Loadouts.Utils;

namespace Loadouts.UI
{
    class BaseUIState : UIState
    {
        public UIImage loadoutBar;
        public UIText loadoutText;
        //Might want to make this a ModConfig variable?
        private readonly int maxLoadouts = 10;
        public override void OnInitialize() => CreateLayout();

        void CreateLayout()
        {
            loadoutBar = new UIImage(ModContent.GetTexture("Loadouts/Textures/LoadoutBar"));
            loadoutBar.Width.Set(98f, 0);
            loadoutBar.Height.Set(31f, 0);
            loadoutBar.Left.Set(Main.screenWidth - 165f, 0);
            loadoutBar.Top.Set(Main.screenHeight / 2f + 350f, 0);
            loadoutBar.OnMouseDown += (element, listener) =>
            {
                Vector2 barPos = new Vector2(loadoutBar.Left.Pixels, loadoutBar.Top.Pixels);
                Vector2 clickPos = Vector2.Subtract(element.MousePosition, barPos);

                if (clickPos.X > 5f && clickPos.X < 95f && clickPos.Y > 4f && clickPos.Y < 27f)
                {
                    if (clickPos.X <= 40f && clickPos.X <= 25f) Click(0);       //Minus
                    else if (clickPos.X <= 40f && clickPos.X >= 25f) Click(1);  //Left
                    else if (clickPos.X >= 60f && clickPos.X <= 75f) Click(2);  //Right
                    else if (clickPos.X >= 60f && clickPos.X >= 75f) Click(3);  //Plus
                }
            };

            loadoutText = new UIText("");
            loadoutText.Left.Set(45f, 0);
            loadoutText.Top.Set(6f, 0);

            loadoutBar.Append(loadoutText);
            Append(loadoutBar);
        }

        void Click(int index)
        {
            ELPlayer mp = Main.LocalPlayer.GetModPlayer<ELPlayer>();
            //Don't want the delete request to be on hold if player clicks somewhere else
            if (index != 0)
                ConfirmDelete.deleteRequest = false;

            switch (index)
            {
                case 0: //Minus
                    if (!ConfirmDelete.deleteRequest && mp.loadouts.Count > 1)
                    {
                        Main.NewText("Are you sure you want to remove this loadout? Every item in the loadout will be deleted. Type \"/confirm\" to remove it.", Color.Red);
                        ConfirmDelete.deleteRequest = true;
                    }
                    break;
                case 1: //Left
                    if (mp.loadoutIndex > 0)
                    {
                        mp.loadouts[mp.loadoutIndex].SaveLoadout();
                        mp.loadouts[--mp.loadoutIndex].LoadLoadout();
                    }
                    break;
                case 2: //Right
                    if (mp.loadoutIndex < mp.loadouts.Count - 1)
                    {
                        mp.loadouts[mp.loadoutIndex].SaveLoadout();
                        mp.loadouts[++mp.loadoutIndex].LoadLoadout();
                    }
                    break;
                case 3: //Plus
                    if (mp.loadouts.Count < maxLoadouts)
                    {
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
                if (loadoutBar == null) CreateLayout();
                else if (loadoutBar.IsMouseHovering) Main.LocalPlayer.mouseInterface = true;

                loadoutText.SetText(Main.LocalPlayer.GetModPlayer<ELPlayer>().loadoutIndex.ToString());
                loadoutBar.Left.Set(Main.screenWidth - 165f, 0);
                float offsetY = Main.LocalPlayer.extraAccessory && Main.expertMode ? 155f : 110f;

                if (Main.mapStyle == 1) offsetY += 255f;
                loadoutBar.Top.Set(offsetY, 0);
            }
            else { loadoutBar?.Remove(); loadoutBar = null; }
        }
    }
}
