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
using Terraria.ModLoader.UI;
using Terraria.Localization;

namespace WeaponOutLite.Common.Systems
{
    public class UIDisplay : ModSystem
    {
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
            // Layer names come from the Main.SetupDrawInterfaceLayers method
            int inventoryIndex = layers.FindIndex(layer => layer.Name == "Vanilla: Inventory");
            if (inventoryIndex >= 0) {
                if (Main.playerInventory) {
                    var layer = new GameInterfaceLayer("yo", InterfaceScaleType.UI);
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

            // Default locked state (off)
            var texture = TextureAssets.InventoryTickOff.Value;
            var hoverText = "WeaponOut: " + Lang.inter[73]; // "Off" see en_US.Legacy
            var modPlayer = Main.LocalPlayer.GetModPlayer<WeaponOutPlayerRenderer>();
            if(modPlayer == null) { return; }

            bool forcedVisuals = ModContent.GetInstance<WeaponOutServerConfig>().EnableForcedWeaponOutVisuals;

            if (ModContent.GetInstance<WeaponOutServerConfig>().EnableWeaponOutVisuals || forcedVisuals) {

                // Enabled but turned off (hidden)
                hoverText = "WeaponOut: " + Lang.inter[60]; // "Hidden"

                if (modPlayer.isShowingHeldItem || forcedVisuals) {

                    // Enabled and turned on
                    texture = TextureAssets.InventoryTickOn.Value;
                    hoverText = "WeaponOut: " + Lang.inter[59]; // "Visible"

                    // Except its being forced on, so locked anyway
                    if (forcedVisuals) {
                        hoverText = "WeaponOut: " + Lang.inter[72]; // "On"
                    }

                    if(modPlayer.HeldItem != null) {
                        PoseSetClassifier.GetItemPoseGroupData(Main.LocalPlayer.HeldItem, out PoseStyleID.PoseGroup currentPoseGroup, out _);
                        hoverText += " (";
                        hoverText += PoseStyleID.MapPoseGroupToString(currentPoseGroup);
                        hoverText += ")";
                    }
                }
            }

            // Get bounding box of texture.
            var textureRect = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            if (textureRect.Contains(Main.mouseX, Main.mouseY)) {
                Main.hoverItemName = hoverText;
                Main.blockMouse = true;

                // On click
                if (Main.mouseLeft && Main.mouseLeftRelease) {

                    // Toggle
                    modPlayer.IsShowingHeldItem = !modPlayer.IsShowingHeldItem;

                    // Play "Click" sound
                    SoundEngine.PlaySound(SoundID.MenuTick);
                }
                if (Main.mouseRight && Main.mouseRightRelease) {
                    /*
                     * Function for changing the custom hold style for the currently held objects...
                     * But until there is a method of saving pending config changes to file outside of internal ConfigManager.Save....
                     * this function will remain unused.
                     * TODO: see if this has changed in tmodloader for 1.4.4, or just implement a copy of the code thusly:
                     
                            Terraria.ModLoader.Config.ModConfig config = ModContent.GetInstance<WeaponOutClientConfig>();
                            System.IO.Directory.CreateDirectory(Terraria.ModLoader.Config.ConfigManager.ModConfigPath);
                            string filename = config.Mod.Name + "_" + config.Name + ".json";
                            string path = System.IO.Path.Combine(Terraria.ModLoader.Config.ConfigManager.ModConfigPath, filename);
                            string json = Newtonsoft.Json.JsonConvert.SerializeObject((object)config, Terraria.ModLoader.Config.ConfigManager.serializerSettings);
                            System.IO.File.WriteAllText(path, json);
                    */

                    Main.NewText(
                        Language.GetTextValue("Mods.WeaponOut.Config.Instructions") +
                        Language.GetTextValue("LegacyMenu.14") +
                        " [g:5] " +
                        Language.GetTextValue("tModLoader.ModConfiguration") +
                        " [g:5] WeaponOut Lite: " + 
                        Language.GetTextValue("Mods.WeaponOut.Config.ClientSideConfigLabel")
                        );
                }
            }

            Main.spriteBatch.Draw(texture, position, null, Color.White);
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