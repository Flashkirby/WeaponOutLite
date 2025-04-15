using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using WeaponOutLite.Common.Configs;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary>
    /// Polearm variant of Two hand forward
    /// </summary>
    public class StancePoleTwoHand : OnBack
    {
        public override int GetID() => DrawItemPoseID.StancePoleTwoHand;

        private bool CanUseBasePose(Player p, int timer) => timer == 0 || p.grapCount > 0 || p.pulley || p.shieldRaised;

        public override short DrawDepth(Player p, Item i, int timer) {
            if (CanUseBasePose(p, timer) || DrawHelper.AnimLinearNormal(30, timer) > 0.2f) {
                return base.DrawDepth(p, i, timer);
            }
            return DrawDepthID.Hand;
        }

        public override int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer) {
            if (CanUseBasePose(p, timer)) {
                return bodyFrame;
            }

            // TODO: Although this is called in PostUpdate, worth checking if this can potentially cause gameplay issues
            p.shield = -1; // Hide shield
            p.cShield = -1; // Hide cosmetic shield

            float t = DrawHelper.AnimLinearNormal(30, timer);
            float sheatheRotation = 0f;
            if (t > 0) {
                sheatheRotation = DrawHelper.AnimArmRaiseLower(t) * 1f;
            }

            if (bodyFrame == 0 && t < 0.3f) {
                if(p.legFrame.Y == 0) p.legFrame.Y = 9 * p.legFrame.Height;

                Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.ThreeQuarters;
                p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * (-0.3f - sheatheRotation * 1.5f) * p.direction);

                Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.Full;
                p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * (-0.2f - sheatheRotation) * p.direction);
            }
            else {

                Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.Full;
                p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * (-0.15f - sheatheRotation * 1.5f) * p.direction);

                Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.Full;
                p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * (-0.1f - sheatheRotation) * p.direction);
            }

            return bodyFrame;
        }

        public override DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {
            DrawData idleData = base.CalculateDrawData(data, p, height, width, bodyFrame, timer);
            if (CanUseBasePose(p, timer)) {
                return idleData;
            }

            data = data.SetOrigin(0.4f, 0.6f, p);

            if (bodyFrame == 0) {
                data.position += new Vector2(12, 6);
            }
            else if (bodyFrame >= 5) {
                data.position += new Vector2(9, 12);

                data.rotation += (float)(Math.PI * 0.15f);
            }
            else if (p.IsMountPoseActive()) {
                float speedRotation = 0.125f;
                if (ModContent.GetInstance<WeaponOutClientConfig>().EnableWeaponPhysics) {
                    float maxSpeed = 3f;
                    speedRotation = Math.Clamp(p.velocity.X * p.direction, 0f, maxSpeed);
                    speedRotation = speedRotation / maxSpeed * 0.125f;
                }
                // 0.25f to 0.00f
                data.rotation -= (float)(Math.PI * (-speedRotation));
                data.position += new Vector2(14, 4);
            }

            // Sheathing OnBack
            float t = DrawHelper.AnimOverEaseOutNormal(30, timer);
            if (t > 0)
            {
                data.position.X += 16f * (float)Math.Sin(t * Math.PI);
                data.position.Y -= height / 2 * (float)Math.Sin(t * Math.PI);
                data = DrawHelper.LerpData(data, idleData, t);
            }

            return data.WithWaistOffset(p);
        }
    }
}
