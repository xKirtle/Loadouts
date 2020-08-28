using System.ComponentModel;
using Terraria.ModLoader.Config;
using Loadouts.UI;

namespace Loadouts
{
    class Config : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [OptionStrings(new string[] { "Above accessories", "Below inventory" })]
        [DefaultValue("Above accessories")]
        public string UIPosition;

        void DoSomething()
        {
            if (UIPosition == "Above accessories") BaseUIState.uiPosConfig = 0;
            else if (UIPosition == "Below inventory") BaseUIState.uiPosConfig = 1;
        }

        public override void OnLoaded() => DoSomething();
        public override void OnChanged() => DoSomething();
    }
}
