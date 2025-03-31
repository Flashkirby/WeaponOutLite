using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using WeaponOutLite.Common.Configs;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary> Hold facing down with arms bent. </summary>
    public class StanceBowHunt : OnBackBow
    {
        public override int GetID() => DrawItemPoseID.StanceBowHunt;

        private bool CanUseBasePose(Player p, int timer) => timer == 0 || p.grapCount > 0 || p.pulley || p.mount.Active;

        public override short DrawDepth(Player p, Item i, int timer) {
            if (CanUseBasePose(p, timer) || DrawHelper.AnimLinearNormal(30, timer) > 0.5f) {
                return base.DrawDepth(p, i, timer);
            }
            return DrawDepthID.Hand;
        }

        public override int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer) {
            if (CanUseBasePose(p, timer)) {
                return base.UpdateIdleBodyFrame(p, i, bodyFrame, timer);
            }

            // Sheathing
            float t = DrawHelper.AnimEaseInEaseOutNormal(30, timer);
            if (t > 0) {
                var frontArm = Player.CompositeArmStretchAmount.Full;
                var sheatheRotation = DrawHelper.AnimArmRaiseLower(t) * 0.375f;
                p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * (sheatheRotation) * p.direction);
                return bodyFrame;
            }

            if (bodyFrame == 0) {
                var backArm = Player.CompositeArmStretchAmount.Full;
                p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * (-0.125f) * p.direction);
                var frontArm = Player.CompositeArmStretchAmount.Quarter;
                p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * (-0.25f) * p.direction);
            }
            if(bodyFrame >= 5) {
                var backArm = Player.CompositeArmStretchAmount.Full;
                var frontArm = Player.CompositeArmStretchAmount.None;

                // Motion determined by fall speed, or forward running speed
                var armMotion = 0f;
                if (ModContent.GetInstance<WeaponOutClientConfig>().EnableWeaponPhysics) {
                    var motion = -p.velocity.Y * p.gravDir;
                    var motionR = -Math.Clamp(motion * 0.02f, -0.39f, 0.39f);
                    armMotion += motionR;
                    if(armMotion > 0) {
                        frontArm = Player.CompositeArmStretchAmount.Quarter;
                        armMotion /= 2; 
                    }
                }

                p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * (-0.125f) * p.direction);
                p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * (-0.25f + armMotion) * p.direction);
            }
            return bodyFrame;
        }

        public override DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {
            DrawData idleData = base.CalculateDrawData(data, p, height, width, bodyFrame, timer);
            if (CanUseBasePose(p, timer)) {
                return idleData;
            }

            data = data.SetOrigin(0.5f, 0.5f, p);

            if (bodyFrame == 0) {
                data.rotation += (float)(Math.PI * -1.675f);
                data.position += new Vector2(12, 14);
            }
            else if (bodyFrame >= 5) {
                data.rotation += (float)(Math.PI * -1.75f);
                data.position += new Vector2(12, 12);
            }

            // Motion determined by fall speed, or forward running speed
            if (ModContent.GetInstance<WeaponOutClientConfig>().EnableWeaponPhysics) {
                var motion = -p.velocity.Y * p.gravDir;
                var motionR = -Math.Clamp(motion * 0.02f, -0.39f, 0.39f);
                data.rotation -= motionR;
            }

            // Sheathing
            float t = DrawHelper.AnimEaseInEaseOutNormal(30, timer);
            if (t > 0) {
                data.position.X -= 64 * t;
                data.rotation += MathHelper.Pi * 2f * t;
                if (t > 0.5f) data = data.ApplyFlip(p);
                data = DrawHelper.LerpData(data, idleData, t);
            }

            return data.WithWaistOffset(p);
        }
    }
}
