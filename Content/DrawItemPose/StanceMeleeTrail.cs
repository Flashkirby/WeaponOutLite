using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary>
    /// The "hero" pose where the sword faces down and away from the body.
    /// </summary>
    public class StanceMeleeTrail : OnWaistSheathe
    {
        public override int GetID() => DrawItemPoseID.StanceMeleeTrail;

        private bool CanUseBasePose(Player p, int timer) => timer == 0 || p.grapCount > 0 || p.pulley;

        public override short DrawDepth(Player p, Item i, int timer) {
            if (CanUseBasePose(p, timer) || p.IsMountPoseActive() || DrawHelper.AnimLinearNormal(20, timer) > 0.2f) {
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
            else if (bodyFrame == 0) {
                Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.Full;
                p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * 0.075f * p.direction);
            }
            else if (bodyFrame == 5) {
                Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.Full;
                p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * 0.25f * p.direction);
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

            // Face forward
            data = data.SetOrigin(0.1f, 0.9f, p).ApplyFlip(p).RotateFaceForward(p, height, width);

            // Rotate to face back and down
            data.rotation += (float)(Math.PI * 1f);

            if (p.shieldRaised) {
                data.position += new Vector2(
                    (-6),
                    (-6));
                data.rotation += (float)(Math.PI * 1.75f);
            }
            else if (bodyFrame == 0) {
                // Standing
                data.position += new Vector2(
                    (-4),
                    (8));

                data.rotation += (float)(Math.PI * 1.75f);
            }
            else if (bodyFrame > 5) {
                // Running
                data.position += new Vector2(
                    (-5),
                    (6));
                data = data.WithWaistOffset(p);

                data.rotation += (float)(Math.PI * 1.825f);
            }
            else if (bodyFrame == 5) {
                // Jumping
                data.position += new Vector2(
                    (-9),
                    (6));

                data.rotation += (float)(Math.PI * 1.925f);
            }

            // Sheathing
            float t = DrawHelper.AnimEaseOutNormal(20, timer);
            data = DrawHelper.LerpData(data, idleData, t);

            return data;
        }
    }
}
