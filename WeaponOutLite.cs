using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using WeaponOutLite.ID;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.Content.DrawItemPose;
using Microsoft.Xna.Framework;
using WeaponOutLite.Common.Players;
using Terraria.ID;
using Terraria.Audio;
using Terraria.GameContent;
using ReLogic.Graphics;
using WeaponOutLite.Common.Configs;
using System;
using Terraria.DataStructures;
using System.Collections.Specialized;
using Terraria.UI;

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

        public bool TerrariaOverhaulModLoaded = false;

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
        /// This is for setting up items that don't work with the standard automatic style seletion
        /// </summary>
        internal HybridDictionary customItemHoldGroups;

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
            customItemHoldStyles = new HashSet<int>();

            Common.WeaponOutLayerRenderer.ItemProjTextureCache = new List<Microsoft.Xna.Framework.Graphics.Texture2D>();

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
            customItemHoldStyles = null;

            Common.WeaponOutLayerRenderer.ItemProjTextureCache = null;

            // Moved to UIDisplay
            //On.Terraria.Main.DrawInventory -= OnDrawInventory;
        }

        // Moved to UIDisplay
        //private void OnDrawInventory(On.Terraria.Main.orig_DrawInventory orig, Main self) {
        //    orig(self);
        //}

    }
}