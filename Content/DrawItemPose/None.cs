using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary>
    /// Specifically, nothing
    /// </summary>
    public class None : IDrawItemPose
    {
        public int GetID() => DrawItemPoseID.None;

        public short DrawDepth(Player p, Item i, int timer) => DrawDepthID.Back;

        public int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer) => bodyFrame;

        public DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {
            // Transparent
            data.color = Color.Transparent;
            return data;
        }
    }
}
