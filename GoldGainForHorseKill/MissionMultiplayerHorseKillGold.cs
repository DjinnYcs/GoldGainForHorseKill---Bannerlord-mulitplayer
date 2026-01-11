using NetworkMessages.FromServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.MissionRepresentatives;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace GoldGainForHorseKill
{
    public class MissionWithGoldGainForHorseKilling : MissionNetwork
    {
        public override void OnAgentHit(Agent affectedAgent, Agent affectorAgent, in MissionWeapon affectorWeapon, in Blow blow, in AttackCollisionData attackCollisionData)
        {
            if (affectedAgent!=null && affectorAgent!=null && affectedAgent.State == AgentState.Active && affectedAgent.IsMount && 
                affectedAgent.RiderAgent!=null && !affectorAgent.IsFriendOf(affectedAgent.RiderAgent) && !blow.IsFallDamage && affectedAgent.Health<=0)
            {
                    var gameMode = Mission.GetMissionBehavior<MissionMultiplayerGameModeBase>();
                    if (gameMode == null) return;

                    var perkHandler = MPPerkObject.GetPerkHandler(affectorAgent.MissionPeer);
                    MissionRepresentativeBase rep = affectorAgent.MissionPeer.Representative as MissionRepresentativeBase;

                    List<KeyValuePair<ushort, int>> goldChangeEventList = new List<KeyValuePair<ushort, int>>();
                    int goldOnKill = perkHandler != null ? perkHandler.GetGoldOnKill(0, 0) : 0;
                    if (goldOnKill > 0)
                    {
                        goldChangeEventList.Add(new KeyValuePair<ushort, int>(2048, goldOnKill));
                    }

                    if (goldChangeEventList.Count > 0 && !affectorAgent.MissionPeer.Peer.Communicator.IsServerPeer && affectorAgent.MissionPeer.Peer.Communicator.IsConnectionActive)
                    {
                        GameNetwork.BeginModuleEventAsServer(affectorAgent.MissionPeer.Peer);
                        GameNetwork.WriteMessage(new GoldGain(goldChangeEventList));
                        GameNetwork.EndModuleEventAsServer();
                    }

                    gameMode.ChangeCurrentGoldForPeer(affectorAgent.MissionPeer, rep.Gold + goldOnKill);
            }
        }

    }
}
