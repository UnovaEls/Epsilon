using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epsilon.Alpha.Elsword.Buff
{
    internal abstract class CountDownBuff : Buff
    {
        public override Color GetColor()
        {
            double time = GetTime();

            if (time <= 0)
                return this.GoColor;
            else if (time <= this.Warn)
                return this.WarnColor;

            return this.StopColor;
        }
    }
}
