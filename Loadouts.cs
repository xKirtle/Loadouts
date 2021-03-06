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

            if (!Main.dedServ && Main.netMode != NetmodeID.Server)
            {
                BaseUIState = new BaseUIState();
                BaseUIState.Activate();
                BaseUserInterface = new UserInterface();
                BaseUserInterface.SetState(BaseUIState);
            }
        }

        public override void Unload()
        {
            BaseUserInterface = null;
            BaseUIState = null;
            leftArrow = null;
            rightArrow = null;
        }

        private GameTime _lastUpdateUiGameTime;
        internal UserInterface BaseUserInterface;
        internal BaseUIState BaseUIState;
        public override void UpdateUI(GameTime gameTime)
        {
            _lastUpdateUiGameTime = gameTime;
            if (BaseUserInterface?.CurrentState != null)
                BaseUserInterface.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            //https://github.com/tModLoader/tModLoader/wiki/Vanilla-Interface-layers-values
            int interfaceLayer = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
            if (interfaceLayer != -1)
            {
                layers.Insert(interfaceLayer, new LegacyGameInterfaceLayer(
                    "Loadouts: Base UI",
                    delegate
                    {
                        if (_lastUpdateUiGameTime != null && BaseUserInterface?.CurrentState != null)
                            BaseUserInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);

                        return true;
                    },
                       InterfaceScaleType.UI));
            }
        }
    }
}