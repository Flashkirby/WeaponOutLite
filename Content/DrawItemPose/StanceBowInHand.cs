using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary> Hold undrawn in the offhand. </summary>
    public class StanceBowInHand : OnBackBow
    {
        public override int GetID() => DrawItemPoseID.StanceBowInHand;

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
                var sheatheRotation = DrawHelper.AnimArmRaiseLower(t);
                if (bodyFrame == 5) {
                    sheatheRotation = -1.25f -  sheatheRotation * 0.5f;
                }
                else {
                    sheatheRotation = sheatheRotation * 0.375f;
                }
                p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * (sheatheRotation) * p.direction);
                return bodyFrame;
            }

            return bodyFrame;
        }

        public override DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {
            DrawData idleData = base.CalculateDrawData(data, p, height, width, bodyFrame, timer);
            if (CanUseBasePose(p, timer)) {
                return idleData;
            }

            data = data.SetOrigin(0.5f, 0.5f, p);

            if (bodyFrame == 0) { // Standing
                data.rotation += (float)(Math.PI * -1.375f);
                data.position += new Vector2(
                    -4, 
                    10);
            }
            else if (bodyFrame > 5) { // Walk Cycles
                data.rotation += (float)(Math.PI * -1.5f);
                data.position += new Vector2(
                    0,
                    10);
                data = data.WithHandOffset(p);
            }
            else if (bodyFrame == 5) { // Jumping
                data.rotation += (float)(Math.PI * -0.375f);
                data.position += new Vector2(
                    -11,
                    -8f);
            }


            // Sheathing
            float t = DrawHelper.AnimEaseInEaseOutNormal(30, timer);
            if (t > 0) {
                data.position.X -= 64 * t;
                data.rotation += MathHelper.Pi * 2f * t;
                if (t > 0.5f) data = data.ApplyFlip(p);
                data = DrawHelper.LerpData(data, idleData, t);
            }

            return data;
        }
    }
}
