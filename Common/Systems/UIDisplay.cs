using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using WeaponOutLite.ID;
using WeaponOutLite.Common.GlobalDrawItemPose;
using Microsoft.Xna.Framework;
using WeaponOutLite.Common.Players;
using Terraria.ID;
using Terraria.Audio;
using Terraria.GameContent;
using ReLogic.Graphics;
using WeaponOutLite.Common.Configs;
using System;
using Terraria.UI;
using Terraria.Localization;
using Terraria.ModLoader.Config;
using static WeaponOutLite.ID.PoseStyleID;
using static WeaponOutLite.ID.DrawItemPoseID;

namespace WeaponOutLite.Common.Systems
{
    public class UIDisplay : ModSystem
    {
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
            // Layer names come from the Main.SetupDrawInterfaceLayers method
            int inventoryIndex = layers.FindIndex(layer => layer.Name == "Vanilla: Inventory");
            if (inventoryIndex >= 0) {
                if (Main.playerInventory) {
                    var layer = new GameInterfaceLayer("WeaponOutLiteEye", InterfaceScaleType.UI);
                    layers.Insert(inventoryIndex, WeaponOutLiteDisplayItemInterface.CreateDefault());
                }
            }
        }
    }
    public class WeaponOutLiteDisplayItemInterface : GameInterfaceLayer
    {
        public WeaponOutLiteDisplayItemInterface(string name, InterfaceScaleType scaleType) : base(name, scaleType) { }

        public static WeaponOutLiteDisplayItemInterface CreateDefault() {
            return new WeaponOutLiteDisplayItemInterface("WeaponOutLite: Display Item", InterfaceScaleType.UI);
        }

        protected override bool DrawSelf() {

            UpdateEyeButton(new Vector2(21, 4));

            SHOW_DEBUG_UI();

            return true;
        }

        /// <summary>
        /// Draw the display Eye
        /// </summary>
        /// <param name="position"></param>
        private void UpdateEyeButton(Vector2 position) {
            // It would be really cool to add this as an option to BuilderAccTogglesUI like block swap, as opposed to just sitting on its own next to the inventory
            if (!WeaponOutLite.ClientConfig.EnableWeaponOutVisuals) { return; }

            // Default locked state (off)
            var texture = TextureAssets.InventoryTickOff.Value;
            var hoverText = "WeaponOut: " + Lang.inter[73]; // "Off" see en_US.Legacy

            if (!Main.LocalPlayer.TryGetModPlayer<WeaponOutPlayerRenderer>(out var modPlayer)) { return; }

            // Enabled but turned off (hidden)
            hoverText = "WeaponOut: " + Lang.inter[60]; // "Hidden"

            if (modPlayer.isShowingHeldItem)
            {

                // Enabled and turned on
                texture = TextureAssets.InventoryTickOn.Value;
                hoverText = "WeaponOut: " + Lang.inter[59]; // "Visible"

                if (modPlayer.HeldItem != null)
                {
                    // Get the group to display
                    PoseSetClassifier.GetItemPoseGroupData(Main.LocalPlayer.HeldItem, out PoseStyleID.PoseGroup currentPoseGroup, out _, out bool poseIsDefault);
                    if (!poseIsDefault)
                    {
                        hoverText += $" [{PoseStyleID.MapPoseGroupToString(currentPoseGroup)}]";
                    }
                }
            }

            // Get bounding box of texture.
            var textureRect = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            if (textureRect.Contains(Main.mouseX, Main.mouseY))
            {
                Main.hoverItemName = hoverText;
                Main.blockMouse = true;

                // On click
                if (Main.mouseLeft && Main.mouseLeftRelease)
                {
                    // Toggle
                    modPlayer.IsShowingHeldItem = !modPlayer.IsShowingHeldItem;

                    // Play "Click" sound
                    SoundEngine.PlaySound(SoundID.MenuTick);
                }
                if (Main.mouseRight && Main.mouseRightRelease)
                {
                    // Cycle to Next
                    if(UpdateWeaponOverride())
                    {
                        // Play "Click" sound for next item
                        SoundEngine.PlaySound(SoundID.MenuTick);
                    }
                    else
                    {
                        // Play "Click" sound for resetting
                        SoundEngine.PlaySound(SoundID.Unlock);
                    }
                }
            }

            Main.spriteBatch.Draw(texture, position, null, Color.White);
        }

        /// <summary>
        /// Applies the next pose group to the currently held item.
        /// </summary>
        /// <returns>true if an override is applied, and false if the override has been cleared. </returns>
        private static bool UpdateWeaponOverride()
        {
            bool isOverride = true;
            /*
             * Function for changing the custom hold style for the currently held objects...
             * WeaponOutLite.ClientHoldOverride.Save();
            */

            // To customise item hold styles: Settings -> Mod Configurations -> WeaponOut Lite: Personal Preferences
            if (!WeaponOutLite.ClientHoldOverride.EnableWeaponQuickWeaponCategorisation)
            {
                Main.NewText(
                    Language.GetTextValue("Mods.WeaponOutLite.Common.Instructions") +
                    Language.GetTextValue("LegacyMenu.14") +
                    " [g:5] " +
                    Language.GetTextValue("tModLoader.ModConfiguration") +
                    " [g:5] WeaponOut Lite: " +
                    Language.GetTextValue("Mods.WeaponOutLite.Configs.WeaponOutClientHoldOverride.DisplayName")
                    , Color.LightGray);
            }
            else
            {
                Item item = Main.LocalPlayer.HeldItem;
                var itemDefinition = new ItemDefinition(item.type);

                // Fetch the default posegroup of the item
                PoseSetClassifier.GetItemPoseGroupData(Main.LocalPlayer.HeldItem, out PoseStyleID.PoseGroup originalPoseGroup, out _, out _, allowOverride: false);

                // Use the latest item definition, or create a new unassigned one
                var match = WeaponOutLite.ClientHoldOverride.StyleOverrideList.FindLast(i => i.Item.Type == item.type);
                ItemDrawOverrideData overrideData = match;
                if (overrideData == null)
                {
                    overrideData = new ItemDrawOverrideData()
                    {
                        Item = itemDefinition,
                        ForcePoseGroup = originalPoseGroup,
                        ForceDrawItemPose = DrawItemPose.Unassigned,
                    };
                }

                // Fetch the next pose group from the current one, or loop back around to Unassigned
                List<PoseGroup> poseGroupList = Enum.GetValues(typeof(PoseGroup)).Cast<PoseGroup>().ToList();
                int indexOfPoseGroup = poseGroupList.IndexOf(overrideData.ForcePoseGroup);
                int nextPoseGroup = indexOfPoseGroup + 1;
                if (nextPoseGroup <= (int)PoseGroup.Unassigned || nextPoseGroup >= poseGroupList.Count)
                {
                    // Loop back to the start (but skip 0 which is unassigned)
                    overrideData.ForcePoseGroup = poseGroupList[1];
                }
                else
                {
                    overrideData.ForcePoseGroup = poseGroupList[nextPoseGroup];
                }

                // Revert to "unassigned" if current pose group equals the calculated type
                if (overrideData.ForcePoseGroup == originalPoseGroup)
                {
                    overrideData.ForcePoseGroup = PoseStyleID.PoseGroup.Unassigned;
                    isOverride = false;
                }

                // Update the style override, and save changes
                // this may need optimisation if it turns out to be expensive
                WeaponOutLite.ClientHoldOverride.StyleOverrideListUpsert(overrideData);
                WeaponOutLite.ClientHoldOverride.SaveChanges(broadcast: false);
            }
            return isOverride;
        }

        private void SHOW_DEBUG_UI() {
            if (WeaponOutLite.TEXT_DEBUG != null) {
                string TEXT_DEBUG = WeaponOutLite.TEXT_DEBUG;
                if (TEXT_DEBUG.Length > 505) { TEXT_DEBUG = TEXT_DEBUG.Substring(0, 505); }
                var font = FontAssets.MouseText.Value;
                Vector2 size = font.MeasureString(TEXT_DEBUG);
                Main.spriteBatch.DrawString(font, TEXT_DEBUG,
                    Main.ScreenSize.ToVector2() * new Vector2(0.5f, 0.05f) - (size * new Vector2(0.5f, 0.0f)) / Main.UIScale,
                    Color.White);
                WeaponOutLite.TEXT_DEBUG = null;
                if (WeaponOutLite.DEBUG_MULTIPLAYER) TEXT_DEBUG += "multiplayer logging enabled\n";
                if (WeaponOutLite.DEBUG_TEST_ITEMS) TEXT_DEBUG += "debug items loaded\n";
            }

            if (WeaponOutLite.DEBUG_MULTIPLAYER) {
                // Debug draw player data
                foreach (Player p in Main.player) {
                    if (p.active) {

                        var modPlayer = p.GetModPlayer<WeaponOutPlayerRenderer>();
                        var font = FontAssets.MouseText.Value;
                        string text = "";
                        Vector2 size = font.MeasureString(text);
                        if (modPlayer.CurrentDrawItemPose == null) continue;
                        text += $"{p.name}\n" +
                            (!modPlayer.IsShowingHeldItem ? "hidden " : "") +
                            (modPlayer.DrawHeldItem ? $"{modPlayer.CurrentDrawItemPose.GetType().Name} " : "") +
                            (modPlayer.CombatDelayTimer != 0 ? $"\nt={modPlayer.CombatDelayTimer}" : "");
                        Main.spriteBatch.DrawString(font, text,
                            (new Vector2(0, 64) + p.BottomLeft - Main.screenPosition - size * 0.5f) / Main.UIScale,
                            Color.LightPink);
                    }
                }
            }
        }
    }
}