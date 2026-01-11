using NetworkMessages.FromServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.MissionRepresentatives;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace GoldGainForHorseKill
{
    public class TeamDeathmatchMissionRepresentativeHorseKillEffect : TeamDeathmatchMissionRepresentative
    {
        public int GoldCalc(MPPerkObject.MPPerkHandler killerPerkHandler)
        {
            List<KeyValuePair<ushort, int>> goldChangeEventList = new List<KeyValuePair<ushort, int>>();

            int goldOnKill = killerPerkHandler != null ? killerPerkHandler.GetGoldOnKill(0, 0) : 0;
            if (goldOnKill > 0)
            {
                goldChangeEventList.Add(new KeyValuePair<ushort, int>((ushort)2048, goldOnKill));
            }

            if (goldChangeEventList.Count > 0 && !this.Peer.Communicator.IsServerPeer && this.Peer.Communicator.IsConnectionActive)
            {
                GameNetwork.BeginModuleEventAsServer(this.Peer);
                GameNetwork.WriteMessage((GameNetworkMessage)new GoldGain(goldChangeEventList));
                GameNetwork.EndModuleEventAsServer();
            }

            return goldOnKill;
        }

        //public int GetGoldOnHorseKill(MPPerkObject.MPPerkHandler killerPerkHandler, float attackerValue, float victimValue)
        //{
        //    int goldOnKill = 0;
        //    bool isWarmup = killerPerkHandler.IsWarmup;
        //    foreach (MPPerkObject perk in (List<MPPerkObject>)killerPerkHandler._perks)
        //        goldOnKill += perk.GetGoldOnKill(isWarmup, killerPerkHandler._agent, attackerValue, victimValue);
        //    return goldOnKill;
        //}
    }
}
