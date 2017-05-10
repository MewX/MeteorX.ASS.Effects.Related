using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    public abstract class BaseAnime : ASSEffect
    {
        public string InFileName = null;
        public string OutFileName = null;
        public int FontWidth = 25;
        public int FontHeight = 25;
        public int MarginLeft = 10;
        public int MarginRight = 10;
        public int MarginTop = 10;
        public int MarginBottom = 10;
        public int PlayResX = 640;
        public int PlayResY = 360;
        public double Shift = 0;

        public float FontSizeRateW = 1.0f;
        public float FontSizeRateH = 1.0f;
        public int FontSpace = -10;
        public Font Font = new Font("宋体", 11);

        public abstract void Run();

        Bitmap temp_img = null;
        Graphics temp_g = null;

        public double Mask_WidthScale = 1.0;
        public double Mask_HeightScale = 1.0;

        /// <summary>
        /// x0 < x1, y0 < y1
        /// </summary>
        public ASSEvent CreateRectangle(double start, double end, string style, double x0, double y0, double x1, double y1)
        {
            int Rate = 4;
            return
            new ASSEvent
            {
                Start = start,
                End = end,
                Layer = 1,
                Style = style,
                Effect = "",
                MarginL = "0000",
                MarginR = "0000",
                MarginV = "0000",
                Name = "NTP",
                Text = ASSEffect.pos(0, 0) + ASSEffect.p(Rate) + string.Format("m {0} {1} l {2} {3} {4} {5} {6} {7}", Math.Round(x0 * Math.Pow(2, Rate - 1)), Math.Round(y0 * Math.Pow(2, Rate - 1)), Math.Round(x1 * Math.Pow(2, Rate - 1)), Math.Round(y0 * Math.Pow(2, Rate - 1)), Math.Round(x1 * Math.Pow(2, Rate - 1)), Math.Round(y1 * Math.Pow(2, Rate - 1)), Math.Round(x0 * Math.Pow(2, Rate - 1)), Math.Round(y1 * Math.Pow(2, Rate - 1)))
            };
        }

        /// <summary>
        /// x0 < x1, y0 < y1
        /// </summary>
        public ASSEvent CreateRectangle(double start, double end, string style, double x0, double y0, double x1, double y1, double Ox, double Oy, string effectStr)
        {
            x0 -= Ox;
            y0 -= Oy;
            x1 -= Ox;
            y1 -= Oy;
            int Rate = 4;
            return
            new ASSEvent
            {
                Start = start,
                End = end,
                Layer = 1,
                Style = style,
                Effect = "",
                MarginL = "0000",
                MarginR = "0000",
                MarginV = "0000",
                Name = "NTP",
                Text = effectStr + ASSEffect.p(Rate) + string.Format("m {0} {1} l {2} {3} {4} {5} {6} {7}", Math.Round(x0 * Math.Pow(2, Rate - 1)), Math.Round(y0 * Math.Pow(2, Rate - 1)), Math.Round(x1 * Math.Pow(2, Rate - 1)), Math.Round(y0 * Math.Pow(2, Rate - 1)), Math.Round(x1 * Math.Pow(2, Rate - 1)), Math.Round(y1 * Math.Pow(2, Rate - 1)), Math.Round(x0 * Math.Pow(2, Rate - 1)), Math.Round(y1 * Math.Pow(2, Rate - 1)))
            };
        }

        public ASSEvent CreatePixel(double start, double end, int x, int y, string c1, string a1)
        {
            return
            new ASSEvent
            {
                Start = start,
                End = end,
                Layer = 0,
                Style = "pt",
                Effect = "",
                MarginL = "0000",
                MarginR = "0000",
                MarginV = "0000",
                Name = "NTP",
                Text = ASSEffect.pos(x, y) + ASSEffect.c(1, c1) + ASSEffect.a(1, a1) + ASSEffect.frz(90) + '.'
            };
        }

        public ASSEvent CreateMovingPixel(double start, double end, int x0, int y0, int x1, int y1, string c1, string a1)
        {
            return CreateMovingPixel(start, end, x0, y0, x1, y1, c1, a1, 0);
        }

        public ASSEvent CreateMovingPixel(double start, double end, int x0, int y0, int x1, int y1, string c1, string a1, int fade_mode)
        {
            string fade_string = "";
            switch (fade_mode)
            {
                case 0: break;
                case 1: fade_string = ASSEffect.fad(end - start, 0); break;
                case 2: fade_string = ASSEffect.fad(0, end - start); break;
                default: break;
            }
            return
            new ASSEvent
            {
                Start = start,
                End = end,
                Layer = 0,
                Style = "pt",
                Effect = "",
                MarginL = "0000",
                MarginR = "0000",
                MarginV = "0000",
                Name = "NTP",
                Text = fade_string + ASSEffect.move(x0, y0, x1, y1) + ASSEffect.c(1, c1) + ASSEffect.a(1, a1) + '.'
            };
        }

        Graphics GetGraphics()
        {
            if (temp_img == null)
            {
                temp_img = new Bitmap(1000, 1000);
                temp_g = Graphics.FromImage(temp_img);
            }
            temp_g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            temp_g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            temp_g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            return temp_g;
        }

        public virtual StringMask GetMask(string s, int x, int y)
        {
            if (s.Trim() == "")
                return new StringMask { Height = FontHeight, Width = FontWidth, X0 = x, Y0 = y, Points = new List<ASSPoint>() };
            Graphics g = GetGraphics();
            g.Clear(Color.Black);
            g.DrawString(s, Font, new SolidBrush(Color.White), 0, 0);
            List<ASSPoint> result = new List<ASSPoint>();
            for (int i = 0; i < 200; i++)
                for (int j = 0; j < 50; j++)
                    if (temp_img.GetPixel(i, j).G > 0)
                    {
                        ASSPoint newP = new ASSPoint { X = (int)((double)i * Mask_WidthScale) + x, Y = (int)((double)j * Mask_HeightScale) + y, Brightness = temp_img.GetPixel(i, j).G };
                        if (!result.Any(p => p.X == newP.X && p.Y == newP.Y && p.Brightness == newP.Brightness))
                            result.Add(newP);
                    }
            if (result.Count == 0)
                return new StringMask { Height = 0, Width = 0, X0 = x, Y0 = y, Points = result };
            int xmin = 10000;
            int ymin = 10000;
            int xmax = -1;
            int ymax = -1;
            foreach (ASSPoint pt in result)
            {
                if (xmin > pt.X) xmin = pt.X;
                if (xmax < pt.X) xmax = pt.X;

                if (ymin > pt.Y) ymin = pt.Y;
                if (ymax < pt.Y) ymax = pt.Y;
            }
            foreach (ASSPoint pt in result)
            {
                pt.X -= xmin - x;
            }
            return new StringMask { Height = ymax - ymin + 1, Width = xmax - xmin + 1, X0 = x, Y0 = y, Points = result };
        }

        public virtual Size GetSize(string s)
        {
            Graphics g = GetGraphics();
            StringMask mk = GetMask(s, this.PlayResX / 2, this.PlayResY / 2);
            return new Size { Height = mk.Height, Width = mk.Width };
        }

        public int GetTotalWidth(ASSEvent ev)
        {
            List<KElement> a = ev.SplitK(false);
            int sum = 0;
            foreach (KElement k in a)
                sum += GetSize(k.KText).Width;
            return sum + (a.Count - 1) * FontSpace;
        }
    }
}
