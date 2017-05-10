using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MeteorX.AssTools.KaraokeApp.Model
{
    public class Bezier : BaseCurve
    {
        public ASSPoint P0, P1, P2, P3;

        public Bezier(ASSPoint p0, ASSPoint p1, ASSPoint p2, ASSPoint p3)
        {
            this.P0 = p0;
            this.P1 = p1;
            this.P2 = p2;
            this.P3 = p3;

            MinT = 0;
            MaxT = 1;
        }

        public override ASSPointF GetPointF(double t)
        {
            ASSPointF pt = new ASSPointF
            {
                X = (Math.Pow(1 - t, 3) * P0.X + 3.0 * Math.Pow(1 - t, 2) * t * P1.X + 3.0 * (1 - t) * Math.Pow(t, 2) * P2.X + Math.Pow(t, 3) * P3.X),
                Y = (Math.Pow(1 - t, 3) * P0.Y + 3.0 * Math.Pow(1 - t, 2) * t * P1.Y + 3.0 * (1 - t) * Math.Pow(t, 2) * P2.Y + Math.Pow(t, 3) * P3.Y),
                T = t
            };
            return pt;
        }

        public ASSPoint Get(float t)
        {
            ASSPoint pt = new ASSPoint
            {
                X = (int)(Math.Pow(1 - t, 3) * P0.X + 3.0 * Math.Pow(1 - t, 2) * t * P1.X + 3.0 * (1 - t) * Math.Pow(t, 2) * P2.X + Math.Pow(t, 3) * P3.X),
                Y = (int)(Math.Pow(1 - t, 3) * P0.Y + 3.0 * Math.Pow(1 - t, 2) * t * P1.Y + 3.0 * (1 - t) * Math.Pow(t, 2) * P2.Y + Math.Pow(t, 3) * P3.Y)
            };
            return pt;
        }

        public List<ASSPoint> Create(float step)
        {
            List<ASSPoint> result = new List<ASSPoint>();
            float t = 0;
            while (true)
            {
                ASSPoint pt = new ASSPoint
                {
                    X = (int)(Math.Pow(1 - t, 3) * P0.X + 3.0 * Math.Pow(1 - t, 2) * t * P1.X + 3.0 * (1 - t) * Math.Pow(t, 2) * P2.X + Math.Pow(t, 3) * P3.X),
                    Y = (int)(Math.Pow(1 - t, 3) * P0.Y + 3.0 * Math.Pow(1 - t, 2) * t * P1.Y + 3.0 * (1 - t) * Math.Pow(t, 2) * P2.Y + Math.Pow(t, 3) * P3.Y)
                };
                if (result.Count == 0 || result[result.Count - 1].X != pt.X || result[result.Count - 1].Y != pt.Y)
                    result.Add(pt);

                if (t >= 1) break;
                t += step;
                if (t > 1) t = 1;
            }
            return result;
        }
    }
}
