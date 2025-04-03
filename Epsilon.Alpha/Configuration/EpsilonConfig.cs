using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epsilon.Alpha.Configuration
{
    internal class EpsilonConfig
    {
        public EpsilonConfig()
        {
            this.ElswordTitle = string.Empty;
            this.SpecialActiveSkillKeys = new HashSet<Keys>();
            this.AwakeningKeys = new HashSet<Keys>();
            this.OtherTitleArrowKeys = new HashSet<Keys>();
        }

        public string ElswordTitle { get; set; }

        public double BuffLocationX { get; set; }

        public double BuffLocationY { get; set; }

        public HashSet<Keys> SpecialActiveSkillKeys { get; set; }

        public HashSet<Keys> AwakeningKeys { get; set; }

        public Keys TitleSwapKey { get; set; }

        public Keys NightParadeArrowKey { get; set; }

        public Keys FreedShadowArrowKey { get; set; }

        public Keys TheSettingSunArrowKey { get; set; }

        public HashSet<Keys> OtherTitleArrowKeys { get; set; }

        public Keys ResetTranscendenceKey { get; set; }
    }
}
