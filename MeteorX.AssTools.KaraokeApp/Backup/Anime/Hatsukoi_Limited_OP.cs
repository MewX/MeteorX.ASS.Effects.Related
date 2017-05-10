using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.Model;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class Hatsukoi_Limited_OP : BaseAnime2
    {
        public Hatsukoi_Limited_OP()
        {
            this.FontHeight = 40;
            this.FontSpace = 5;
            this.IsAvsMask = true;
            this.MarginLeft = 35;
            this.MarginBottom = 35;
            this.MarginTop = 35;
            this.MarginRight = 35;
            this.PlayResX = 1280;
            this.PlayResY = 720;
            this.InFileName = @"G:\Workshop\hatsukoi\op_k.ass";
            this.OutFileName = @"G:\Workshop\hatsukoi\op.ass";
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS() { Header = ass_in.Header, Events = new List<ASSEvent>() };

            for (int iEv = 0; iEv < ass_in.Events.Count; iEv++)
            {
                bool isJp = iEv <= 13;
                //if (isJp) continue;
                //if (iEv != 5 && iEv != 6) continue;
                ASSEvent ev = ass_in.Events[iEv];
                List<KElement> kelems = ev.SplitK(true);
                int x0 = MarginLeft;
                if (!isJp)
                {
                    int totalWidth = 0;
                    foreach (KElement ke in kelems)
                    {
                        this.MaskStyle = "Style: Default,宋体,40,&H00FFFFFF,&HFFFFFFFF,&HFFFFFFFF,&HFFFFFFFF,0,0,0,0,100,100,0,0,0,0,0,7,0,0,0,0";
                        string outlineFontname = isJp ? "DFGMaruMoji-SL" : "華康少女文字W5(P)";
                        int outlineEncoding = isJp ? 128 : 136;
                        int yoffset = 279;
                        int ox_offset = 0;
                        string outlineString = GetOutline(10, 10, ke.KText[0], outlineFontname, outlineEncoding, FontHeight, 0, yoffset);
                        Size sz = GetMask(p(4) + outlineString, 0, 0).GetSize();
                        if (ke.KText.Trim().Length == 0) sz.Width = this.FontHeight;
                        totalWidth += sz.Width + FontSpace;
                    }
                    x0 = PlayResX - MarginRight - totalWidth;
                }
                int bakx0 = x0;
                int y0 = (!isJp) ? (PlayResY - MarginBottom - FontHeight) : MarginTop;
                int kSum = 0;
                string outlines = "";
                double lastKStart = -1;
                double lastx0 = 0;
                double lastt0 = 0;
                for (int iK = 0; iK < kelems.Count; iK++)
                {
                    if (iK == 15)
                    {
                        int asfasd = 2;
                    }
                    Console.WriteLine("{0} / {1} : {2} / {3}", iEv + 1, ass_in.Events.Count, iK + 1, kelems.Count);
                    KElement ke = kelems[iK];
                    /*this.MaskStyle = isJp ?
                        "Style: Default,DFGMaruMoji-SL,40,&H00FFFFFF,&HFFFFFFFF,&HFFFFFFFF,&HFFFFFFFF,0,0,0,0,100,100,0,0,0,0,0,5,0,0,0,128" :
                        "Style: Default,方正准圆_GBK,40,&H00FFFFFF,&HFFFFFFFF,&HFFFFFFFF,&HFFFFFFFF,0,0,0,0,100,100,0,0,0,0,0,5,0,0,0,134";*/
                    this.MaskStyle = "Style: Default,宋体,40,&H00FFFFFF,&HFFFFFFFF,&HFFFFFFFF,&HFFFFFFFF,0,0,0,0,100,100,0,0,0,0,0,7,0,0,0,0";
                    string outlineFontname = isJp ? "DFGMaruMoji-SL" : "華康少女文字W5(P)";
                    int outlineEncoding = isJp ? 128 : 136;
                    int yoffset = 279;
                    int ox_offset = 0;
                    string outlineString = "";
                    Size sz = new Size();
                    if (ke.KText.Trim().Length == 0)
                    {
                        sz = new Size { Width = FontHeight, Height = FontHeight };
                    }
                    else
                    {
                        outlineString = GetOutline(10, 10, ke.KText[0], outlineFontname, outlineEncoding, FontHeight, 0, yoffset);
                        sz = GetMask(p(4) + outlineString, 0, 0).GetSize();
                    }
                    double kStart = ev.Start + kSum * 0.01;
                    if (ke.IsSplit)
                    {
                        if (lastKStart >= 0)
                            kStart = lastKStart;
                        else
                            lastKStart = kStart;
                    }
                    else
                    {
                        lastKStart = -1;
                    }
                    double kEnd = kStart + ke.KValue * 0.01;
                    kSum += ke.KValue;
                    int x = x0 + this.FontSpace + sz.Width / 2;
                    int y = y0 + FontHeight / 2;
                    int x_an7 = x0;
                    int y_an7 = y0;
                    x0 += this.FontSpace + sz.Width;
                    lastx0 = x0;
                    if (ke.KText.Trim().Length == 0) continue;
                    if (!isJp)
                    {
                        if (ke.KText == "（") x -= 19;
                        if (ke.KText == "）") x -= 2;
                        if (ke.KText == "！") x -= 10;
                    }
                    outlineString = GetOutline(ox_offset + x - sz.Width / 2, y - FontHeight / 2, ke.KText[0], outlineFontname, outlineEncoding, FontHeight, 0, yoffset);
                    outlines += outlineString;
                    double perc = (double)iK / (double)(kelems.Count - 1);

                    double t0 = ev.Start - 0.5 + iK * 0.04;
                    lastt0 = t0;
                    double t1 = t0 + 0.3;
                    double t2 = kStart - 0.02;
                    double t3 = t2 + 0.4;
                    double t4 = ev.End - 0.5 + iK * 0.04;
                    //t3 = (t4 - t2) * (1 - (1 - perc) * 0.25) + t2;
                    if (t3 > t4) t3 = t4;
                    double t5 = t4 + 0.3;

                    if (!isJp) t2 = ev.Start + (x0 - bakx0) / 600.0 + 0.3;

                    string mainColor = Common.scaleColor("3F40FF", "3F68FF", perc);
                    string shadowColor = Common.scaleColor("3F40FF", "3F68FF", 1 - perc);

                    for (int i = -4; i <= 4; i++)
                    {
                        ass_out.AppendEvent(70, "pt", t0, t1,
                            move(0 + i * 5, 0, 0, 0) + fad(0.15, 0) + a(1, "DD") + a(3, "DD") + c(1, mainColor) +
                            ybord(0) + xbord(10 + Math.Abs(i) * 1) + blur(8 + Math.Abs(i) * 1) +
                            t(bord(2).t() + blur(0).t()) +
                            p(4) + outlineString);
                    }
                    ass_out.AppendEvent(49, "pt", t1 - 0.3, t4,
                        pos(0, 0) + a(1, "FF") + a(3, "00") + c(3, shadowColor) + bord(5) + blur(5) + fad(0.3, 0) +
                        p(4) + outlineString);
                    if (isJp)
                    {
                        ass_out.AppendEvent(70, "pt", t2, t3,
                            pos(0, 0) + a(1, "00") + a(3, "00") + c(3, mainColor) + bord(2) + fad(0, t3 - t2) +
                            p(4) + outlineString);
                        ass_out.AppendEvent(69, "pt", t2, t3,
                            pos(0, 0) + a(1, "FF") + a(3, "00") + bord(8) + fad(0, t3 - t2) + blur(8) +
                            p(4) + outlineString);
                    }
                    ass_out.AppendEvent(50, "pt", t1, t4,
                        pos(0, 0) + fad(0.3, 0) + a(1, "00") + a(3, "00") + c(1, mainColor) + bord(2) +
                        p(4) + outlineString);

                    //if (isJp)
                    {
                        double dt = 1.7;
                        for (double ptt0 = t2; ptt0 < t4; ptt0 += dt)
                        {
                            double fadeout = 0.5;
                            double fadein = 0.5;
                            if (ptt0 == t2) fadein = 0.2;
                            double ptt1 = ptt0 + dt;
                            if (ptt1 > t4)
                            {
                                ptt1 = t4;
                                fadeout = 0.2;
                            }
                            string s = "";
                            double r1 = 40;
                            double r2 = 40;
                            for (int i = 0; i < 20; i++)
                            {
                                double ag = Common.RandomDouble(rnd, 0, 2 * Math.PI);
                                double ptx = Math.Cos(ag) * r1;
                                double pty = Math.Sin(ag) * r2;
                                if (i == 0) s += "m";
                                if (i == 1) s += " l";
                                s += string.Format(" {0} {1}", (int)ptx, (int)pty);
                            }
                            ass_out.AppendEvent(80, "pt", ptt0 - fadein, ptt1 + fadeout,
                                clip(4, outlineString) + pos(x + 5, y) + org(x, y) + a(1, "22") + fad(fadein, fadeout) +
                                t(frz((int)((ptt0 - ptt1) * 360 / dt)).t()) + blur(3) +
                                p(1) + s);
                        }
                    }

                    for (int i = 0; i < 10; i++)
                    {
                        string moveString = move(0, 0, 0 - 100, 0);
                        if (iEv == 2 || iEv == 4) moveString = move(0, 0, 0, 0 + 100);
                        if (iEv == 5 || iEv == 6) moveString = move(0, 0, 0, 0 + 100);
                        ass_out.AppendEvent(60 - i, "pt", t4 + i * 0.01, t5 + i * 0.01,
                            moveString + fad(0, 0.2) + a(1, "00") + a(3, "77") + c(1, mainColor) + bord(2) +
                            t(blur(5).t()) + t(0, 0.05, a(1, "DD").t() + a(3, "DD").t()) + t(0.25, 0.3, a(1, "00").t()) +
                            p(4) + outlineString);
                        ass_out.AppendEvent(50 - i, "pt", t4 + i * 0.01, t5 + i * 0.01,
                            moveString + move(x, y, x - 100, y) + fad(0, 0.2) + a(1, "FF") + a(3, "CC") + c(3, shadowColor) + bord(5) + blur(5) +
                            p(4) + outlineString);
                    }

                }

                if (iEv == 0 || iEv == 3 || true)
                {
                    double xstart = 0;
                    double xend = lastx0 + FontHeight;
                    if (!isJp)
                    {
                        xstart = bakx0 - FontHeight;
                        xend = PlayResX;
                    }
                    ass_out.AppendEvent(200, "pt", ev.Start, ev.Start + (xend - xstart) / 600.0,
                        clip(4, outlines) + move(xstart, y0 + FontHeight / 2, xend, y0 + FontHeight / 2) +
                        a(1, "00") + frz(-45) + blur(8) + fscx(200) +
                        p(1) + "m 10 -50 l 10 50 -10 50 -10 -50");
                }

                if (!isJp) continue;

                string[] SS =
                {
                    "xxxxxxxxx.xx.x",
                    "xx.x...xxxxxx."
                };
                for (int i = 0; i < 2; i++)
                {
                    if (SS[1 - i][iEv] == '.') continue;

                    Line line = new Line { X0 = lastx0 + 5, Y0 = y0 + FontHeight + 7, X1 = MarginLeft };
                    if (i == 1)
                    {
                        line.X0 = MarginLeft;
                        line.X1 = lastx0 + 5;
                        line.Y0 = y0 - 8;
                    }
                    line.Y1 = line.Y0;
                    CompositeCurve curve = new CompositeCurve { MinT = ev.End - 0.5 };
                    if (i == 1) curve.MinT = ev.Start - 0.5;
                    curve.MaxT = curve.MinT + Math.Abs(line.X0 - line.X1) / 1000.0;
                    curve.AddCurve(curve.MinT, curve.MaxT, line);
                    List<ASSPointF> pts = curve.GetPath_DT(1.0 / 1000.0);
                    foreach (ASSPointF pt in pts)
                    {
                        ass_out.AppendEvent(8, "pt", pt.T, pt.T + 0.8,
                            pos(pt.X, pt.Y) + a(1, "77") + a(3, "F4") + bord(1) + blur(1) + c(3, "FF973F") + c(1, "FF973F") +
                            t(0, 0.2, bord(6).t() + blur(6).t()) +
                            t(0.6, 0.8, bord(1).t() + blur(1).t()) +
                            p(1) + "m 0 0 l 1 0 1 1 0 1");
                        ass_out.AppendEvent(9, "pt", pt.T, pt.T + 0.8,
                            pos(pt.X, pt.Y) + a(1, "77") + a(3, "AA") + bord(1) + blur(1) + c(3, "FF973F") + c(1, "FF973F") +
                            t(0, 0.1, bord(1.8).t() + blur(1.8).t()) +
                            t(0.1, 0.2, bord(2.3).t() + blur(2.3).t()) +
                            t(0.2, 0.6, bord(1.8).t() + blur(1.8).t()) +
                            t(0.6, 0.8, bord(1).t() + blur(1).t()) +
                            p(1) + "m 0 0 l 1 0 1 1 0 1");
                        ass_out.AppendEvent(10, "pt", pt.T, pt.T + 0.8,
                            pos(pt.X, pt.Y) + a(1, "77") + a(3, "AA") + bord(0.7) + blur(0.7) +
                            t(0, 0.1, bord(1.6).t() + blur(1.6).t()) +
                            t(0.1, 0.2, bord(2.1).t() + blur(2.1).t()) +
                            t(0.2, 0.6, bord(1.6).t() + blur(1.6).t()) +
                            t(0.6, 0.8, bord(0.8).t() + blur(0.8).t() + a(3, "FF").t()) +
                            p(1) + "m 0 0 l 1 0 1 1 0 1");
                    }
                }
            }

            Console.WriteLine(ass_out.Events.Count);
            ass_out.SaveFile(this.OutFileName);
        }
    }
}
