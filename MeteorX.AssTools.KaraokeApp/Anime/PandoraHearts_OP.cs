using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.Model;
using MeteorX.AssTools.KaraokeApp.Toys;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class PandoraHearts_OP : BaseAnime2
    {
        public PandoraHearts_OP()
        {
            this.FontHeight = 25;
            this.FontSpace = 1;
            this.IsAvsMask = true;
            this.MarginLeft = 20;
            this.MarginBottom = 20;
            this.MarginTop = 20;
            this.MarginRight = 20;
            this.PlayResX = 640;
            this.PlayResY = 480;
            this.InFileName = @"G:\Workshop\Pandora Hearts\op_k.ass";
            this.OutFileName = @"G:\Workshop\Pandora Hearts\op.ass";
            this.MaskStyle = "Style: Default,DFMincho-UB,25,&H00FFFFFF,&HFFFFFFFF,&HFFFFFFFF,&HFFFFFFFF,0,0,0,0,100,100,0,0,0,0,0,5,0,0,0,128";
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS() { Header = ass_in.Header, Events = new List<ASSEvent>() };

            for (int iEv = 0; iEv < ass_in.Events.Count; iEv++)
            {
                if (iEv != 0) continue;
                ASSEvent ev = ass_in.Events[iEv];
                List<KElement> kelems = ev.SplitK(true);
                int x0 = MarginLeft;
                int totalWidth = GetTotalWidth(ev);
                x0 = (PlayResX - MarginLeft - MarginRight - totalWidth) / 2 + MarginLeft;
                int bakx0 = x0;
                int y0 = PlayResY - MarginBottom - FontHeight;
                int kSum = 0;
                double lastx0 = 0;
                double lastt0 = 0;
                string outlines = "";
                for (int iK = 0; iK < kelems.Count; iK++)
                {
                    Console.WriteLine("{0} / {1} : {2} / {3}", iEv + 1, ass_in.Events.Count, iK + 1, kelems.Count);
                    KElement ke = kelems[iK];
                    Size sz = new Size();
                    if (ke.KText.Trim().Length == 0)
                    {
                        sz = new Size { Width = FontHeight, Height = FontHeight };
                    }
                    else
                    {
                        sz = GetMask(ke.KText, x0, y0).GetSize();
                    }
                    double kStart = ev.Start + kSum * 0.01;
                    double kEnd = kStart + ke.KValue * 0.01;
                    kSum += ke.KValue;
                    int x = x0 + this.FontSpace + sz.Width / 2;
                    int y = y0 + FontHeight / 2;
                    int x_an7 = x0;
                    int y_an7 = y0;
                    x0 += this.FontSpace + sz.Width;
                    lastx0 = x0;
                    if (ke.KText.Trim().Length == 0) continue;

                    string outlineFontname = "DFMincho-UB";
                    int outlineEncoding = 128;
                    string outlineString = GetOutline(x - FontHeight / 2, y - FontHeight / 2, ke.KText[0], outlineFontname, outlineEncoding, FontHeight, 0, 168);
                    outlines += outlineString;

                    double appearOffset = (x - PlayResX * 0.5) * (2.0 / PlayResX);
                    double t0 = ev.Start + appearOffset;
                    double t1 = t0 + 0.6;
                    if (t1 > kStart) t1 = kStart;
                    double t2 = kStart;
                    double t3 = kEnd;
                    double t4 = ev.End + appearOffset - 0.3;
                    double t5 = t4 + 0.6;

                    ass_out.AppendEvent(20, "", t0, t5, // Blured Shadow
                        pos(x + 1, y + 1) + fad(0.6, 0.6) + a(1, "00") + c(1, "000000") + blur(2) +
                        ke.KText);

                    ass_out.AppendEvent(40, "pt", t0, t5, // Black
                        pos(x, y) + clip(4, outlineString) + fad(0.6, 0.6) + a(1, "00") + c(1, "000000") +
                        p(1) + "m -20 -20 l 20 -20 20 20 -20 20");
                    ass_out.AppendEvent(41, "pt", t0, t5, // White
                        pos(x, y + 3) + clip(4, outlineString) + fad(0.6, 0.6) + a(3, "00") +
                        bord(10) + blur(10) +
                        p(1) + "m -20 15 l 20 15 20 16 -20 16");

                    // Highlight
                    ass_out.AppendEvent(50, "", t2, t3,
                        pos(x, y) + fad(0, t3 - t2) + a(3, "00") + bord(2) + blur(2) +
                        a(1, "00") + c(1, "000000") + t(0, 0.04, fsc(130, 130).t()) + t(0.3, t3 - t2, fsc(110, 110).t()) +
                        ke.KText);

                    {
                        string bak = this.MaskStyle;
                        this.MaskStyle = "Style: Default,宋体,40,&H00FFFFFF,&HFFFFFFFF,&HFFFFFFFF,&HFFFFFFFF,0,0,0,0,100,100,0,0,0,0,0,7,0,0,0,0";
                        StringMask mask = GetMask(p(4) + outlineString, 0, 0);
                        this.MaskStyle = bak;
                        StringBuilder sb = new StringBuilder();
                        foreach (ASSPoint pt in mask.Points)
                        {
                            double xag = (pt.X - x) / ((double)FontHeight * 0.5) * 0.5;
                            xag -= 0.5;
                            double lw = Math.Cos(xag) * 100;
                            if (Math.Abs(lw) < 20) lw = 20 * Common.RandomSig(rnd);
                            double yag = (pt.Y - y) / ((double)FontHeight * 0.5) * 0.5;
                            int ptx = (int)(pt.X + lw);
                            int pty = (int)(Math.Sin(yag) * FontHeight + pt.Y);
                            Console.WriteLine(xag);
                            if (pt.X < x)
                                sb.Append(string.Format(" m {0} {1} l {2} {3} {4} {5} c", pt.X, pt.Y, pt.X, pt.Y + 1, ptx, pty));
                            else
                                sb.Append(string.Format(" m {0} {1} l {2} {3} {4} {5} c", pt.X, pt.Y, ptx, pty, pt.X, pt.Y + 1));
                        }
                        ass_out.AppendEvent(100, "pt", t2, t3 + 0.5,
                            pos(0, 0) + a(1, "70") + be(1) + fad(0, t3 - t2) +
                            org(x, y) + t(fry(90).t()) +
                            p(1) + sb);
                    }
                }

            }

            Console.WriteLine(ass_out.Events.Count);
            ass_out.SaveFile(this.OutFileName);
        }
    }
}
