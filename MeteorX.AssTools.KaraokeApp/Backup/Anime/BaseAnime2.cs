using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeGUI;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data.Linq;
using MeteorX.AssTools.KaraokeApp.Model;
using System.Runtime.InteropServices;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    public abstract class BaseAnime2 : BaseAnime
    {
        /// <summary>
        /// style name must be "Default", set mask color to "00FF0000" and others to "XX00XXXX".
        /// </summary>
        public string MaskStyle { get; set; }

        public bool IsAvsMask { get; set; }

        /// <summary>
        /// 空格的宽度
        /// </summary>
        public int WhitespaceWidth { get; set; }

        public BaseAnime2()
        {
            IsAvsMask = true;
            WhitespaceWidth = -1;
        }

        protected Random rnd = new Random();

        int[,] map;

        public void InitBFS()
        {
            map = new int[this.PlayResX, this.PlayResY];
        }

        public int[] CalculateBFSOrder(StringMask mask)
        {
            for (int i = 0; i < this.PlayResX; i++)
                for (int j = 0; j < this.PlayResY; j++)
                    map[i, j] = -1;
            for (int i = 0; i < mask.Points.Count; i++)
            {
                ASSPoint pt = mask.Points[i];
                map[pt.X, pt.Y] = i;
            }
            int[] dx0 = { 0, 1, 0, -1 };
            int[] dy0 = { 1, 0, -1, 0 };
            int[] dx1 = { 1, 1, -1, -1 };
            int[] dy1 = { -1, 1, -1, 1 };
            int[] ind = new int[mask.Points.Count]; // 顺序标号
            for (int i = 0; i < ind.Length; i++) ind[i] = -1;
            Queue<int> q;
            int left = ind.Length; // 剩下
            while (left > 0)
            {
                q = new Queue<int>();
                for (int i = 0; i < ind.Length; i++)
                    if (ind[i] == -1)
                    {
                        q.Enqueue(i);
                        ind[i] = 0;
                        break;
                    }
                while (q.Count > 0)
                {
                    int s = q.Dequeue();
                    left--;
                    ASSPoint pt = mask.Points[s];
                    int x1, y1, t;
                    for (int i = 0; i < 4; i++)
                    {
                        x1 = pt.X + dx0[i];
                        y1 = pt.Y + dy0[i];
                        if (x1 >= 0 && x1 < PlayResX && y1 >= 0 && y1 < PlayResY && map[x1, y1] >= 0)
                        {
                            t = map[x1, y1];
                            if (ind[t] < 0)
                            {
                                ind[t] = ind[s] + 1;
                                q.Enqueue(t);
                            }
                        }
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        //if (Common.RandomBool(rnd, 0.5)) continue;
                        break;
                        x1 = pt.X + dx0[i];
                        y1 = pt.Y + dy0[i];
                        if (x1 >= 0 && x1 < PlayResX && y1 >= 0 && y1 < PlayResY && map[x1, y1] >= 0)
                        {
                            t = map[x1, y1];
                            if (ind[t] < 0)
                            {
                                ind[t] = ind[s] + 1;
                                q.Enqueue(t);
                            }
                        }
                    }
                }
            }
            return ind;
        }

        public override StringMask GetMask(string s, int x, int y)
        {
            return GetMask(s, x, y, this.MaskStyle);
        }

        public StringMask GetMask(string s, int x, int y, string style)
        {
            if (!IsAvsMask)
                return base.GetMask(s, x, y);
            if (s.Trim() == "")
                return new StringMask { Height = FontHeight, Width = (WhitespaceWidth >= 0) ? WhitespaceWidth : FontWidth, X0 = x, Y0 = y, Points = new List<ASSPoint>() };

            using (MaskDataContext db = new MaskDataContext())
            {
                var ma = db.Masks.Where(m => m.X == x && m.Y == y && m.PlayResX == this.PlayResX && m.PlayResY == this.PlayResY && m.Style == this.MaskStyle && m.Str == s.GetHashCode().ToString());
                if (ma.Count() > 0)
                {
                    return new BinaryFormatter().Deserialize(new MemoryStream(ma.First().Data.ToArray())) as StringMask;
                }
            }

            // generate ass file
            string assFN = "BaseAnime2_Temp.ass";
            StreamWriter assOut = new StreamWriter(new FileStream(assFN, FileMode.Create), Encoding.Unicode);
            assOut.WriteLine("[Script Info]");
            assOut.WriteLine("Synch Point:0");
            assOut.WriteLine("ScriptType: v4.00+");
            assOut.WriteLine("Collisions:Normal");
            assOut.WriteLine("PlayResX:{0}", this.PlayResX);
            assOut.WriteLine("PlayResY:{0}", this.PlayResY);
            assOut.WriteLine("Timer:100.0000");
            assOut.WriteLine("");
            assOut.WriteLine("[V4+ Styles]");
            assOut.WriteLine("Format: Name, Fontname, Fontsize, PrimaryColour, SecondaryColour, OutlineColour, BackColour, Bold, Italic, Underline, StrikeOut, ScaleX, ScaleY, Spacing, Angle, BorderStyle, Outline, Shadow, Alignment, MarginL, MarginR, MarginV, Encoding");
            assOut.WriteLine(style);
            assOut.WriteLine("");
            assOut.WriteLine("[Events]");
            assOut.WriteLine("Format: Layer, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text");
            assOut.WriteLine("Dialogue: 0,0:00:00.00,0:01:00.00,Default,NTP,0000,0000,0000,,{0}{1}", ASSEffect.pos(x, y), s);
            assOut.Close();

            // generate avs file
            string avsFN = "BaseAnime2_Temp.avs";
            StreamWriter avsOut = new StreamWriter(new FileStream(avsFN, FileMode.Create), Encoding.Default);
            avsOut.WriteLine("BlankClip(height={0}, width={1}, length=1000, fps=23.976)", this.PlayResY, this.PlayResX);
            avsOut.WriteLine("ConvertToRGB24()");
            avsOut.WriteLine("TextSub(\"{0}\")", assFN);
            avsOut.Close();

            AvsFile avs = AvsFile.OpenScriptFile(avsFN);
            if (!avs.CanReadVideo) return null;
            IVideoReader ivr = avs.GetVideoReader();
            Bitmap bmp = ivr.ReadFrameBitmap(0);
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            List<ASSPoint> result = new List<ASSPoint>();
            unsafe
            {
                byte* p = (byte*)(void*)bd.Scan0;
                for (int x1 = 0; x1 < bmp.Width; x1++)
                    for (int y1 = 0; y1 < bmp.Height; y1++)
                    {
                        byte* q = p + bd.Stride * y1 + x1 * 3;
                        if (q[0] > 0)
                            result.Add(new ASSPoint { Brightness = q[0], X = x1, Y = y1 });
                    }
            }
            bmp.UnlockBits(bd);
            bmp.Dispose();
            avs.Dispose();

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

            StringMask sm = new StringMask { Height = ymax - ymin + 1, Width = xmax - xmin + 1, X0 = x, Y0 = y, Points = result };
            //sm.CalculateEdgeDistance();

            using (MaskDataContext db = new MaskDataContext())
            {
                try
                {
                    MemoryStream ms = new MemoryStream();
                    new BinaryFormatter().Serialize(ms, sm);
                    ms.Position = 0;
                    db.Masks.InsertOnSubmit(new Mask { X = x, Y = y, PlayResX = this.PlayResX, PlayResY = this.PlayResY, Style = this.MaskStyle, Str = s.GetHashCode().ToString(), Data = new Binary(ms.ToArray()) });
                    db.SubmitChanges();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            return sm;
        }

        public static string CreatePolygon(Random rnd, double r1, double r2, int v)
        {
            string s = @"{\p4}";
            double[] a = new double[v];
            for (int i = 0; i < a.Length; i++)
                a[i] = Common.RandomDouble(rnd, 0, 2 * Math.PI);
            Array.Sort(a);
            for (int i = 0; i < a.Length; i++)
            {
                if (i == 0) s += "m";
                else s += " l";
                s += string.Format(" {0} {1}", (int)(Math.Round(Math.Cos(a[i]) * r1)), (int)(Math.Round(Math.Sin(a[i]) * r2)));
            }
            return s;
        }

        public static string CreateBezier(Random rnd, double r1, double r2, int v)
        {
            string s = @"{\p4}";
            double[] a = new double[v];
            for (int i = 0; i < a.Length; i++)
                a[i] = Common.RandomDouble(rnd, 0, 2 * Math.PI);
            Array.Sort(a);
            for (int i = 0; i < a.Length; i++)
            {
                if (i == 0) s += "m";
                else s += " b";
                s += string.Format(" {0} {1}", (int)(Math.Round(Math.Cos(a[i]) * r1)), (int)(Math.Round(Math.Sin(a[i]) * r2)));
            }
            return s;
        }

        public static Bezier CreateBezier3(Random rnd, double ox, double oy, double a, double b)
        {
            string s = @"{\p1}";
            double[] aa = new double[4];
            for (int i = 0; i < 4; i++)
                aa[i] = Common.RandomDouble(rnd, 0, 2 * Math.PI);
            ASSPoint[] pp = new ASSPoint[4];
            int j = Common.RandomInt(rnd, 0, 4);
            for (int i = 0; i < 4; i++)
            {
                if (i == 0) s += "m";
                else s += " s";
                j = (j + 1) % aa.Length;
                pp[i] = new ASSPoint { X = (int)(Math.Round(ox + Math.Cos(aa[j]) * a)), Y = (int)(Math.Round(oy + Math.Sin(aa[j]) * b)) };
            }
            return new Bezier(pp[0], pp[1], pp[2], pp[3]);
        }

        //MeasureString(int* width, int* height, WCHAR* fontName, BYTE fontCharset, int fontHeight, WCHAR* str)
        [DllImport("GraphLib.dll", CharSet = CharSet.Unicode)]
        static private extern bool MeasureString(ref int width, ref int height, string fontName, byte fontCharset, int fontHeight, int fontSpace, string str);

        public static Size MeasureString(string fontname, byte fontCharset, int fontHeight, int fontSpace, string str)
        {
            int w = 0;
            int h = 0;
            MeasureString(ref w, ref h, fontname, fontCharset, fontHeight, fontSpace, str);
            return new Size(w, h);
        }

        [DllImport("GraphLib.dll", CharSet = CharSet.Unicode)]
        static private extern bool GetOutline(char[] buf, ref int buflen, char thisChar, char[] fontName, byte fontCharset, int fontHeight, int fontWeight, short yOffset, short OX, short OY);

        /// <summary>
        /// \p4\an7 is default
        /// </summary>
        public static string GetOutline(int x, int y, char ch, string fontname, int fontencoding, int fontheight, int fontweight, int yoffset)
        {
            string fn = fontname;
            string ss = "";
            char[] cc = new char[10000];
            int cclen = 1;
            bool b1 = GetOutline(cc, ref cclen, ch, Encoding.Unicode.GetChars(Encoding.Unicode.GetBytes(fn)), (byte)fontencoding, fontheight * 8, 0, (short)yoffset, (short)(x * 8), (short)(y * 8));
            for (int i = 0; i < cclen; i++)
                ss += cc[i];
            return ss;
        }

        public static string Pts2AssVec(IList<ASSPointF> pts)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < pts.Count; i++)
            {
                if (i == 0) sb.Append("m");
                sb.Append(string.Format(" {0} {1}", (int)pts[i].X, (int)pts[i].Y));
                if (i == 0) sb.Append(" l");
            }
            return sb.ToString();
        }
    }
}
