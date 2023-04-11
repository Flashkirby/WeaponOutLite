using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary> Standard rotated on the back. </summary>
    public class OnBackFlail : IDrawItemPose
    {
        public virtual int GetID() => DrawItemPoseID.BackFlail;

        public virtual short DrawDepth(Player p, Item i, int timer) => DrawDepthID.Back;

        public virtual int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer) => bodyFrame;

        public virtual DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {

            data = data.SetOrigin(0.6f, 1f, p);

            data.position += new Vector2(
                0, 
                16).Round2();

            return data.WithWaistOffset(p);
        }
    }
}
