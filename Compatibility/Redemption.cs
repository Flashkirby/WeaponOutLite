using Terraria;
using Terraria.ModLoader;

namespace WeaponOutLite.Compatibility
{
    /// <summary>
    /// Mod of Redemption 
    /// https://steamcommunity.com/sharedfiles/filedetails/?id=2893332653
    /// </summary>
    internal static class Redemption
    {
        public static bool Found => ModLoader.HasMod("Redemption");

        /// <summary>
        /// Call this in PostSetupContent for default weapon values.
        /// </summary>
        public static void PostSetupContent()
        {
            if (!Found) return;

            WeaponOutLite mod = WeaponOutLite.GetMod();
            Mod src = ModLoader.GetMod("Redemption");

            var modItems = src.GetContent<ModItem>();
            foreach (var i in modItems)
            {
                switch (i.Name)
                {
                    case "XenoXyston": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.Spear); break;
                    case "PureIronSword": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.LargeMelee); break;
                    case "DragonCleaver": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.LargeMelee); break;
                    case "SlayerFist": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.Ignore); break;
                    case "SunInThePalm": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.Ignore); break;
                    case "HydrasMaw": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.GiantGun); break;
                    case "TeslaCoil": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.Staff); break;
                    case "EaglecrestGlove": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.Ignore); break;
                    case "PulseBlade": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.Ignore); break;
                    case "HyperTechRevolvers": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.Pistol); break;
                    case "AndroidHologram": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.MagicItem); break;
                }
            }
        }
    }
}
