using Terraria;
using Terraria.ModLoader;

namespace WeaponOutLite.Compatibility
{
    /// <summary>
    /// Thorium Mod
    /// https://steamcommunity.com/sharedfiles/filedetails/?id=2909886416
    /// </summary>
    internal static class ThoriumMod
    {
        public static bool Found => ModLoader.HasMod("ThoriumMod");

        /// <summary>
        /// Call this in PostSetupContent for default weapon values.
        /// </summary>
        public static void PostSetupContent()
        {
            if (!Found) return;

            WeaponOutLite mod = WeaponOutLite.GetMod();
            Mod src = ModLoader.GetMod("ThoriumMod");

            var modItems = src.GetContent<ModItem>();
            foreach (var i in modItems)
            {
                switch (i.Name)
                {
                    case "GoldenLocks": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.Ignore); break;
                    case "SoulReaver": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.Spear); break;
                    case "TerrariumSpear": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.Spear); break;
                    case "HellishHalberd": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.LargeMelee); break;
                    case "LingeringWill": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.LargeMelee); break;
                    case "EssenceofFlame": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.LargeMelee); break;

                    case "Purify": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.MagicBook); break;
                    case "Stun":
                    case "Siphon":
                    case "Poison":
                    case "Pierce":
                    case "Ignite":
                    case "Freeze":
                    case "Dissolve":
                    case "Charm":
                    case "Confuse":
                        mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.MagicItem); break;

                    case "MagicConch": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.Item); break;
                    case "HeavensGate": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.SmallMelee); break;
                    case "DynastyWarFan": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.ThrownThin); break;
                    case "KineticKnife": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.ThrownThin); break;
                    case "SnowWhite": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.Rapier); break;
                    
                    case "WitherStaff":
                    case "UselessStaff":
                    case "AncientSpark": 
                        mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.Staff); break;

                    case "GeomancersBrush": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.SmallTool); break;
                    case "OmegaBlaster": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.Launcher); break;
                    
                    case "ChargedSplasher": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.PowerTool); break;
                    
                    case "MastersLibram": mod.compatibilityItemHoldGroups.Add(i.Item.type, ID.PoseStyleID.PoseGroup.MagicItem); break;

                }
            }
        }
    }
}
