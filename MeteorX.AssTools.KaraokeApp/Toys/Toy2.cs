using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteorX.AssTools.KaraokeApp.Toys
{
    class Toy2
    {
        public double RA { get; set; }

        public double RB { get; set; }

        public double X { get; set; }

        public double Y { get; set; }

        public double V { get; set; }

        public string Create(Random rnd)
        {
            string s = "";
            for (int i = 0; i < V; i++)
            {
                double ag = Common.RandomDouble(rnd, 0, 2 * Math.PI);
                double ptx = Math.Cos(ag) * RA + X;
                double pty = Math.Sin(ag) * RB + Y;
                if (i == 0) s += "m";
                if (i == 1) s += " l";
                s += string.Format(" {0} {1}", (int)ptx, (int)pty);
            }
            return s;
        }
    }
}
