using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteorX.AssTools.KaraokeApp.Model
{
    public class Sine : BaseCurve
    {
        public double A { get; set; }

        public double X0 { get; set; }

        public double Y0 { get; set; }

        public double XScale { get; set; }

        public override ASSPointF GetPointF(double t)
        {
            return new ASSPointF { X = X0 + XScale * (t - MinT), Y = Y0 + A * Math.Sin(t), T = t };
        }

        public static Sine Create(double x1, double y1, double x2, double y2)
        {
            if (y1 < y2)
                return new Sine() { A = (y2 - y1) * 0.5, MinT = Math.PI * -0.5, MaxT = Math.PI * 0.5, X0 = x1, Y0 = (y1 + y2) * 0.5, XScale = (x2 - x1) / Math.PI };
            else
                return new Sine() { A = (y1 - y2) * 0.5, MinT = Math.PI * 0.5, MaxT = Math.PI * 1.5, X0 = x1, Y0 = (y1 + y2) * 0.5, XScale = (x2 - x1) / Math.PI };
        }
    }
}
