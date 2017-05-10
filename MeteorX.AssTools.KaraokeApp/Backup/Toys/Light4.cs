using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteorX.AssTools.KaraokeApp.Toys
{
    class Light4
    {
        /// <summary>
        /// 返回3个assdraw : 中心, 扩散, 阴影
        /// </summary>
        /// <param name="rnd"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public List<String> Create(Random rnd, double scale)
        {
            double r1 = 5;
            double r2 = 80;
            Func<double, double, string> f1 = (a, b) => string.Format(" {0} {1}", (int)(Math.Round(a)), (int)(Math.Round(b)));
            string s = @"{\p1}m ";
            string s1 = @"{\p1}m ";
            string s2 = @"{\p1}m ";
            int diag = 3;
            bool first = true;
            for (int iag = 0; iag < 360; )
            {
                double ag = (double)iag / 360.0 * Math.PI * 2;
                r1 = Common.RandomDouble(rnd, 5, 12) * scale;
                r2 = Common.RandomDouble(rnd, 80, 120) * scale;
                double x = Math.Cos(ag) * r1;
                double y = Math.Sin(ag) * r1;
                double x3 = Math.Cos(ag) * r2;
                double y3 = Math.Sin(ag) * r2;
                iag += diag;
                ag = (double)iag / 360.0 * Math.PI * 2;
                double x1 = Math.Cos(ag) * r2;
                double y1 = Math.Sin(ag) * r2;
                double x2 = Math.Cos(ag) * r2 * 0.5;
                double y2 = Math.Sin(ag) * r2 * 0.5;
                s += f1(x, y);
                s1 += f1(x, y);
                if (Common.RandomBool(rnd, 0.8)) s2 += f1(x3, y3); else s2 += f1(x, y);
                if (first)
                {
                    s += " l";
                    s1 += " l";
                    s2 += " l";
                    first = false;
                }
                s += f1(x1, y1);
                s1 += f1(x2, y2);
                s2 += f1(x1, y1);
                iag += diag;
            }
            List<string> result = new List<string>();
            result.Add(s1);
            result.Add(s);
            result.Add(s2);
            return result;
        }
    }
}
