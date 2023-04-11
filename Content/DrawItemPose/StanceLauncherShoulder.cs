using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary> 
    /// version of OnBackShoulder
    /// Works as a launcher on shoulder -> upright back storage.
    /// </summary>
    public class StanceLauncherShoulder : OnBackUpright
    {
        public override int GetID() => DrawItemPoseID.StanceLauncherShoulder;

        private bool CanUseBasePose(Player p, int timer) =>
            timer == 0 || p.shieldRaised;

        public override short DrawDepth(Player p, Item i, int timer) {
            if (CanUseBasePose(p, timer) || DrawHelper.AnimLinearNormal(30, timer) > 0.5f) {
                return base.DrawDepth(p, i, timer);
            }
            return DrawDepthID.OffHand;
        }

        public override int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer) {
            if (CanUseBasePose(p, timer)) {
                return bodyFrame;
            }

            p.shield = -1; // Hide shield
            p.cShield = -1; // Hide cosmetic shield

            float t = DrawHelper.AnimLinearNormal(30, timer);
            float sheatheRotation = 0f;
            if (t > 0) {
                sheatheRotation = DrawHelper.AnimArmRaiseLower(t) * 1f;
            }

            Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.Full;
            p.SetCompositeArmBack(enabled: true, backArm, MathHelper.Pi * (-0.365f - sheatheRotation * 1f) * p.direction);

            return bodyFrame;
        }

        public override DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {
            DrawData idleData = base.CalculateDrawData(data, p, height, width, bodyFrame, timer);
            if (CanUseBasePose(p, timer)) {
                return idleData;
            }

            // 4 pixel height buffer offset
            data = data.SetOrigin(0.25f, 0.75f, p).RotateFaceForward(p, height - 6, width);
            data.position += new Vector2(-12 + Math.Min(height, width) / 2, 0).Round2(p.direction, 1);

            // Sheathing
            float t = DrawHelper.AnimOverEaseNormal(30, timer);
            data = DrawHelper.LerpData(data, idleData, t);

            return data.WithWaistOffset(p);
        }
    }
}
