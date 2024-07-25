using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using WeaponOutLite.Common.Configs;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary>
    /// The down facing guard stance, like DRK
    /// </summary>
    public class StanceMeleeTwoHandGuard : OnBack
    {
        public override int GetID() => DrawItemPoseID.StanceTwoHandGuard;

        private bool CanUseBasePose(Player p, int timer) => timer == 0 || p.grapCount > 0 || p.pulley;

        public override short DrawDepth(Player p, Item i, int timer) {
            if (p.shieldRaised) {
                timer = timer != 0 ? int.MaxValue : 0;
            }
            if (CanUseBasePose(p, timer) || p.IsMountPoseActive() || DrawHelper.AnimLinearNormal(30, timer) > 0.2f) {
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

            if (p.shieldRaised) {
                // cool guard pose for shielding (only for brand of inferno, but modded weapons might also do cool stuff with this)
                Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.Quarter;
                p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * (-0.5f) * p.direction);

                Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.Full;
                p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * (-0.8f) * p.direction);
            }
            else if (bodyFrame == 0 && t < 0.4f) {
                if (p.legFrame.Y == 0) p.legFrame.Y = 17 * p.legFrame.Height;

                Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.Full;
                p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * (-0.2f - sheatheRotation * 1.5f) * p.direction);

                Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.ThreeQuarters;
                p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * (-0.5f - sheatheRotation) * p.direction);
            }
            else {
                Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.Full;
                p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * (-0.1f - sheatheRotation * 1.5f) * p.direction);

                Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.None;
                p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * (-0.25f - sheatheRotation) * p.direction);
            }

            return bodyFrame;
        }

        public override DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {
            DrawData idleData = base.CalculateDrawData(data, p, height, width, bodyFrame, timer);
            if (CanUseBasePose(p, timer)) {
                return idleData;
            }

            data = data.SetOrigin(0.1f, 0.9f, p);

            if (p.shieldRaised) {
                // shield pose
                timer = int.MaxValue;
                if (p.shieldParryTimeLeft > 0) {
                    data.position += new Vector2(8 - 8f / p.shieldParryTimeLeft, -4);
                }
                else {
                    data.position += new Vector2(8, -4);
                }

                data.rotation += (float)(Math.PI * 0.5f);
            }
            else if (bodyFrame == 0 || p.IsMountPoseActive()) {
                data.position += new Vector2(8, 6);

                data.rotation += (float)(Math.PI * 0.4f);
            }
            else if (bodyFrame >= 5) {
                data.position += new Vector2(2, 9);

                data.rotation += (float)(Math.PI * 0.3f);
            }

            // Sheathing
            float t = DrawHelper.AnimOverEaseOutNormal(30, timer);
            data.position.X += 16f * (float)Math.Sin(t * Math.PI);
            data.position.Y -= height / 2 * (float)Math.Sin(t * Math.PI);
            if (t > 1f / 2f) {
                // flip item at the halfway point
                data = data.ApplyFlip(p);
                data.rotation -= MathHelper.PiOver2;
                data = DrawHelper.LerpData(data, idleData, t);
            }
            else {
                idleData.rotation += MathHelper.PiOver2;
                data = DrawHelper.LerpData(data, idleData, t);
            }

            return data.WithWaistOffset(p);
        }
    }
}
