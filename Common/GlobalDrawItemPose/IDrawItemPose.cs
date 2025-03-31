using Terraria;
using Terraria.DataStructures;

namespace WeaponOutLite.Common.GlobalDrawItemPose
{
    public interface IDrawItemPose
    {
        int GetID();

        short DrawDepth(Player p, Item i, int timer);

        int UpdateIdleBodyFrame(Player p, Item i, int bodyFrame, int timer);

        DrawData CalculateDrawData(DrawData data, Player p, float height, float width, int bodyFrame, int timer);
    }
}
