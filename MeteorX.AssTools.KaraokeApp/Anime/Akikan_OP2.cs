using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.Model;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class Akikan_OP2 : BaseAnime2
    {
        public Akikan_OP2()
        {
            InFileName = @"G:\Workshop\akikan\op_k.ass";
            OutFileName = @"G:\Workshop\akikan\op_jp.ass";

            this.FontWidth = 44;
            this.FontHeight = 44;
            this.FontSpace = 3;

            this.PlayResX = 1280;
            this.PlayResY = 720;
            this.MarginBottom = 25;
            this.MarginLeft = 25;
            this.MarginRight = 25;
            this.MarginTop = 25;

            this.Font = new System.Drawing.Font("DFGMaruMoji-SL", 44, GraphicsUnit.Pixel);
            this.IsAvsMask = true;
        }

        public override void Run()
        {
            string ms3 = "Style: Default,DFGMaruMoji-SL,44,&H0000FFFF,&HFF000000,&H00FF0000,&HFF000000,0,0,0,0,100,100,2,0,1,2,0,5,30,30,10,128";
            string msc = "Style: Default,華康少女文字W5(P),44,&H0000FFFF,&HFF000000,&H00FF0000,&HFF000000,0,0,0,0,100,100,2,0,1,2,0,5,30,30,10,136";
            // 8BFF97 green

            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS();

            ass_out.Header = ass_in.Header;
            ass_out.Events = new List<ASSEvent>();

            Random rnd = new Random();

            int testEv = -1;

            for (int iEv = 0; iEv <= 22; iEv++)
            {
                if (testEv >= 0 && iEv != testEv) continue;
                ASSEvent ev = ass_in.Events[iEv];
                List<KElement> kelems = ev.SplitK(false);

                this.MaskStyle = ms3;
                double sw = (iEv % 2 == 0) ? 0 : GetTotalWidth(ev);
                /// an7 pos
                int x0 = (iEv % 2 == 0) ? MarginLeft : (int)(PlayResX - MarginRight - sw);
                int y0 = PlayResY - MarginBottom - FontHeight;

                int kSum = 0;

                for (int iK = 0; iK < kelems.Count; iK++)
                {
                    //if (iK > 3) continue;
                    Console.WriteLine("{0} / {1} : {2} / {3}", iEv + 1, ass_in.Events.Count, iK + 1, kelems.Count);
                    KElement ke = kelems[iK];
                    double r = (double)iK / (double)(kelems.Count - 1);
                    this.MaskStyle = ms3;
                    StringMask mask = GetMask(ke.KText, x0, y0);
                    Size sz = new Size(mask.Width, mask.Height);

                    double kStart = ev.Start + kSum * 0.01;
                    double kEnd = kStart + ke.KValue * 0.01;
                    kSum += ke.KValue;

                    /// an5 pos
                    int x = x0 + this.FontSpace + sz.Width / 2;
                    int y = y0 + FontHeight / 2;

                    x0 += this.FontSpace + sz.Width;
                    y0 = y0;

                    if (ke.KText.Trim().Length == 0) continue;

                    string col1 = "3CD846";
                    string green = col1;
                    if (iEv == 1) col1 = "C13BA5";
                    if (iEv == 2) col1 = "3E58A6";
                    if (iEv == 3) col1 = (iK % 2 == 0) ? "D4004D" : "E79805";
                    if (iEv == 4)
                        if (iK <= 7) col1 = (iK % 2 == 0) ? "D4004D" : "E79805";
                        else if (iK <= 9) col1 = "1FBD3E";
                        else col1 = "C41426";
                    if (iEv == 5)
                        if (iK >= 8 && iK <= 10) col1 = "F1D53C";
                    if (iEv == 6)
                        if (iK >= 4 && iK <= 8) col1 = "F1D53C";
                        else if (iK >= 10) col1 = "F25756";
                    if (iEv == 7)
                        if (iK >= 6 && iK <= 10) col1 = "5D477C";
                    if (iEv == 8)
                        if (iK <= 1) col1 = "5D477C";
                    if (iEv == 9) col1 = "4399AE";
                    if (iEv == 10 || iEv == 11) col1 = "AE4343";
                    if (iEv == 12) col1 = "4344AE";
                    if (iEv == 13) col1 = (iK % 2 == 0) ? green : "DC49A6";
                    if (iEv == 14) col1 = (iK % 2 == 0) ? green : "C13BA5";
                    if (iEv == 15) col1 = (iK % 2 == 0) ? green : "3E58A6";
                    if (iEv == 16) col1 = (iK % 2 == 0) ? "D4004D" : "E79805";
                    if (iEv == 17)
                        if (iK <= 7) col1 = (iK % 2 == 0) ? "D4004D" : "E79805";
                        else if (iK <= 9) col1 = "1FBD3E";
                        else col1 = "C41426";
                    if (iEv == 18 || iEv == 19) col1 = (iK % 2 == 0) ? green : "4B84C7";
                    if (iEv == 20) col1 = (iK % 2 == 0) ? green : "C3577F";
                    if (iEv == 21) col1 = (iK % 2 == 0) ? green : "882DB5";
                    if (iEv == 22)
                        if (iK >= 5 && iK <= 10) col1 = (iK % 2 == 0) ? "D4004D" : "E79805";
                        else if (iK <= 12) col1 = "1FBD3E";
                        else col1 = "C41426";


                    col1 = ASSColor.HtmlToASS(col1);

                    // bezier
                    if (iEv >= 5)
                    {
                        List<ASSPoint> pts = new Bezier(
                            new ASSPoint { X = (iEv % 2 == 0) ? x - 100 : x + 100, Y = y - 50 },
                            new ASSPoint { X = x + ((iEv % 2 == 0) ? 50 : -40), Y = y - 50 },
                            new ASSPoint { X = x + ((iEv % 2 == 0) ? 50 : -40), Y = y + 30 },
                            new ASSPoint { X = (iEv % 2 == 0) ? x - 100 : x + 100, Y = y + 30 }
                            ).Create(0.005f);
                        double lastt0 = kStart - 0.3;
                        for (int i = 0; i < pts.Count; i++)
                        {
                            ASSPoint pt = pts[i];
                            double t0 = kStart - 0.3 + 0.6 * (double)i / (double)pts.Count;
                            double t1 = t0 + 0.3;
                            ass_out.Events.Add(
                                ev.StartReplace(t0).EndReplace(t1).StyleReplace("pt").LayerReplace(15).TextReplace(
                                ASSEffect.pos(pt.X, pt.Y) + ASSEffect.c(1, "FFFFFF") + ASSEffect.a(1, "00") +
                                ASSEffect.c(3, col1) + ASSEffect.a(3, "77") + ASSEffect.bord((iEv >= 0) ? 2 : 1) + ASSEffect.blur((iEv >= 0) ? 2 : 1) + ASSEffect.fad(0, 0.3) +
                                ASSEffect.t(0, t1 - t0, ASSEffect.c(1, "FFFFFF").t() + ASSEffect.c(3, "FFFFFF").t()) +
                                @"{\p1}m 0 0 l 1 0 1 1 0 1"));
                            if (t0 - lastt0 >= 0.04 || i + 1 == pts.Count)
                            {
                                string colb = Common.scaleColor(col1, "FFFFFF", 0.3);
                                ass_out.Events.Add(
                                ev.StartReplace(t0).EndReplace(t0 + 0.04).StyleReplace("pt").LayerReplace(16).TextReplace(
                                ASSEffect.pos(pt.X, pt.Y) + ASSEffect.c(1, "FFFFFF") + ASSEffect.a(1, "00") +
                                ASSEffect.c(3, colb) + ASSEffect.a(3, "00") + ASSEffect.bord((iEv >= 0) ? 6 : 4) + ASSEffect.blur((iEv >= 0) ? 5 : 3) +
                                @"{\p1}m 0 0 l 1 0 1 1 0 1"));
                                lastt0 = t0;
                            }
                        }
                    }

                    //if (iEv <= 2)
                    {
                        double jumpTime = 0.5;
                        double t0 = kStart - jumpTime;
                        double t1 = t0;
                        double dt = 0.01;
                        Func<double, double> f_y = ti => y - 1100.0 * (0.25 * jumpTime * jumpTime - ((ti - t0) - 0.5 * jumpTime) * ((ti - t0) - 0.5 * jumpTime));
                        Func<double, double> f_x = ti => x;
                        Func<double, int> f_fs = ti => (int)(1 + Math.Round((ti - kStart + jumpTime) / jumpTime * FontWidth));
                        if (iEv >= 5 && iEv <= 12)
                        {
                            f_y = ti => y;
                            f_x = ti => x - 100 * (ti - t0) / jumpTime + 100;
                            f_fs = ti => FontWidth;
                        }
                        double d12 = 0.2;
                        if (iEv > 12)
                        {
                            t0 -= d12;
                            t1 = t0;
                            f_fs = ti => (int)(1 + Math.Round((ti - kStart + d12 + jumpTime) / jumpTime * FontWidth));
                        }
                        for (; t1 <= kStart - ((iEv > 12) ? d12 : 0); t1 += dt)
                        {
                            ass_out.Events.Add(
                                ev.StartReplace(t1 - dt - 0.1).EndReplace(t1 - 0.1).TextReplace(
                                ASSEffect.pos(f_x(t1), f_y(t1)) + ASSEffect.fs(f_fs(t1)) +
                                ASSEffect.c(1, (iEv >= 5) ? "555555" : col1) + ASSEffect.c(3, "FFFFFF") +
                                ke.KText));
                            ass_out.Events.Add(
                                ev.StartReplace(t1 - dt - 0.1).EndReplace(t1 - dt - 0.1 + 0.4).LayerReplace(5).TextReplace(
                                ASSEffect.pos(f_x(t1), f_y(t1)) + ASSEffect.fs(f_fs(t1)) + ASSEffect.be(1) +
                                ASSEffect.c(1, "FFFFFF") + ASSEffect.c(3, "FFFFFF") +
                                ASSEffect.a(1, "AA") + ASSEffect.a(3, "AA") + ASSEffect.fad(0, 0.3) +
                                ke.KText));
                        }
                        t1 -= dt + 0.1;
                        double t2 = ev.End + r * 1 - 0.9;
                        if (iEv >= 5)
                        {
                            if (iEv >= 9)
                            {
                                ass_out.Events.Add(
                                    ev.StartReplace(kStart - 0.05).EndReplace(t2).LayerReplace(8).TextReplace(
                                    ASSEffect.pos(x, y) + ASSEffect.a(3, "44") + ASSEffect.a(1, "FF") + ASSEffect.c(3, col1) +
                                    ASSEffect.bord((iEv >= 13) ? 8 : 5) + ASSEffect.blur((iEv >= 13) ? 7 : 4) + ASSEffect.fad(0, 0.3) +
                                    ke.KText));
                            }
                            else
                            {
                                ass_out.Events.Add(
                                    ev.StartReplace(kStart - 0.05).EndReplace(t2).LayerReplace(8).TextReplace(
                                    ASSEffect.pos(x, y) + ASSEffect.a(3, "44") + ASSEffect.a(1, "FF") + ASSEffect.c(3, "FFFFFF") +
                                    ASSEffect.bord(3) + ASSEffect.blur(2) + ASSEffect.fad(0, 0.3) +
                                    ke.KText));
                            }
                            ass_out.Events.Add(
                                ev.StartReplace(kStart - 0.05).EndReplace(kStart + ((iEv >= 13) ? 0.3 : 0.15)).LayerReplace(15).TextReplace(
                                ASSEffect.pos(x, y) + ASSEffect.a(3, "00") + ASSEffect.c(3, "FFFFFF") + ASSEffect.a(1, "00") + ASSEffect.c(1, "FFFFFF") +
                                ASSEffect.bord(5) + ASSEffect.blur(4) +
                                ASSEffect.fad(0, 0.2) + ke.KText));
                            ass_out.Events.Add(
                                ev.StartReplace(kStart - 0.05).EndReplace(t2).LayerReplace(13).TextReplace(
                                ASSEffect.pos(x, y) + ASSEffect.a(3, "FF") + ASSEffect.a(1, "00") + ASSEffect.c(1, col1) +
                                ke.KText));
                        }
                        ass_out.Events.Add(
                            ev.StartReplace(t1).EndReplace(t2).TextReplace(
                                ASSEffect.pos(x, y) +
                                ASSEffect.c(1, (iEv >= 5) ? "555555" : col1) + ASSEffect.c(3, "FFFFFF") +
                                ke.KText));
                        {
                            double ag = Common.RandomDouble(rnd, 0, Math.PI * 2);
                            if (iEv >= 9 && iEv <= 11) ag = Math.PI * 0.75;
                            double ra = 100;
                            double x1 = x + ra * Math.Cos(ag);
                            double y1 = y + ra * Math.Sin(ag);
                            bool first = true;
                            for (double t3 = t2; t3 < t2 + 0.4; t3 += 0.02)
                            {
                                double t4 = t3 + 0.5;
                                string cole = Common.scaleColor(col1, "FFFFFF", (t3 - t2) / 0.5);
                                if (iEv <= 3) cole = "FFFFFF";
                                if (iEv >= 9 && iEv <= 11) cole = "FFFFFF";
                                ass_out.Events.Add(
                                    ev.StartReplace(t3).EndReplace(t4).LayerReplace(first ? 10 : 5).TextReplace(
                                    ASSEffect.move(x, y, x1, y1) + ASSEffect.fad(0, 0.3) + ASSEffect.be(first ? 0 : 1) +
                                    ASSEffect.c(1, first ? col1 : cole) + ASSEffect.a(1, first ? "00" : "AA") +
                                    ASSEffect.c(3, first ? "FFFFFF" : cole) + ASSEffect.a(3, first ? "00" : "AA") +
                                        ke.KText));
                                first = false;
                            }
                        }
                    }
                }

            }

            for (int iiEv = 23; iiEv <= 45; iiEv++)
            {
                break;
                int iEv = iiEv - 23;
                if (testEv >= 0 && iEv != testEv) continue;
                ASSEvent ev = ass_in.Events[iiEv];
                List<KElement> kelems = ev.SplitK(true);

                this.MaskStyle = msc;
                double sw = (iEv % 2 == 0) ? 0 : GetTotalWidth(ev);
                /// an7 pos
                int x0 = (iEv % 2 == 0) ? MarginLeft : (int)(PlayResX - MarginRight - sw);
                int y0 = MarginTop;

                int kSum = 0;

                for (int iK = 0; iK < kelems.Count; iK++)
                {
                    //if (iK > 3) continue;
                    Console.WriteLine("{0} / {1} : {2} / {3}", iiEv + 1, ass_in.Events.Count, iK + 1, kelems.Count);
                    KElement ke = kelems[iK];
                    double r = (double)iK / (double)(kelems.Count - 1);
                    this.MaskStyle = ms3;
                    Size sz = GetSize(ke.KText);

                    double kStart = ev.Start + kSum * 0.01;
                    double kEnd = kStart + ke.KValue * 0.01;
                    kSum += ke.KValue;

                    /// an5 pos
                    int x = x0 + this.FontSpace + sz.Width / 2;
                    int y = y0 + FontHeight / 2;

                    x0 += this.FontSpace + sz.Width;
                    y0 = y0;

                    if (ke.KText.Trim().Length == 0) continue;

                    string col1 = "3CD846";
                    string green = col1;
                    if (iEv == 1) col1 = "C13BA5";
                    if (iEv == 2) col1 = "3E58A6";
                    if (iEv == 3) col1 = (iK % 2 == 0) ? "D4004D" : "E79805";
                    if (iEv == 4)
                        if (iK <= 12) col1 = (iK % 2 == 0) ? "D4004D" : "E79805";
                        else if (iK <= 13) col1 = "1FBD3E";
                        else col1 = "C41426";
                    if (iEv == 5)
                        if (iK >= 10) col1 = "F1D53C";
                    if (iEv == 6)
                        if (iK >= 4 && iK <= 5) col1 = "F1D53C";
                        else if (iK >= 10) col1 = "F25756";
                    if (iEv == 7)
                        if (iK >= 5) col1 = "5D477C";
                    if (iEv == 8)
                        if (iK >= 3 && iK <= 4) col1 = "5D477C";
                    if (iEv == 9) col1 = "4399AE";
                    if (iEv == 10 || iEv == 11) col1 = "AE4343";
                    if (iEv == 12) col1 = "4344AE";
                    if (iEv == 13) col1 = (iK % 2 == 0) ? green : "DC49A6";
                    if (iEv == 14) col1 = (iK % 2 == 0) ? green : "C13BA5";
                    if (iEv == 15) col1 = (iK % 2 == 0) ? green : "3E58A6";
                    if (iEv == 16) col1 = (iK % 2 == 0) ? "D4004D" : "E79805";
                    if (iEv == 17)
                        if (iK <= 12) col1 = (iK % 2 == 0) ? "D4004D" : "E79805";
                        else if (iK <= 13) col1 = "1FBD3E";
                        else col1 = "C41426";
                    if (iEv == 18 || iEv == 19) col1 = (iK % 2 == 0) ? green : "4B84C7";
                    if (iEv == 20) col1 = (iK % 2 == 0) ? green : "C3577F";
                    if (iEv == 21) col1 = (iK % 2 == 0) ? green : "882DB5";
                    if (iEv == 22)
                        if (iK >= 4 && iK <= 11) col1 = (iK % 2 == 0) ? "D4004D" : "E79805";
                        else if (iK <= 12) col1 = "1FBD3E";
                        else col1 = "C41426";


                    col1 = ASSColor.HtmlToASS(col1);

                    {
                        double jumpTime = 0.5;
                        double t0 = kStart - jumpTime;
                        double t1 = t0;
                        double dt = 0.01;
                        Func<double, double> f_y = ti => y;
                        Func<double, double> f_x = ti => x - 100 * (ti - t0) / jumpTime + 100;
                        Func<double, int> f_fs = ti => FontWidth;
                        for (; t1 <= kStart; t1 += dt)
                        {
                            ass_out.Events.Add(
                                ev.StartReplace(t1 - dt - 0.1).EndReplace(t1 - 0.1).TextReplace(
                                ASSEffect.pos(f_x(t1), f_y(t1)) + ASSEffect.fs(f_fs(t1)) +
                                ASSEffect.c(1, col1) + ASSEffect.c(3, "FFFFFF") +
                                ke.KText));
                            ass_out.Events.Add(
                                ev.StartReplace(t1 - dt - 0.1).EndReplace(t1 - dt - 0.1 + 0.4).LayerReplace(5).TextReplace(
                                ASSEffect.pos(f_x(t1), f_y(t1)) + ASSEffect.fs(f_fs(t1)) + ASSEffect.be(1) +
                                ASSEffect.c(1, "FFFFFF") + ASSEffect.c(3, "FFFFFF") +
                                ASSEffect.a(1, "AA") + ASSEffect.a(3, "AA") + ASSEffect.fad(0, 0.3) +
                                ke.KText));
                        }
                        t1 -= dt + 0.1;
                        double t2 = ev.End + r * 1 - 0.9;
                        ass_out.Events.Add(
                            ev.StartReplace(t1).EndReplace(t2).TextReplace(
                                ASSEffect.pos(x, y) +
                                ASSEffect.c(1, col1) + ASSEffect.c(3, "FFFFFF") +
                                ke.KText));
                        {
                            double ag = Common.RandomDouble(rnd, 0, Math.PI * 2);
                            if (iEv >= 9 && iEv <= 11) ag = Math.PI * 1.25;
                            double ra = 100;
                            double x1 = x + ra * Math.Cos(ag);
                            double y1 = y + ra * Math.Sin(ag);
                            bool first = true;
                            for (double t3 = t2; t3 < t2 + 0.4; t3 += 0.02)
                            {
                                double t4 = t3 + 0.5;
                                string cole = Common.scaleColor(col1, "FFFFFF", (t3 - t2) / 0.5);
                                if (iEv <= 3) cole = "FFFFFF";
                                if (iEv >= 9 && iEv <= 11) cole = "FFFFFF";
                                ass_out.Events.Add(
                                    ev.StartReplace(t3).EndReplace(t4).LayerReplace(first ? 10 : 5).TextReplace(
                                    ASSEffect.move(x, y, x1, y1) + ASSEffect.fad(0, 0.3) + ASSEffect.be(first ? 0 : 1) +
                                    ASSEffect.c(1, first ? col1 : cole) + ASSEffect.a(1, first ? "00" : "AA") +
                                    ASSEffect.c(3, first ? "FFFFFF" : cole) + ASSEffect.a(3, first ? "00" : "AA") +
                                        ke.KText));
                                first = false;
                            }
                        }
                    }
                }

            }

            ass_out.SaveFile(OutFileName);
            Console.WriteLine("Lines : {0}", ass_out.Events.Count);
        }
    }
}
