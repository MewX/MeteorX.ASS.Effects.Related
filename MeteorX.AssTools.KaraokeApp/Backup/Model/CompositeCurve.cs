using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteorX.AssTools.KaraokeApp.Model
{
    public class CompositeCurve : BaseCurve
    {
        protected internal class CompositeCurve_Element
        {
            public BaseCurve Curve { get; set; }
            public double StartT { get; set; }
            public double EndT { get; set; }

            public override string ToString()
            {
                return string.Format("{0} {1}", StartT, EndT);
            }
        }

        protected List<CompositeCurve_Element> Curves = new List<CompositeCurve_Element>();

        double Curves_MinT = 1e8;
        CompositeCurve_Element Curves_Min, Curves_Max;
        double Curves_MaxT = -1e8;

        public void AddCurve(double startt, double endt, BaseCurve curve)
        {
            CompositeCurve_Element elem = new CompositeCurve_Element { StartT = startt, EndT = endt, Curve = curve };
            Curves.Add(elem);
            if (Curves_MaxT < endt)
            {
                Curves_MaxT = endt;
                Curves_Max = elem;
            }
            if (Curves_MinT > startt)
            {
                Curves_MinT = startt;
                Curves_Min = elem;
            }
        }

        public override ASSPointF GetPointF(double t)
        {
            foreach (CompositeCurve_Element elem in Curves)
                if (Common.InRange(elem.StartT, elem.EndT, t))
                {
                    BaseCurve curve = elem.Curve;
                    ASSPointF pt = curve.GetPointF((t - elem.StartT) / (elem.EndT - elem.StartT) * (curve.MaxT - curve.MinT) + curve.MinT);
                    pt.T = (pt.T - curve.MinT) / (curve.MaxT - curve.MinT) * (elem.EndT - elem.StartT) + elem.StartT;
                    return pt;
                }
            return null;
            if (t < Curves_MinT)
                return Curves_Min.Curve.GetMinPointF();
            if (t > Curves_MaxT)
                return Curves_Max.Curve.GetMaxPointF();
            throw new Exception();
        }
    }
}
