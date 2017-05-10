using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteorX.AssTools.KaraokeApp.Model
{
    class Brown : CompositeCurve
    {
        public double Speed { get; set; }

        public double X0 { get; set; }

        public double Y0 { get; set; }

        public double R { get; set; }

        void Calculate()
        {
            double x1 = X0;
            double y1 = Y0;
            double ti = MinT;
            while (ti <= MaxT)
            {
                double ag = Common.RandomDouble(rnd, 0, Math.PI * 2);
                double x2 = X0 + R * Math.Cos(ag);
                double y2 = Y0 + R * Math.Sin(ag);
                double dis = Common.GetDistance(x1, y1, x2, y2);
                double newt = ti + dis / Speed;
                bool isEnd = false;
                if (newt > MaxT)
                {
                    isEnd = true;
                    double r2 = R / ((newt - ti) / (MaxT - ti));
                    x2 = X0 + r2 * Math.Cos(ag);
                    y2 = Y0 + r2 * Math.Sin(ag);
                    newt = MaxT;
                }
                AddCurve(ti, newt, new Line { X0 = x1, Y0 = y1, X1 = x2, Y1 = y2 });
                if (isEnd) break;
                ti = newt;
                x1 = x2;
                y1 = y2;
            }
        }

        public override ASSPointF GetPointF(double t)
        {
            if (Curves == null || Curves.Count == 0) Calculate();
            return base.GetPointF(t);
        }
    }
}
