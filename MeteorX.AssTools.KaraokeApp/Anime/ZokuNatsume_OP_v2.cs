using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.Effect;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class ZokuNatsume_OP_v2 : BaseAnime
    {
        public ZokuNatsume_OP_v2()
        {
            InFileName = @"G:\Workshop\natsume2\op_k_v2.ass";
            OutFileName = @"G:\Workshop\natsume2\op_v2.ass";

            this.FontWidth = 34;
            this.FontHeight = 34;
            this.FontSpace = 1;

            this.PlayResX = 848;
            this.PlayResY = 480;
            this.MarginBottom = 15;
            this.MarginLeft = 15;
            this.MarginRight = 10;
            this.MarginTop = 10;

            this.Font = new System.Drawing.Font("DFPKaiSho-Md", 24);
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS();

            ass_out.Header = ass_in.Header;
            ass_out.Events = new List<ASSEvent>();

            Random rnd = new Random();

            int mask_xOffset = -4;

            double pp = 0.6;

            for (int iEv = 0; iEv < ass_in.Events.Count; iEv++)
            {
                ASSEvent ev = ass_in.Events[iEv];
                List<KElement> kelems = ev.SplitK(false);

                /// an7 pos
                int x0 = MarginLeft;
                int y0 = PlayResY - MarginBottom - FontHeight;

                int kSum = 0;

                for (int i = 0; i < kelems.Count; i++)
                {
                    KElement ke = kelems[i];
                    double r = (double)i / (double)(kelems.Count - 1);
                    double r0 = 1.0 - r;
                    //Size sz = GetSize(ke.KText);
                    StringMask mask = GetMask(ke.KText, x0 + FontSpace, y0);
                    Size sz = new Size(mask.Width, mask.Height);

                    double kStart = kSum * 0.01;
                    double kEnd = kStart + ke.KValue * 0.01;

                    /// an5 pos
                    int x = x0 + FontSpace + sz.Width / 2;
                    int y = y0 + FontHeight / 2;

                    x0 += this.FontSpace + sz.Width;
                    y0 = y0;

                    kSum += ke.KValue;

                    double t0 = ev.Start - r0 * 0.5;
                    double t1 = t0 + 0.2; // 出现
                    double t2 = ev.Start + kStart; // 保持
                    double t3 = t2 + 0.2; // K效果
                    double t4 = ev.End - r0 * 0.5; // 保持
                    double t5 = t4 + 0.2; // 消失

                    ass_out.Events.Add(
                        ev.StartReplace(t0).EndReplace(t1).TextReplace(
                        ASSEffect.pos(x, y) + ASSEffect.be(100) +
                        ASSEffect.fad(t1 - t0, 0) + ASSEffect.t(0, t1 - t0, ASSEffect.be(1).t()) +
                        ke.KText));

                    ass_out.Events.Add(
                        ev.StartReplace(t1).EndReplace(t2).TextReplace(
                        ASSEffect.pos(x, y) + ASSEffect.be(1) +
                        ke.KText));

                    ass_out.Events.Add(
                        ev.StartReplace(t2).EndReplace(t4).TextReplace(
                        ASSEffect.pos(x, y) + ASSEffect.be(1) + ASSEffect.a(1, "FF") + ASSEffect.c(3, "FFFFFF") + 
                        ke.KText));

                    foreach (ASSPoint pt in mask.Points)
                    {
                        if (!Common.RandomBool(rnd, pp)) continue;
                        double rStart = t2;
                        double rEnd = t2 + Common.RandomDouble(rnd, 0.1, 1.5);
                        int dx = rnd.Next() % 20 + 8;
                        int dy = rnd.Next() % 20 + 8;
                        int xx2 = pt.X;
                        int yy2 = pt.Y;
                        if (Common.RandomBool(rnd, (double)(pt.X - mask.X0) / (double)mask.Width)) xx2 += dx; else xx2 -= dx;
                        if (Common.RandomBool(rnd, (double)(pt.Y - mask.Y0) / (double)mask.Height)) yy2 += dy; else yy2 -= dy;
                        ass_out.Events.Add(
                            ev.StartReplace(rStart).EndReplace(rEnd).StyleReplace("pt").TextReplace(
                            ASSEffect.move(pt.X, pt.Y, xx2, yy2) + ASSEffect.fad(0, rEnd - rStart) +
                            "o"));
                    }

                    ass_out.Events.Add(
                        ev.StartReplace(t4).EndReplace(t5).TextReplace(
                        ASSEffect.pos(x, y) + ASSEffect.be(1) + ASSEffect.a(1, "FF") + ASSEffect.c(3, "FFFFFF") + 
                        ASSEffect.fad(0, t5 - t4) + ASSEffect.t(0, t5 - t4, ASSEffect.be(100).t()) +
                        ke.KText));
                }
            }

            ass_out.SaveFile(OutFileName);
        }
    }
}
