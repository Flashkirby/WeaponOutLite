using Terraria.DataStructures;
using Terraria.ModLoader;
using WeaponOutLite.Common.Configs;
using WeaponOutLite.Common.Players;
using WeaponOutLite.ID;

namespace WeaponOutLite.Common
{
    public class WeaponOutItemHeldLayer : PlayerDrawLayer
    {
        /// <summary>
        /// Insert layer in at different positions depending on the draw style setting.
        /// EXPERIMENTAL (I don't know how Between works)
        /// Layer order can be found under PlayerDrawLayers.FixedVanillaLayers
        /// </summary>
        public override Position GetDefaultPosition() {
            var pos = new Multiple
            {
                {
                    new Between(PlayerDrawLayers.JimsCloak, PlayerDrawLayers.MountBack),
                    (PlayerDrawSet drawinfo) => GetDrawDepth(drawinfo) == DrawDepthID.Back
                },
                {
                    new Between(PlayerDrawLayers.Skin, PlayerDrawLayers.Leggings),
                    (PlayerDrawSet drawinfo) => GetDrawDepth(drawinfo) == DrawDepthID.OffHand
                },
                {
                    new Between(PlayerDrawLayers.SolarShield, PlayerDrawLayers.ArmOverItem),
                    (PlayerDrawSet drawinfo) => GetDrawDepth(drawinfo) == DrawDepthID.Hand
                },
                {
                    new Between(PlayerDrawLayers.HandOnAcc, PlayerDrawLayers.BladedGlove),
                    (PlayerDrawSet drawinfo) => GetDrawDepth(drawinfo) == DrawDepthID.Front
                }
            };
            return pos;
        }

        private short GetDrawDepth(PlayerDrawSet drawInfo) {
            // Check if the current draw item style wants to set the draw layer
            if (!drawInfo.drawPlayer.TryGetModPlayer<WeaponOutPlayerRenderer>(out var modPlayer)) { return DrawDepthID.Back; }
            return modPlayer.CurrentDrawItemPose?.DrawDepth(drawInfo.drawPlayer, drawInfo.drawPlayer.HeldItem, modPlayer.CombatDelayTimer) ?? 0;
        }

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) {
            return base.GetDefaultVisibility(drawInfo) && 
                WeaponOutLayerRenderer.CanDrawBaseDrawData(drawInfo,
                out _,
                out _,
                out _,
                out _,
                out _);
        }

        protected override void Draw(ref PlayerDrawSet drawInfo) {
            // If not enabled, stop
            if (!WeaponOutLite.ClientConfig.EnableWeaponOutVisuals) return;

            WeaponOutLayerRenderer.DrawPlayerItem(ref drawInfo);
        }
    }
}
