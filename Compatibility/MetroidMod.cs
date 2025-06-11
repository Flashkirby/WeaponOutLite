using System;
using Terraria.ModLoader;

namespace WeaponOutLite.Compatibility
{
    /// <summary>
    /// Metroid Mod
    /// https://steamcommunity.com/sharedfiles/filedetails/?id=2984059720
    /// </summary>
    internal static class MetroidMod
    {
        public static bool Found => ModLoader.HasMod("MetroidMod");

        internal static void PostSetupContent()
        {
            if (!Found) return;

            WeaponOutLite mod = WeaponOutLite.GetMod();
            Mod src = ModLoader.GetMod("MetroidMod");

            var modItems = src.GetContent<ModItem>();
            foreach (var i in modItems)
            {
                switch (i.Name)
                {
                    case "ArmCannon": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.Ignore); break;
                }
            }
        }
    }
}
