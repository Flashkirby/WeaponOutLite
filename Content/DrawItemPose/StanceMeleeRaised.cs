using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary>
    /// Based on the way Link holds his sword in SSBU.
    /// </summary>
    public class StanceMeleeRaised : OnWaistSheathe
    {
        public override int GetID() => DrawItemPoseID.StanceMeleeRaised;

        private bool CanUseBasePose(Player p, int timer) => timer == 0 || p.grapCount > 0 || p.pulley;

        public override short DrawDepth(Player p, Item i, int timer) {
            if (CanUseBasePose(p, timer) || p.IsMountPoseActive() || DrawHelper.AnimLinearNormal(20, timer) > 0.2f) {
                return base.DrawDepth(p, i, timer);
            }
            return p.shieldRaised || p.bodyFrame.Y < 5 * p.bodyFrame.Height ? DrawDepthID.Front : DrawDepthID.Hand;
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

            if (bodyFrame == 0) {
                Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.Full;
                p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * 0.1f * p.direction);
            }
            else if (bodyFrame == 5) {
                Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.Full;
                p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * 0.375f * p.direction);
            }
            else if (bodyFrame > 5) {
                Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.ThreeQuarters;
                p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * 0.125f * p.direction);
            }

            return bodyFrame;
        }

        public override DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {
            DrawData idleData = base.CalculateDrawData(data, p, height, width, bodyFrame, timer); 
            if (CanUseBasePose(p, timer)) return idleData;

            // Sheathing
            float t = DrawHelper.AnimEaseOutNormal(20, timer);

            // Face forward
            data = data.SetOrigin(0.5f - 0.4f * (width / height), 0.9f, p);
            if (bodyFrame < 5 || t > 0) {
                data = data.ApplyFlip(p);
            }
            data = data.RotateFaceForward(p, height, width);

            // Rotate to face back and down
            data.rotation += (float)(Math.PI * 1f);

            if (p.shieldRaised) {
                data.position += new Vector2(
                    (-5),
                    (5));
                data.rotation += (float)(Math.PI * 2.3f);
            }
            else if (bodyFrame == 0) {
                // Standing
                data.position += new Vector2(
                    (-8),
                    (8));

                data.rotation += (float)(Math.PI * 2.25f);
            }
            else if (bodyFrame > 5) {
                // Running
                data.position += new Vector2(
                    (-10),
                    (10));
                data = data.WithWaistOffset(p);

                data.rotation += (float)(Math.PI * 1.125f);
            }
            else if (bodyFrame == 5) {
                // Jumping
                data.position += new Vector2(
                    (-16),
                    (2));

                data.rotation += (float)(Math.PI * 1.375f);
            }

            data = DrawHelper.LerpData(data, idleData, t);

            return data;
        }
    }
}
