using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteorX.AssTools.KaraokeApp
{
    public class ASSEffect : Common
    {
        public static string r()
        {
            return @"{\r}";
        }

        public static string fs(int z)
        {
            return @"{\fs" + z + "}";
        }

        public static string p(int b)
        {
            return @"{\p" + b + "}";
        }

        public static string ybord(int b)
        {
            return @"{\ybord" + b + "}";
        }

        public static string xbord(int b)
        {
            return @"{\xbord" + b + "}";
        }

        public static string ybord(double b)
        {
            return @"{\ybord" + b.ToString("0.00") + "}";
        }

        public static string xbord(double b)
        {
            return @"{\xbord" + b.ToString("0.00") + "}";
        }

        public static string bord(int b)
        {
            return @"{\bord" + b + "}";
        }

        public static string shad(int b)
        {
            return @"{\shad" + b + "}";
        }

        public static string bord(double b)
        {
            return @"{\bord" + b.ToString("0.00") + "}";
        }

        public static string be(int b)
        {
            return @"{\be" + b + "}";
        }

        public static string blur(int b)
        {
            return @"{\blur" + b + "}";
        }

        public static string blur(double b)
        {
            return @"{\blur" + b.ToString("0.00") + "}";
        }

        public static string frz(int ag)
        {
            return @"{\frz" + ag + "}";
        }

        public static string fry(int ag)
        {
            return @"{\fry" + ag + "}";
        }

        public static string frx(int ag)
        {
            return @"{\frx" + ag + "}";
        }

        public static string fsc(int sc)
        {
            return fsc(sc, sc);
        }

        public static string fsc(int x, int y)
        {
            return @"{\fscx" + x + @"\fscy" + y + "}";
        }

        public static string fscx(int x)
        {
            return @"{\fscx" + x + "}";
        }

        public static string fscx(double x)
        {
            return fscx((int)x);
        }

        public static string fscy(int y)
        {
            return @"{\fscy" + y + "}";
        }

        public static string an(int index)
        {
            return @"{\an" + index + "}";
        }

        public static string K(double dur)
        {
            return @"{\K" + (int)Math.Round(dur * 100) + "}";
        }

        public static string fad(double tin, double tout)
        {
            return @"{\fad(" + (int)Math.Round(tin * 1000) + "," + (int)Math.Round(tout * 1000) + ")}";
        }

        public static string c(int number, string color)
        {
            return @"{\" + number + "c&H" + color + "&}";
        }

        public static string c(ASSColor color)
        {
            return "{" + color.ToString() + "}";
        }

        public static string a(int number, string color)
        {
            return @"{\" + number + "a&H" + color + "&}";
        }

        public static string move(int x1, int y1, int x2, int y2)
        {
            return @"{\move(" + x1 + "," + y1 + "," + x2 + "," + y2 + @")}";
        }

        public static string move(ASSPointF p1, ASSPointF p2)
        {
            return move(p1.X, p1.Y, p2.X, p2.Y);
        }

        public static string move(double x1, double y1, double x2, double y2)
        {
            return @"{\move(" + (int)(Math.Round(x1)) + "," + (int)(Math.Round(y1)) + "," + (int)(Math.Round(x2)) + "," + (int)(Math.Round(y2)) + @")}";
        }

        public static string move(double x1, double y1, double x2, double y2, double start, double end)
        {
            return @"{\move(" + x1.ToString("0.00") + "," + y1.ToString("0.00") + "," + x2.ToString("0.00") + "," + y2.ToString("0.00") + "," + (int)(Math.Round(start * 1000)) + "," + (int)(Math.Round(end * 1000)) + @")}";
        }

        public static string move_offset(double x1, double y1, double x2, double y2, double start, double offset)
        {
            return move(x1, y1, x2, y2, start, start + offset);
        }

        public static string pos(int x, int y)
        {
            return @"{\pos(" + x + "," + y + @")}";
        }

        public static string pos(double x, double y)
        {
            return @"{\pos(" + x.ToString("0.00") + "," + y.ToString("0.00") + @")}";
        }

        public static string org(double x, double y)
        {
            return @"{\org(" + (int)(Math.Round(x)) + "," + (int)(Math.Round(y)) + @")}";
        }

        public static string clip(int p, string mask)
        {
            return @"{\clip(" + p + "," + mask + @")}";
        }

        public static string clip(int x0, int y0, int x1, int y1)
        {
            return @"{\clip(" + x0 + "," + y0 + "," + x1 + "," + y1 + @")}";
        }

        public static string iclip(int p, string mask)
        {
            return @"{\iclip(" + p + "," + mask + @")}";
        }

        public static string iclip(int x0, int y0, int x1, int y1)
        {
            return @"{\iclip(" + x0 + "," + y0 + "," + x1 + "," + y1 + @")}";
        }

        public static string clip_pixel(int x, int y)
        {
            return @"{\clip(" + x + "," + y + "," + (x + 1) + "," + (y + 1) + @")}";
        }

        public static string t(double t1, double t2, string effect)
        {
            return @"{\t(" + (int)Math.Round(t1 * 1000) + "," + (int)Math.Round(t2 * 1000) + "," + effect + ")}";
        }

        public static string t(double t1, double t2, double acc, string effect)
        {
            return @"{\t(" + (int)Math.Round(t1 * 1000) + "," + (int)Math.Round(t2 * 1000) + "," + acc.ToString("0.00") + "," + effect + ")}";
        }

        public static string t(string effect)
        {
            return @"{\t(" + effect + ")}";
        }

        public static string t_offset(double t1, double offset, string effect)
        {
            return t(t1, t1 + offset, effect);
        }
    }
}
