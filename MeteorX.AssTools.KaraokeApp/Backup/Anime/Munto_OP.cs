using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.Effect;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class Munto_OP : BaseAnime
    {
        public Munto_OP()
        {
            InFileName = @"G:\Workshop\munto\op_k.ass";
            OutFileName = @"G:\Workshop\munto\op.ass";

            this.FontWidth = 30;
            this.FontHeight = 30;
            this.FontSpace = 2;

            this.PlayResX = 848;
            this.PlayResY = 480;
            this.MarginBottom = 25;
            this.MarginLeft = 25;
            this.MarginRight = 25;
            this.MarginTop = 25;

            this.Font = new System.Drawing.Font("DFGMaruGothic-Md", 30, GraphicsUnit.Pixel);
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS();

            ass_out.Header = ass_in.Header;
            ass_out.Events = new List<ASSEvent>();

            Particle2 Par;

            string[] colList = 
            {
                "05C1FF",
                "05C1FF",
                "0505FF",
                "0505FF",
                "FF6305",
                "FF0528",
                "0505FF",
                "FF6305",
                "FF6305",
                "FF6305",
                "FF6305",
                "FF6305",
            };

            for (int iEv = 0; iEv < ass_in.Events.Count; iEv++)
            {
                //if (iEv > 1) continue;
                ASSEvent ev = ass_in.Events[iEv];
                List<KElement> kelems = ev.SplitK(false);

                double sw = GetTotalWidth(ev);
                /// an7 pos
                int x0 = (iEv % 2 == 0) ? MarginLeft : (PlayResX - MarginRight - (int)sw);
                int y0 = PlayResY - MarginBottom - FontHeight;

                int kSum = 0;

                int bak_x0 = x0;

                string col1 = colList[iEv];
                string col3 = col1;

                for (int i = 0; i < kelems.Count; i++)
                {
                    KElement ke = kelems[i];
                    double r = (double)i / (double)(kelems.Count - 1);
                    Size sz = GetSize(ke.KText);

                    double kStart = kSum * 0.01;
                    double kEnd = kStart + ke.KValue * 0.01;
                    kSum += ke.KValue;

                    /// an5 pos
                    int x = x0 + this.FontSpace + sz.Width / 2;
                    int y = y0 + FontHeight / 2;

                    x0 += this.FontSpace + sz.Width;
                    y0 = y0;

                    if (ke.KText.Trim().Length == 0) continue;

                    double t0 = ev.Start + r * 0.6 - 0.9;
                    if (iEv % 2 == 1) t0 = ev.Start - r * 0.6 - 0.3;
                    double t1 = t0 + 0.3;
                    double t2 = ev.Start + kStart;
                    if (t1 > t2) t2 = t1;
                    double t3 = t2 + 0.3;
                    double t4 = ev.End + r * 0.6 - 0.6;
                    if (iEv % 2 == 1) t4 = ev.End - r * 0.6;
                    double t5 = t4 + 0.3;

                    string colBak = col1;
                    for (int yScan = y0; yScan <= y0 + FontHeight; yScan++)
                    {
                        col1 = col3 = Common.scaleColor(colBak, "FFFFFF", y0, y0 + FontHeight, yScan);
                        string colb = Common.scaleColor("000000", "777777", y0, y0 + FontHeight, yScan);
                        for (int j = -5; j <= 5; j++)
                        {
                            ass_out.Events.Add(
                                ev.StartReplace(t0).EndReplace(t1).TextReplace(
                                ASSEffect.clip(0, yScan, PlayResX, yScan + 1) +
                                ASSEffect.move(x + j * 5, y, x, y) + ASSEffect.a(1, Common.ToHex2(Math.Abs(j) * 40)) + ASSEffect.fad(t1 - t0, 0) + ASSEffect.c(1, colb) +
                                ke.KText));
                        }
                        ass_out.Events.Add(
                            ev.StartReplace(t0).EndReplace(t2).TextReplace(
                                ASSEffect.clip(0, yScan, PlayResX, yScan + 1) +
                            ASSEffect.pos(x, y) + ASSEffect.a(1, "00") + ASSEffect.fad(t1 - t0, 0) + ASSEffect.c(1, colb) +
                            ke.KText));
                        ass_out.Events.Add(
                            ev.StartReplace(t2).EndReplace(t3).TextReplace(
                            ASSEffect.clip(0, yScan, PlayResX, yScan + 1) +
                            //                        ASSEffect.pos(x, y) + ASSEffect.a(1, "00") + ASSEffect.c(1, "111111") + ASSEffect.t(0, t3 - t2, ASSEffect.c(1, col1).t() + ASSEffect.a(1, "FF").t() + ASSEffect.a(3, "00").t() + ASSEffect.blur(2).t()) + ASSEffect.c(3, col3) +
                            ASSEffect.pos(x, y) + ASSEffect.a(1, "00") + ASSEffect.c(1, "111111") + ASSEffect.t(0, t3 - t2, ASSEffect.c(1, col1).t()) +
                            ke.KText));
                        ass_out.Events.Add(
                            ev.StartReplace(t3).EndReplace(t4).TextReplace(
                            ASSEffect.clip(0, yScan, PlayResX, yScan + 1) +
                            //ASSEffect.pos(x, y) + ASSEffect.a(1, "FF") + ASSEffect.blur(2) + ASSEffect.a(3, "00") + ASSEffect.c(3, col3) +
                            ASSEffect.pos(x, y) + ASSEffect.a(1, "00") + ASSEffect.c(1, col1) +
                            ke.KText));
                        for (int j = -5; j <= 5; j++)
                        {
                            ass_out.Events.Add(
                                ev.StartReplace(t4).EndReplace(t5).LayerReplace(5).TextReplace(
                                ASSEffect.clip(0, yScan, PlayResX, yScan + 1) +
                                //ASSEffect.move(x, y, x + j * 5, y) + ASSEffect.a(1, Common.ToHex2(Math.Abs(j) * 40)) + ASSEffect.c(1, col1) + ASSEffect.fad(0, t1 - t0) + ASSEffect.blur(2) + ASSEffect.a(3, "00") + ASSEffect.c(3, col3) +
                                ASSEffect.move(x, y, x + j * 5, y) + ASSEffect.a(1, Common.ToHex2(Math.Abs(j) * 40)) + ASSEffect.c(1, col1) + ASSEffect.fad(0, t1 - t0) +
                                ke.KText));
                        }
                        ass_out.Events.Add(
                            ev.StartReplace(t4).EndReplace(t5).TextReplace(
                            ASSEffect.clip(0, yScan, PlayResX, yScan + 1) +
                            //ASSEffect.pos(x, y) + ASSEffect.a(1, "FF") + ASSEffect.fad(0, t5 - t4) + ASSEffect.blur(2) + ASSEffect.c(3, col3) +
                            ASSEffect.pos(x, y) + ASSEffect.a(1, "00") + ASSEffect.fad(0, t5 - t4) + ASSEffect.c(1, col1) +
                            ke.KText));
                    }
                    col1 = col3 = colBak;
                    for (int j = 1; j <= 5; j++)
                    {
                        ass_out.Events.Add(
                            ev.StartReplace(t2 + j * 0.04).EndReplace(t3 + j * 0.04).TextReplace(
                            ASSEffect.move(x, y, x, y - 20) + ASSEffect.c(1, "FFFFFF") + ASSEffect.a(1, Common.ToHex2(Math.Abs(j) * 40)) + ASSEffect.fsc(100 + j * 8, 100 + j * 8) + ASSEffect.t(0, t3 - t2, ASSEffect.fsc(100 + j * 8 + 20, 100 + j * 8 + 20).t()) +
                            ke.KText));
                    }
                    ass_out.Events.Add(
                        ev.StartReplace(t2 + 0.15).EndReplace(t2 + 1).TextReplace(
                        ASSEffect.move(x, y, x, y + 30) + ASSEffect.a(1, "77") + ASSEffect.c(1, col1) + ASSEffect.fad(0, 0.5) + ASSEffect.fsc(70, 70) +
                        ke.KText));

                    Par = new Particle2("FFDE7D", "FFCC33", t2, t2 + 0.3, 0.001, 1, -15, 15, -15, 15, 1, 2) { Star = false, Pt0Size = 2 };
                    ass_out.Events.AddRange(Par.Create(new MovingRound(t2, t2 + 0.3, x, y, 30, -Math.PI) { MinDX = -2, MaxDX = 2, MinDY = 2, MaxDY = 2 }));
                }
                x0 = bak_x0;
                string arcCol = "FFFFFF";
                if (iEv % 2 == 0)
                {
                    Par = new Particle2(arcCol, "FFCC33", ev.Start - 0.6 - 0.3, ev.Start - 0.3, 0.005, 8, sw / 0.6 - 40, sw / 0.6 + 40, -10, 10, 2, 5) { Star = false, Pt0Size = 2 };
                    ass_out.Events.AddRange(Par.Create(new MovingArc(ev.Start - 0.6 - 0.3, ev.Start - 0.3, x0 - 80, y0 + FontHeight / 2, x0 + sw - 80, y0 + FontHeight / 2, 60, -1.2, 1.2) { GaussRnd = 2 }));
                }
                else
                {
                    Par = new Particle2(arcCol, "FFCC33", ev.Start - 0.6 - 0.3, ev.Start - 0.3, 0.005, 8, -sw / 0.6 - 40, -sw / 0.6 + 40, -10, 10, 2, 5) { Star = false, Pt0Size = 2 };
                    ass_out.Events.AddRange(Par.Create(new MovingArc(ev.Start - 0.6 - 0.3, ev.Start - 0.3, x0 + sw + 80, y0 + FontHeight / 2, x0 + 80, y0 + FontHeight / 2, 60, Math.PI - 1.2, Math.PI + 1.2) { GaussRnd = 2 }));
                }
            }

            ass_out.SaveFile(OutFileName);
        }
    }
}
