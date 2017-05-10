using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    abstract class BaseAnime3 : BaseAnime2
    {
        public string FontName { get; set; }

        public byte FontCharset { get; set; }

        public int FontHeight { get; set; }

        public string c(int index, string col1, string col2)
        {
            return c(index, col1) + t(c(index, col2).t());
        }

        public string CreateCircle(double rin, double rout)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"{\p4}");
            rin *= 8;
            rout *= 8;
            Func<double, double, string> f1 = (x, y) => string.Format(" {0} {1}", (int)x, (int)y);
            double d43 = 4.0 / 3.0;
            sb.Append(" m" + f1(0, -rout));
            sb.Append(" b" + f1(rout * d43, -rout) + f1(rout * d43, rout) + f1(0, rout));
            sb.Append(" b" + f1(-rout * d43, rout) + f1(-rout * d43, -rout) + f1(0, -rout) + " c");
            sb.Append(" m" + f1(0, -rin));
            sb.Append(" b" + f1(-rin * d43, -rin) + f1(-rin * d43, rin) + f1(0, rin));
            sb.Append(" b" + f1(rin * d43, rin) + f1(rin * d43, -rin) + f1(0, -rin) + " c");
            return sb.ToString();
        }

        public override System.Drawing.Size GetSize(string s)
        {
            return MeasureString(FontName, FontCharset, FontHeight, FontSpace, s);
        }
    }
}
