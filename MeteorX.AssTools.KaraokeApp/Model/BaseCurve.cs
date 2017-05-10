using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteorX.AssTools.KaraokeApp.Model
{
    public abstract class BaseCurve
    {
        public double MinT { get; set; }

        public double MaxT { get; set; }

        public abstract ASSPointF GetPointF(double t);

        protected static Random rnd = new Random();

        public ASSPointF GetMinPointF()
        {
            return GetPointF(MinT);
        }

        public ASSPointF GetMaxPointF()
        {
            return GetPointF(MaxT);
        }

        public string Move(double t0, double t1)
        {
            ASSPointF p0 = GetPointF(t0);
            ASSPointF p1 = GetPointF(t1);
            if (p0 == null || p1 == null) throw new Exception();
            return ASSEffect.move(p0, p1);
        }

        public List<ASSPointF> GetPath_DT(double dt)
        {
            List<ASSPointF> result = new List<ASSPointF>();
            for (double t = MinT; t <= MaxT; t += dt)
                result.Add(GetPointF(t));
            ASSPointF ed = GetPointF(MaxT);
            if (ed != null && result[result.Count - 1].GetDis(ed) > 1)
                result.Add(ed);
            return result;
        }

        public List<ASSPointF> GetPath_Dis(double mindis, double maxdis)
        {
            List<ASSPointF> result = new List<ASSPointF>();
            ASSPointF last = GetPointF(MinT);
            result.Add(last);
            double dt = (MaxT - MinT) / 100.0;
            double t = MinT;
            bool b0 = false;
            bool b1 = false;
            ASSPointF ed = GetPointF(MaxT);
            while (t <= MaxT)
            {
                ASSPointF pt = GetPointF(t + dt);
                if (pt == null)
                    if (ed.GetDis(last) <= maxdis)
                        break;
                    else
                    {
                        dt = dt / Math.Sqrt(maxdis / mindis);
                        continue;
                    }
                double dis = pt.GetDis(last);
                if (dis > maxdis)
                {
                    dt = dt / Math.Sqrt(maxdis / mindis);
                    b0 = true;
                    b1 = false;
                    continue;
                }
                else if (dis < mindis)
                {
                    if (b0)
                    {
                    }
                    else
                    {
                        dt = dt * Math.Sqrt(maxdis / mindis);
                        b0 = false;
                        b1 = true;
                        continue;
                    }
                }

                result.Add(pt);
                last = pt;
                t += dt;
                b0 = b1 = false;
                //Console.WriteLine("{0} {1}", t, pt);
            }
            if (result[result.Count - 1].GetDis(ed) > 1)
                result.Add(ed);
            return result;
        }
    }
}
