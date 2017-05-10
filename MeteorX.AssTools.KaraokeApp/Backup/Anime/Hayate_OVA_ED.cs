using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class Hayate_OVA_ED : BaseAnime2
    {
        public Hayate_OVA_ED()
        {
            InFileName = @"G:\Workshop\hayate\ed_k.ass";
            OutFileName = @"G:\Workshop\hayate\ed.ass";

            this.FontWidth = 35;
            this.FontHeight = 35;
            this.FontSpace = 3;

            this.PlayResX = 1280;
            this.PlayResY = 720;
            this.MarginBottom = 25;
            this.MarginLeft = 25;
            this.MarginRight = 25;
            this.MarginTop = 25;

            this.Font = new System.Drawing.Font("DFGMaruGothic-Md", 30, GraphicsUnit.Pixel);
            this.IsAvsMask = true;
        }

        double beatStart = 25 * 60 + 36 + 0.50;
        double BPM = 82.6;
        Beat[] beatsAr = null;

        class Beat
        {
            public double Time { get; set; }
            public int Strength { get; set; }
            public int Id { get; set; }
            public bool Reg { get; set; }
        }

        Beat[] GetBeats()
        {
            if (beatsAr != null)
                return beatsAr;
            List<Beat> bs = new List<Beat>();
            int count = 0;
            for (double t = beatStart; t < 27 * 60; t += 60.0 / BPM)
            {
                if (count == 62)
                {
                    bs.Add(new Beat { Time = t, Strength = 5, Id = count++ });
                    bs.Add(new Beat { Time = t + 60.0 / BPM * 0.5, Strength = 5, Id = count++ });
                    bs.Add(new Beat { Time = t + 60.0 / BPM * 0.75, Strength = 5, Id = count++ });
                    continue;
                }
                if (count == 89 || count == 91)
                {
                    bs.Add(new Beat { Time = t, Strength = 2, Id = count++ });
                    bs.Add(new Beat { Time = t + 60.0 / BPM * 0.5, Strength = 2, Id = count++ });
                    continue;
                }
                if (count == 93 || count == 96)
                {
                    bs.Add(new Beat { Time = t, Strength = 5, Id = count++ });
                    bs.Add(new Beat { Time = t + 60.0 / BPM * 0.25, Strength = 5, Id = count++ });
                    bs.Add(new Beat { Time = t + 60.0 / BPM * 0.5, Strength = 5, Id = count++ });
                    continue;
                }
                if (count == 107 || count == 116)
                {
                    bs.Add(new Beat { Time = t, Strength = 10, Id = count++ });
                    bs.Add(new Beat { Time = t + 60.0 / BPM * 0.25, Strength = 10, Id = count++ });
                    bs.Add(new Beat { Time = t + 60.0 / BPM * 0.5, Strength = 10, Id = count++ });
                    continue;
                }
                if (count == 145)
                {
                    bs.Add(new Beat { Time = t, Strength = 10, Id = count++ });
                    bs.Add(new Beat { Time = t + 60.0 / BPM * 0.25, Strength = 10, Id = count++ });
                    bs.Add(new Beat { Time = t + 60.0 / BPM * 0.5, Strength = 10, Id = count++ });
                    bs.Add(new Beat { Time = t + 60.0 / BPM * 0.75, Strength = 10, Id = count++ });
                    continue;
                }
                if (count == 149)
                {
                    bs.Add(new Beat { Time = t, Strength = 0, Id = count++ });
                    bs.Add(new Beat { Time = t + 60.0 / BPM * 0.5, Strength = 0, Id = count++ });
                    continue;
                }
                if (count == 179)
                {
                    bs.Add(new Beat { Time = t, Strength = 10, Id = count++, Reg = false });
                    bs.Add(new Beat { Time = t + 60.0 / BPM * 0.25, Strength = 2, Id = count++, Reg = false });
                    bs.Add(new Beat { Time = t + 60.0 / BPM * 0.5, Strength = 10, Id = count++, Reg = true });
                    bs.Add(new Beat { Time = t + 60.0 / BPM * 0.75, Strength = 2, Id = count++, Reg = false });
                    continue;
                }
                if (count == 183)
                {
                    bs.Add(new Beat { Time = t, Strength = 5, Id = count++, Reg = true });
                    bs.Add(new Beat { Time = t + 60.0 / BPM * 0.25, Strength = 5, Id = count++, Reg = false });
                    bs.Add(new Beat { Time = t + 60.0 / BPM * 0.5, Strength = 5, Id = count++, Reg = false });
                    bs.Add(new Beat { Time = t + 60.0 / BPM * (0.5 + 1.0 / 8.0), Strength = 5, Id = count++, Reg = false });
                    bs.Add(new Beat { Time = t + 60.0 / BPM * (0.5 + 2.0 / 8.0), Strength = 5, Id = count++, Reg = false });
                    continue;
                }
                if (count == 218)
                {
                    bs.Add(new Beat { Time = t, Strength = 2, Id = count++, Reg = false });
                    bs.Add(new Beat { Time = t + 60.0 / BPM * 0.25, Strength = 2, Id = count++, Reg = false });
                    bs.Add(new Beat { Time = t + 60.0 / BPM * 0.5, Strength = 2, Id = count++, Reg = false });
                    bs.Add(new Beat { Time = t + 60.0 / BPM * 0.75, Strength = 2, Id = count++, Reg = false });
                    continue;
                }
                bs.Add(new Beat { Time = t, Strength = (count >= 99 && count < 149) ? 5 : 2, Id = count++, Reg = true });
                bs.Add(new Beat { Time = t + 60.0 / BPM * 0.5, Strength = 5, Id = count++, Reg = false });
            }
            beatsAr = bs.Where(bt => bt.Id < 230).ToArray();
            return beatsAr;
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS();

            ass_out.Header = ass_in.Header;
            ass_out.Events = new List<ASSEvent>();

            string ms1 = "Style: Default,DFGMaruGothic-Md,35,&H00FF0000,&HFF000000,&HFFFFFFFF,&HFFFF0000,-1,0,0,0,100,100,2,0,1,2,0,5,25,25,25,128";
            string ptstr = @"{\p1}m 0 0 l 1 0 1 1 0 1";

            Random rnd = new Random();

            for (int iEv = 0; iEv < ass_in.Events.Count; iEv++)
            {
                //if (iEv != 1) continue;
                ASSEvent ev = ass_in.Events[iEv];
                List<KElement> kelems = ev.SplitK(false);

                this.MaskStyle = ms1;
                int x0 = MarginLeft;
                int startx0 = x0;
                int y0 = PlayResY - MarginBottom - FontHeight;

                if (iEv == -1)
                {
                    bool zz = false;
                    foreach (Beat bt in GetBeats())
                    {
                        ass_out.Events.Add(
                            ev.StartReplace(bt.Time).EndReplace(bt.Time + 0.3).TextReplace(
                            ASSEffect.pos(30, zz ? 30 : 80) + ASSEffect.an(7) + ASSEffect.a(1, "00") + ASSEffect.c(1, "FFFFFF") + ASSEffect.a(3, "FF") +
                            ASSEffect.fad(0.05, 0.2) +
                            string.Format("ID:{0} STR:{1}", bt.Id, bt.Strength)));
                        zz = !zz;
                    }
                }

                if (iEv == 0)
                {
                    int reg0 = 0;
                    int[] blurstrar = new int[] { 2, 3, 4, 5, 6, 7, 8 };
                    blurstrar = blurstrar.Select(bs => bs + 4).ToArray();
                    foreach (Beat bt in GetBeats())
                    {
                        if (bt.Id >= 151)
                        {
                            if (bt.Reg) reg0 = 0; else reg0++;
                            int blurstr = blurstrar[reg0];
                            ass_out.Events.Add(
                                ev.StartReplace(bt.Time).EndReplace(bt.Time + 0.3).LayerReplace(5).TextReplace(
                                ASSEffect.an(7) + ASSEffect.pos(0, 0) +
                                ASSEffect.a(1, "00") + ASSEffect.c(1, "FFFFFF") +
                                ASSEffect.a(3, "44") + ASSEffect.c(3, "ED6D64") +
                                ASSEffect.fad(0.05, 0.2) +
                                ASSEffect.blur(blurstr) + ASSEffect.bord(blurstr) +
                                @"{\p1}m 0 678 l 1280 678 1280 679 0 679"));
                            ass_out.Events.Add(
                                ev.StartReplace(bt.Time).EndReplace(bt.Time + 0.3).LayerReplace(6).TextReplace(
                                ASSEffect.an(7) + ASSEffect.pos(0, 0) +
                                ASSEffect.a(1, "00") + ASSEffect.c(1, "FFFFFF") +
                                ASSEffect.a(3, "44") + ASSEffect.c(3, "FFFFFF") +
                                ASSEffect.fad(0.05, 0.2) +
                                ASSEffect.blur(2) + ASSEffect.bord(2) +
                                @"{\p1}m 0 678 l 1280 678 1280 679 0 679"));
                        }
                    }

                    foreach (Beat bt in GetBeats())
                    {
                        if (bt.Id < 151 || bt.Strength == 0) continue;
                        for (int i = 0; i < 0.3 * 1500; i++)
                        {
                            double parx0 = Common.RandomInt(rnd, 0, PlayResX);
                            double pary0 = Common.RandomInt(rnd, 678 - 30, 678 + 20);
                            double parx1 = parx0 + Common.RandomInt(rnd, -5, 5);
                            double pary1 = pary0 - 30;
                            double part0 = bt.Time;
                            double part1 = bt.Time + 0.3;
                            ass_out.Events.Add(
                                ev.StartReplace(part0).EndReplace(part1).StyleReplace("pt").LayerReplace(3).TextReplace(
                                ASSEffect.move(parx0, pary0, parx1, pary1) +
                                ASSEffect.a(1, "00") + ASSEffect.c(1, "FFFFFF") +
                                ASSEffect.a(3, "55") + ASSEffect.c(3, "ED6D64") +
                                ASSEffect.bord(2) + ASSEffect.blur(2) +
                                ASSEffect.fad(0.05, 0.2) +
                                ASSEffect.frz(Common.RandomInt(rnd, 0, 360)) +
                                CreatePolygon(rnd, 10, 25, 6)));
                        }
                    }
                }

                int kSum = 0;

                //if (!(iEv == 1 || iEv == 3 || iEv == 5 || iEv == 7))
                if (iEv + 1 < ass_in.Events.Count && ass_in.Events[iEv + 1].Start - ev.End > 0.5)
                    ev.End = ass_in.Events[iEv + 1].Start - 0.5;

                for (int iK = 0; iK < kelems.Count; iK++)
                {
                    //if (iK > 3) continue;
                    Console.WriteLine("{0} / {1} : {2} / {3}", iEv + 1, ass_in.Events.Count, iK + 1, kelems.Count);
                    KElement ke = kelems[iK];
                    double r = (double)iK / (double)(kelems.Count - 1);
                    this.MaskStyle = ms1;
                    Size sz = GetSize(ke.KText);
                    if (ke.KText.Trim() == "")
                        sz.Width = 15;
                    double kStart = ev.Start + kSum * 0.01;
                    double kEnd = kStart + ke.KValue * 0.01;
                    kSum += ke.KValue;
                    if (iK + 1 == kelems.Count && kEnd < ev.End)
                        kEnd = ev.End;
                    int x = x0 + this.FontSpace + sz.Width / 2;
                    int y = y0 + FontHeight / 2;
                    StringMask mask = GetMask(ke.KText, x, y);
                    x0 += this.FontSpace + sz.Width;
                    y0 = y0;
                    if (ke.KText.Trim().Length == 0) continue;

                    double r0 = ev.Start - 0.3;
                    double t0 = kStart - 0.4;
                    double t1 = t0 + 0.4;
                    double t2 = ev.End;

                    double r1 = r0 + 0.3;
                    if (!(iEv == 0 || iEv == 2 || iEv == 4 || iEv == 6 || iEv == 8))
                    {
                        for (int i = -1; i <= 1; i++)
                        {
                            ass_out.Events.Add(
                                ev.StartReplace(r0).EndReplace(r1).TextReplace(
                                ASSEffect.move(x + i * 10, y, x, y) + ASSEffect.a(1, "00") + ASSEffect.c(1, "FFFFFF") +
                                ASSEffect.a(3, "00") + ASSEffect.c(3, "FFFFFF") +
                                ASSEffect.xbord(20) + ASSEffect.be(20) + ASSEffect.ybord(0) +
                                ASSEffect.t(0, r1 - r0, ASSEffect.xbord(0).t() + ASSEffect.be(0).t()) +
                                ASSEffect.fad((r1 - r0) * 1, 0) +
                                ke.KText));
                        }
                    }
                    else
                    {
                        ass_out.Events.Add(
                            ev.StartReplace(ev.Start - 0.3).EndReplace(t1).LayerReplace(10).TextReplace(
                            ASSEffect.pos(x, y) + ASSEffect.a(1, "77") + ASSEffect.c(1, "777777") + ASSEffect.a(3, "FF") +
                            ASSEffect.fad(0.5, 0) +
                            ke.KText));
                    }

                    if (iEv == 4 || iEv == 5)
                    {
                        ass_out.Events.Add(
                            ev.StartReplace(kStart - 0.1).EndReplace(kEnd).StyleReplace("pt").LayerReplace(0).TextReplace(
                            ASSEffect.an(7) + ASSEffect.pos(0, 0) +
                            ASSEffect.a(1, "00") + ASSEffect.c(1, "FFFFFF") +
                            ASSEffect.a(3, "77") + ASSEffect.c(3, "ED6D64") +
                            ASSEffect.bord(10) + ASSEffect.blur(10) +
                            ASSEffect.fad(0.2, 0.2) +
                            @"{\p1}" + string.Format("m {0} {1} l {0} {2} {3} {2} {3} {1}", x, PlayResY, PlayResY - MarginBottom - FontHeight - 285, x + 1)));
                        ass_out.Events.Add(
                            ev.StartReplace(kStart - 0.1).EndReplace(kEnd).StyleReplace("pt").LayerReplace(1).TextReplace(
                            ASSEffect.an(7) + ASSEffect.pos(0, 0) +
                            ASSEffect.a(1, "00") + ASSEffect.c(1, "FFFFFF") +
                            ASSEffect.a(3, "77") + ASSEffect.c(3, "FFFFFF") +
                            ASSEffect.bord(2) + ASSEffect.blur(2) +
                            ASSEffect.fad(0.2, 0.2) +
                            @"{\p1}" + string.Format("m {0} {1} l {0} {2} {3} {2} {3} {1}", x, PlayResY, PlayResY - MarginBottom - FontHeight - 285, x + 1)));
                        for (int i = 0; i < (int)((kEnd - kStart) * 200); i++)
                        {
                            double parx0 = Common.RandomInt(rnd, x - 5, x + 5);
                            double pary0 = Common.RandomInt(rnd, PlayResY - MarginBottom - FontHeight - 285, PlayResY);
                            double parx1 = parx0 + Common.RandomInt(rnd, -20, 20);
                            double pary1 = pary0 + Common.RandomInt(rnd, -5, 5);
                            double part0 = Common.RandomDouble(rnd, kStart - 0.1, kEnd - 0.1);
                            double part1 = part0 + Common.GetDistance(parx0, pary0, parx1, pary1) / 50.0;
                            ass_out.Events.Add(
                                ev.StartReplace(part0).EndReplace(part1).StyleReplace("pt").LayerReplace(3).TextReplace(
                                ASSEffect.move(parx0, pary0, parx1, pary1) +
                                ASSEffect.a(1, "00") + ASSEffect.c(1, "FFFFFF") +
                                ASSEffect.a(3, "55") + ASSEffect.c(3, "ED6D64") +
                                ASSEffect.bord(2) + ASSEffect.blur(2) +
                                ASSEffect.fad(0.05, 0.2) +
                                ASSEffect.frz(Common.RandomInt(rnd, 0, 360)) +
                                CreatePolygon(rnd, 10, 25, 6)));
                        }
                    }

                    if (iEv == 0 || iEv == 2 || iEv == 4 || iEv == 6 || iEv == 8)
                    {
                        double dt = 0.002;
                        double dx, dy;
                        {
                            double t = t1;
                            double ag = Math.PI * 2.0 * (t - t0) / (t1 - t0) - Math.PI * 0.5;
                            double ca = 40;
                            double cb = 18;
                            double cx0 = ca * Math.Cos(ag);
                            double cy0 = cb * Math.Sin(-ag);
                            double cag = Math.PI * 0.25;
                            double cx1 = cx0 * Math.Cos(cag) + cy0 * Math.Sin(cag);
                            double cy1 = -cx0 * Math.Sin(cag) + cy0 * Math.Cos(cag);
                            dx = -cx1;
                            dy = -cy1;
                        }
                        for (double t = t0; t <= t1; t += dt)
                        {
                            double ag = Math.PI * 2.0 * (t - t0) / (t1 - t0) - Math.PI * 0.5;
                            double ca = 40;
                            double cb = 18;
                            double cx0 = ca * Math.Cos(ag);
                            double cy0 = cb * Math.Sin(-ag);
                            double cag = Math.PI * 0.25;
                            double cx1 = cx0 * Math.Cos(cag) + cy0 * Math.Sin(cag);
                            double cy1 = -cx0 * Math.Sin(cag) + cy0 * Math.Cos(cag);
                            cx1 += x + dx;
                            cy1 += y + dy;
                            int fs = (int)((double)this.FontHeight * (t - t0) / (t1 - t0) + 1);
                            ass_out.Events.Add(
                                ev.StartReplace(t).EndReplace(t + 0.4).LayerReplace(20).TextReplace(
                                ASSEffect.pos(cx1, cy1) + ASSEffect.fs(fs) +
                                ASSEffect.a(1, "E0") + ASSEffect.c(1, "FFFFFF") + ASSEffect.a(3, "E0") + ASSEffect.c(3, "FFFFFF") +
                                ASSEffect.fad(0, 0.3) +
                                ASSEffect.bord(1) + ASSEffect.blur(1) +
                                ke.KText));
                            ass_out.Events.Add(
                                ev.StartReplace(t).EndReplace(t + dt).TextReplace(
                                ASSEffect.pos(cx1, cy1) + ASSEffect.fs(fs) +
                                ASSEffect.pos(x, y) + ASSEffect.a(1, "00") + ASSEffect.c(1, "FFFFFF") + ASSEffect.a(3, "FF") +
                                ke.KText));
                        }
                        ass_out.Events.Add(
                            ev.StartReplace(t1).EndReplace(t1 + 0.35).LayerReplace(30).TextReplace(
                            ASSEffect.pos(x, y) + ASSEffect.a(1, "00") + ASSEffect.c(1, "FFFFFF") + ASSEffect.a(3, "00") + ASSEffect.c(3, "FFFFFF") +
                            ASSEffect.bord(8) + ASSEffect.blur(8) + ASSEffect.fad(0.05, 0.2) +
                            ke.KText));
                        ass_out.Events.Add(
                            ev.StartReplace(t1).EndReplace(t2 + 0.2).LayerReplace(30).TextReplace(
                            ASSEffect.pos(x, y) + ASSEffect.a(1, "BB") + ASSEffect.c(1, "FFFFFF") + ASSEffect.a(3, "BB") + ASSEffect.c(3, "FFFFFF") +
                            ASSEffect.bord(2) + ASSEffect.blur(2) + ASSEffect.fad(0.2, 0.2) +
                            ke.KText));
                        ass_out.Events.Add(
                            ev.StartReplace(t1).EndReplace(t2).TextReplace(
                            ASSEffect.pos(x, y) + ASSEffect.a(1, "00") + ASSEffect.c(1, "FFFFFF") + ASSEffect.a(3, "FF") +
                            ke.KText));
                    }
                    else
                    {
                        t0 += 0.2;
                        t1 += 0.2;
                        if (t0 < ev.Start)
                        {
                            t0 = ev.Start;
                            t1 = t0 + 0.4;
                        }
                        ass_out.Events.Add(
                            ev.StartReplace(r1 - 0.2).EndReplace(t0 + 0.2).LayerReplace(30).TextReplace(
                            ASSEffect.pos(x, y) + ASSEffect.a(1, "E0") + ASSEffect.c(1, "FFFFFF") + ASSEffect.a(3, "E0") + ASSEffect.c(3, "FFFFFF") +
                            ASSEffect.bord(2) + ASSEffect.blur(2) + ASSEffect.fad(0.2, 0.2) +
                            ke.KText));
                        ass_out.Events.Add(
                            ev.StartReplace(r1).EndReplace(t0).TextReplace(
                            ASSEffect.pos(x, y) + ASSEffect.a(1, "00") + ASSEffect.c(1, "FFFFFF") + ASSEffect.a(3, "FF") +
                            ke.KText));
                        double dt = 0.002;
                        double dx, dy;
                        {
                            double t = t1;
                            double ag = Math.PI * 2.0 * (t - t0) / (t1 - t0) - Math.PI * 0.5;
                            double ca = 40;
                            double cb = 18;
                            double cx0 = ca * Math.Cos(ag);
                            double cy0 = cb * Math.Sin(-ag);
                            double cag = Math.PI * 0.25;
                            double cx1 = cx0 * Math.Cos(cag) + cy0 * Math.Sin(cag);
                            double cy1 = -cx0 * Math.Sin(cag) + cy0 * Math.Cos(cag);
                            dx = -cx1;
                            dy = -cy1;
                        }
                        for (double t = t0; t <= t1; t += dt)
                        {
                            double ag = Math.PI * 2.0 * (t1 - t) / (t1 - t0) - Math.PI * 0.5;
                            double ca = 40;
                            double cb = 18;
                            double cx0 = ca * Math.Cos(ag);
                            double cy0 = cb * Math.Sin(-ag);
                            double cag = Math.PI * 0.25;
                            double cx1 = cx0 * Math.Cos(cag) + cy0 * Math.Sin(cag);
                            double cy1 = -cx0 * Math.Sin(cag) + cy0 * Math.Cos(cag);
                            cx1 += x + dx;
                            cy1 += y + dy;
                            int fs = (int)((double)this.FontHeight * (t1 - t) / (t1 - t0) + 1);
                            ass_out.Events.Add(
                                ev.StartReplace(t).EndReplace(t + 0.4).LayerReplace(20).TextReplace(
                                ASSEffect.pos(cx1, cy1) + ASSEffect.fs(fs) +
                                ASSEffect.a(1, "BB") + ASSEffect.c(1, "FFFFFF") + ASSEffect.a(3, "BB") + ASSEffect.c(3, "FFFFFF") +
                                ASSEffect.fad(0, 0.3) +
                                ASSEffect.bord(1) + ASSEffect.blur(1) +
                                ke.KText));
                            ass_out.Events.Add(
                                ev.StartReplace(t).EndReplace(t + dt).TextReplace(
                                ASSEffect.pos(cx1, cy1) + ASSEffect.fs(fs) +
                                ASSEffect.pos(x, y) + ASSEffect.a(1, "00") + ASSEffect.c(1, "FFFFFF") + ASSEffect.a(3, "FF") +
                                ke.KText));
                        }
                    }

                    double bt_lo = t1;
                    bt_lo = ev.Start - 0.3 + 0.2;
                    double bt_hi = t2;
                    Func<double, bool> bbt = ti => ti >= t1;
                    if (!(iEv == 0 || iEv == 2 || iEv == 4 || iEv == 6 || iEv == 8))
                    {
                        bt_lo = ev.Start - 0.2;
                        bt_hi = t1;
                        bbt = ti => ti < kStart;
                    }
                    if (iEv == 0 || iEv == 2 || iEv == 4 || iEv == 6 || iEv == 8)
                    {
                        double t3 = t2 + 0.3;
                        if (iEv + 1 < ass_in.Events.Count && ass_in.Events[iEv + 1].Start - ev.End < 0.3)
                            t3 = t2 + 0.1;
                        for (int i = -1; i <= 1; i++)
                        {
                            ass_out.Events.Add(
                                ev.StartReplace(t2).EndReplace(t3).TextReplace(
                                ASSEffect.move(x, y, x + i * 10, y) + ASSEffect.a(1, "00") + ASSEffect.c(1, "FFFFFF") +
                                ASSEffect.a(3, "00") + ASSEffect.c(3, "FFFFFF") +
                                ASSEffect.bord(0) + ASSEffect.blur(0) +
                                ASSEffect.t(0, t3 - t2, ASSEffect.xbord(20).t() + ASSEffect.be(20).t()) +
                                ASSEffect.fad(0, t3 - t2) +
                                ke.KText));
                        }
                    }
                    else
                    {
                        double te = ev.End + 0.5;
                        //if (iEv == 9) te -= 1;
                        bt_hi = te - 0.5;
                        ass_out.Events.Add(
                            ev.EndReplace(te).LayerReplace(10).TextReplace(
                            ASSEffect.pos(x, y) + ASSEffect.a(1, "77") + ASSEffect.c(1, "777777") + ASSEffect.a(3, "FF") +
                            ASSEffect.fad(0, 0.5) +
                            ke.KText));
                    }

                    foreach (Beat bt in GetBeats())
                    {
                        if (bt.Strength == 0) continue;
                        if (bt.Time >= bt_lo && bt.Time < bt_hi)
                        {
                            string col = "FFFFFF";
                            int blurstr = 4;
                            string ac = "44";
                            if (bt.Strength == 2)
                            {
                                col = "EEEEEE";
                                blurstr = 3;
                                ac = "77";
                            }
                            if (bt.Strength == 10)
                            {
                                blurstr = 5;
                                ac = "00";
                            }
                            if (!bbt(bt.Time))
                            {
                                blurstr = 2;
                                col = "111111";
                            }
                            ass_out.Events.Add(
                                ev.StartReplace(bt.Time).EndReplace(bt.Time + 0.3).LayerReplace(55).TextReplace(
                                ASSEffect.pos(x, y) + ASSEffect.fad(0.05, 0.2) +
                                ASSEffect.a(1, ac) + ASSEffect.c(1, col) + ASSEffect.a(3, ac) + ASSEffect.c(3, col) +
                                ASSEffect.bord(blurstr) + ASSEffect.blur(blurstr) +
                                ke.KText));
                        }
                    }
                }
            }

            ass_out.SaveFile(OutFileName);
            Console.WriteLine("Lines : {0}", ass_out.Events.Count);
        }
    }
}
