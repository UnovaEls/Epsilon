using Epsilon.Alpha.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epsilon.Alpha.Elsword.Buff
{
    internal interface IBuff : IOverlayDraw
    {
        double GetTime();

        Image? Icon { get; set; }

        int MaxCounter { get; set; }

        int Warn { get; set; }

        Color GoColor { get; set; }

        Color WarnColor { get; set; }

        Color StopColor { get; set; }
    }
}
