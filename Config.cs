using System.ComponentModel;
using Terraria.ModLoader.Config;
using Loadouts.UI;

namespace Loadouts
{
    class Config : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Label("Loadouts auto save interval in minutes")]
        [Range(1, 30)]
        [DefaultValue(3)]
        [Increment(1)]
        public int saveInterval;

        void UpdateInterval()
        {
            ELPlayer.saveInterval = saveInterval;
        }

        public override void OnLoaded() => UpdateInterval();
        public override void OnChanged() => UpdateInterval();
    }
}