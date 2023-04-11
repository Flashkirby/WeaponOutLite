using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary>
    /// Pretty standard fore and down facing weapon hold.
    /// </summary>
    public class StanceMeleeReady : OnWaistSheathe
    {
        public override int GetID() => DrawItemPoseID.StanceMeleeReady;

        private bool CanUseBasePose(Player p, int timer) => timer == 0 || p.grapCount > 0 || p.pulley;

        public override short DrawDepth(Player p, Item i, int timer) {
            if (CanUseBasePose(p, timer) || p.IsMountPoseActive() || DrawHelper.AnimLinearNormal(20, timer) > 0.15f) {
                return base.DrawDepth(p, i, timer);
            }
            return DrawDepthID.Hand;
        }

        public override int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer) {
            if (CanUseBasePose(p, timer) || p.IsMountPoseActive()) {
                return base.UpdateIdleBodyFrame(p, i, bodyFrame, timer);
            }

            float t = DrawHelper.AnimLinearNormal(20, timer);
            if (t > 0f) {
                Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.ThreeQuarters;
                float sheatheRotation = DrawHelper.AnimArmRaiseLower(t) * -0.5f;
                if (bodyFrame == 5) {
                    sheatheRotation = 1f - sheatheRotation;
                }
                p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * sheatheRotation * p.direction);
                return bodyFrame;
            }

            if (p.shieldRaised) {
                Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.Quarter;
                p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * 0.5f * p.direction);
            }
            else if (bodyFrame > 5) {
                Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.Full;
                p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * 0.125f * p.direction);
            }

            return bodyFrame;
        }

        public override DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {
            DrawData idleData = base.CalculateDrawData(data, p, height, width, bodyFrame, timer);
            if (CanUseBasePose(p, timer)) return idleData;

            // Face top right
            data = data.SetOrigin(0.1f, 0.9f, p).RotateFaceForward(p, height, width);
            data.rotation += 3 * MathHelper.PiOver2;

            if (p.shieldRaised) {
                data.position += new Vector2(
                    (-7),
                    (-3));
                data.rotation -= (float)(Math.PI * 0.5f);
            }
            else if (bodyFrame == 0) {
                // Standing
                data.position += new Vector2(
                    (-8),
                    (13));

                data.rotation += (float)(Math.PI * 0.6f);
            }
            else if (bodyFrame > 5) {
                // Running
                data.position += new Vector2(
                    (-13),
                    (10));
                data = data.WithWaistOffset(p);

                data.rotation += (float)(Math.PI * 0.75f);
            }
            else if (bodyFrame == 5) {
                // Jumping
                data.position += new Vector2(
                    (-6),
                    (-8));

                data.rotation -= (float)(Math.PI * 2.25f);
            }

            // Sheathing
            float t = DrawHelper.AnimEaseOutNormal(20, timer);
            if (t > 0f) {
                data.position += new Vector2(26f, -18f) * t;

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
