using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary> Standard rotated on the back. </summary>
    public class OnBack : IDrawItemPose
    {
        public virtual int GetID() => DrawItemPoseID.Back;

        public virtual short DrawDepth(Player p, Item i, int timer) => DrawDepthID.Back;

        public virtual int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer) => bodyFrame;

        public virtual DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {

            data = data.SetOrigin(0.5f, 0.5f, p).RotateFaceForward(p, height, width);

            data.position += new Vector2(
                2 + width *  -0.1f, 
                height * 0.05f).Round2();
            data.rotation -= MathHelper.Pi * 1.25f;

            return data.WithWaistOffset(p);
        }
    }
}
