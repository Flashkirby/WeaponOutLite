using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    public class StanceRapierFootForward : OnWaistSheathe
    {
        public override int GetID() => DrawItemPoseID.StanceRapierFootForward;

        private bool CanUseBasePose(Player p, int timer) => timer == 0 || p.grapCount > 0 || p.pulley;

        public override short DrawDepth(Player p, Item i, int timer) {
            if (CanUseBasePose(p, timer)) {
                return base.DrawDepth(p, i, timer);
            }
            return DrawDepthID.OffHand;
        }

        public override int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer) {
            if (CanUseBasePose(p, timer)) {
                return base.UpdateIdleBodyFrame(p, i, bodyFrame, timer);
            }
            p.shield = -1; // Hide shield
            p.cShield = -1; // Hide cosmetic shield

            var sheathingNormal = DrawHelper.AnimEaseInNormal(30, timer);

            if (bodyFrame == 0) {
                if (p.legFrame.Y == 0) p.legFrame.Y = 9 * p.legFrame.Height;

                Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.Full;
                if (sheathingNormal > 0f) { backArm = Player.CompositeArmStretchAmount.ThreeQuarters; }
                if (sheathingNormal > 0.2f) { backArm = Player.CompositeArmStretchAmount.Full; }

                p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * (-0.375f + sheathingNormal * 0.25f) * p.direction);
            }
            if (bodyFrame > 5) {

                Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.ThreeQuarters;

                if (bodyFrame % 7 < 4) {
                    if (bodyFrame >= 14) {
                        backArm = Player.CompositeArmStretchAmount.Quarter;
                    }
                    else {
                        backArm = Player.CompositeArmStretchAmount.Full;
                    }
                }
                p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * -0.375f * p.direction);
            }

            return bodyFrame;
        }

        public override DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {
            DrawData idleData = base.CalculateDrawData(data, p, height, width, bodyFrame, timer);
            if (CanUseBasePose(p, timer)) {
                return idleData;
            }

            // Face top right
            data = data.SetOrigin(0.1f, 0.9f, p).RotateFaceForward(p, height, width);
            data.rotation -= MathHelper.PiOver4;

            if (bodyFrame == 0) {
                // Standing
                data.position += new Vector2(
                    (14),
                    (4));

                data.rotation += (float)(Math.PI * 0.3f);
            }
            else if (bodyFrame > 5) {
                // Running
                data.position += new Vector2(
                    (12),
                    (4));
                data = data.WithWaistOffset(p);

                if (bodyFrame % 7 < 4) {
                    if (bodyFrame >= 14) {
                        data.position += new Vector2(-2,-1);
                    }
                    else {
                        data.position += new Vector2(2,1);
                    }
                }

                data.rotation += (float)(Math.PI * 0.25f);
            }
            else if (bodyFrame == 5) {
                // Jumping
                data.position += new Vector2(
                    (7),
                    (-8));

                data.rotation += (float)(Math.PI * 0.175f);
            }


            // Move to point upwards
            var t = DrawHelper.AnimLinearNormal(30, timer);
            // Sheathing
            if (t > 0f) {
                data.position += new Vector2(16f, -12f) * t;
                data.rotation -= MathHelper.Pi * 2.75f * t;

                // flip item at the halfway point
                if (t > 1f / 2f) {
                    data = data.ApplyFlip(p);
                    // if it looks like a sword object, try to keep the correct visual rotation after flipping
                    // double the offset values since the lerp will be halfway to the resting point
                    if (width < height * 1.5f && height < width * 1.5f) {
                        data.rotation -= MathHelper.PiOver2 * 2f;
                    }
                    else {
                        data = data.SetOrigin(0.1f, 0.5f - 0.4f * 2f, p);
                    }
                    data = DrawHelper.LerpData(data, idleData, t);
                }
                else {
                    data = DrawHelper.LerpData(data, idleData, t);
                }

            }
            return data;
        }
    }
}
