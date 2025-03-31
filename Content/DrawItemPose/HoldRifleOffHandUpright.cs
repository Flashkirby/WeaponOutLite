using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using WeaponOutLite.Common.GlobalDrawItemPose;
using WeaponOutLite.ID;

namespace WeaponOutLite.Content.DrawItemPose
{
    /// <summary>
    /// Hold face diagonally downwards
    /// </summary>
    public class HoldRifleOffHandUpright : IDrawItemPose
    {
        public virtual int GetID() => DrawItemPoseID.HoldRifleOffHandUpright;

        public virtual short DrawDepth(Player p, Item i, int timer) => DrawDepthID.Back;

        public virtual int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer) {
            // TODO: Although this is called in PostUpdate, worth checking if this can potentially cause gameplay issues
            p.shield = -1; // Hide shield
            p.cShield = -1; // Hide cosmetic shield

            if (bodyFrame <= 5) {
                // standing
                p.SetCompositeArmBack(enabled: true, Player.CompositeArmStretchAmount.ThreeQuarters, (float)Math.PI * (-0.375f) * p.direction);
            }
            else {
                // moving
                p.SetCompositeArmBack(enabled: true, Player.CompositeArmStretchAmount.Quarter, (float)Math.PI * (-0.5f) * p.direction);
            }

            return bodyFrame;
        }

        public virtual DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer) {
            //data.origin = DrawHelper.NewOrigin(0.5f, 0.5f, data, p).Round2(p);
            data = data.SetOrigin(0.25f, 0.5f, p);
            data.rotation -= (float)(Math.PI * 0.5f); // rotate 90 deg
            data.position += new Vector2(14, Math.Min(23 - width / 4, width / 4).Round2());
            
            if(bodyFrame > 5) {
                data.position -= new Vector2(2, 0);
            }
            return data.WithWaistOffset(p);
        }
    }
}
