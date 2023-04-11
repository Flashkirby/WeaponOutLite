using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary>
    /// Holding the flail facing downwards
    /// </summary>
    public class StanceFlailReady : OnBackFlail
    {
        public override int GetID() => DrawItemPoseID.StanceFlailReady;

        private bool CanUseBasePose(Player p, int timer) => timer == 0 || p.grapCount > 0 || p.pulley;

        public override short DrawDepth(Player p, Item i, int timer) {
            if (CanUseBasePose(p, timer) || DrawHelper.AnimLinearNormal(30, timer) > 0.25f) {
                return base.DrawDepth(p, i, timer);
            }
            return DrawDepthID.Hand;
        }

        public override int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer) {
            if (CanUseBasePose(p, timer)) {
                return base.UpdateIdleBodyFrame(p, i, bodyFrame, timer);
            }

            float t = DrawHelper.AnimLinearNormal(30, timer);
            if (t > 0 && bodyFrame != 5) {
                float sheatheRotation = DrawHelper.AnimArmRaiseLower(t, 1f) * -1f;

                Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.Full;
                if (t > 0.4 && t < 0.6) frontArm = Player.CompositeArmStretchAmount.ThreeQuarters;

                p.SetCompositeArmFront(enabled: true, frontArm, (float)MathHelper.PiOver2 * sheatheRotation * p.direction);
            }

            return bodyFrame;
        }

        public override DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {
            DrawData idleData = base.CalculateDrawData(data, p, height, width, bodyFrame, timer);
            if (CanUseBasePose(p, timer)) {
                return idleData;
            }

            data = data.SetOrigin(0.6f, 1f, p);


            if (bodyFrame == 0) {
                // Standing
                data.position += new Vector2(-6, 8);
                data.rotation += (float)(Math.PI * 1f);
            }
            else if (bodyFrame > 5) {
                // Running
                data.position += new Vector2(-6, 6);
                data = data.WithHandOffset(p);
                data.rotation += (float)(Math.PI * 0.825f);
            }
            else if (bodyFrame == 5) {
                // Jumping
                data.position += new Vector2(-6, -10);
                data.rotation += (float)(Math.PI * -0.5f);
            }

            // Sheathing
            float t = DrawHelper.AnimEaseInEaseOutNormal(20, timer - 10);
            data.position.X += 8f * (float)Math.Sin(t * Math.PI);
            data = DrawHelper.LerpData(data, idleData, t);

            return data;
        }
    }
}
