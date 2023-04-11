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
    /// Always aiming, bolt action after firing
    /// </summary>
    public class StanceRifleBoltAction : OnBackDownward
    {
        public override int GetID() => DrawItemPoseID.StanceRifleBoltAction;

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
            float t = DrawHelper.AnimLinearNormal(30, timer);
            float sheatheRotation = 0f;
            if (t > 0) {
                sheatheRotation = DrawHelper.AnimArmRaiseLower(t) * 1f;
            }

            if (sheatheRotation > 0) {
                Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.ThreeQuarters;
                if (sheatheRotation > 0.5f) backArm = Player.CompositeArmStretchAmount.None;
                p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * (-sheatheRotation * 0.5f) * p.direction);

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

                p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * -0.4f * p.direction);
                p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * frontArmRotMod * p.direction);
            }

            // Moving to rest pose
            float delay = 10f;
            float animationTime = Math.Max(0, -(timer - ModContent.GetInstance<WeaponOutClientConfig>().CombatDelayTimerMax * 60f) - delay);
            if (t == 0 && animationTime <= 40) {
                var at = DrawHelper.AnimLinearNormal(40, animationTime);
                if (at > 0) {
                    var backArmRotation = Math.Min(DrawHelper.AnimArmRaiseLower(at, 1f), 0.75f);

                    Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.Full;
                    p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * (-0.4f + backArmRotation * -0.5f) * p.direction);

                    // cocking arm animation
                    var armRotation = DrawHelper.AnimArmRaiseLower(at, 0.75f);
                    var boltActionArm = Player.CompositeArmStretchAmount.Full;
                    if (armRotation > 0.9f) boltActionArm = Player.CompositeArmStretchAmount.ThreeQuarters;
                    if (armRotation > 0.93f) boltActionArm = Player.CompositeArmStretchAmount.Quarter;
                    if (armRotation > 0.96f) boltActionArm = Player.CompositeArmStretchAmount.None;

                    p.SetCompositeArmFront(enabled: true, boltActionArm, (float)Math.PI * (-0.4f - armRotation * 0.4f) * p.direction);
                }
            }

            return bodyFrame;
        }

        public override DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {
            DrawData idleData = base.CalculateDrawData(data, p, height, width, bodyFrame, timer);
            if (CanUseBasePose(p, timer)) {
                return idleData;
            }

            data = data.SetOrigin(0.25f, 1f / 3f, p);

            if (bodyFrame == 3) {
                // Standing
                data.position += new Vector2(8, 0);
            }
            else if (bodyFrame > 5) {
                // Running
                data.position += new Vector2(6, 0);
                if (p.velocity.Y != 0) {
                    data.position += new Vector2(-1, 2);
                    data.rotation += 0.175f;
                }
                data = data.WithHandOffset(p);
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
            else if (bodyFrame == 5) {
                // Jumping
                data.position += new Vector2(6, 0);
            }

            // Sheathing
            float t = DrawHelper.AnimEaseInEaseOutNormal(30, timer);
            data.position.X += 8f * (float)Math.Sin(t * Math.PI);
            data = DrawHelper.LerpData(data, idleData, t);

            // If not sheathihng, moving to rest pose
            float delay = 10f;
            float animationTime = Math.Max(0, -(timer - ModContent.GetInstance<WeaponOutClientConfig>().CombatDelayTimerMax * 60f) - delay);
            if (t == 0 && animationTime <= 40) {
                var at = DrawHelper.AnimLinearNormal(40, animationTime); // from 1 to 0

                // if animations enabled, do this cool effect
                if (at > 0 && animationTime == 10) {
                    //SoundEngine.PlaySound(SoundID.Camera.WithPitchOffset(-0.25f).WithVolumeScale(0.375f), p.MountedCenter);
                    // ammo box noise
                    SoundEngine.PlaySound(SoundID.Item149.WithPitchOffset(0.5f).WithVolumeScale(0.375f), p.MountedCenter);

                }
                var gunRotation = DrawHelper.AnimArmRaiseLower(at, 0.75f);

                if (at > 0.75f) {
                    data.rotation -= MathHelper.Pi * 0.375f;
                    data.rotation = MathHelper.Lerp(data.rotation, p.itemRotation * p.direction * p.gravDir, (at - 0.75f) * 4);
                    data.position.X = MathHelper.Lerp(
                        data.position.X - 8f, 
                        data.position.X - Math.Min(data.rotation, 0f) * -height * 0.25f, (at - 0.75f) * 4);
                }
                else {
                    if (at >= 0.5f) gunRotation = 1f;
                    data.position.X += -8f * gunRotation;
                    data.rotation -= MathHelper.Pi * 0.375f * gunRotation;
                }
            }

            return data;
        }
    }
}
