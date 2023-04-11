using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    public class StanceInHand : OnWaistSheathe
    {
        public override int GetID() => DrawItemPoseID.StanceInHand;

        private bool CanUseBasePose(Player p, int timer) => timer == 0 || p.grapCount > 0 || p.pulley;

        public override short DrawDepth(Player p, Item i, int timer) {
            if (CanUseBasePose(p, timer) || p.IsMountPoseActive() || DrawHelper.AnimLinearNormal (20, timer) > 0.4f) {
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
                Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.Full;
                if (t > 0.5f) frontArm = Player.CompositeArmStretchAmount.ThreeQuarters;
                float sheatheRotation = DrawHelper.AnimArmRaiseLower(t) * -0.375f;
                if(bodyFrame == 5) {
                    sheatheRotation = 1f - sheatheRotation;
                }
                p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * sheatheRotation * p.direction);
            }
            return bodyFrame;
        }

        public override DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {
            DrawData idleData = base.CalculateDrawData(data, p, height, width, bodyFrame, timer);
            if (CanUseBasePose(p, timer)) {
                return idleData;
            }

            data = data.SetOrigin(0.05f, 0.5f + 0.35f, p);

            if (bodyFrame == 0) { // Standing
                data.rotation += (float)(Math.PI * 0.5f);
                data.position += new Vector2(-6f, 8f);
            }
            else if (bodyFrame == 5) { // Jumping
                data.rotation += (float)(Math.PI * -2.25f);
                data.position += new Vector2(
                    (-9),
                    (-7f));
            }
            else if (bodyFrame > 5) { // Walk Cycles
                data.rotation += (float)(Math.PI * 0.35f);
                data.position += new Vector2(
                    (-5f),
                    (5f));
                data = data.WithHandOffset(p);
            }
            else { // Grapple/Pulley
                data.color = Color.Transparent;
            }

            // Sheathing
            float t = DrawHelper.AnimEaseInEaseOutNormal(20, timer);
            if (t > 0) {
                //data = data.RotateFaceForward(p, height, width).ApplyFlip(p);
                data.position += new Vector2(26f + width / 4, -10f) * t;
                data.rotation += MathHelper.Pi * 2f;
                // flip item at the halfway point
                // double the offset values since the lerp will be halfway to the resting point
                if (t > 1f / 2f) {
                    data = data.ApplyFlip(p);
                    // if it looks like a sword object, try to keep the correct visual rotation after flipping
                    if (width < height * 1.5f && height < width * 1.5f) {
                        data.rotation -= MathHelper.Pi * 1f;
                    }
                    else {
                        data = data.SetOrigin(0.05f, 0.5f - 0.35f * 2f, p);
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
