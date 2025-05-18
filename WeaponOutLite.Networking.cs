using System.Collections.Generic;
using System.IO;

using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

using WeaponOutLite.Common.Players;
using WeaponOutLite.ID;

namespace WeaponOutLite
{
    partial class WeaponOutLite
    {
        internal enum MessageType : byte
        {
            UpdateWeaponVisual = 3,
            UpdateCombatTimer = 4
        }

        /// <summary>
        /// Receive a packet.
        /// For clients, this will be a packet from the server who whoAmI 256
        /// For the server, this will be a packet with the whoAmI of the player
        /// </summary>
        public override void HandlePacket(BinaryReader reader, int whoAmI) {
            MessageType code = (MessageType)reader.ReadByte();

            if (DEBUG_MULTIPLAYER) {
                string text = $"Mod Packet Received from {whoAmI} with code: {code}";
                if (Main.dedServ) { System.Console.WriteLine(text); } else { Main.NewText(text); }
            }

            switch (code) {
                case MessageType.UpdateWeaponVisual:
                    ReceiveUpdateWeaponVisual(reader, whoAmI); break;
                case MessageType.UpdateCombatTimer:
                    ReceiveUpdateCombatTimer(reader, whoAmI); break;
            }
        }

        /// <summary>
        /// Send a net message to update the current item holding state.
        /// </summary>
        /// <param name="modPlayer"></param>
        internal void SendUpdateWeaponVisual(WeaponOutPlayerRenderer modPlayer) {
            if (Main.netMode == NetmodeID.SinglePlayer) return;

            ModPacket message = GetPacket();
            message.Write((byte)MessageType.UpdateWeaponVisual);
            message.Write((byte)modPlayer.Player.whoAmI);
            message.Write(modPlayer.IsShowingHeldItem);

            // If null, broadcast the default item style
            message.Write(modPlayer.CurrentDrawItemPose?.GetID() ?? DrawItemPoseID.Unassigned);

            // As the client, this will simply send this packet to the server (it is unaffected by the parameters)
            // As the server, this will specify to send a the packet to every client, except the ignored one (and itself, of course)
            // In practice, this means that after receiving from the original client, the server will send the packet to update the remaining clients
            message.Send(-1, modPlayer.Player.whoAmI);
        }

        private void ReceiveUpdateWeaponVisual(BinaryReader reader, int whoAmI) {

            int playerWhoAmI = reader.ReadByte();
            bool isShowingItem= reader.ReadBoolean();
            int holdStyleID = reader.ReadInt32();
            if (DEBUG_MULTIPLAYER) {
                string text = $"Mod Packet ReceiveUpdateWeaponVisual: {playerWhoAmI} is show {isShowingItem} | with style  {holdStyleID}";
                if (Main.dedServ) { System.Console.WriteLine(text); } else { Main.NewText(text); }
            }

            if (!Main.player[playerWhoAmI].TryGetModPlayer<WeaponOutPlayerRenderer>(out var modPlayer)) { return; }
            modPlayer.IsShowingHeldItem = isShowingItem;

            try {
                modPlayer.CurrentDrawItemPose = ItemPoses[holdStyleID];
            }
            catch {
                modPlayer.CurrentDrawItemPose = ItemPoses[DrawItemPoseID.Unassigned];
            }

            // If we are the server, we just received this from the updating client.
            // Therefore, send this update out to the remaining players, which will 
            // reach this method again, but stop here to prevent a feedback loop.
            if (Main.dedServ) {
                SendUpdateWeaponVisual(modPlayer);
            }
        }

        internal void SendUpdateCombatTimer(WeaponOutPlayerRenderer modPlayer) {
            if (Main.netMode == NetmodeID.SinglePlayer) return;

            ModPacket message = GetPacket();
            message.Write((byte)MessageType.UpdateCombatTimer);
            message.Write((byte)modPlayer.Player.whoAmI);
            message.Write(modPlayer.CombatDelayTimer);
            message.Send(-1, modPlayer.Player.whoAmI); // all clients excluding origin
        }


        private void ReceiveUpdateCombatTimer(BinaryReader reader, int whoAmI) {

            int playerWhoAmI = reader.ReadByte();
            var combatTimer = reader.ReadInt32();

            if (!Main.player[playerWhoAmI].TryGetModPlayer<WeaponOutPlayerRenderer>(out var modPlayer)) { return; }
            modPlayer.CombatDelayTimer = combatTimer;

            // Forward server->clients
            if (Main.dedServ) {
                SendUpdateCombatTimer(modPlayer);
            }
        }
    }
}