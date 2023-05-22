using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using WeaponOutLite.Common.Configs;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary>
    /// Derived from StanceRiflePointDown, but with pump action mag clearing before sheathe with back hand.
    /// Specifically for the boomstick and shotgun. Tactical shotgun is too big to look decent
    /// </summary>
    public class StanceRiflePumpAction : OnBackDownward
    {
        public override int GetID() => DrawItemPoseID.StanceRiflePumpAction;

        private bool CanUseBasePose(Player p, int timer) => timer == 0 || p.grapCount > 0 || p.pulley;

        public override short DrawDepth(Player p, Item i, int timer) {
            if (CanUseBasePose(p, timer) || DrawHelper.AnimLinearNormal(30, timer) > 0.4f) {
                return base.DrawDepth(p, i, timer);
            }
            return DrawDepthID.Hand;
        }

        public override int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer) {
            if (CanUseBasePose(p, timer)) {
                return bodyFrame;
            }

            // sheathe
            int sheatheDuration = 30;
            float t = DrawHelper.AnimLinearNormal(sheatheDuration, timer);
            float sheatheRotation = 0f;
            if (t > 0) {
                sheatheRotation = DrawHelper.AnimArmRaiseLower(t) * 1f;
            }

            if (sheatheRotation > 0) {
                Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.ThreeQuarters;
                if (sheatheRotation > 0.5f) backArm = Player.CompositeArmStretchAmount.None;
                p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * (-sheatheRotation * 0.75f) * p.direction);

                Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.Quarter;
                if (sheatheRotation > 0.75f) frontArm = Player.CompositeArmStretchAmount.Full;
                else if (sheatheRotation > 0.5f) frontArm = Player.CompositeArmStretchAmount.ThreeQuarters;
                p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * (-sheatheRotation * 1f) * p.direction);
            }
            else if (bodyFrame == 0) {
                bodyFrame = 3;
            }
            else if (bodyFrame >= 5) {

                Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.Full;

                Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.Full;
                float frontArmRotMod = -0.4f;

                if (bodyFrame > 5) {

                    if (bodyFrame % 7 < 4) {
                        if (bodyFrame >= 14) {
                            // Full
                        }
                        else {
                            backArm = Player.CompositeArmStretchAmount.Quarter;
                            frontArm = Player.CompositeArmStretchAmount.Quarter;
                            frontArmRotMod += 0.04f;
                        }
                    }
                    else {
                        // running
                        backArm = Player.CompositeArmStretchAmount.ThreeQuarters;
                        frontArm = Player.CompositeArmStretchAmount.ThreeQuarters;
                        frontArmRotMod += 0.02f;
                    }
                }

                p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * -0.3f * p.direction);
                p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * frontArmRotMod * p.direction);
            }

            // Moving to rest pose
            int pumpAnimationMax = 45;
            float animationTime = Math.Clamp(timer - sheatheDuration, -1, pumpAnimationMax + sheatheDuration + 1);
            if (t == 0 && animationTime <= pumpAnimationMax) {
                var at = DrawHelper.AnimLinearNormal(pumpAnimationMax, animationTime);
                if (at > 0) {
                    var backArmRotation = Math.Min(DrawHelper.AnimArmRaiseLower(at, 1f), 0.5f);

                    Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.Full;

                    // cocking arm animation
                    var armRotation = DrawHelper.AnimArmRaiseLower(at, 0.7f);
                    var pumpActionArm = Player.CompositeArmStretchAmount.Full;
                    if (armRotation > 0.75f) pumpActionArm = Player.CompositeArmStretchAmount.ThreeQuarters;
                    if (armRotation > 0.85f) pumpActionArm = Player.CompositeArmStretchAmount.Quarter;
                    if (armRotation > 0.9f) pumpActionArm = Player.CompositeArmStretchAmount.None;

                    p.SetCompositeArmBack(enabled: true, pumpActionArm, (float)Math.PI * (-0.4f + backArmRotation * -0.65f) * p.direction);
                    p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * (-0.375f + armRotation * 0.125f) * p.direction);
                }
            }

            return bodyFrame;
        }

        public override DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {
            DrawData idleData = base.CalculateDrawData(data, p, height, width, bodyFrame, timer);
            if (CanUseBasePose(p, timer)) {
                return idleData;
            }

            // Sheathing calculations
            float t = DrawHelper.AnimEaseInEaseOutNormal(30, timer);
            int sheatheDuration = 30;
            int pumpAnimationMax = 45;
            float animationTime = Math.Clamp(timer - sheatheDuration, 0, pumpAnimationMax + sheatheDuration);
            bool inPumpAnimation = animationTime <= pumpAnimationMax;

            // Normal posing
            data = data.SetOrigin(0.25f, 1f / 3f, p);
            data.position += new Vector2(8, 0);

            if (bodyFrame == 3) {
                // Standing
            }
            else if (bodyFrame > 5) {
                // Running
                if (p.velocity.Y != 0) {
                    data.position += new Vector2(-1, 2);
                    data.rotation += 0.175f;
                }
                if (inPumpAnimation) {
                    data = data.WithWaistOffset(p);
                }
                else {
                    data = data.WithHandOffset(p);
                }
                float rot = 0;
                switch (bodyFrame) {
                    case 06: rot = -0.75f; break;
                    case 07: rot = -1f; break;
                    case 08: rot = -0.0f; break;
                    case 09: rot = 0.25f; break;
                    case 10: rot = 0.5f; break;
                    case 11: rot = 0.25f; break;
                    case 12: rot = 0f; break;
                    case 13: rot = -0.75f; break;
                    case 14: rot = -1f; break;
                    case 15: rot = 0f; break;
                    case 16: rot = 0.25f; break;
                    case 17: rot = 0.5f; break;
                    case 18: rot = 0.25f; break;
                    case 19: rot = 0f; break;
                }
                data.rotation += 0.05f * rot;
            }
            else if (bodyFrame == 5 && !inPumpAnimation) {
                // Jumping
                data.position += new Vector2(6, 0);
            }

            // If not sheathihng, moving to rest pose
            if (inPumpAnimation) {
                var at = DrawHelper.AnimLinearNormal(pumpAnimationMax, animationTime); // from 1 to 0

                // if animations enabled, do this cool effect
                if (at > 0 && animationTime == 40) {
                    //SoundEngine.PlaySound(SoundID.Camera.WithPitchOffset(-0.25f).WithVolumeScale(0.375f), p.MountedCenter);
                    // ammo box noise
                    SoundEngine.PlaySound(SoundID.Item149.WithPitchOffset(0.75f).WithVolumeScale(0.3f), p.MountedCenter);

                }
                var gunRotation = DrawHelper.AnimArmRaiseLower(Math.Min(at, 0.75f), 0.75f);

                if (at >= 0.5f) {
                    // Lean forward slightly again after the pullback
                    gunRotation = 1 + (0.5f - at) * 0.5f;
                }
                data.position.X += -4f * gunRotation;
                data.position.Y += 4f * gunRotation;
                data.rotation -= MathHelper.Pi * 0.375f * gunRotation;
            }

            // Sheathing
            data.position.X -= height / 2 * (float)Math.Sin(t * Math.PI);
            data.position.Y -= width / 2 * (float)Math.Sin(t * Math.PI);
            data.rotation -= 4f * t;
            data = DrawHelper.LerpData(data, idleData, t);

            return data;
        }
    }
}
