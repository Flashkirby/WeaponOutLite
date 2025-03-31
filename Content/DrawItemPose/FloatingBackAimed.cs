using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary>
    /// Floating and pointing forwards, with back hand held out
    /// </summary>
    public class FloatingBackAimed : IDrawItemPose
    {
        public virtual int GetID() => DrawItemPoseID.FloatingBackAimed;

        public virtual short DrawDepth(Player p, Item i, int timer) => DrawDepthID.Back;

        public virtual int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer) => bodyFrame;

        public virtual DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {

            data = DrawHelper.RotateFaceForward(data, p, height, width);

            Vector2 spin = new Vector2(MathF.Sin(Main.GlobalTimeWrappedHourly * 2f) * 0.5f, -MathF.Cos(Main.GlobalTimeWrappedHourly * 2f));
            data.position += new Vector2(
                -Math.Max(height, width) / 2f,
                -(height + width) / 4f) + spin * 2f;
            data.position += (-p.velocity * 1f) * p.Directions; //momentum

            data.rotation += (float)(spin.Y * 0.02f); // 

            return data;
        }
    }
}
