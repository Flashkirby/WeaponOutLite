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
    /// The roof stance, or perhaps more familiarly as the way the TF2 Demoman holds his claymore.
    /// </summary>
    public class StanceMeleeTwoHandHighlander: OnBack
    {
        public override int GetID() => DrawItemPoseID.StanceTwoHandHighlander;

        private bool CanUseBasePose(Player p, int timer) => timer == 0 || p.grapCount > 0 || p.pulley;

        public override short DrawDepth(Player p, Item i, int timer) {
            if (p.shieldRaised) {
                timer = timer != 0 ? int.MaxValue : 0;
            }
            if (CanUseBasePose(p, timer) || p.IsMountPoseActive() || DrawHelper.AnimLinearNormal(30, timer) > 0.2f) {
                return base.DrawDepth(p, i, timer);
            }
            return DrawDepthID.Back;
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
                if(p.legFrame.Y == 0) p.legFrame.Y = 17 * p.legFrame.Height;

                Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.None;
                p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * (-0.6f - sheatheRotation * 1.5f) * p.direction);

                Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.Full;
                p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * (-0.425f - sheatheRotation) * p.direction);
            }else {
                Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.None;
                p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * (-0.8f - sheatheRotation * 1.5f) * p.direction);

                Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.Full;
                p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * (-0.475f - sheatheRotation) * p.direction);
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
                if(p.shieldParryTimeLeft > 0) {
                    data.position += new Vector2(8 - 8f / p.shieldParryTimeLeft, -4);
                }
                else {
                    data.position += new Vector2(8, -4);
                }

                data.rotation += (float)(Math.PI * 0.5f);
            }
            else if (bodyFrame == 0) {
                data.position += new Vector2(11, 1);

                data.rotation += (float)(Math.PI * -0.23f);

            }
            else if (bodyFrame >= 5 || p.IsMountPoseActive()) {
                float physicsRotation = 0f;
                var motionNormal = 0f;
                if (ModContent.GetInstance<WeaponOutClientConfig>().EnableWeaponPhysics) {
                    motionNormal = Math.Clamp(p.velocity.X * p.direction * 0.1f, -1f, 1f);
                }
                physicsRotation -= motionNormal * 0.15f;

                data.position += new Vector2(9, 2);

                data.rotation += (float)(Math.PI * -0.325f + physicsRotation);
            }

            // Sheathing OnBack
            float t = DrawHelper.AnimOverEaseOutNormal(30, timer);
            if (t > 0)
            {
                data.position.X -= width * (float)Math.Sin(t * Math.PI * 0.25f);
                data.position.Y -= (32 + height / 2) * (float)Math.Sin(t * Math.PI * 0.5f);
                data = DrawHelper.LerpData(data, idleData, t);
            }

            return data.WithWaistOffset(p);
        }
    }
}
