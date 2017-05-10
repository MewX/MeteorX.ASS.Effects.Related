using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MeteorX.AssTools.KaraokeApp
{
    static class StaticCommon
    {
        internal static string t(this string s)
        {
            return s.Replace("{", "").Replace("}", "");
        }
    }

    public class Common
    {
        /// <summary>
        /// for ass draw
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string Coor2Str(double x, double y)
        {
            return " " + ((int)x).ToString() + " " + ((int)y).ToString();
        }

        public static bool IsLetter(char ch)
        {
            ch = (ch + "").ToLower()[0];
            return Char.IsLetter(ch) && (int)ch >= (int)'a' && (int)ch <= (int)'z';
        }

        public static double Min(double a, double b)
        {
            return (a < b) ? a : b;
        }

        public static double Max(double a, double b)
        {
            return (a < b) ? b : a;
        }

        public static double scaleDouble(double lo, double hi, double sc)
        {
            if (sc < 0) sc = 0;
            if (sc > 1) sc = 1;
            return lo + (hi - lo) * sc;
        }

        public static bool InRange(double lo, double hi, double x)
        {
            return (x >= lo) && (hi >= x);
        }
        /// <summary>
        /// String Format : BBGGRR
        /// </summary>
        /// <param name="startColor"></param>
        /// <param name="endColor"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static string scaleColor(string startColor, string endColor, double scale)
        {
            if (scale < 0) scale = 0;
            if (scale > 1) scale = 1;
            double b0 = Hex2Dec(startColor.Substring(0, 2));
            double g0 = Hex2Dec(startColor.Substring(2, 2));
            double r0 = Hex2Dec(startColor.Substring(4, 2));
            double b1 = Hex2Dec(endColor.Substring(0, 2));
            double g1 = Hex2Dec(endColor.Substring(2, 2));
            double r1 = Hex2Dec(endColor.Substring(4, 2));
            return ToBBGGRR(b0 + (b1 - b0) * scale, g0 + (g1 - g0) * scale, r0 + (r1 - r0) * scale);
        }

        public static string scaleColor(string col1, string col2, string col3, double scale)
        {
            if (scale < 0) scale = 0;
            if (scale > 1) scale = 1;
            if (scale <= 0.5) return scaleColor(col1, col2, scale * 2.0);
            else return scaleColor(col2, col3, (scale - 0.5) * 2.0);
        }

        public static string scaleAlpha(string startAlpha, string endAlpha, double scale)
        {
            double a0 = Hex2Dec(startAlpha);
            double a1 = Hex2Dec(endAlpha);
            return ToHex2(a0 + (a1 - a0) * scale);
        }

        public static string scaleColor(string startColor, string endColor, double lo, double hi, double k)
        {
            return scaleColor(startColor, endColor, (double)(k - lo) / (double)(hi - lo));
        }

        public static string scaleColor(ASSColor startColor, ASSColor endColor, double scale)
        {
            return scaleColor(startColor.ToColString(), endColor.ToColString(), scale);
        }

        public static string adjustBrightness(string color, int brightness)
        {
            double b0 = Hex2Dec(color.Substring(0, 2));
            double g0 = Hex2Dec(color.Substring(2, 2));
            double r0 = Hex2Dec(color.Substring(4, 2));
            return ToBBGGRR(b0 * brightness / 255.0, g0 * brightness / 255.0, r0 * brightness / 255.0);
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

        public static string ToHex2(double x)
        {
            string s = ((int)x).ToString("x").ToUpper();
            if (s.Length == 1) s = "0" + s;
            return s;
        }

        public static int Hex2Dec(string hex)
        {
            int a = 0;
            hex = hex.ToUpper();
            for (int i = 0; i < hex.Length; i++)
                if (char.IsDigit(hex[i]))
                    a = a * 16 + Convert.ToInt32(hex[i].ToString());
                else
                    a = a * 16 + ((int)(hex[i]) - (int)('A') + 10);
            return a;
        }

        public static string GetDialog(double start, double end, string style, string text)
        {
            return string.Format("Dialogue: 0,{0},{1},{3},NTP,0000,0000,0000,,{2}\r\n", Common.ToTimeString(start), Common.ToTimeString(end), text, style);
        }

        public static string GetDialog(double start, double end, string style, string text, int layer)
        {
            return string.Format("Dialogue: {4},{0},{1},{3},NTP,0000,0000,0000,,{2}\r\n", Common.ToTimeString(start), Common.ToTimeString(end), text, style, layer);
        }

        public static string ToTimeString(double time)
        {
            if (time < 0) time = 0;
            int hour = (int)(time / 3600);
            time -= hour * 3600;
            int minute = (int)(time / 60);
            time -= minute * 60;
            int second = (int)time;
            int milsec = (int)Math.Round(((time - second) * 100));

            if (milsec >= 100)
            {
                second += milsec / 100;
                milsec %= 100;
                if (second >= 60)
                {
                    minute += second / 60;
                    second %= 60;
                    if (minute >= 60)
                    {
                        hour += minute / 60;
                        minute %= 60;
                    }
                }
            }

            return string.Format("{0}:{1:00}:{2:00}.{3:00}", hour, minute, second, milsec);
        }

        public static double ToTime(string timeString)
        {
            string[] tmp1 = timeString.Split(':');
            return Convert.ToDouble(tmp1[0]) * 3600 + Convert.ToDouble(tmp1[1]) * 60 + Convert.ToDouble(tmp1[2]);
        }

        public static int Find(List<string> list, string tar)
        {
            if (list == null || tar == null) return -1;
            for (int i = 0; i < list.Count; i++)
                if (list[i] == tar) return i;
            return -1;
        }

        public static SizeF MeasureString(string s, Font font)
        {
            Bitmap bm = new Bitmap(100, 100);
            Graphics g = Graphics.FromImage(bm);
            SizeF sz = g.MeasureString(s, font);
            g.Dispose();
            bm.Dispose();
            return sz;
        }

        public static double RandomDouble(Random rnd, double lo, double hi)
        {
            return lo + (hi - lo) * rnd.NextDouble();
        }

        public static double RandomDouble_Gauss(Random rnd, double lo, double hi)
        {
            return RandomDouble_Gauss(rnd, lo, hi, 6);
        }

        public static double RandomDouble_Gauss(Random rnd, double lo, double hi, int g)
        {
            double sum = 0;
            for (int i = 0; i < g; i++)
                sum += RandomDouble(rnd, lo, hi);
            return sum / g;
        }

        public static int RandomInt(Random rnd, int lo, int hi)
        {
            if (lo > hi) return RandomInt(rnd, hi, lo);
            else return lo + rnd.Next(hi - lo + 1);
        }

        public static int RandomInt_Gauss(Random rnd, int mid, int r)
        {
            int sum = 0;
            for (int i = 0; i < 6; i++)
                sum += RandomInt(rnd, mid - r, mid + r);
            return sum / 6;
        }

        /// <summary>
        /// 在一个长为l的序列(0, 1, ..., l-1)中, 按以c为峰值的正态分布产生一个随机序号
        /// </summary>
        /// <param name="rnd"></param>
        /// <param name="mid"></param>
        /// <param name="r"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <returns></returns>
        public static int RandomInt_Gauss2(Random rnd, int l, int c)
        {
            if (c < 0 || c >= l) throw new Exception();
            while (true)
            {
                int x = RandomInt_Gauss(rnd, c, Math.Max(l - c - 1, c));
                if (x >= 0 && x < l) return x;
            }
        }

        public static ASSPoint RandomPoint(Random rnd, double xl, double xh, double yl, double yh)
        {
            return new ASSPoint
            {
                X = (int)(RandomDouble(rnd, xl, xh)),
                Y = (int)(RandomDouble(rnd, yl, yh))
            };
        }

        public static ASSPointF RandomPointF(Random rnd, double xl, double xh, double yl, double yh)
        {
            return new ASSPointF
            {
                X = (RandomDouble(rnd, xl, xh)),
                Y = (RandomDouble(rnd, yl, yh))
            };
        }

        public static bool RandomBool(Random rnd, double p)
        {
            return rnd.NextDouble() < p;
        }

        public static int RandomSig(Random rnd)
        {
            return RandomBool(rnd, 0.5) ? 1 : -1;
        }

        public static ASSColor RandomColor(Random rnd, int index)
        {
            return new ASSColor { A = 0, Index = index, R = rnd.Next(256), G = rnd.Next(256), B = rnd.Next(256) };
        }

        public static ASSColor RandomColor(Random rnd, int index, ASSColor color1, ASSColor color2)
        {
            return new ASSColor { A = RandomInt(rnd, color1.A, color2.A), Index = index, R = RandomInt(rnd, color1.R, color2.R), G = RandomInt(rnd, color1.G, color2.G), B = RandomInt(rnd, color1.B, color2.B) };
        }

        static public double Sqr(double x)
        {
            return x * x;
        }

        static public double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Sqr(x1 - x2) + Sqr(y1 - y2));
        }

        /// <summary>
        /// ASS coordinates, NOT Cartesian coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="ox"></param>
        /// <param name="oy"></param>
        /// <returns></returns>
        static public double GetAngle(double x, double y, double ox, double oy)
        {
            double r = GetDistance(x, y, ox, oy);
            if (r == 0) return 0;
            double a = Math.Acos((x - ox) / r);
            if (y > oy) a = Math.PI * 2.0 - a;
            return a;
        }
    }
}
