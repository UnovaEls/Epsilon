using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epsilon.Alpha.Elsword.Buff
{
    internal abstract class ManualBuff : Buff
    {
        protected ManualBuff(ManualCountBehavior countBehavior) : base()
        {
            this.CountBehavior = countBehavior;
        }

        public ManualCountBehavior CountBehavior { get; set; }
    }
}
