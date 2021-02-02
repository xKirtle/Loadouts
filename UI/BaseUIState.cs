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
    public class BaseUIState : UIState
    {
        public static Menu menu; 
        public override void OnInitialize()
        {
            menu = new Menu();
            Append(menu);
            menu.Hide();
        }

        public override void Update(GameTime gameTime)
        {
            if (Main.playerInventory)
            {
                menu.Show();
                menu.Update();
            }
            else
            {
                menu.Hide();
            }
        }
    }
}