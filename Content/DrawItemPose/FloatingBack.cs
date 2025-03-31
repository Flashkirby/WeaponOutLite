using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary>
    /// Floating behind and downwards, like Terraprisma
    /// </summary>
    public class FloatingBack : IDrawItemPose
    {
        public virtual int GetID() => DrawItemPoseID.FloatingBack;

        public virtual short DrawDepth(Player p, Item i, int timer) => DrawDepthID.Back;

        public virtual int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer) => bodyFrame;

        public virtual DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {

            data = DrawHelper.RotateFaceForward(data, p, height, width);
            data.rotation -= MathHelper.PiOver4;

            Vector2 spin = new Vector2(MathF.Sin(Main.GlobalTimeWrappedHourly * 2f) * p.direction, -MathF.Cos(Main.GlobalTimeWrappedHourly * 2f));

            data.position += new Vector2(-24f, 8f - Math.Max(height,width) / 4f ) + spin * 2f;
            data.position += (-p.velocity) * p.Directions; // momentum

            data.rotation += (float)(Math.PI * 0.8f + spin.X * -0.02f); // rotate 180 deg

            return data;
        }
    }
}
