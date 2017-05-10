using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteorX.AssTools.KaraokeApp.Model
{
    public class Line : BaseCurve
    {
        public double X0, X1, Y0, Y1;

        public double Acc;

        public Line()
        {
            MinT = 0;
            MaxT = 1;
            Acc = 1;
        }

        public override ASSPointF GetPointF(double t)
        {
            return new ASSPointF { X = X0 + (X1 - X0) * Math.Pow(t, Acc), Y = Y0 + (Y1 - Y0) * Math.Pow(t, Acc), T = t };
        }

        /// <summary>
        /// 在 矩形 (x0, y0) (x1, y1) 内以速度speed (pixel/sec)运动, 时间范围为[tstart, tend]
        /// </summary>
        public static CompositeCurve Create1(double x0, double y0, double x1, double y1, double speed, double tstart, double tend)
        {
            CompositeCurve curve = new CompositeCurve { MinT = tstart, MaxT = tend };
            double lastx = Common.RandomDouble(rnd, x0, x1);
            double lasty = Common.RandomDouble(rnd, y0, y1);
            double tnow = tstart;
            while (tnow + 1e-8 < tend)
            {
                double newx = Common.RandomDouble(rnd, x0, x1);
                double newy = Common.RandomDouble(rnd, y0, y1);
                double dis = Common.GetDistance(lastx, lasty, newx, newy);
                double tcost = dis / speed;
                if (tnow + tcost > tend)
                {
                    //TODO
                }
                curve.AddCurve(tnow, tnow + tcost, new Line { X0 = lastx, Y0 = lasty, X1 = newx, Y1 = newy });
                tnow += tcost;
                lastx = newx;
                lasty = newy;
            }
            return curve;
        }
    }
}
