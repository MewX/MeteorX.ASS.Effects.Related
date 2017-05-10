using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class MariaHolic_ED : BaseAnime
    {
        public MariaHolic_ED()
        {
            InFileName = @"G:\Workshop\maria holic\ed_k.ass";
            OutFileName = @"G:\Workshop\maria holic\ed.ass";

            this.FontWidth = 35;
            this.FontHeight = 35;
            this.FontSpace = 2;

            this.PlayResX = 848;
            this.PlayResY = 480;
            this.MarginBottom = 15;
            this.MarginLeft = 15;
            this.MarginRight = 15;
            this.MarginTop = 15;

            this.Font = new System.Drawing.Font("DFPMaruMoji-W9", 35, GraphicsUnit.Pixel);
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS();

            ass_out.Header = ass_in.Header;
            ass_out.Events = new List<ASSEvent>();

            string ptString = @"{\p8}m 0 0 l 128 0 128 128 0 128";
            string[] colString = { "4BFCCC", "CE8046", "52065D", "8F0036", "33AE52", "6ADB57" };
            colString = colString.Select(s => ASSColor.HtmlToASS(s)).ToArray();

            for (int iEv = 0; iEv < ass_in.Events.Count; iEv++)
            {
                Console.WriteLine("{0} / {1}", iEv + 1, ass_in.Events.Count);

                ASSEvent ev = ass_in.Events[iEv];
                List<KElement> kelems = ev.SplitK(true);

                /// an7 pos
                int x0 = (PlayResX - GetTotalWidth(ev)) / 2;
                int y0 = PlayResY - MarginBottom - FontHeight;

                Random rnd = new Random();

                int kSum = 0;

                string col0 = colString[iEv % colString.Length];
                string col1 = (iEv >= 7 && iEv <= 10) ? "CCCCCC" : "CCCCCC";
                string col20 = "FFFFFF";
                string col21 = "CCCCCC";
                string col22 = "888888";
                string col23 = "444444";
                string col24 = "111111";
                if (iEv >= 7 && iEv <= 10)
                {
                    col20 = "111111";
                    col21 = "444444";
                    col23 = "CCCCCC";
                    col24 = "FFFFFF";
                }

                for (int iK = 0; iK < kelems.Count; iK++)
                {
                    KElement ke = kelems[iK];
                    double r = (double)iK / (double)(kelems.Count - 1);
                    Size sz = GetSize(ke.KText);

                    /// an5 pos
                    int x = x0 + this.FontSpace + sz.Width / 2;
                    int y = y0 + FontHeight;

                    int mask_x0 = x0 + 2;
                    int mask_y0 = y0 + 0;
                    StringMask mask = GetMask(ke.KText, mask_x0, mask_y0);
                    if (ke.KText == "ト") mask = GetMask(ke.KText, mask_x0 + 3, mask_y0);
                    if (ke.KText == "ル") mask = GetMask(ke.KText, mask_x0 + 1, mask_y0);
                    if (ke.KText == "ば") mask = GetMask(ke.KText, mask_x0 + 1, mask_y0);
                    if (ke.KText == "し") mask = GetMask(ke.KText, mask_x0 + 1, mask_y0);
                    if (ke.KText == "ダ") mask = GetMask(ke.KText, mask_x0 + 1, mask_y0);
                    if (ke.KText == "ユ") mask = GetMask(ke.KText, mask_x0 + 1, mask_y0);
                    if (ke.KText == "ン") mask = GetMask(ke.KText, mask_x0, mask_y0);
                    if (ke.KText == "ざ") mask = GetMask(ke.KText, mask_x0, mask_y0);
                    if (ke.KText == "に") mask = GetMask(ke.KText, mask_x0 - 1, mask_y0);
                    if (ke.KText == "浮") mask = GetMask(ke.KText, mask_x0 - 1, mask_y0);
                    if (ke.KText == "な") mask = GetMask(ke.KText, mask_x0 - 1, mask_y0);
                    if (ke.KText == "く") mask = GetMask(ke.KText, mask_x0 - 1, mask_y0);
                    if (ke.KText == "の") mask = GetMask(ke.KText, mask_x0 - 1, mask_y0);
                    if (ke.KText == "肩") mask = GetMask(ke.KText, mask_x0 - 1, mask_y0);
                    if (ke.KText == "を") mask = GetMask(ke.KText, mask_x0 - 1, mask_y0);
                    if (ke.KText == "て") mask = GetMask(ke.KText, mask_x0 - 1, mask_y0);
                    if (ke.KText == "テ") mask = GetMask(ke.KText, mask_x0 - 1, mask_y0);
                    if (ke.KText == "熱") mask = GetMask(ke.KText, mask_x0 - 1, mask_y0);
                    if (ke.KText == "る") mask = GetMask(ke.KText, mask_x0 - 1, mask_y0);
                    if (ke.KText == "こ") mask = GetMask(ke.KText, mask_x0 - 1, mask_y0);
                    if (ke.KText == "読") mask = GetMask(ke.KText, mask_x0 - 1, mask_y0);
                    if (ke.KText == "ま") mask = GetMask(ke.KText, mask_x0 - 1, mask_y0);
                    if (ke.KText == "さ") mask = GetMask(ke.KText, mask_x0 - 1, mask_y0);
                    if (ke.KText == "波") mask = GetMask(ke.KText, mask_x0 - 1, mask_y0);
                    if (ke.KText == "時") mask = GetMask(ke.KText, mask_x0 - 1, mask_y0);
                    if (ke.KText == "間") mask = GetMask(ke.KText, mask_x0 - 1, mask_y0);
                    if (ke.KText == "柄") mask = GetMask(ke.KText, mask_x0 - 1, mask_y0);
                    if (ke.KText == "焼") mask = GetMask(ke.KText, mask_x0 - 1, mask_y0);

                    x0 += this.FontSpace + sz.Width;
                    y0 = y0;

                    double kStart = ev.Start + kSum * 0.01;
                    kSum += ke.KValue;
                    double kEnd = ev.Start + kSum * 0.01;

                    double t0 = ev.Start + r * 1.0 - 1.0;
                    double t1 = t0 + 0.2;
                    double t2 = t0 + 0.4;
                    double t3 = t0 + 0.6;
                    double t4 = t0 + 0.8;
                    double t5 = kStart - 0.1;
                    double t6 = kStart + 0.05;
                    double t7 = kStart + 0.15;
                    double t8 = kStart + 0.3;
                    double t9 = ev.End + r * 1.0 - 1.0;
                    double tA = t9 + 0.4;

                    ass_out.Events.Add(
                        ev.StyleReplace("bd").StartReplace(t5).EndReplace(t7).TextReplace(
                        ASSEffect.pos(x, y) + ASSEffect.fad(t6 - t5, 0) + ASSEffect.bord(15) +
                        ASSEffect.a(1, "FF") + ASSEffect.c(3, col0) + ASSEffect.blur(10) +
                        ke.KText));
                    ass_out.Events.Add(
                        ev.StyleReplace("bd").StartReplace(t7).EndReplace(t8 + 0.15).TextReplace(
                        ASSEffect.pos(x, y) + ASSEffect.bord(15) + ASSEffect.fad(0, t8 + 0.15 - t7) +
                        ASSEffect.a(1, "FF") + ASSEffect.c(3, col0) + ASSEffect.blur(10) +
                        ke.KText));
                    ass_out.Events.Add(
                        ev.StyleReplace("bd").StartReplace(t7).EndReplace(t8 + 0.15).TextReplace(
                        ASSEffect.pos(x, y) + ASSEffect.fad(t8 + 0.15 - t7, 0) +
                        ASSEffect.a(1, "FF") + ASSEffect.c(3, col0) + ASSEffect.blur(10) + ASSEffect.a(3, "77") +
                        ke.KText));
                    ass_out.Events.Add(
                        ev.StyleReplace("bd").StartReplace(t8 + 0.15).EndReplace(t9).TextReplace(
                        ASSEffect.pos(x, y) +
                        ASSEffect.a(1, "FF") + ASSEffect.c(3, col0) + ASSEffect.blur(10) + ASSEffect.a(3, "77") +
                        ke.KText));
                    ass_out.Events.Add(
                        ev.StyleReplace("bd").StartReplace(t9).EndReplace(tA).TextReplace(
                        ASSEffect.pos(x, y) + ASSEffect.fad(0, tA - t9) +
                        ASSEffect.a(1, "FF") + ASSEffect.c(3, col0) + ASSEffect.blur(10) + ASSEffect.a(3, "77") +
                        ke.KText));

                    ass_out.Events.Add(
                        ev.StartReplace(kEnd - 0.1).EndReplace(t9).TextReplace(
                        ASSEffect.pos(x, y) + ASSEffect.fad(t9 - kEnd + 0.1, 0) +
                        ASSEffect.a(3, "FF") + ASSEffect.c(1, col0) +
                        ke.KText));
                    ass_out.Events.Add(
                        ev.StartReplace(t9).EndReplace(tA).TextReplace(
                        ASSEffect.pos(x, y) + ASSEffect.fad(0, tA - t9) +
                        ASSEffect.a(3, "FF") + ASSEffect.c(1, col0) +
                        ke.KText));

                    ass_out.Events.Add(
                        ev.StartReplace(t0).EndReplace(t1).TextReplace(
                        ASSEffect.move(x, y, x, y - 75) + ASSEffect.a(1, "FF") + ASSEffect.a(3, "FF") + ASSEffect.blur(1) + ASSEffect.c(3, col20) +
                        ASSEffect.t(0, t1 - t0, ASSEffect.a(3, "C0").t() + ASSEffect.frx(180).t() + ASSEffect.c(3, col21).t()) +
                        ke.KText));
                    ass_out.Events.Add(
                        ev.StartReplace(t1).EndReplace(t2).TextReplace(
                        ASSEffect.move(x, y - 75, x, y) + ASSEffect.a(1, "FF") + ASSEffect.a(3, "C0") + ASSEffect.frx(180) + ASSEffect.blur(1) + ASSEffect.c(3, col21) +
                        ASSEffect.t(0, t2 - t1, ASSEffect.a(3, "80").t() + ASSEffect.frx(360).t() + ASSEffect.c(3, col22).t()) +
                        ke.KText));
                    ass_out.Events.Add(
                        ev.StartReplace(t2).EndReplace(t3).TextReplace(
                        ASSEffect.move(x, y, x, y - 75) + ASSEffect.a(1, "FF") + ASSEffect.a(3, "80") + ASSEffect.blur(1) + ASSEffect.c(3, col22) +
                        ASSEffect.t(0, t3 - t2, ASSEffect.a(3, "40").t() + ASSEffect.frx(180).t() + ASSEffect.c(3, col23).t()) +
                        ke.KText));
                    ass_out.Events.Add(
                        ev.StartReplace(t3).EndReplace(t4).TextReplace(
                        ASSEffect.move(x, y - 75, x, y) + ASSEffect.a(1, "FF") + ASSEffect.a(3, "40") + ASSEffect.frx(180) + ASSEffect.blur(1) + ASSEffect.c(3, col23) +
                        ASSEffect.t(0, t4 - t3, ASSEffect.a(3, "00").t() + ASSEffect.frx(360).t() + ASSEffect.c(3, col24).t() + ASSEffect.blur(1).t()) +
                        ke.KText));
                    ass_out.Events.Add(
                        ev.StartReplace(t4).EndReplace(t5).TextReplace(
                        ASSEffect.pos(x, y) + ASSEffect.c(3, col24) +
                        ASSEffect.a(1, "FF") + ASSEffect.blur(1) +
                        ke.KText));
                    ass_out.Events.Add(
                        ev.StartReplace(t5).EndReplace(t8 + 0.1).TextReplace(
                        ASSEffect.pos(x, y) +
                        ASSEffect.a(1, "FF") + ASSEffect.t(0, t8 + 0.1 - t5, ASSEffect.c(3, col20).t() + ASSEffect.fry(360 * 3).t()) + ASSEffect.blur(1) +
                        ke.KText));
                    ass_out.Events.Add(
                        ev.StartReplace(t8 + 0.1).EndReplace(t9).TextReplace(
                        ASSEffect.pos(x, y) +
                        ASSEffect.a(1, "FF") + ASSEffect.c(3, col20) + ASSEffect.blur(1) +
                        ke.KText));
                    ass_out.Events.Add(
                        ev.StartReplace(t9).EndReplace(tA).TextReplace(
                        ASSEffect.pos(x, y) +
                        ASSEffect.a(1, "FF") + ASSEffect.c(3, col20) + ASSEffect.blur(1) +
                        ASSEffect.fad(0, tA - t9) + ASSEffect.t(0, tA - t9, ASSEffect.fry(700).t()) +
                        ke.KText));

                    foreach (ASSPoint pt in mask.Points)
                    {
                        double ptx0 = Common.RandomInt_Gauss(rnd, pt.X, 10);
                        double pty0 = Common.RandomInt_Gauss(rnd, pt.Y - 60, 10);
                        double ptx1 = Common.RandomInt_Gauss(rnd, pt.X - 60, 20);
                        double pty1 = Common.RandomInt_Gauss(rnd, pt.Y, 10);
                        double pt_kt = ((double)(pt.X - mask.X0) / (double)mask.Width) * (kEnd - kStart) + kStart;
                        double ptt0 = Common.RandomDouble(rnd, pt_kt - 0.4, pt_kt);
                        double ptt1 = Common.RandomDouble(rnd, ptt0 + 0.25, ptt0 + 0.45);
                        double ptt2 = ev.End - 1.0 * (1.0 - r) + 0.3 * (double)(pt.Y - y0) / (double)FontHeight + Common.RandomDouble(rnd, 0, 0.2);
                        double ptt3 = Common.RandomDouble(rnd, ptt2 + 0.25, ptt2 + 0.45);
                        string ptcol = Common.scaleColor(col0, col1, (double)(pt.Y - y0) / (double)FontHeight);

                        ass_out.Events.Add(
                            ev.StartReplace(ptt0).EndReplace(ptt1).StyleReplace("pt").TextReplace(
                            ASSEffect.move(ptx0, pty0, pt.X, pt.Y) +
                            ASSEffect.a(1, Common.ToHex2(255 - pt.Brightness)) + ASSEffect.c(1, ptcol) +
                            ASSEffect.fad(0.4 * (ptt1 - ptt0), 0) +
                            ptString
                            ));
                        ass_out.Events.Add(
                            ev.StartReplace(ptt1).EndReplace(ptt2).StyleReplace("pt").TextReplace(
                            ASSEffect.pos(pt.X, pt.Y) +
                            ASSEffect.a(1, Common.ToHex2(255 - pt.Brightness)) + ASSEffect.c(1, ptcol) +
                            ptString
                            ));
                        ass_out.Events.Add(
                            ev.StartReplace(ptt2).EndReplace(ptt3).StyleReplace("pt").TextReplace(
                            ASSEffect.move(pt.X, pt.Y, ptx1, pty1) +
                            ASSEffect.a(1, Common.ToHex2(255 - pt.Brightness)) + ASSEffect.c(1, ptcol) +
                            ASSEffect.fad(0, ptt3 - ptt2) +
                            ptString
                            ));
                    }
                }
            }
            ass_out.SaveFile(OutFileName);
        }
    }
}
