using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteorX.AssTools.KaraokeApp
{
    public class ASSColor
    {
        public int A { get; set; }

        public int R { get; set; }

        public int G { get; set; }

        public int B { get; set; }

        public int Index { get; set; }

        public string ToBGR()
        {
            return "\\" + Index + "c&H" + ToBBGGRR(B, G, R) + "&";
        }

        public string ToColString()
        {
            return ToBBGGRR(B, G, R);
        }

        public string ToA()
        {
            return "\\" + Index + "a&H" + ToHex2(A) + "&";
        }

        public override string ToString()
        {
            return ToA() + ToBGR();
        }

        public static string ToHex2(double x)
        {
            string s = ((int)x).ToString("x").ToUpper();
            if (s.Length == 1) s = "0" + s;
            return s;
        }

        public static ASSColor FromRGB(int index, double r, double g, double b)
        {
            return FromBBGGRR(index, ToBBGGRR(b, g, r));
        }

        public static ASSColor FromBBGGRR(int index, string s)
        {
            int b0 = Common.Hex2Dec(s.Substring(0, 2));
            int g0 = Common.Hex2Dec(s.Substring(2, 2));
            int r0 = Common.Hex2Dec(s.Substring(4, 2));
            return new ASSColor { A = 0, R = r0, G = g0, B = b0, Index = index };
        }

        public static string HtmlToASS(string s)
        {
            int b0 = Common.Hex2Dec(s.Substring(0, 2));
            int g0 = Common.Hex2Dec(s.Substring(2, 2));
            int r0 = Common.Hex2Dec(s.Substring(4, 2));
            return new ASSColor { A = 0, R = b0, G = g0, B = r0, Index = 1 }.ToColString();
        }

        public static string ToBBGGRR(double b, double g, double r)
        {
            if (b < 0) b = 0;
            if (b > 255) b = 255;
            if (g < 0) g = 0;
            if (g > 255) g = 255;
            if (r < 0) r = 0;
            if (r > 255) r = 255;
            return ToHex2(b) + ToHex2(g) + ToHex2(r);
        }
    }
}
