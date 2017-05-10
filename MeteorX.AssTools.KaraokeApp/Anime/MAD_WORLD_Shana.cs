using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.Effect;
using MeteorX.AssTools.KaraokeApp.Model;

namespace MeteorX.AssTools.KaraokeApp.Anime.Test
{
    class MAD_WORD_Shana : BaseAnime2
    {
        public MAD_WORD_Shana()
        {
            InFileName = @"G:\Workshop\shana\shana_k.ass";
            OutFileName = @"G:\Workshop\shana\shana.ass";

            this.FontWidth = 22;
            this.FontHeight = 22;
            this.FontSpace = 4;

            this.PlayResX = 640;
            this.PlayResY = 480;
            this.MarginBottom = 18;
            this.MarginLeft = 18;
            this.MarginRight = 18;
            this.MarginTop = 18;

            this.Font = new System.Drawing.Font("ＤＦＰまるもじ体W3", 30, GraphicsUnit.Pixel);
            this.MaskStyle = "Style: Default,DFPBrushRD-W12,22,&H00FFFFFF,&HFF000000,&HFF555555,&HFF222222,0,0,0,0,100,100,1,0,1,2,2,5,30,30,18,128";
            this.IsAvsMask = true;
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS();

            ass_out.Header = ass_in.Header;
            ass_out.Events = new List<ASSEvent>();

            Random rnd = new Random();

            string col0 = "96726A";
            string col1 = "D17C6A";
            string ptcol2 = "644BFF";
            string ptcol1 = "5BFF4B";
            string ptstr = @"{\p1}m 0 0 l 1 0 1 1 0 1";

            string[] ptcollist =
            {
                "5A3AFF",
                "3A5BFF",
                "FF3A5D",
                "3AFF4E",
                "FF3AD9",
                "FFD13A"
            };
            string lastptcol = "";

            string[] firestr =
            {
                @"{\p5}m 0 0 b 8 1 9 -28 -2 -41 b 3 -23 -17 0 0 0",
                @"{\p5}m 1 41 b 20 39 1 13 13 -11 b 5 1 -18 22 1 41",
                @"{\p4}m 6 39 b 4 29 1 13 27 -37 b -1 -3 -15 34 6 39",
                @"{\p5}m -3 43 b -9 22 -1 33 16 -47 b 1 3 -17 0 -3 43"
            };

            string spstr = @"{\p1}m 3 0 b 7 0 12 0 16 0 b 19 0 19 2 16 2 b 12 2 7 2 3 2 b 0 2 0 0 3 0{\p0}";

            for (int iEv = 0; iEv < ass_in.Events.Count; iEv++)
            {
                //if (iEv > 0) continue;
                ASSEvent ev = ass_in.Events[iEv];
                List<KElement> kelems = ev.SplitK(false);

                int x0 = MarginLeft;
                int startx0 = x0;
                int y0 = PlayResY - MarginBottom - FontHeight;
                int kSum = 0;

                List<ASSPointF> poslist = new List<ASSPointF>();

                string ptcol = ptcollist[Common.RandomInt(rnd, 0, ptcollist.Length - 1)];
                while (ptcol == lastptcol)
                    ptcol = ptcollist[Common.RandomInt(rnd, 0, ptcollist.Length - 1)];
                lastptcol = ptcol;

                for (int iK = 0; iK < kelems.Count; iK++)
                {
                    Console.WriteLine("{0} / {1} : {2} / {3}", iEv + 1, ass_in.Events.Count, iK + 1, kelems.Count);
                    KElement ke = kelems[iK];
                    double r = (double)iK / (double)(kelems.Count - 1);
                    Size sz = GetSize(ke.KText);
                    double kStart = ev.Start + kSum * 0.01;
                    double kEnd = kStart + ke.KValue * 0.01;
                    kSum += ke.KValue;
                    int x = x0 + this.FontSpace + sz.Width / 2;
                    int y = y0 + FontHeight / 2;
                    StringMask mask = GetMask(ke.KText, x, y);
                    x0 += this.FontSpace + sz.Width;
                    y0 = y0;
                    if (ke.KText.Trim().Length == 0) continue;
                    poslist.Add(new ASSPointF { X = x, Y = y, Start = kStart, End = kEnd });

                    double t0 = ev.Start - 0.5 + iK * 0.05;
                    double t1 = t0 + 0.3;
                    double t2 = kStart;
                    double t3 = kEnd;
                    double t4 = ev.End - 0.5 + iK * 0.05;
                    double t5 = t4 + 0.3;

                    ass_out.AppendEvent(60, "Default", t0, t1,
                        pos(x, y) + frz(-30) + a(1, "00") +
                        fad((t1 - t0) * 0.5, 0) + fsc(130, 200) + blur(3) + bord(0) +
                        t(0, t1 - t0, frz(0).t() + fsc(100, 100).t() + c(1, col0).t() + blur(0).t()) +
                        ke.KText);
                    ass_out.AppendEvent(50, "Default", t1, t2,
                        pos(x, y) + a(1, "00") + c(1, col0) +
                        ke.KText);
                    ass_out.AppendEvent(60, "Default", t2, t3,
                        pos(x, y) + a(1, "00") + a(3, "00") + c(3, col1) +
                        bord(4) + blur(4) + fsc(200, 200) +
                        t(0, t3 - t2, fsc(110, 110).t() + bord(2).t() + blur(2).t()) +
                        ke.KText);
                    ass_out.AppendEvent(30, "Default", t2, t4,
                        pos(x, y) + a(1, "FF") + a(3, "44") +
                        bord(2) + blur(2) +
                        fad(0.3, 0.1) +
                        ke.KText);
                    ass_out.AppendEvent(50, "Default", t3, t4,
                        pos(x, y) + a(1, "00") + c(1, "FFFFFF") + a(3, "00") +
                        c(3, col1) + bord(1) +
                        ke.KText);
                    ass_out.AppendEvent(40, "Default", t4, t5,
                        pos(x, y) + a(1, "00") + c(1, "FFFFFF") + a(3, "FF") +
                        bord(0) + blur(0) + fad(0, t5 - t4) +
                        t(0, t5 - t4, frz(30).t() + fsc(130, 200).t() + blur(4).t()) +
                        ke.KText);

                    //continue;
                    for (int i = 0; i < (t2 - t1) * 100; i++)
                    {
                        ASSPoint pt = mask.Points[Common.RandomInt(rnd, 0, mask.Points.Count - 1)];
                        double ptx = pt.X;
                        double pty = pt.Y;
                        double ptt0 = Common.RandomDouble(rnd, t1, t2);
                        double ptt1 = ptt0 + 0.3;
                        int tmpz = Common.RandomInt(rnd, 0, 359);
                        string tmpstr = firestr[Common.RandomInt(rnd, 0, firestr.Length - 1)];
                        ass_out.AppendEvent(100, "pt", ptt0, ptt1,
                            move(ptx, pty, ptx, pty - 20) + a(1, "D0") + c(1, "B79E8A") +
                            a(3, "C0") + c(3, "B79E8A") + bord(3) + blur(3) +
                            frz(tmpz) +
                            t(0, ptt1 - ptt0, fsc(80, 80).t() + bord(1).t() + frx(0).t() + fry(361).t() + frz(-57).t()) +
                            tmpstr);
                    }
                    for (int i = 0; i < (t4 - t3) * 100; i++)
                    {
                        ASSPoint pt = mask.Points[Common.RandomInt(rnd, 0, mask.Points.Count - 1)];
                        double ptx = pt.X;
                        double pty = pt.Y;
                        double ptt0 = Common.RandomDouble(rnd, t3, t4);
                        double ptt1 = ptt0 + 0.3;
                        int tmpz = Common.RandomInt(rnd, 0, 359);
                        string tmpstr = firestr[Common.RandomInt(rnd, 0, firestr.Length - 1)];
                        ass_out.AppendEvent(100, "pt", ptt0, ptt1,
                            move(ptx, pty, ptx, pty - 20) + a(1, "D0") + c(1, "3D3EE7") + //354CB9
                            a(3, "C0") + c(3, "3D3EE7") + bord(3) + blur(3) +
                            frz(tmpz) +
                            t(0, ptt1 - ptt0, fsc(80, 80).t() + bord(1).t() + frx(0).t() + fry(361).t() + frz(-57).t()) +
                            tmpstr);
                    }
                }

                //continue;

                for (int i = 1; i < poslist.Count; i++)
                    if (poslist[i].End - poslist[i].Start < 1e-8)
                    {
                        poslist[i - 1].End -= 0.1;
                        poslist[i].Start -= 0.1;
                    }

                for (int i = 0; i < 3; i++)
                {
                    /*
                    if (i > 1) continue;
                    CompositeCurve curve = new CompositeCurve() { MinT = ev.Start - 0.1, MaxT = ev.End + (PlayResX - x0) * 0.002 };
                    bool lastup = i == 0;
                    if (lastup)
                        curve.AddCurve(curve.MinT, ev.Start, new Line() { MinT = 0, MaxT = 1, X0 = 0, X1 = MarginLeft, Y0 = y0 - FontHeight * 0.5, Y1 = y0 - FontHeight * 0.5 });
                    else
                        curve.AddCurve(curve.MinT, ev.Start, new Line() { MinT = 0, MaxT = 1, X0 = 0, X1 = MarginLeft, Y0 = y0 + FontHeight * 1.5, Y1 = y0 + FontHeight * 1.5 });
                    string ptcol = lastup ? ptcol1 : ptcol2;
                    double lastx1 = MarginLeft;
                    double lasty1 = 0;
                    double lastt = 0;
                    foreach (ASSPointF pos in poslist)
                    {
                        double thisx1 = pos.X + FontWidth / 2;
                        double yy0 = y0 - FontHeight * 0.5;
                        double yy1 = y0 + FontHeight * 1.5;
                        if (!lastup)
                        {
                            yy0 = y0 + FontHeight * 1.5;
                            yy1 = y0 - FontHeight * 0.5;
                        }
                        lastup = !lastup;
                        //curve.AddCurve(pos.Start, pos.End, new Line() { MinT = 0, MaxT = 1, X0 = lastx1, X1 = thisx1, Y0 = yy0, Y1 = yy1 });
                        curve.AddCurve(pos.Start, pos.End, Sine.Create(lastx1, yy0, thisx1, yy1));
                        lastx1 = thisx1;
                        lasty1 = yy1;
                        lastt = pos.End;
                    }
                    curve.AddCurve(lastt, curve.MaxT, new Line() { MinT = 0, MaxT = 1, X0 = lastx1, X1 = PlayResX, Y0 = lasty1, Y1 = lasty1 });
                     * */
                    CompoundCurve curve = new CompoundCurve() { MinT = ev.Start - 0.2, MaxT = ev.End + 1 };
                    curve.AddCurve(curve.MinT, curve.MaxT, new Circle() { X0 = 0, Y0 = 0, R = 30, MinT = 0 + i * 2.0 / 3.0 * Math.PI, MaxT = Math.PI * 2 * (curve.MaxT - curve.MinT) * 1.3 + i * 2.0 / 3.0 * Math.PI });
                    CompositeCurve pathcurve = new CompositeCurve() { MinT = curve.MinT, MaxT = curve.MaxT };
                    double lastx = 0;
                    double lastt = curve.MinT;
                    double yyy = y0 + FontHeight * 0.5;
                    for (int j = 0; j < poslist.Count; j++)
                    {
                        double thist = poslist[j].Start;
                        double thisx = poslist[j].X;
                        pathcurve.AddCurve(lastt, thist, new Line() { X0 = lastx, X1 = thisx, Y0 = yyy, Y1 = yyy });
                        if (j + 1 == poslist.Count)
                            thist = curve.MaxT;
                        else
                        {
                            thist = poslist[j + 1].Start - 0.2;
                            if (thist < poslist[j].Start) thist = poslist[j].Start * 0.25 + poslist[j + 1].Start * 0.75;
                        }
                        pathcurve.AddCurve(poslist[j].Start, thist, new Line() { X0 = thisx, X1 = thisx, Y0 = yyy, Y1 = yyy });
                        lastt = thist;
                        lastx = thisx;
                    }
                    curve.AddCurve(curve.MinT, curve.MaxT, pathcurve);

                    List<ASSPointF> pts = curve.GetPath_Dis(1, 1.2);
                    foreach (ASSPointF pt in pts)
                    {
                        double ptx2 = Common.RandomDouble(rnd, pt.X - 10, pt.X - 15);
                        double pty2 = pt.Y * 0.25 + (y0 + FontHeight * 0.5) * 0.75;
                        ass_out.AppendEvent(55, "pt", pt.T, pt.T + 0.3,
                            pos(pt.X, pt.Y) +
  //                          move(pt.X, pt.Y, ptx2, pty2, 0.4, 1) +
                            a(1, "AA") + c(1, "FFFFFF") +
                            a(3, "AA") + c(3, ptcol) +
                            bord(6) + blur(6) +
                            t(0, 0.1, bord(3).t() + blur(3).t()) +
                            fad(0, 0.2) +
                            ptstr);
                        ass_out.AppendEvent(55, "pt", pt.T, pt.T + 0.3,
                            pos(pt.X, pt.Y) +
//                            move(pt.X, pt.Y, ptx2, pty2, 0.4, 1) +
                            a(1, "AA") + c(1, "FFFFFF") +
                            a(3, "00") + c(3, "FFFFFF") +
                            bord(4) + blur(4) +
                            t(0, 0.1, bord(1).t() + blur(1).t() + a(3, "44").t()) +
                            fad(0, 0.2) +
                            ptstr);
                    }
                }
            }

            ass_out.SaveFile(OutFileName);
            Console.WriteLine("Lines : {0}", ass_out.Events.Count);
        }
    }
}
