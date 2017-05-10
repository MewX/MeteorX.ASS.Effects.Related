using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteorX.AssTools.KaraokeApp.Model
{
    public class CompoundCurve : CompositeCurve
    {
        public override ASSPointF GetPointF(double t)
        {
            ASSPointF pt = new ASSPointF { X = 0, Y = 0, T = t };
            foreach (CompositeCurve_Element elem in Curves)
            {
                BaseCurve curve = elem.Curve;
                ASSPointF pt0 = elem.Curve.GetPointF((t - MinT) / (MaxT - MinT) * (curve.MaxT - curve.MinT) + curve.MinT);
                if (pt0 == null) return null;
                pt.X += pt0.X;
                pt.Y += pt0.Y;
            }
            return pt;
        }
    }
}
