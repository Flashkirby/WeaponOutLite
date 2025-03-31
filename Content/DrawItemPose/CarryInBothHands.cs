using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    // Basically like hand, but with bigger objects
    public class CarryInBothHands : IDrawItemPose
    {
        public virtual int GetID() => DrawItemPoseID.CarryInBothHands;

        public virtual short DrawDepth(Player p, Item i, int timer) => DrawDepthID.Hand;

        public virtual int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer) {
            // Although this is called in PostUpdate, worth checking if this can potentially cause gameplay issues
            p.shield = -1; // Hide shield
            p.cShield = -1; // Hide cosmetic shield

            if (bodyFrame == 0) {
                Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.Full;
                p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * -0.15f * p.direction);

                Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.Full;
                p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * -0.2f * p.direction);
            }
            else if (bodyFrame == 5) {
                Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.Full;
                p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * -0.3f * p.direction);

                Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.Full;
                p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * -0.35f * p.direction);
            }
            else if (bodyFrame > 5) {
                Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.Full;
                p.SetCompositeArmBack(enabled: true, backArm, 0);

                Player.CompositeArmStretchAmount frontArm = Player.CompositeArmStretchAmount.Full;
                p.SetCompositeArmFront(enabled: true, frontArm, (float)Math.PI * -0.1f * p.direction);
            }

            return bodyFrame;
        }

        public virtual DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {

            data = data.SetOrigin(0.05f, 0.95f, p);

            int playerBodyFrameNum = p.bodyFrame.Y / p.bodyFrame.Height;
            if (playerBodyFrameNum == 0) {
                data.position += new Vector2(8, 12);
            }
            else if (playerBodyFrameNum == 5) {
                data.position += new Vector2(10, 8);
            }
            else if (playerBodyFrameNum > 5) {
                data.position += new Vector2(4, 13);

                // Move to arm position when mounted
                if (p.mount.Active) {
                    data.position += new Vector2(2, -6);
                }

                data.rotation = (float)(Math.PI * 0.075f);
            }
            else {
                data.color = Color.Transparent;
            }

            return data.WithWaistOffset(p);
        }
    }
}
