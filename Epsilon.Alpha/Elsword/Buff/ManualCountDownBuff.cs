using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epsilon.Alpha.Elsword.Buff
{
    internal class ManualCountDownBuff : CountDownBuff
    {
        private ManualCountBehavior _behavior;

        public ManualCountDownBuff(ManualCountBehavior behavior)
        {
            _behavior = behavior;
        }

        public override double GetTime()
        {
            double secondsSince = GetTimeSinceReset();

            if (secondsSince < 0)
                return 0;

            double inverted = this.MaxCounter - (int)secondsSince;

            if (_behavior == ManualCountBehavior.Continue)
                return inverted;

            return Math.Max(inverted, 0);
        }
    }
}
