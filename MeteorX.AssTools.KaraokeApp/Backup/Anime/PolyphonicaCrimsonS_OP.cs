using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.Model;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class PolyphonicaCrimsonS_OP : BaseAnime2
    {
        public PolyphonicaCrimsonS_OP()
        {
            this.FontHeight = 28;
            this.FontSpace = 2;
            this.IsAvsMask = true;
            this.MarginLeft = 20;
            this.MarginBottom = 20;
            this.MarginTop = 20;
            this.MarginRight = 20;
            this.PlayResX = 848;
            this.PlayResY = 480;
            this.InFileName = @"g:\workshop\Polyphonica Crimson S\op_k.ass";
            this.OutFileName = @"g:\workshop\Polyphonica Crimson S\op.ass";
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS() { Header = ass_in.Header, Events = new List<ASSEvent>() };

            string cPink = "8283FE";

            for (int iEv = 0; iEv < ass_in.Events.Count; iEv++)
            {
                bool isJp = iEv <= 12;
                //if (isJp) continue;
                //if (iEv != 13) continue;
                //if (!(iEv >= 0 && 7 >= iEv)) continue;
                ASSEvent ev = ass_in.Events[iEv];
                List<KElement> kelems = ev.SplitK(!isJp);
                if (!isJp)
                    foreach (KElement ke in kelems)
                        ke.KValue = 10;
                int x0 = MarginLeft;
                int y0 = (isJp) ? (PlayResY - MarginBottom - FontHeight) : MarginTop;
                int kSum = 0;
                string outlines = "";
                double ev0_sp = 500;
                for (int iK = 0; iK < kelems.Count; iK++)
                {
                    Console.WriteLine("{0} / {1} : {2} / {3}", iEv + 1, ass_in.Events.Count, iK + 1, kelems.Count);
                    this.MaskStyle = isJp ?
                        "Style: Default,DFGMaruGothic-Md,28,&H00FFFFFF,&HFFFFFFFF,&HFFFFFFFF,&HFFFFFFFF,0,0,0,0,100,100,0,0,0,0,0,5,0,0,0,128" :
                        "Style: Default,方正准圆_GBK,28,&H00FFFFFF,&HFFFFFFFF,&HFFFFFFFF,&HFFFFFFFF,0,0,0,0,100,100,0,0,0,0,0,5,0,0,0,134";
                    string evStyle = isJp ? "jp" : "cn";
                    string outlineFontname = isJp ? "DFGMaruGothic-Md" : "方正准圆_GBK";
                    int outlineEncoding = isJp ? 128 : 134;
                    KElement ke = kelems[iK];
                    Size sz = GetSize(ke.KText);
                    double kStart = ev.Start + kSum * 0.01;
                    double kEnd = kStart + ke.KValue * 0.01;
                    kSum += ke.KValue;
                    int x = x0 + this.FontSpace + sz.Width / 2;
                    int y = y0 + FontHeight / 2;
                    int x_an7 = x0;
                    int y_an7 = y0;
                    StringMask mask = GetMask(ke.KText, x, y);
                    x0 += this.FontSpace + sz.Width;
                    if (ke.KText.Trim().Length == 0) continue;
                    string outlineString = GetOutline(x - sz.Width / 2, y - FontHeight / 2, ke.KText[0], outlineFontname, outlineEncoding, FontHeight, 0, 191);
                    if (!isJp)
                        outlineString = GetOutline(x - sz.Width / 2, y - FontHeight / 2, ke.KText[0], outlineFontname, outlineEncoding, FontHeight, 0, 177);
                    outlines += outlineString;

                    if (iEv == 0)
                    {
                        double t0 = kStart - 1;
                        double t1 = kStart - 0.1;
                        double t2 = kStart + 0.4;
                        double t3 = ev.End - 0.5 + iK * 0.07;
                        double t4 = t3 + 0.5;

                        string cMain = "000071";
                        string cMain2 = "1DA4DD";// "10B7FC";

                        ass_out.AppendEvent(30, evStyle, t0, t3,
                            pos(x + 2, y + 2) + a(1, "00") + c(1, "000000") + blur(1.2) + fad(0.8, 0) +
                            ke.KText);
                        ass_out.AppendEvent(30, evStyle, t3, t4,
                            pos(x + 2, y + 2) + a(1, "00") + c(1, "000000") + blur(1.2) + fad(0, t4 - t3) +
                            ke.KText);
                        ass_out.AppendEvent(40, "pt", t1, 7 + x / ev0_sp + 0.3,
                            clip(4, outlineString) + pos(x, y) + a(1, "33") + c(1, "222222") + fad(t2 - t1, 0.3) +
                            p(1) + "m -20 -20 l 20 -20 20 20 -20 20");
                        ass_out.AppendEvent(40, "pt", 7 + x / ev0_sp - 0.3, t3,
                            clip(4, outlineString) + pos(x, y) + a(1, "33") + c(1, cMain) + fad(0.3, 0) +
                            p(1) + "m -20 -20 l 20 -20 20 20 -20 20");
                        ass_out.AppendEvent(40, "pt", t3, t4,
                            clip(4, outlineString) + pos(x, y) + a(1, "33") + c(1, cMain) + fad(0, t4 - t3) +
                            p(1) + "m -20 -20 l 20 -20 20 20 -20 20");
                        for (int i = 0; i < 4; i++)
                        {
                            int lumsz = 5 - i;
                            ass_out.AppendEvent(40, "pt", 7 + x / ev0_sp - 0.3, t4,
                                pos(0, 0) + a(1, "FF") + a(3, "DD") + blur(lumsz) + bord(lumsz) + c(3, "1D4FDD") + fad(0.3, 0.5) +
                                p(4) + outlineString);
                        }
                        double lumX = Common.RandomInt(rnd, x - 12, x + 12);
                        double lumY = Common.RandomInt(rnd, y - 12, y + 12);
                        for (int i = 0; i < 3; i++)
                        {
                            int lumsz = 8 + i * 2;
                            int lumsz2 = lumsz - 1;
                            ass_out.AppendEvent(50, "pt", t1, t2,
                                clip(4, outlineString) + pos(lumX, lumY) +
                                a(1, "44") + a(3, "00") + c(1, cMain) + c(3, cMain) + t(bord(lumsz).t() + blur(lumsz).t()) +
                                p(1) + "m 0 0 l 1 0 1 1 0 1");
                            ass_out.AppendEvent(50, "pt", t2, 7 + x / ev0_sp + 0.3, // Speed : 200
                                clip(4, outlineString) + pos(lumX, lumY) + fad(0, 0.3) +
                                a(1, "44") + a(3, "00") + c(1, cMain) + c(3, cMain) + bord(lumsz) + blur(lumsz) +
                                p(1) + "m 0 0 l 1 0 1 1 0 1");
                            ass_out.AppendEvent(50, "pt", 7 + x / ev0_sp - 0.3, t3,// Speed : 200
                                clip(4, outlineString) + pos(lumX, lumY) + fad(0.3, 0) +
                                a(1, "44") + a(3, "00") + c(1, cMain2) + c(3, cMain2) + bord(lumsz2) + blur(lumsz2) +
                                p(1) + "m 0 0 l 1 0 1 1 0 1");
                            ass_out.AppendEvent(50, "pt", t3, t4,
                                clip(4, outlineString) + pos(lumX, lumY) +
                                a(1, "44") + a(3, "00") + c(1, cMain2) + c(3, cMain2) + bord(lumsz2) + blur(lumsz2) +
                                t(bord(0).t() + blur(0).t()) +
                                p(1) + "m 0 0 l 1 0 1 1 0 1");
                        }
                        for (int i = 0; i < 4; i++)
                        {
                            double ptt0 = Common.RandomDouble(rnd, 7, 9);
                            double ptt1 = ptt0 + 0.5;
                            double ptx0 = Common.RandomDouble(rnd, x - 15, x + 15);
                            double pty0 = Common.RandomDouble(rnd, y - 15, y + 15) + 40;
                            double ptx1 = Common.RandomDouble(rnd, ptx0 + 10, ptx0 + 15);
                            double pty1 = Common.RandomDouble(rnd, pty0 - 80, pty0 - 100);
                            for (int j = 0; j < 3; j++)
                            {
                                double lumsz = 4 - j * 1;
                                ass_out.AppendEvent(90, "pt", ptt0, ptt1,
                                    move(ptx0, pty0, ptx1, pty1) + a(1, "44") + a(3, "00") +
                                    bord(lumsz) + blur(lumsz) + fad(0, ptt1 - ptt0) +
                                    p(1) + "m 0 0 l 1 0 1 1 0 1");
                            }
                            ass_out.AppendEvent(89, "pt", ptt0, ptt1,
                                move(ptx0, pty0, ptx1, pty1) + a(1, "44") + a(3, "77") +
                                bord(8) + blur(8) + fad(0, ptt1 - ptt0) +
                                p(1) + "m 0 0 l 1 0 1 1 0 1");
                        }
                    }
                    else if (iEv == 1)
                    {
                        double t0 = ev.Start - 0.5 + iK * 0.07;
                        double t1 = kStart - 0.1;
                        double t2 = kStart + 0.4;
                        double t3 = ev.End - 0.5 + iK * 0.07;
                        double t4 = t3 + 0.5;

                        string cMain = "FFFFFF";
                        if (iEv != 1) cMain = cPink;
                        string cShad = "222222";
                        if (iEv != 1) cShad = "FFFFFF";

                        ass_out.AppendEvent(30, evStyle, t0, t3,
                            pos(x + 2, y + 2) + a(1, "00") + c(1, "000000") + blur(1.2) + fad(0.8, 0) +
                            ke.KText);
                        ass_out.AppendEvent(30, evStyle, t3, t4,
                            pos(x + 2, y + 2) + a(1, "00") + c(1, "000000") + blur(1.2) + fad(0, t4 - t3) +
                            ke.KText);
                        ass_out.AppendEvent(40, "pt", t1, t3,
                            clip(4, outlineString) + pos(x, y) + a(1, "33") + c(1, cShad) + fad(t2 - t1, 0) +
                            p(1) + "m -20 -20 l 20 -20 20 20 -20 20");
                        ass_out.AppendEvent(40, "pt", t3, t4,
                            clip(4, outlineString) + pos(x, y) + a(1, "33") + c(1, cShad) + fad(0, t4 - t3) +
                            p(1) + "m -20 -20 l 20 -20 20 20 -20 20");
                        for (int i = 0; i < 4; i++)
                        {
                            int lumsz = 5 - i;
                            ass_out.AppendEvent(30, "pt", t1, t4,
                                pos(0, 0) + a(1, "FF") + a(3, "DD") + blur(lumsz) + bord(lumsz) + c(3, "FFFFFF") + fad(0.3, 0.5) +
                                p(4) + outlineString);
                        }
                        double lumX = Common.RandomInt(rnd, x - 12, x + 12);
                        double lumY = Common.RandomInt(rnd, y - 12, y + 12);
                        for (int i = 0; i < 3; i++)
                        {
                            int lumsz = 8 + i * 2;
                            ass_out.AppendEvent(50, "pt", t1, t2,
                                clip(4, outlineString) + pos(lumX, lumY) +
                                a(1, "44") + a(3, "00") + c(1, cMain) + c(3, cMain) + t(bord(lumsz).t() + blur(lumsz).t()) +
                                p(1) + "m 0 0 l 1 0 1 1 0 1");
                            ass_out.AppendEvent(50, "pt", t2, t3,
                                clip(4, outlineString) + pos(lumX, lumY) +
                                a(1, "44") + a(3, "00") + c(1, cMain) + c(3, cMain) + bord(lumsz) + blur(lumsz) +
                                p(1) + "m 0 0 l 1 0 1 1 0 1");
                            ass_out.AppendEvent(50, "pt", t3, t4,
                                clip(4, outlineString) + pos(lumX, lumY) +
                                a(1, "44") + a(3, "00") + c(1, cMain) + c(3, cMain) + bord(lumsz) + blur(lumsz) +
                                t(bord(0).t() + blur(0).t()) +
                                p(1) + "m 0 0 l 1 0 1 1 0 1");
                        }
                        for (int i = 0; i < 30 * (t3 - t1); i++)
                        {
                            double ptt0 = Common.RandomDouble(rnd, t1, t3);
                            double ptt1 = ptt0 + 0.5;
                            int pxid = Common.RandomInt(rnd, 0, mask.Points.Count - 1);
                            ASSPoint ogpt = mask.Points[pxid];
                            double ptx0 = ogpt.X;
                            double pty0 = ogpt.Y;
                            double ptx1 = Common.RandomDouble(rnd, ptx0 - 50, ptx0 + 50);
                            double pty1 = Common.RandomDouble(rnd, pty0 - 50, pty0 + 50);
                            string obj = CreatePolygon(rnd, 10, 15, 5);
                            string moveString = Common.RandomBool(rnd, 0.75) ? move(ptx0, pty0, ptx1, pty1) : move(ptx1, pty1, ptx0, pty0);
                            ass_out.AppendEvent(20, "pt", ptt0, ptt1,
                                moveString + a(1, "00") + a(3, "55") + c(1, "A266FD") + c(3, "A266FD") +
                                bord(2) + blur(2) + fad(0, ptt1 - ptt0) +
                                obj);
                            ass_out.AppendEvent(20, "pt", ptt0, ptt1,
                                moveString + a(1, "00") + a(3, "00") + c(1, "A266FD") + c(3, "A266FD") +
                                bord(1.2) + blur(1.2) + fad(0, ptt1 - ptt0) +
                                obj);
                            ass_out.AppendEvent(20, "pt", ptt0, ptt1,
                                moveString + a(1, "00") + a(3, "00") +
                                bord(0.8) + blur(0.8) + fad(0, ptt1 - ptt0) +
                                obj);
                        }
                    }
                    else if (isJp)
                    {
                        double t0 = ev.Start - 0.5 + iK * 0.07;
                        double t1 = kStart - 0.1;
                        double t2 = kStart + 0.4;
                        double t3 = ev.End - 0.5 + iK * 0.07;
                        double t4 = t3 + 0.5;

                        string cMain = "3C3DFF";
                        if (iEv == 2) cMain = "FF8D3C";
                        if (iEv == 3) cMain = "FC7D7F";
                        if (iEv == 4) cMain = "FFC6D2";
                        if (iEv == 5) cMain = "5758FF";
                        if (iEv == 6) cMain = "5758FF";
                        if (iEv == 7) cMain = "FF55C6";
                        if (iEv >= 8) cMain = "FF8D3C";
                        string cShad = "EEEEEE";

                        ass_out.AppendEvent(30, evStyle, t0, t3,
                            pos(x + 2, y + 2) + a(1, "00") + c(1, "000000") + blur(1.2) + fad(0.8, 0) +
                            ke.KText);
                        ass_out.AppendEvent(30, evStyle, t3, t4,
                            pos(x + 2, y + 2) + a(1, "00") + c(1, "000000") + blur(1.2) + fad(0, t4 - t3) +
                            ke.KText);
                        ass_out.AppendEvent(40, "pt", t1, t3,
                            clip(4, outlineString) + pos(x, y) + a(1, "33") + c(1, cShad) + fad(t2 - t1, 0) +
                            p(1) + "m -20 -20 l 20 -20 20 20 -20 20");
                        ass_out.AppendEvent(40, "pt", t3, t4,
                            clip(4, outlineString) + pos(x, y) + a(1, "33") + c(1, cShad) + fad(0, t4 - t3) +
                            p(1) + "m -20 -20 l 20 -20 20 20 -20 20");
                        for (int i = 0; i < 4; i++)
                        {
                            int lumsz = 5 - i;
                            ass_out.AppendEvent(30, "pt", t1, t4,
                                pos(0, 0) + a(1, "FF") + a(3, "DD") + blur(lumsz) + bord(lumsz) + c(3, cMain) + fad(0.3, 0.5) +
                                p(4) + outlineString);
                        }
                        double lumX = Common.RandomInt(rnd, x - 12, x + 12);
                        double lumY = Common.RandomInt(rnd, y - 12, y + 12);
                        for (int i = 0; i < 3; i++)
                        {
                            int lumsz = 8 + i * 2;
                            ass_out.AppendEvent(50, "pt", t1, t2,
                                clip(4, outlineString) + pos(lumX, lumY) +
                                a(1, "44") + a(3, "00") + c(1, cMain) + c(3, cMain) + t(bord(lumsz).t() + blur(lumsz).t()) +
                                p(1) + "m 0 0 l 1 0 1 1 0 1");
                            ass_out.AppendEvent(50, "pt", t2, t3,
                                clip(4, outlineString) + pos(lumX, lumY) +
                                a(1, "44") + a(3, "00") + c(1, cMain) + c(3, cMain) + bord(lumsz) + blur(lumsz) +
                                p(1) + "m 0 0 l 1 0 1 1 0 1");
                            ass_out.AppendEvent(50, "pt", t3, t4,
                                clip(4, outlineString) + pos(lumX, lumY) +
                                a(1, "44") + a(3, "00") + c(1, cMain) + c(3, cMain) + bord(lumsz) + blur(lumsz) +
                                t(bord(0).t() + blur(0).t()) +
                                p(1) + "m 0 0 l 1 0 1 1 0 1");
                        }

                        if (iEv == 2)
                        {
                            for (double ti = t1; ti <= kEnd + 0.3; ti += 0.01)
                            {
                                double ag = ti * Math.PI * 1.5;
                                int iag = (int)((ag / 2.0 / Math.PI) * 360) % 360;
                                string alpha = "00";
                                if (kEnd - ti < 0.3)
                                    alpha = Common.scaleAlpha("FF", "00", (kEnd - ti) / 0.3);
                                if (ti - t1 < 0.3)
                                    alpha = Common.scaleAlpha("FF", "00", (ti - t1) / 0.3);
                                ass_out.AppendEvent(10, "pt", ti, ti + 0.5,
                                    pos(x, y) + a(1, alpha) + be(1) + frz(-iag) + fad(0, 0.3) +
                                    p(1) + "m 1 0 l 0 30 -1 0 0 -30");
                            }
                        }
                        if (iEv == 3)
                        {
                            for (int i = 0; i < 15 * (t3 - t1); i++)
                            {
                                int pxid = Common.RandomInt(rnd, 0, mask.Points.Count - 1);
                                ASSPoint ogpt = mask.Points[pxid];
                                int ptx0 = ogpt.X;
                                int pty0 = y;
                                double ptt0 = Common.RandomDouble(rnd, t1, t3);
                                double ptt1 = ptt0 + 0.1;
                                ass_out.AppendEvent(110, "pt", ptt0, ptt1,
                                    clip(4, outlineString) + pos(ptx0, pty0) + a(1, "77") + blur(1.8) + fad(0, 0) + c(1, "000000") + frz(-30) +
                                    p(1) + "m 2 0 l 0 20 -2 0 0 -20");
                            }
                        }
                        if (iEv == 4)
                        {
                            for (int i = 0; i < 25 * (t3 - t1); i++)
                            {
                                double ptt0 = Common.RandomDouble(rnd, t1, t1 + 0.3);
                                double ptt1 = ptt0 + Common.RandomDouble(rnd, 0, t3 - t1 - 0.3) + 1;
                                int pxid = Common.RandomInt(rnd, 0, mask.Points.Count - 1);
                                ASSPoint ogpt = mask.Points[pxid];
                                double ptx0 = ogpt.X;
                                double pty0 = ogpt.Y;
                                double ptx1 = Common.RandomDouble(rnd, ptx0 - 50, ptx0 - 150);
                                double pty1 = Common.RandomDouble(rnd, pty0 + 10, pty0 + 50);
                                string obj = CreatePolygon(rnd, 7, 10, 5);
                                string moveString = move(ptx0, pty0, ptx1, pty1, ptt1 - ptt0 - 1, ptt1 - ptt0);
                                /*ass_out.AppendEvent(110, "pt", ptt0, ptt1,
                                    moveString + a(1, "00") + a(3, "55") + c(1, "FFF3F3") + c(3, "FFF3F3") +
                                    bord(2) + blur(2) + fad(0, ptt1 - ptt0) +
                                    obj);*/
                                ass_out.AppendEvent(110, "pt", ptt0, ptt1,
                                    moveString + a(1, "00") + a(3, "00") + c(1, "FFF3F3") + c(3, "FFF3F3") +
                                    bord(1.2) + blur(1.2) + fad(0, ptt1 - ptt0) +
                                    obj);
                                ass_out.AppendEvent(110, "pt", ptt0, ptt1,
                                    moveString + a(1, "00") + a(3, "00") +
                                    bord(0.8) + blur(0.8) + fad(0, ptt1 - ptt0) +
                                    obj);
                            }
                        }
                        if (iEv == 5 || iEv == 6 || iEv == 7)
                        {
                            ass_out.AppendEvent(109, "jp", kStart, kStart + 0.15,
                                pos(x, y) + a(1, "00") + a(3, "44") + bord(4) + blur(4) + fad(0, 0.12) +
                                ke.KText);
                            ass_out.AppendEvent(109, "jp", kStart, kStart + 0.15,
                                pos(x, y) + a(1, "00") + a(3, "44") + bord(6) + blur(6) + fad(0, 0.12) +
                                ke.KText);
                            for (int i = 0; i < 2; i++)
                            {
                                CompositeCurve curve = new CompositeCurve { MinT = kStart - 0.5 * 0.25, MaxT = kStart + 0.5 * 0.25 };
                                Line line = new Line { X0 = x + 50 - 5 - 5, X1 = x - 50 - 5 - 5, Y0 = y - 30 - 5 - 5, Y1 = y + 30 - 5 - 5 };
                                if (i == 1)
                                    line = new Line { Y1 = y - 30 + i * 15 - 5, Y0 = y + 30 + i * 15 - 5, X1 = x + 50 + i * 15 - 5, X0 = x - 50 + i * 15 - 5 };
                                curve.AddCurve(curve.MinT, curve.MaxT, line);
                                List<ASSPointF> pts = curve.GetPath_Dis(1, 1.1);
                                foreach (ASSPointF pt in pts)
                                {
                                    if (!Common.InRange(0, PlayResX, pt.X) || !Common.InRange(0, PlayResY, pt.Y)) continue;
                                    ass_out.AppendEvent(0, "pt", pt.T, pt.T + 0.25,
                                        pos(pt.X, pt.Y) + a(1, "00") + a(3, "77") + c(1, cMain) + c(3, cMain) + fad(0, 0.1) +
                                        bord(1.5) + blur(1.5) +
                                        p(1) + "m 0 0 l 1 0 1 1 0 1");
                                    ass_out.AppendEvent(0, "pt", pt.T, pt.T + 0.25,
                                        pos(pt.X, pt.Y) + a(1, "00") + a(3, "BB") + c(1, cMain) + c(3, cMain) + fad(0, 0.1) +
                                        bord(4) + blur(4) +
                                        p(1) + "m 0 0 l 1 0 1 1 0 1");
                                    ass_out.AppendEvent(2, "pt", pt.T, pt.T + 0.25,
                                        pos(pt.X, pt.Y) + a(1, "00") + a(3, "44") + c(1, "FFFFFF") + c(3, "FFFFFF") + fad(0, 0.1) +
                                        bord(1) + blur(1) +
                                        p(1) + "m 0 0 l 1 0 1 1 0 1");
                                }
                                pts = curve.GetPath_DT(0.01);
                                foreach (ASSPointF pt in pts)
                                {
                                    if (!Common.InRange(0, PlayResX, pt.X) || !Common.InRange(0, PlayResY, pt.Y)) continue;
                                    ass_out.AppendEvent(115, "pt", pt.T, pt.T + 0.01,
                                        pos(pt.X, pt.Y) + a(1, "00") + a(3, "00") + c(1, "FFFFFF") + c(3, "FFFFFF") +
                                        bord(8) + blur(8) +
                                        p(1) + "m 0 0 l 1 0 1 1 0 1");
                                    ass_out.AppendEvent(114, "pt", pt.T, pt.T + 0.01,
                                        pos(pt.X, pt.Y) + a(1, "00") + a(3, "00") + c(1, "FFFFFF") + c(3, "FFFFFF") +
                                        bord(6) + blur(6) +
                                        p(1) + "m 0 0 l 1 0 1 1 0 1");
                                }
                            }
                        }
                        if (iEv >= 8 && iEv <= 12)
                        {
                            ass_out.AppendEvent(109, "jp", kStart, kStart + 0.35,
                                pos(x, y) + a(1, "00") + a(3, "44") + bord(4) + blur(4) + fad(0, 0.25) +
                                ke.KText);
                            ass_out.AppendEvent(109, "jp", kStart, kStart + 0.35,
                                pos(x, y) + a(1, "00") + a(3, "44") + bord(6) + blur(6) + fad(0, 0.25) +
                                ke.KText);
                            string pCol = "A266FD";
                            if (iEv >= 11) pCol = Common.scaleColor("FFFFFF", pCol, 0.5);
                            for (int i = 0; i < (30 + (iEv - 7) * 10) * (t3 - t1); i++)
                            {
                                double ptt0 = Common.RandomDouble(rnd, t1, t3);
                                double ptt1 = ptt0 + 0.5;
                                int pxid = Common.RandomInt(rnd, 0, mask.Points.Count - 1);
                                ASSPoint ogpt = mask.Points[pxid];
                                double ptx0 = ogpt.X;
                                double pty0 = ogpt.Y;
                                double ptx1 = Common.RandomDouble(rnd, ptx0 - 50, ptx0 + 50);
                                double pty1 = Common.RandomDouble(rnd, pty0 - 50, pty0 + 50);
                                string obj = CreatePolygon(rnd, 10, 15, 5);
                                string moveString = Common.RandomBool(rnd, 0.75) ? move(ptx0, pty0, ptx1, pty1) : move(ptx1, pty1, ptx0, pty0);
                                ass_out.AppendEvent(20, "pt", ptt0, ptt1,
                                    moveString + a(1, "00") + a(3, "55") + c(1, pCol) + c(3, pCol) +
                                    bord(2) + blur(2) + fad(0, ptt1 - ptt0) +
                                    obj);
                                ass_out.AppendEvent(20, "pt", ptt0, ptt1,
                                    moveString + a(1, "00") + a(3, "00") + c(1, pCol) + c(3, pCol) +
                                    bord(1.2) + blur(1.2) + fad(0, ptt1 - ptt0) +
                                    obj);
                                ass_out.AppendEvent(20, "pt", ptt0, ptt1,
                                    moveString + a(1, "00") + a(3, "00") +
                                    bord(0.8) + blur(0.8) + fad(0, ptt1 - ptt0) +
                                    obj);
                            }
                        }
                    } if (!isJp)
                    {
                        if (iEv == 13)
                        {
                            double t0 = kStart - 1;
                            double t1 = kStart - 0.1;
                            double t2 = kStart + 0.4;
                            double t3 = ev.End - 0.5 + iK * 0.07;
                            double t4 = t3 + 0.5;

                            string cMain = "000071";
                            string cMain2 = "1DA4DD";// "10B7FC";

                            ass_out.AppendEvent(30, evStyle, t0, t3,
                                pos(x + 2, y + 2) + a(1, "00") + c(1, "000000") + blur(1.2) + fad(0.8, 0) +
                                ke.KText);
                            ass_out.AppendEvent(30, evStyle, t3, t4,
                                pos(x + 2, y + 2) + a(1, "00") + c(1, "000000") + blur(1.2) + fad(0, t4 - t3) +
                                ke.KText);
                            ass_out.AppendEvent(40, "pt", t1, 7 + x / ev0_sp + 0.3,
                                clip(4, outlineString) + pos(x, y) + a(1, "33") + c(1, "222222") + fad(t2 - t1, 0.3) +
                                p(1) + "m -20 -20 l 20 -20 20 20 -20 20");
                            ass_out.AppendEvent(40, "pt", 7 + x / ev0_sp - 0.3, t3,
                                clip(4, outlineString) + pos(x, y) + a(1, "33") + c(1, cMain) + fad(0.3, 0) +
                                p(1) + "m -20 -20 l 20 -20 20 20 -20 20");
                            ass_out.AppendEvent(40, "pt", t3, t4,
                                clip(4, outlineString) + pos(x, y) + a(1, "33") + c(1, cMain) + fad(0, t4 - t3) +
                                p(1) + "m -20 -20 l 20 -20 20 20 -20 20");
                            for (int i = 0; i < 4; i++)
                            {
                                int lumsz = 5 - i;
                                ass_out.AppendEvent(40, "pt", 7 + x / ev0_sp - 0.3, t4,
                                    pos(0, 0) + a(1, "FF") + a(3, "DD") + blur(lumsz) + bord(lumsz) + c(3, "1D4FDD") + fad(0.3, 0.5) +
                                    p(4) + outlineString);
                            }
                            double lumX = Common.RandomInt(rnd, x - 12, x + 12);
                            double lumY = Common.RandomInt(rnd, y - 12, y + 12);
                            for (int i = 0; i < 3; i++)
                            {
                                int lumsz = 8 + i * 2;
                                int lumsz2 = lumsz - 1;
                                ass_out.AppendEvent(50, "pt", t1, t2,
                                    clip(4, outlineString) + pos(lumX, lumY) +
                                    a(1, "44") + a(3, "00") + c(1, cMain) + c(3, cMain) + t(bord(lumsz).t() + blur(lumsz).t()) +
                                    p(1) + "m 0 0 l 1 0 1 1 0 1");
                                ass_out.AppendEvent(50, "pt", t2, 7 + x / ev0_sp + 0.3, // Speed : 200
                                    clip(4, outlineString) + pos(lumX, lumY) + fad(0, 0.3) +
                                    a(1, "44") + a(3, "00") + c(1, cMain) + c(3, cMain) + bord(lumsz) + blur(lumsz) +
                                    p(1) + "m 0 0 l 1 0 1 1 0 1");
                                ass_out.AppendEvent(50, "pt", 7 + x / ev0_sp - 0.3, t3,// Speed : 200
                                    clip(4, outlineString) + pos(lumX, lumY) + fad(0.3, 0) +
                                    a(1, "44") + a(3, "00") + c(1, cMain2) + c(3, cMain2) + bord(lumsz2) + blur(lumsz2) +
                                    p(1) + "m 0 0 l 1 0 1 1 0 1");
                                ass_out.AppendEvent(50, "pt", t3, t4,
                                    clip(4, outlineString) + pos(lumX, lumY) +
                                    a(1, "44") + a(3, "00") + c(1, cMain2) + c(3, cMain2) + bord(lumsz2) + blur(lumsz2) +
                                    t(bord(0).t() + blur(0).t()) +
                                    p(1) + "m 0 0 l 1 0 1 1 0 1");
                            }
                        }
                        else
                        {
                            double t0 = ev.Start - 0.5 + iK * 0.07;
                            double t1 = kStart - 0.1;
                            double t2 = kStart + 0.4;
                            double t3 = ev.End - 0.5 + iK * 0.07;
                            double t4 = t3 + 0.5;

                            string cMain = "3C3DFF";
                            int jEv = iEv - 13;
                            if (jEv == 1) cMain = "FFFFFF";
                            if (jEv == 2) cMain = "FF8D3C";
                            if (jEv == 3) cMain = "FC7D7F";
                            if (jEv == 4) cMain = "FFC6D2";
                            if (jEv == 5) cMain = "5758FF";
                            if (jEv == 6) cMain = "5758FF";
                            if (jEv == 7) cMain = "FF55C6";
                            if (jEv >= 8) cMain = "FF8D3C";
                            string cShad = "EEEEEE";
                            if (jEv == 1) cShad = "222222";

                            ass_out.AppendEvent(30, evStyle, t0, t3,
                                pos(x + 2, y + 2) + a(1, "00") + c(1, "000000") + blur(1.2) + fad(0.8, 0) +
                                ke.KText);
                            ass_out.AppendEvent(30, evStyle, t3, t4,
                                pos(x + 2, y + 2) + a(1, "00") + c(1, "000000") + blur(1.2) + fad(0, t4 - t3) +
                                ke.KText);
                            ass_out.AppendEvent(40, "pt", t1, t3,
                                clip(4, outlineString) + pos(x, y) + a(1, "33") + c(1, cShad) + fad(t2 - t1, 0) +
                                p(1) + "m -20 -20 l 20 -20 20 20 -20 20");
                            ass_out.AppendEvent(40, "pt", t3, t4,
                                clip(4, outlineString) + pos(x, y) + a(1, "33") + c(1, cShad) + fad(0, t4 - t3) +
                                p(1) + "m -20 -20 l 20 -20 20 20 -20 20");
                            for (int i = 0; i < 4; i++)
                            {
                                int lumsz = 5 - i;
                                ass_out.AppendEvent(30, "pt", t1, t4,
                                    pos(0, 0) + a(1, "FF") + a(3, "DD") + blur(lumsz) + bord(lumsz) + c(3, cMain) + fad(0.3, 0.5) +
                                    p(4) + outlineString);
                            }
                            double lumX = Common.RandomInt(rnd, x - 12, x + 12);
                            double lumY = Common.RandomInt(rnd, y - 12, y + 12);
                            for (int i = 0; i < 3; i++)
                            {
                                int lumsz = 8 + i * 2;
                                ass_out.AppendEvent(50, "pt", t1, t2,
                                    clip(4, outlineString) + pos(lumX, lumY) +
                                    a(1, "44") + a(3, "00") + c(1, cMain) + c(3, cMain) + t(bord(lumsz).t() + blur(lumsz).t()) +
                                    p(1) + "m 0 0 l 1 0 1 1 0 1");
                                ass_out.AppendEvent(50, "pt", t2, t3,
                                    clip(4, outlineString) + pos(lumX, lumY) +
                                    a(1, "44") + a(3, "00") + c(1, cMain) + c(3, cMain) + bord(lumsz) + blur(lumsz) +
                                    p(1) + "m 0 0 l 1 0 1 1 0 1");
                                ass_out.AppendEvent(50, "pt", t3, t4,
                                    clip(4, outlineString) + pos(lumX, lumY) +
                                    a(1, "44") + a(3, "00") + c(1, cMain) + c(3, cMain) + bord(lumsz) + blur(lumsz) +
                                    t(bord(0).t() + blur(0).t()) +
                                    p(1) + "m 0 0 l 1 0 1 1 0 1");
                            }
                        }
                    }
                }

                if (iEv == 0 || iEv == 13)
                {
                    double ptx0 = MarginLeft;
                    double ptx1 = x0 + 30;
                    double pty = y0 + FontHeight / 2;
                    string cMain = "1DA4DD";
                    double tStart = 7;
                    for (int i = 0; i < 3; i++)
                    {
                        int lumsz = 18 + i * 2;
                        ass_out.AppendEvent(70, "pt", tStart, tStart + (ptx1 - ptx0) / ev0_sp,
                            clip(4, outlines) + move(ptx0, pty, ptx1, pty) + fad(0.3, 0.3) +
                            bord(lumsz) + blur(lumsz) + a(1, "44") + a(3, "00") + c(1, cMain) + c(3, cMain) +
                            p(1) + "m 0 -20 l 1 -20 1 20 0 20");
                    }
                }
            }

            Console.WriteLine(ass_out.Events.Count);
            ass_out.SaveFile(this.OutFileName);
        }
    }
}
