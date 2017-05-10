using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.Model;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class supercell_The_story_you_dont_know : BaseAnime2
    {
        public supercell_The_story_you_dont_know()
        {
            this.FontHeight = 25;
            this.FontSpace = 2;
            this.IsAvsMask = true;
            this.MarginLeft = 20;
            this.MarginBottom = 20;
            this.MarginTop = 20;
            this.MarginRight = 20;
            this.PlayResX = 848;
            this.PlayResY = 480;
            this.InFileName = @"G:\Workshop\supercell - The story you don't know\kashi_k.ass";
            this.OutFileName = @"G:\Workshop\supercell - The story you don't know\kashi.ass";
        }

        public override Size GetSize(string s)
        {
            if (s == " ") return new Size { Width = this.FontHeight / 2, Height = this.FontHeight };
            Size sz = base.GetSize(s);
            if (s.IndexOf('i') >= 0) sz.Width += 3;
            return sz;
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS() { Header = ass_in.Header, Events = new List<ASSEvent>() };

            for (int iEv = 0; iEv < ass_in.Events.Count; iEv++)
            {
                bool isJp = iEv <= 47;
                this.MaskStyle = "Style:Default,DFSoGei-W5,25,&H00FFFFFF,&HFFFFFFFF,&HFFFFFFFF,&HFFFFFFFF,0,0,0,0,100,100,0,0,1,1,0,5,0,0,0,1";
                int jEv = isJp ? iEv : iEv - 48;
                //if (jEv > 0) continue;
                //if (iEv > 2) continue;
                ASSEvent ev = ass_in.Events[iEv];
                List<KElement> kelems = ev.SplitK(false);
                List<ASSPointF> path = new List<ASSPointF>();

                {
                    int totalWidth = GetTotalWidth(ev);
                    int x0 = (PlayResX - MarginRight - totalWidth - MarginLeft) / 2 + MarginLeft;
                    int y0 = (isJp) ? (PlayResY - MarginBottom - FontHeight) : MarginTop;
                    int kSum = 0;
                    for (int iK = 0; iK < kelems.Count; iK++)
                    {
                        double sr = (double)iK / (double)(kelems.Count - 1);
                        Console.WriteLine("{0} / {1} : {2} / {3}", iEv + 1, ass_in.Events.Count, iK + 1, kelems.Count);
                        KElement ke = kelems[iK];
                        Size sz = GetSize(ke.KText);
                        double kStart = ev.Start + kSum * 0.01;
                        double kEnd = kStart + ke.KValue * 0.01;
                        kSum += ke.KValue;
                        int x = x0 + this.FontSpace + sz.Width / 2;
                        int y = y0 + FontHeight / 2;
                        int x_an7 = x0;
                        int y_an7 = y0;
                        x0 += this.FontSpace + sz.Width;
                        if (ke.KText.Trim().Length == 0) continue;
                        StringMask mask = GetMask(ke.KText, x, y);
                        string evStyle = isJp ? "jp" : "roma";

                        path.Add(new ASSPointF { X = x, Y = y, Start = kStart, End = kEnd, T = kStart });

                        double t0 = ev.Start - 0.5 + iK * 0.065;
                        double t1 = t0 + 0.5;
                        double t2 = kStart;
                        double t21 = kEnd;
                        double t3 = ev.End - 0.5 + iK * 0.065;
                        double t4 = t3 + 0.5;
                        if (t21 > t3) t21 = t3;

                        string MainColor = "0000A7";
                        {
                            Line line = new Line { X0 = x, X1 = x, Y0 = y - 35, Y1 = y, Acc = 0.4 };
                            CompositeCurve curve = new CompositeCurve { MinT = t0, MaxT = t1 };
                            curve.AddCurve(t0, t1, line);
                            foreach (ASSPointF pt in curve.GetPath_Dis(1, 1.1).OrderByDescending(xx => xx.T))
                            {
                                ass_out.AppendEvent(40, evStyle, pt.T, pt.T + 0.3,
                                    pos(pt.X, pt.Y) + fad(0, 0.3) + a(1, "DD") + blur(1.8) + c(1, MainColor) +
                                    a(3, "DD") + c(3, "EEEEEE") + bord(1.8) +
                                    ke.KText);
                            }
                        }
                        {
                            Line line = new Line { X0 = x, X1 = x, Y0 = y, Y1 = y + 35, Acc = 0.4 };
                            CompositeCurve curve = new CompositeCurve { MinT = t3, MaxT = t4 };
                            curve.AddCurve(t3, t4, line);
                            foreach (ASSPointF pt in curve.GetPath_Dis(1, 1.1).OrderByDescending(xx => xx.T))
                            {
                                double last = 0.3 + 0.2 - Math.Abs((t4 - t3) * 0.5 + t3 - pt.T) * 1.2;
                                ass_out.AppendEvent(40, evStyle, pt.T, pt.T + last,
                                    pos(pt.X, pt.Y) + fad(0, last) + a(1, "DD") + blur(1.8) + c(1, MainColor) +
                                    a(3, "DD") + c(3, "EEEEEE") + bord(1.8) +
                                    ke.KText);
                            }
                        }

                        ass_out.AppendEvent(40, evStyle, t1 - 0.1, t2,
                            fad(0.4, 0) + pos(x + 2, y + 2) + a(1, "77") + c(1, "000000") + blur(1) +
                            ke.KText);
                        ass_out.AppendEvent(40, evStyle, t21, t3 + 0.1,
                            fad(0, 0.4) + pos(x + 2, y + 2) + a(1, "77") + c(1, "000000") + blur(1) +
                            ke.KText);

                        /*
                        ass_out.AppendEvent(50, evStyle, t1, t2,
                            pos(x, y) + a(1, "66") + c(1, MainColor) + blur(1) + a(3, "00") + c(3, "EEEEEE") + bord(1.8) +
                            ke.KText);
                        ass_out.AppendEvent(50, evStyle, t21, t3,
                            pos(x, y) + a(1, "66") + c(1, MainColor) + blur(1) + a(3, "00") + c(3, "EEEEEE") + bord(1.8) +
                            ke.KText);
                         * */
                        {
                            Func<double, double> tempFunc = xx => Math.Sin(xx * 1.4);
                            Func<double, string> colFunc = xx => ((tempFunc(xx) > 0) ? ASSColor.FromRGB(1, tempFunc(xx) * 255.0, 0, 255.0).ToColString() : ASSColor.FromRGB(1, 0, -tempFunc(xx) * 255.0, 255.0).ToColString());
                            string tstring = "";
                            tstring += t(0, t2 - t1, c(1, colFunc(t2)).t());
                            tstring += t(t2 - t1, t2 - t1 + 0.01, a(1, "FF").t() + a(3, "FF").t());
                            tstring += t(t21 - t1 - 0.01, t21 - t1, a(1, "66").t() + a(3, "00").t() + c(1, colFunc(t21)).t());
                            tstring += t(t21 - t1, t3 - t1, c(1, MainColor).t());
                            ass_out.AppendEvent(50, evStyle, t1, t3,
                                pos(x, y) + a(1, "66") + c(1, MainColor) + blur(1) + a(3, "00") + c(3, "EEEEEE") + bord(1.8) +
                                tstring +
                                ke.KText);
                            for (double ti = t2; ti < t21 && ti < t2 + 0.2; ti += 0.01)
                            {
                                ass_out.AppendEvent(60, evStyle, ti, t21,
                                    fad(0, t21 - ti) + pos(x, y) + a(1, "AA") + c(1, "FFFFFF") + blur(0) + a(3, "EE") + c(3, colFunc(ti)) + bord(2.5) +
                                    t(0, 0.3, 0.6, fsc(150, 150).t()) + t(0.3, t21 - ti, blur(2.5).t() + bord(3.5).t()) +
                                    ke.KText);
                            }
                        }
                    }
                }

                {
                    double spd = 750; // pixel / sec
                    double x0 = path[0].X - 30;
                    double y0 = path[0].Y;
                    double t0 = path[0].T - 0.3;
                    CompositeCurve curve = new CompositeCurve { MinT = ev.Start - 0.3, MaxT = ev.End };
                    bool lastVertical = true;
                    int p = 0;
                    while (t0 < curve.MaxT)
                    {
                        double x1 = x0;
                        double y1 = y0;
                        while (p + 1 < path.Count && path[p].End <= t0) p++;
                        int sig = Common.RandomSig(rnd);
                        if (lastVertical)
                        {
                            if (x0 < path[p].X) sig = 1; else sig = -1;
                            x1 = x0 + Common.RandomDouble(rnd, 5, 50) * sig;
                        }
                        else
                        {
                            if (y0 < path[p].Y) sig = 1; else sig = -1;
                            y1 = y0 + Common.RandomDouble(rnd, 5, 40) * sig;
                        }
                        double t1 = t0 + Common.GetDistance(x0, y0, x1, y1) / spd;
                        if (t1 >= curve.MaxT) t1 = curve.MaxT;
                        curve.AddCurve(t0, t1, new Line { X0 = x0, Y0 = y0, X1 = x1, Y1 = y1 });
                        x0 = x1;
                        y0 = y1;
                        t0 = t1;
                        lastVertical = !lastVertical;
                    }
                    foreach (ASSPointF pt in curve.GetPath_Dis(0.9, 1.0))
                    {
                        double tt = Math.Sin(pt.T * 1.4); // DO NO CHANGE
                        string col = "";
                        if (tt > 0) col = ASSColor.FromRGB(1, tt * 255.0, 0, 255.0).ToColString(); else col = ASSColor.FromRGB(1, 0, -tt * 255.0, 255.0).ToColString();
                        ass_out.AppendEvent(20, "pt", pt.T, pt.T + 1.5,
                            fad(0, 0.5) + pos(pt.X, pt.Y) + bord(0.8) + blur(1) +
                            a(1, "00") + a(3, "77") + c(1, Common.scaleColor("FFFFFF", col, 0.7)) + c(3, col) +
                            ASSEffect.p(1) + "m 0 0 l 1 0 1 1 0 1");
                    }
                }

            }

            Console.WriteLine(ass_out.Events.Count);
            ass_out.SaveFile(this.OutFileName);
        }
    }
}
