using Epsilon.Alpha.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epsilon.Alpha.Elsword.Buff
{
    internal class ElswordBuffs
    {
        public ElswordBuffs()
        {
            this.FreedShadow = new ManualCountDownBuff(ManualCountBehavior.Stop) { Icon = Resources.FreedShadow, MaxCounter = 60, Warn = 5, GoColor = Color.Green, WarnColor = Color.Yellow, StopColor = Color.Red };
            this.NightParade = new ManualCountDownBuff(ManualCountBehavior.Stop) { Icon = Resources.NightParade, MaxCounter = 25, Warn = 5, GoColor = Color.Green, WarnColor = Color.Yellow, StopColor = Color.Red };
            this.TheSettingSun = new ManualCountDownBuff(ManualCountBehavior.Stop) { Icon = Resources.TheSettingSun, MaxCounter = 30, Warn = 5, GoColor = Color.Green, WarnColor = Color.Yellow, StopColor = Color.Red };
            this.Transcendence = new AutoCountDownBuff() { Icon = Resources.Transcendence, MaxCounter = 20, Warn = 5, GoColor = Color.CornflowerBlue, WarnColor = Color.CornflowerBlue, StopColor = Color.CornflowerBlue };
        }

        public ManualCountDownBuff FreedShadow { get; }

        public ManualCountDownBuff NightParade { get; }

        public ManualCountDownBuff TheSettingSun { get; }

        public AutoCountDownBuff Transcendence { get; }
    }
}
