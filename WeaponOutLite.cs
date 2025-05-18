using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using WeaponOutLite.ID;
using WeaponOutLite.Common.GlobalDrawItemPose;
using System;
using Terraria.DataStructures;
using static WeaponOutLite.ID.PoseStyleID;
using static WeaponOutLite.ID.DrawItemPoseID;
using WeaponOutLite.Common.Configs;

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
        internal const bool DEBUG_MULTIPLAYER = false;
        internal const bool DEBUG_TEST_ITEMS = false; 
        internal const bool DEBUG_EXPERIMENTAL = false;
        internal static string TEXT_DEBUG = null;

        private static WeaponOutLite instance;

        internal static WeaponOutClientConfig ClientConfig;
        internal static WeaponOutClientHoldOverride ClientHoldOverride;
        internal static WeaponOutServerConfig ServerConfig;

        // Mod Integrations ♥
        // Migrated to WeaponOutLite.Compatibility namespace

        /// <summary>
        /// Hash of item ids that should use the custom style - if an item type is contained here it skips normal pose classification.
        /// This can be modified by any mod via the RegisterCustomItemStyle method exposed in WeaponOutLite.ModCalls.cs
        /// </summary>
        internal HashSet<int> customItemHoldStyles;

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
        internal Dictionary<int, PoseGroup> priorityItemHoldGroups;

        /// <summary>
        /// For mod compatibility, we can manually assign styles from this mod. 
        /// This is the same as priorityItemHoldGroups, but is lower priority
        /// This can still be overwritten by priorityItemHoldGroups, which mod authors have access to via ModCalls.
        /// </summary>
        internal Dictionary<int, PoseGroup> compatibilityItemHoldGroups;

        /// <summary>
        /// List of item ids that should prioritise a pose, used in PoseSetClassifier before other selections
        /// This is for advanced mod integrations looking to specify preferences/behaviours on specific items.
        /// </summary>
        internal Dictionary<int, DrawItemPose> priorityItemHoldPose;

        public static WeaponOutLite GetMod() {
            return instance;
        }

        /// <summary>
        /// Styles mapped to IDs for Net Code syncing
        /// </summary>
        public Dictionary<int, IDrawItemPose> ItemPoses = new Dictionary<int, IDrawItemPose>();

        public override void Load() {
            instance = this;
            ItemPoses = DrawItemPoseID.LoadPoses();

            customItemHoldStyles = new HashSet<int>();
            customPreDrawDataFuncs = new List<Func<Player, Item, DrawData, float, float, int, int, DrawData>>();
            customDrawDepthFuncs = new List<Func<Player, Item, short, int, short>>();
            customUpdateIdleBodyFrameFuncs = new List<Func<Player, Item, int, int, int>>();

            priorityItemHoldGroups = new Dictionary<int, PoseGroup>();
            compatibilityItemHoldGroups = new Dictionary<int, PoseGroup>();
            priorityItemHoldPose = new Dictionary<int, DrawItemPose>();

            Common.WeaponOutLayerRenderer.Load();
        }

        public override void Unload() {
            instance = null;
            ClientConfig = null;
            ClientHoldOverride = null;
            ServerConfig = null;

            ItemPoses = null;

            customItemHoldStyles = null;
            customPreDrawDataFuncs = null;
            customDrawDepthFuncs = null;
            customUpdateIdleBodyFrameFuncs = null;

            priorityItemHoldGroups = null;
            compatibilityItemHoldGroups = null;
            priorityItemHoldPose = null;

            Common.WeaponOutLayerRenderer.Unload();
        }

    }
}