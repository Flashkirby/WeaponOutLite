using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary> For large guns slung across the shoulder. Visually similar to Back and BackBow, but without flipping </summary>
    public class OnBackDownward : IDrawItemPose
    {
        public virtual int GetID() => DrawItemPoseID.BackDownward;

        public virtual short DrawDepth(Player p, Item i, int timer) => DrawDepthID.Back;

        public virtual int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer) => bodyFrame;

        public virtual DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {
            data = data.SetOrigin(0.5f, 0.5f, p);
            // rotate up to towards down as item gets shorter
            var longRotation = 32 / Math.Max(32, width);
            data.rotation += (float)(Math.PI * -(1.15f + 0.2f * longRotation)); // rotate 90 deg
            data.position += new Vector2(-4 + width * 0.05f, 0);
            return data.WithWaistOffset(p);
        }
    }
}
