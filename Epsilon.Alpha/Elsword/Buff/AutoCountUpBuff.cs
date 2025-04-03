using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epsilon.Alpha.Elsword.Buff
{
    internal class AutoCountUpBuff : CountUpBuff
    {
        public override double GetTime()
        {
            double secondsSince = GetTimeSinceReset();

            if (secondsSince < 0)
                return 0;

            return secondsSince % this.MaxCounter;
        }
    }
}
