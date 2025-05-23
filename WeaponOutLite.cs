using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using WeaponOutLite.ID;
using WeaponOutLite.Common.GlobalDrawItemPose;
using System;
using Terraria.DataStructures;
using System.Collections.Specialized;

namespace WeaponOutLite
{
    /// <summary>
    /// WeaponOutLayerRenderer controls drawing on DrawLayer. Selects a pose (MultiP) based on the style (SingleP)
    /// WeaponOutPlayerRenderer controls Item style
    /// Config for single and multiplayer
    /// Mod Calls for specifying items with custom style, and custom properties
    /// </summary>
    partial class WeaponOutLite : Mod
    {
        // TODO: Mirsario — Today at 16:41 I should note that overhaul currently changes the swing animation, and the logistic part of it too.The swing only lasts half of the use time, so check with it.
        // TODO: visual not saved on exiting world (but not game)
        internal const bool DEBUG_MULTIPLAYER = false;
        internal const bool DEBUG_TEST_ITEMS = false; 
        internal const bool DEBUG_EXPERIMENTAL = false;
        internal static string TEXT_DEBUG = null;

        private static WeaponOutLite instance;

        // Mod Integrations ♥
        public static bool TerrariaOverhaulModLoaded { get { return ModLoader.HasMod("TerrariaOverhaul"); } }
        public static bool ItemCustomizerModLoaded { get { return false; } }
        public static bool MeleeEffectsPlusModLoaded { get { return ModLoader.HasMod("MeleeWeaponEffects") || ModLoader.HasMod("MeleeEffects"); } }
        public static bool ArmamentDisplayLiteModLoaded { get { return ModLoader.HasMod("WeaponDisplayLite"); } }

        /// <summary>
        /// Hook for mods to handle predraw for the default style. Return false to override the normal draw code (replace with custom style)
        /// </summary>
        internal List<Func<Player, Item, DrawData, float, float, int, int, DrawData>> customPreDrawDataFuncs;

        /// <summary>
        /// Hook for mods to affect the behaviour of StyleCustom's drawing layer
        /// </summary>
        internal List<Func<Player, Item, short, int, short>> customDrawDepthFuncs;

        /// <summary>
        /// Hook for mods to affect the behaviour of StyleCustom's idle body frame
        /// </summary>
        internal List<Func<Player, Item, int, int, int>> customUpdateIdleBodyFrameFuncs;

        /// <summary>
        /// List of item ids that should prioritise a pose group, used in PoseSetClassifier before other selections
        /// This is for setting up items that don't work with the standard automatic style selection
        /// </summary>
        internal HybridDictionary customItemHoldGroups;

        /// <summary>
        /// List of item ids that should prioritise a pose, used in PoseSetClassifier before other selections
        /// This is for advanced mod integrations looking to specify preferences/behaviours on specific items
        /// </summary>
        internal HybridDictionary customItemHoldPose;

        /// <summary>
        /// List of item ids that should use the custom style
        /// </summary>
        internal HashSet<int> customItemHoldStyles;

        public static WeaponOutLite GetMod() {
            return instance;
        }

        /// <summary>
        /// Styles mapped to IDs for Net Code syncing
        /// </summary>
        public Dictionary<int, IDrawItemPose> DrawStyle = new Dictionary<int, IDrawItemPose>();

        public override void Load() {
            instance = this;
            DrawStyle = DrawItemPoseID.LoadPoses();

            customPreDrawDataFuncs = new List<Func<Player, Item, DrawData, float, float, int, int, DrawData>>();
            customDrawDepthFuncs = new List<Func<Player, Item, short, int, short>>();
            customUpdateIdleBodyFrameFuncs = new List<Func<Player, Item, int, int, int>>();
            customItemHoldGroups = new HybridDictionary();
            customItemHoldPose = new HybridDictionary();
            customItemHoldStyles = new HashSet<int>();

            Common.WeaponOutLayerRenderer.Load();

            // Moved to UIDIsplay
            //On.Terraria.Main.DrawInventory += OnDrawInventory;
        }

        public override void Unload() {
            instance = null;
            DrawStyle = null;

            customPreDrawDataFuncs = null;
            customDrawDepthFuncs = null;
            customUpdateIdleBodyFrameFuncs = null;
            customItemHoldGroups = new HybridDictionary();
            customItemHoldPose = new HybridDictionary();
            customItemHoldStyles = null;

            Common.WeaponOutLayerRenderer.Unload();

            // Moved to UIDisplay
            //On.Terraria.Main.DrawInventory -= OnDrawInventory;
        }

        // Moved to UIDisplay
        //private void OnDrawInventory(On.Terraria.Main.orig_DrawInventory orig, Main self) {
        //    orig(self);
        //}

    }
}