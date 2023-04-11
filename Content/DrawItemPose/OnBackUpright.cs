using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary> For large guns. most weapons are slung pointed down, but larger weapons are best pointed upwards to protect the business end. </summary>
    public class OnBackUpright : IDrawItemPose
    {
        public virtual int GetID() => DrawItemPoseID.BackUpright;

        public virtual short DrawDepth(Player p, Item i, int timer) => DrawDepthID.Back;

        public virtual int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer) => bodyFrame;

        public virtual DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {
            data = data.SetOrigin(0.5f, 0.5f, p).RotateFaceForward(p, height, width * 1.5f); ;
            data.rotation -= (float)(Math.PI * 0.5f); // rotate 90 deg
            data.position = (data.position + new Vector2(-6, 20 - width / 2));
            return data.WithWaistOffset(p);
        }
    }
}
