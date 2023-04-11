using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;
using System.Collections.Generic;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary>
    /// A rendering style for custom items, controlled by method hooks.
    /// Only works if the item has been registered in "customItemHoldStyles"
    /// </summary>
    public class Custom : IDrawItemPose
    {
        public int GetID() => DrawItemPoseID.Custom;

        public short DrawDepth(Player p, Item i, int timer) {
            short drawDepth = 0;
            foreach (var CustomDrawBehind in WeaponOutLite.GetMod().customDrawDepthFuncs) {
                drawDepth = CustomDrawBehind(p, i, drawDepth, timer);
            }
            return drawDepth;
        }

        public int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer) {
            foreach (var CustomUpdateIdleBodyFrame in WeaponOutLite.GetMod().customUpdateIdleBodyFrameFuncs) {
                bodyFrame = CustomUpdateIdleBodyFrame(p, i, bodyFrame, timer);
            }
            return bodyFrame;
        }
        
        public DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {
            return data;
        }
    }
}
