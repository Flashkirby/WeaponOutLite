using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    public class PoleOffHand : IDrawItemPose
    {
        public virtual int GetID() => DrawItemPoseID.PoleOffHand;

        public virtual short DrawDepth(Player p, Item i, int timer) => DrawDepthID.Back;

        public virtual int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer) {
            if (bodyFrame == 0 || bodyFrame == 5) {
                Player.CompositeArmStretchAmount backArm = Player.CompositeArmStretchAmount.ThreeQuarters;
                p.SetCompositeArmBack(enabled: true, backArm, (float)Math.PI * -0.175f * p.direction);
            }
            return bodyFrame;
        }

        public virtual DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {

            data = data.SetOrigin(0.1f, 0.9f, p);

            if (bodyFrame == 0) { // Standing
                data.rotation = (float)(Math.PI * -0.25f);
                data.position += new Vector2(15f, 18f);
            }
            else if (bodyFrame > 5) { // Walk Cycles
                data.rotation = (float)(Math.PI * -0.25f);
                data.position += new Vector2(13f, 16f);
                data = data.WithOffHandOffset(p);
            }
            else { // Jumping/Grapple/Pulley/Mount
                data.rotation = (float)(Math.PI * -0.20f);
                data.position += new Vector2(10f, 19f);
            }
            return data;
        }
    }
}
