using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.Model;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class DuRarara_ED : BaseAnime3
    {
        public DuRarara_ED()
        {
            this.FontHeight = 38;
            this.FontSpace = 0;
            this.IsAvsMask = true;
            this.MarginLeft = 20;
            this.MarginBottom = 20;
            this.MarginTop = 20;
            this.MarginRight = 20;
            this.PlayResX = 1280;
            this.PlayResY = 720;
            this.InFileName = @"G:\Workshop\durarara\ed\ed_k.ass";
            this.OutFileName = @"G:\Workshop\durarara\ed\ed.ass";
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS();

            ass_out.Header = ass_in.Header;
            ass_out.Events = new List<ASSEvent>();

            Random rnd = new Random();

            for (int iEv = 0; iEv < ass_in.Events.Count; iEv++)
            {
                if (iEv <= 2) continue;

                ASSEvent ev = ass_in.Events[iEv];

                /// trick
                if (iEv == 2) ev.Text = ev.Text.Replace(' ', '@');
                List<KElement> kelems = ev.SplitK(true);
                if (iEv == 2)
                {
                    ev.Text = ev.Text.Replace('@', ' ');
                    foreach (KElement ke in kelems)
                        ke.KText = ke.KText.Replace('@', ' ');
                }

                this.FontCharset = 1;
                this.FontName = "EPSON 丸ゴシック体Ｍ";
                this.MaskStyle = "Style: Default,EPSON 丸ゴシック体Ｍ,38,&H00FFFFFF,&HFFFFFFFF,&HFFFFFFFF,&HFFFFFFFF,-1,0,0,0,100,100,0,0,1,0,0,5,0,0,0,1";
                string evStyle = "ed_jp";

                int tw = GetTotalWidth(ev);
                int x0 = (PlayResX - MarginLeft - MarginRight - tw) / 2 + MarginLeft;
                int startx0 = x0;
                int y0 = PlayResY - MarginBottom - FontHeight;
                int kSum = 0;

                List<CompositeCurve> curves = new List<CompositeCurve>();
                List<CompositeCurve> curve_blurs = new List<CompositeCurve>();
                for (int i = 0; i < 4; i++)
                {
                    Brown brown = new Brown { X0 = 0, Y0 = 0, R = 5, Speed = 100, MinT = ev.Start, MaxT = ev.End };
                    CompositeCurve curve = new CompositeCurve { MinT = ev.Start, MaxT = ev.End };
                    curve.AddCurve(ev.Start, ev.End, brown);

                    Brown brown_blur = new Brown { X0 = 0, Y0 = 0, R = 1, Speed = 5, MinT = ev.Start - 1, MaxT = ev.End + 1 };
                    CompositeCurve curve_blur = new CompositeCurve { MinT = ev.Start - 1, MaxT = ev.End + 1 };
                    curve_blur.AddCurve(ev.Start - 1, ev.End + 1, brown_blur);

                    curves.Add(curve);
                    curve_blurs.Add(curve_blur);
                }

                List<int> centerOffsetX = new List<int>();
                if (iEv == 1 || iEv == 2)
                {
                    List<int> tmpx = new List<int>();
                    int tx0 = x0;
                    for (int i = 0; i < kelems.Count; i++)
                    {
                        KElement ke = kelems[i];
                        Size sz = GetSize(ke.KText);
                        int x_an7 = x0;
                        int y_an7 = y0;
                        tmpx.Add(tx0);
                        tx0 += sz.Width + FontSpace;
                        if (ke.KText.Trim().Length == 0) continue;
                    }
                    tmpx.Add(tx0);
                    for (int i = 0; i < kelems.Count; i++)
                    {
                        KElement ke = kelems[i];
                        int j = i;
                        while (j + 1 < kelems.Count)
                        {
                            if (kelems[j + 1].KEnd_NoSplit == ke.KEnd_NoSplit && kelems[j + 1].KStart_NoSplit == ke.KStart_NoSplit) j++; else break;
                        }
                        int centerx = (tmpx[i] + tmpx[j + 1]) / 2;
                        for (; i <= j; i++)
                        {
                            centerOffsetX.Add(tmpx[i] - centerx);
                        }
                        i--;
                    }
                }

                for (int iK = 0; iK < kelems.Count; iK++)
                {
                    Console.WriteLine("{0} / {1} : {2} / {3}", iEv + 1, ass_in.Events.Count, iK + 1, kelems.Count);
                    KElement ke = kelems[iK];
                    Size sz = GetSize(ke.KText);
                    double kStart = ev.Start + kSum * 0.01;
                    double kEnd = kStart + ke.KValue * 0.01;
                    double kStart0 = kStart;
                    double kEnd0 = kEnd;
                    kSum += ke.KValue;
                    kStart = ke.KStart_NoSplit;
                    kEnd = ke.KEnd_NoSplit;
                    int x = x0 + this.FontSpace + sz.Width / 2;
                    int y = y0 + FontHeight / 2;
                    int x_an7 = x0;
                    int y_an7 = y0;
                    x0 += this.FontSpace + sz.Width;
                    if (ke.KText.Trim().Length == 0) continue;

                    if (iEv == 0)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (j == 1) x = MarginLeft + sz.Width / 2;
                            if (j == 2) x = PlayResX - MarginRight - sz.Width / 2;
                            if (j != 0) continue;

                            for (int i = 0; i < 4; i++)
                            {
                                CompositeCurve curve = curves[i];
                                CompositeCurve curve_blur = curve_blurs[i];

                                foreach (ASSPointF pt in curve.GetPath_DT(0.04))
                                {
                                    ASSPointF pt_blur = curve_blur.GetPointF(pt.T);
                                    string a1 = "22";
                                    if (pt.T - ev.Start < 0.3) a1 = Common.scaleAlpha("FF", "22", (pt.T - ev.Start) / 0.3);
                                    if (ev.End - pt.T < 0.3) a1 = Common.scaleAlpha("FF", "22", (ev.End - pt.T) / 0.3);

                                    ass_out.AppendEvent(50, evStyle, pt.T, pt.T + 0.04,
                                        pos(pt.X + x, pt.Y + y) +
                                        a(1, a1) +
                                        c(1, (i % 2 == 0) ? "FFFFFF" : "000000") +
                                        blur(5.0 * ((pt_blur.X + 1.1) * 0.5)) +
                                        ke.KText);
                                }
                            }
                        }
                    }

                    if (iEv == 1 || iEv == 2)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            CompositeCurve curve = curves[i];
                            CompositeCurve curve_blur = curve_blurs[i];

                            foreach (ASSPointF pt in curve.GetPath_DT(0.04))
                            {
                                ASSPointF pt_blur = curve_blur.GetPointF(pt.T);
                                string a1 = "22";
                                if (pt.T - ev.Start < 0.3) a1 = Common.scaleAlpha("FF", "22", (pt.T - ev.Start) / 0.3);
                                if (ev.End - pt.T < 0.3) a1 = Common.scaleAlpha("FF", "22", (ev.End - pt.T) / 0.3);

                                double blur_value = 2.0 * ((pt_blur.X + 2) * 0.5);
                                int ifsc = 100;
                                double kEnd2 = kEnd;
                                bool ink = false;
                                if (pt.T >= kStart && pt.T <= kEnd2)
                                {
                                    ifsc = 135;
                                    ink = true;
                                }
                                string col = (i % 2 == 0) ? "FFFFFF" : "000000";
                                if (i % 2 == 0 && ink)
                                {
                                    col = "FFFFFF";
                                    a1 = "00";
                                }
                                else a1 = "77";

                                ass_out.AppendEvent((i == 0) ? (ink ? 150 : 50) : 100, evStyle, pt.T, pt.T + 0.04,
                                    pos(pt.X + x + (double)(ifsc - 100) / 100.0 * centerOffsetX[iK], pt.Y + y) +
                                    a(1, a1) +
                                    c(1, col) +
                                    blur(blur_value) + fsc(ifsc) +
                                    ke.KText);

                                if (i == 1) continue;
                                double tx = pt.X + x;
                                double dx = -(pt.T - kStart0) / ev.Last * 10.0;
                                double ta1 = 1;
                                if (!ink) ta1 = 0.5;
                                if (pt.T - ev.Start < 0.3) ta1 *= (pt.T - ev.Start) / 0.3;
                                if (ev.End - pt.T < 0.3) ta1 *= (ev.End - pt.T) / 0.3;
                                double tb = blur_value;
                                int tfsc = ifsc + 5;
                                for (int j = 0; j < 10; j++)
                                {
                                    ass_out.AppendEvent(50 + j + 1, evStyle, pt.T, pt.T + 0.04,
                                        pos(tx + (double)(tfsc - 100) / 100.0 * centerOffsetX[iK], pt.Y + y) +
                                        a(1, Common.scaleAlpha("FF", "00", ta1)) +
                                        c(1, col) +
                                        blur(tb) + fsc(tfsc) +
                                        ke.KText);

                                    tx += dx;
                                    if (dx < 0) dx -= 1.0; else dx += 1.0;
                                    ta1 *= 0.9;
                                    tb += 1;
                                    tfsc += (int)Math.Abs(dx);
                                }
                            }
                        }
                    }

                    if (iEv > 2)
                    {
                        ass_out.AppendEvent(49, evStyle, ev.Start, ev.End,
                            pos(x - 1, y - 1) +
                            a(1, "00") + c(1, "000000") + blur(1.1) +
                            ke.KText);
                        ass_out.AppendEvent(50, evStyle, ev.Start, ev.End,
                            pos(x, y) +
                            a(1, "00") + c(1, "000000") + blur(0.9) +
                            ke.KText);
                        ass_out.AppendEvent(51, evStyle, ev.Start, ev.End,
                            pos(x + 2, y + 2) +
                            a(1, "00") + c(1, "000000") + 
                            ke.KText);
                    }
                }
            }

            ass_out.SaveFile(OutFileName);
            Console.WriteLine("Lines : {0}", ass_out.Events.Count);
        }
    }
}
