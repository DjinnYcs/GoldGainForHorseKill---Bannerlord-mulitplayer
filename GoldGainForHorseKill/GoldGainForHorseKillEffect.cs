using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects;

namespace GoldGainForHorseKill
{
    public class GoldGainOnHorseKillEffect : MPPerkEffect
    {
        protected static string StringType = "GoldGainOnHorseKill";

        private int _value;

        protected GoldGainOnHorseKillEffect()
        {
        }

        protected override void Deserialize(XmlNode node)
        {
            this.IsDisabledInWarmup = node?.Attributes?["is_disabled_in_warmup"]?.Value?.ToLower() == "true";
            string s = node?.Attributes?["value"]?.Value;
            if (s == null || !int.TryParse(s, out this._value))
                Debug.Print("provided 'value' is invalid", "GoldGainForHorseKill\\GoldGainForHorseKill\\Class1.cs", nameof(Deserialize), 31);
        }

        public override int GetGoldOnKill(float attackerValue, float victimValue)
        {
            if (victimValue != 0) return 0;

            return this._value;
        }

    }
}
