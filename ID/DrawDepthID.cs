using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaponOutLite.ID
{
    public static class DrawDepthID
    {
        /// <summary> Behind the player. </summary>
        public const short Back = -2;
        /// <summary> Behind the player, but in front of the back arm. </summary>
        public const short OffHand = -1;
        /// <summary> In front of the player, but behind the front arm. </summary>
        public const short Hand = 0;
        /// <summary> In front of the player. </summary>
        public const short Front = 1;
    }
}
