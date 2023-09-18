using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    public class HoldReversedInHand : IDrawItemPose
    {
        public virtual int GetID() => DrawItemPoseID.HoldReversedInHand;

        public virtual short DrawDepth(Player p, Item i, int timer) => DrawDepthID.Hand;

        public virtual int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer) => bodyFrame;

        public virtual DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {

            data = data.SetOrigin(0.1f, 0.9f, p);
            var handleOffset = DrawHelper.GetHandleLength(0.1f, 0.1f, width, height);

            if (bodyFrame == 0) { // Standing
                data.rotation = (float)(Math.PI * -0.5f) * p.gravDir;
                data.position += new Vector2(-6, 14);
            }
            else if (bodyFrame == 5) { // Jumping
                data.rotation = (float)(Math.PI * 0.3f);
                data.position += new Vector2(
                    (-10),
                    (-12f) + (float)Math.Sin(data.rotation) * handleOffset);
            }
            else if (bodyFrame > 5) { // Walk Cycles
                data.rotation = (float)(Math.PI * -0.8f);
                data.position += new Vector2(
                    (2f) + (float)Math.Cos(data.rotation) * handleOffset, 
                    (8) + (float)Math.Sin(data.rotation) * handleOffset);
                data = data.WithHandOffset(p);
            }
            else { // Grapple/Pulley/Mount
                data.color = Color.Transparent;
            }
            return data;
        }
    }
}
