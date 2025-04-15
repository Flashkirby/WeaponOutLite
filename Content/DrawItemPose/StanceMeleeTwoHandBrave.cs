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
    /// The brave stance, also known as サンライズパース. Movement wise drag either above or behind
    /// </summary>
    public class StanceMeleeTwoHandBrave : OnBack
    {
        public override int GetID() => DrawItemPoseID.StanceTwoHandBrave;

        private bool CanUseBasePose(Player p, int timer) => timer == 0 || p.grapCount > 0 || p.pulley;

        public override short DrawDepth(Player p, Item i, int timer) {
            if (p.shieldRaised) {
                timer = timer != 0 ? int.MaxValue : 0;
            }
            if (CanUseBasePose(p, timer) || p.IsMountPoseActive() || DrawHelper.AnimLinearNormal(30, timer) > 0.2f) {
                return base.DrawDepth(p, i, timer);
            }
            if (WeaponOutLite.ClientConfig.EnableWeaponPhysics) {
                if (p.velocity.Y != 0 && p.velocity.Y < 2f) {
                    return DrawDepthID.Back;
                }
                if (p.bodyFrame.Y >= p.bodyFrame.Height * 5 && p.velocity.Y == 0) {
                    var motion = p.velocity.X * p.direction;
                    if (Math.Clamp(motion * 0.625f, 0f, 1f) > 0.7f) {
                        return DrawDepthID.Back;
                    }
                }
            }
            else {
                if(p.bodyFrame.Y >= p.bodyFrame.Height) return DrawDepthID.Back;
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
                Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.ThreeQuarters;
                p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * (-0.5f) * p.direction);

                Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.Full;
                p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * (-0.8f) * p.direction);
            }
            else if (bodyFrame == 0 && t < 0.4f) {
                if(p.legFrame.Y == 0) p.legFrame.Y = 9 * p.legFrame.Height;

                Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.Full;
                p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * (-0.15f - sheatheRotation * 1.5f) * p.direction);

                Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.Full;
                p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * (-0.15f - sheatheRotation) * p.direction);
            }
            else {
                if (p.velocity.Y == 0) {
                    // ground
                    float physicsRotation = 0f;
                    var motionNormal = 1f;
                    if (WeaponOutLite.ClientConfig.EnableWeaponPhysics) {
                        var motion = p.velocity.X * p.direction;
                        motionNormal = Math.Clamp(Math.Max(motion - 1f, 0f) * 0.625f, 0f, 1f);
                    }
                    physicsRotation -= motionNormal * 0.25f;

                    Player.CompositeArmStretchAmount backArm = motionNormal > 0.2f ? Player.CompositeArmStretchAmount.Quarter : Player.CompositeArmStretchAmount.Full;
                    p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * (-0.1f - physicsRotation - sheatheRotation * 1.5f) * p.direction);

                    Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.Full;
                    p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * (-0.15f + physicsRotation - sheatheRotation) * p.direction);
                }
                else {
                    // air
                    float physicsRotation = 0f;
                    var motionNormal = 0f;
                    if (WeaponOutLite.ClientConfig.EnableWeaponPhysics) {
                        motionNormal = Math.Clamp(p.velocity.Y * 0.1f, -1f, 1f);
                    }
                    physicsRotation -= motionNormal * 0.15f;


                    Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.Full;
                    p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * (-1f - physicsRotation * 1.5f - sheatheRotation * 0.75f) * p.direction);

                    Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.Full;
                    p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * (-1.1f - physicsRotation - sheatheRotation * 0.5f) * p.direction);
                }
            }

            return bodyFrame;
        }

        public override DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {
            DrawData idleData = base.CalculateDrawData(data, p, height, width, bodyFrame, timer);
            if (CanUseBasePose(p, timer)) {
                return idleData;
            }

            //data.origin = DrawHelper.NewOrigin(0.1f, 0.9f, data, p).Round2(p);
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
                // idle
                data.position += new Vector2(4, 14);

                data.rotation += (float)(Math.PI * 0.2f);
            }
            else if (bodyFrame >= 5) {
                if(p.velocity.Y == 0) {
                    // ground movement
                    data.position += new Vector2(4, 14);
                    data.rotation += (float)(Math.PI * 0.2f);

                    // Motion determined by horizontal speed
                    var motionNormal = 1f;
                    if (WeaponOutLite.ClientConfig.EnableWeaponPhysics) {
                        var motion = p.velocity.X * p.direction;
                        motionNormal = Math.Clamp(Math.Max(motion - 1f, 0f) * 0.625f, 0f, 1f);
                    }
                    data.position += new Vector2(8 * motionNormal, -10 * motionNormal);
                    data.rotation -= (float)(3.25f * motionNormal);
                }
                else {
                    // aerial movement
                    data.position += new Vector2(-6, -12);
                    data.rotation += (float)(Math.PI * -0.75f);

                    // Motion determined by vertical speed
                    var motionNormal = 0f;
                    if (WeaponOutLite.ClientConfig.EnableWeaponPhysics) {
                        var motion = p.velocity.Y;
                        motionNormal = Math.Clamp(motion * 0.1f, -1f, 1f);
                    }
                    data.position += new Vector2(6f * (motionNormal + 1f), 1f *(motionNormal + 1f));
                    data.rotation += (float)(0.5f * motionNormal);
                }
            }

            // Sheathing OnBack
            float t = DrawHelper.AnimOverEaseOutNormal(30, timer);
            if (t > 0)
            {
                data.position.X += 24f * (float)Math.Sin(t * Math.PI);
                data.position.Y -= height * (float)Math.Sin(t * Math.PI * 0.5f);
                data = DrawHelper.LerpData(data, idleData, t);
            }

            return data.WithWaistOffset(p);
        }
    }
}
