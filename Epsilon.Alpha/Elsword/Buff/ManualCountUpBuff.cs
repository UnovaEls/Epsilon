using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epsilon.Alpha.Elsword.Buff
{
    internal class ManualCountUpBuff : CountUpBuff
    {
        private ManualCountBehavior _behavior;

        public ManualCountUpBuff(ManualCountBehavior behavior)
        {
            _behavior = behavior;
        }

        public override double GetTime()
        {
            double secondsSince = GetTimeSinceReset();

            if (secondsSince < 0)
                return this.MaxCounter;

            if (_behavior == ManualCountBehavior.Continue)
                return secondsSince;

            return Math.Min(secondsSince, this.MaxCounter);
        }
    }
}
