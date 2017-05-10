using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteorX.AssTools.KaraokeApp.Model
{
    public class Circle2 : BaseCurve
    {
        public double X0 { get; set; }

        public double Y0 { get; set; }

        public double A { get; set; }

        public double B { get; set; }

        public double Theta { get; set; }

        public double dTheta { get; set; }

        public bool NormTheta { get; set; }

        public Circle2()
        {
            dTheta = 0;
            NormTheta = true;
        }

        public override ASSPointF GetPointF(double t)
        {
            double bakt = t;
            double cx0 = A * Math.Cos(t);
            double cy0 = B * Math.Sin(t);
            double cx1 = X0 + cx0 * Math.Cos(Theta + dTheta * t) + cy0 * Math.Sin(Theta + dTheta * t);
            double cy1 = Y0 + -cx0 * Math.Sin(Theta + dTheta * t) + cy0 * Math.Cos(Theta + dTheta * t);
            if (NormTheta)
            {
                while (t < 0) t += Math.PI * 2;
                while (t >= Math.PI * 2) t -= Math.PI * 2;
            }
            return new ASSPointF { X = cx1, Y = cy1, T = bakt, Theta = t };
        }
    }
}
