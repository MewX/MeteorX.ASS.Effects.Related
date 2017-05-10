using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteorX.AssTools.KaraokeApp.Model
{
    class BlurCurve : BaseCurve
    {
        private BaseCurve OriginalCurve { get; set; }

        public double BlurRange { get; set; }

        public BlurCurve(BaseCurve oriCurve, double blurRange)
        {
            this.OriginalCurve = oriCurve;
            this.BlurRange = blurRange;

            this.MinT = oriCurve.MinT;
            this.MaxT = oriCurve.MaxT;
        }

        static Random rnd = new Random();

        public override ASSPointF GetPointF(double t)
        {
            List<ASSPointF> tmp = new List<ASSPointF>();
            while (tmp.Count < 10)
            {
                ASSPointF p = OriginalCurve.GetPointF(Common.RandomDouble(rnd, t - BlurRange, t + BlurRange));
                if (p != null) tmp.Add(p);
            }
            ASSPointF pt = new ASSPointF { X = 0, Y = 0, T = t };
            foreach (ASSPointF tmppt in tmp)
            {
                pt.X += tmppt.X;
                pt.Y += tmppt.Y;
            }
            pt.X /= (double)tmp.Count;
            pt.Y /= (double)tmp.Count;
            return pt;
        }
    }
}
