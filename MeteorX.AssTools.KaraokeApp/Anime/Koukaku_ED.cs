using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class Koukaku_ED : BaseAnime2
    {
        public Koukaku_ED()
        {
            InFileName = @"G:\Workshop\Koukaku no Regios\ed_k.ass";
            OutFileName = @"G:\Workshop\Koukaku no Regios\ed.ass";

            this.FontWidth = 35;
            this.FontHeight = 35;
            this.FontSpace = 2;

            this.PlayResX = 848;
            this.PlayResY = 480;
            this.MarginBottom = 15;
            this.MarginLeft = 15;
            this.MarginRight = 15;
            this.MarginTop = 15;

            this.Font = new System.Drawing.Font("DFPOP-SB", 30, GraphicsUnit.Pixel);

            //Style: edj,DFPOP-SB,30,&H00F4F4F4,&HFF600D00,&H00600D00,&H530A5A84,-1,0,0,0,100,100,1,0,1,2,0,1,20,20,15,128
            this.MaskStyle = "Style: Default,DFPOP-SB,30,&H00FF0000,&HFF600D00,&H000000FF,&H530A5A84,-1,0,0,0,100,100,1,0,1,2,0,5,0,0,0,128";

            //this.IsAvsMask = false;
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS();

            ass_out.Header = ass_in.Header;
            ass_out.Events = new List<ASSEvent>();

            string ptString = @"{\p8}m 0 0 l 128 0 128 128 0 128";
            int[,] map = new int[this.PlayResX, this.PlayResY];

            for (int iEv = 0; iEv < ass_in.Events.Count; iEv++)
            {
                //if (iEv != 0) continue;
                ASSEvent ev = ass_in.Events[iEv];
                List<KElement> kelems = ev.SplitK(false);

                /// an7 pos
                int x0 = MarginLeft;
                if (iEv == 5) x0 = PlayResX - GetTotalWidth(ev) - MarginRight;
                int y0 = PlayResY - MarginBottom - FontHeight;

                Random rnd = new Random();

                int kSum = 0;

                for (int iK = 0; iK < kelems.Count; iK++)
                {
                    Console.WriteLine("{0} / {1} : {2} / {3}", iEv + 1, ass_in.Events.Count, iK + 1, kelems.Count);

                    KElement ke = kelems[iK];
                    double r = (double)iK / (double)(kelems.Count - 1);
                    StringMask mask = GetMask(ke.KText, x0, y0);
                    Size sz = new Size(mask.Width, mask.Height);

                    /// an5 pos
                    int x = x0 + this.FontSpace + sz.Width / 2;
                    int y = y0 + FontHeight / 2;
                    mask = GetMask(ke.KText, x, y);

                    x0 += this.FontSpace + sz.Width;
                    y0 = y0;

                    double kStart = ev.Start + kSum * 0.01;
                    kSum += ke.KValue;
                    double kEnd = ev.Start + kSum * 0.01;

                    double t0 = ev.Start - 1.0 + 0.1 * iK;
                    double t1 = t0 + 0.4;
                    double t2 = ev.End - 1.0 + 0.1 * iK;
                    double t3 = t2 + 0.4;

                    for (int i = 1; i <= 5; i++)
                    {
                        ass_out.Events.Add(
                            ev.StartReplace(t0 + i * 0.03).EndReplace(t1 + i * 0.03).TextReplace(
                            ASSEffect.move(x + 30, y - 30, x, y) + ASSEffect.a(1, "FF") + ASSEffect.blur(2) +
                            ASSEffect.fad((t1 - t0) * 2, 0) + ASSEffect.fry(-180) +
                            ASSEffect.t(0, t1 - t0, ASSEffect.fry(0).t()) +
                            ke.KText));
                    }
                    ass_out.Events.Add(
                        ev.StartReplace(t0).EndReplace(t1).TextReplace(
                        ASSEffect.move(x + 30, y - 30, x, y) + ASSEffect.a(1, "FF") + ASSEffect.blur(2) +
                        ASSEffect.fad(t1 - t0, 0) + ASSEffect.fry(-180) +
                        ASSEffect.t(0, t1 - t0, ASSEffect.fry(0).t()) +
                        ke.KText));
                    ass_out.Events.Add(
                        ev.StartReplace(kStart).EndReplace(t2).TextReplace(
                        ASSEffect.pos(x + 1, y + 1) + ASSEffect.a(1, "FF") + ASSEffect.a(3, "77") + ASSEffect.c(3, "000000") +
                        ASSEffect.bord(8) + ASSEffect.blur(8) + ASSEffect.fad(0.2, 0.2) +
                        ke.KText));
                    ass_out.Events.Add(
                        ev.StartReplace(t1).EndReplace(t2).TextReplace(
                        ASSEffect.pos(x, y) + ASSEffect.a(1, "FF") + ASSEffect.blur(2) +
                        ke.KText));
                    ass_out.Events.Add(
                        ev.StartReplace(t0).EndReplace(t1).TextReplace(
                        ASSEffect.move(x + 31, y - 29, x + 1, y + 1) + ASSEffect.a(1, "FF") + ASSEffect.a(3, "77") + ASSEffect.c(3, "000000") +
                        ASSEffect.bord(8) + ASSEffect.blur(8) + ASSEffect.fad(t1 - t0, 0) + ASSEffect.fry(-180) +
                        ASSEffect.t(0, t1 - t0, ASSEffect.fry(0).t()) +
                        ke.KText));
                    ass_out.Events.Add(
                        ev.StartReplace(t1).EndReplace(kStart + 0.2).TextReplace(
                        ASSEffect.pos(x + 1, y + 1) + ASSEffect.a(1, "FF") + ASSEffect.a(3, "77") + ASSEffect.c(3, "000000") +
                        ASSEffect.bord(8) + ASSEffect.blur(8) + ASSEffect.fad(0, 0.2) +
                        ke.KText));
                    for (int i = 1; i <= 5; i++)
                    {
                        ass_out.Events.Add(
                            ev.StartReplace(t2 + i * 0.03).EndReplace(t3 + i * 0.03).TextReplace(
                            ASSEffect.move(x, y, x - 30, y + 30) + ASSEffect.a(1, "00") + ASSEffect.blur(2) +
                            ASSEffect.fad(0, 2 * (t3 - t2)) + ASSEffect.t(0, t3 - t2, ASSEffect.fry(180).t()) +
                            ke.KText));
                    }
                    ass_out.Events.Add(
                        ev.StartReplace(t2).EndReplace(t3).TextReplace(
                        ASSEffect.move(x, y, x - 30, y + 30) + ASSEffect.a(1, "00") + ASSEffect.blur(2) +
                        ASSEffect.fad(0, t3 - t2) + ASSEffect.t(0, t3 - t2, ASSEffect.fry(180).t()) +
                        ke.KText));

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
                                if (Common.RandomBool(rnd, 0.5)) continue;
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

                    for (int i = 0; i < mask.Points.Count; i++)
                    {
                        ASSPoint pt = mask.Points[i];
                        double indsc = 50.0;
                        if (iEv >= 4) indsc = 75.0;
                        double tp0 = kStart - 0.1 + (double)ind[i] / indsc + Common.RandomDouble_Gauss(rnd, -0.2, 0.2);
                        double tp1 = tp0 + 0.3;
                        double tp2 = tp1 + 0.3;
                        string pc0 = "5955FF";
                        string pc1 = "55FDFF";
                        string pc2 = "9C4E4D";
                        ass_out.Events.Add(
                            ev.StartReplace(tp0).EndReplace(tp1).StyleReplace("pt").LayerReplace(5).TextReplace(
                            ASSEffect.pos(pt.X, pt.Y) + ASSEffect.fad(0.3, 0) +
                            ASSEffect.a(1, Common.ToHex2(255 - pt.Brightness)) + ASSEffect.c(1, pc0) +
                            ptString
                            ));
                        ass_out.Events.Add(
                            ev.StartReplace(tp1).EndReplace(tp2).StyleReplace("pt").LayerReplace(5).TextReplace(
                            ASSEffect.pos(pt.X, pt.Y) +
                            ASSEffect.a(1, Common.ToHex2(255 - pt.Brightness)) + ASSEffect.c(1, pc0) +
                            ASSEffect.t(0, tp2 - tp1, ASSEffect.c(1, pc1).t()) +
                            ptString
                            ));
                        ass_out.Events.Add(
                            ev.StartReplace(tp2).EndReplace(t2).StyleReplace("pt").LayerReplace(5).TextReplace(
                            ASSEffect.pos(pt.X, pt.Y) +
                            ASSEffect.a(1, Common.ToHex2(255 - pt.Brightness)) + ASSEffect.c(1, pc1) +
                            ASSEffect.t(0, 0.3, ASSEffect.c(1, pc2).t()) +
                            ptString
                            ));
                    }
                }
            }
            ass_out.SaveFile(OutFileName);
        }
    }
}
