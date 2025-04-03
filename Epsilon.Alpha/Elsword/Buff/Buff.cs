using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epsilon.Alpha.Elsword.Buff
{
    internal abstract class Buff : IBuff
    {
        protected DateTime _reset;

        protected Buff()
        {
            _reset = DateTime.MaxValue;
        }

        protected double GetTimeSinceReset()
        {
            return (DateTime.Now - _reset).TotalSeconds;
        }

        public Image? GetIcon()
        {
            return this.Icon;
        }

        public string GetText()
        {
            return ((int)GetTime()).ToString();
        }

        public void Reset()
        {
            _reset = DateTime.Now;
        }

        public abstract Color GetColor();

        public abstract double GetTime();

        public Image? Icon { get; set; }

        public int MaxCounter { get; set; }

        public int Warn { get; set; }

        public Color GoColor { get; set; }

        public Color WarnColor { get; set; }

        public Color StopColor { get; set; }
    }
}
