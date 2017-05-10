using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteorX.AssTools.KaraokeApp.Model
{
    public class Circle : BaseCurve
    {
        public double X0 { get; set; }

        public double Y0 { get; set; }

        public double R { get; set; }

        public override ASSPointF GetPointF(double t)
        {
            return new ASSPointF { X = X0 + R * Math.Cos(t), Y = Y0 + R * Math.Sin(t), T = t };
        }

        public static Circle Create(double x1, double y1, double x2, double y2, bool clockwise)
        {
            double dis = Common.GetDistance(x1, y1, x2, y2);
            double xmid = (x1 + x2) * 0.5;
            double ymid = (y1 + y2) * 0.5;
            double ag1 = Math.PI * 2 - Common.GetAngle(x1, y1, xmid, ymid);
            double ag2 = Math.PI * 2 - Common.GetAngle(x2, y2, xmid, ymid);
            if (clockwise && ag1 > ag2)
                while (ag1 > ag2) ag1 -= Math.PI * 2;
            return new Circle { X0 = xmid, Y0 = ymid, R = dis * 0.5, MinT = ag1, MaxT = ag2 };
        }
    }
}
