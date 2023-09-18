using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary>
    /// For potions and whatnot
    /// </summary>
    public class HoldInHandFront : IDrawItemPose
    {
        public virtual int GetID() => DrawItemPoseID.HoldInHandFront;

        public virtual short DrawDepth(Player p, Item i, int timer) => DrawDepthID.Hand;

        public virtual int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer) {

            if (bodyFrame == 0) {
                bodyFrame = 3;
            }
            else if (bodyFrame == 5) {
                p.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathHelper.PiOver2 * -p.direction);
            }
            return bodyFrame;
        }

        public virtual DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {

            data = data.SetOrigin(0.25f, 0.5f, p);

            if (bodyFrame == 3) { // Standing
                data.position += new Vector2(12, 6);
            }
            else if (bodyFrame == 5) { // Jumping
                data.position += new Vector2(12, 2);
            }
            else if (bodyFrame > 5) { // Walk Cycles
                data.position += new Vector2(
                    (-0f),
                    (6f));
                data = data.WithHandOffset(p);
            }
            else { // Grapple/Pulley/Mount
                data.color = Color.Transparent;
            }
            return data;
        }
    }
}
