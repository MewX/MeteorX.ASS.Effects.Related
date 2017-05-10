using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class Toradora_OP2 : BaseAnime
    {
        public Toradora_OP2()
        {
            InFileName = @"G:\Workshop\toradora\op2_k.ass";
            OutFileName = @"G:\Workshop\toradora\op2.ass";

            this.FontWidth = 30;
            this.FontHeight = 30;
            this.FontSpace = 1;

            this.PlayResX = 848;
            this.PlayResY = 480;
            this.MarginBottom = 10;
            this.MarginLeft = 15;
            this.MarginRight = 15;
            this.MarginTop = 15;

            this.Font = new System.Drawing.Font("ＤＦＰまるもじ体W3", 30, GraphicsUnit.Pixel);
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS();

            ass_out.Header = ass_in.Header;
            ass_out.Events = new List<ASSEvent>();

            string ptStr = @"{\p4}m 0 100 l 1 1 100 0 1 -1 0 -100 -1 -1 -100 0 -1 1 c m 10 10 s 10 -10 -10 -10 -10 10 c";

            GetMask("！", 0, 0);

            for (int iEv = 0; iEv < ass_in.Events.Count; iEv++)
            {
                ASSEvent ev = ass_in.Events[iEv];
                List<KElement> kelems = ev.SplitK(false);

                /// an7 pos
                int x0 = (PlayResX - GetTotalWidth(ev)) / 2;
                int y0 = PlayResY - MarginBottom - FontHeight;

                Random rnd = new Random();

                int kSum = 0;

                for (int iK = 0; iK < kelems.Count; iK++)
                {
                    KElement ke = kelems[iK];
                    double r = (double)iK / (double)(kelems.Count - 1);
                    Size sz = GetSize(ke.KText);

                    /// an5 pos
                    int x = x0 + this.FontSpace + sz.Width / 2;
                    int y = y0 + FontHeight;

                    StringMask mk = GetMask(ke.KText, x0, y0);
                    if (ke.KText.ToLower() == "silky")
                        mk = GetMask(ke.KText, x0 - 2, y0);

                    int bx0 = x0;
                    int by0 = y0;

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
                    double t5 = ev.End + r * 1.0 - 1.0;
                    if (t5 < t4) t5 = t4;
                    double t6 = t5 + 0.5;

                    ass_out.Events.Add(
                        ev.StartReplace(t0).EndReplace(t1).TextReplace(
                        ASSEffect.move(x, y, x, y - 60) + ASSEffect.a(1, "FF") + ASSEffect.a(3, "FF") +
                        ASSEffect.t(0, t1 - t0, ASSEffect.a(1, "C0").t() + ASSEffect.a(3, "C0").t() + ASSEffect.frx(180).t()) +
                        ke.KText));
                    ass_out.Events.Add(
                        ev.StartReplace(t1).EndReplace(t2).TextReplace(
                        ASSEffect.move(x, y - 60, x, y) + ASSEffect.a(1, "C0") + ASSEffect.a(3, "C0") + ASSEffect.frx(180) +
                        ASSEffect.t(0, t2 - t1, ASSEffect.a(1, "80").t() + ASSEffect.a(3, "80").t() + ASSEffect.frx(360).t()) +
                        ke.KText));
                    ass_out.Events.Add(
                        ev.StartReplace(t2).EndReplace(t3).TextReplace(
                        ASSEffect.move(x, y, x, y - 60) + ASSEffect.a(1, "80") + ASSEffect.a(3, "80") +
                        ASSEffect.t(0, t3 - t2, ASSEffect.a(1, "40").t() + ASSEffect.a(3, "40").t() + ASSEffect.frx(180).t()) +
                        ke.KText));
                    ass_out.Events.Add(
                        ev.StartReplace(t3).EndReplace(t4).TextReplace(
                        ASSEffect.move(x, y - 60, x, y) + ASSEffect.a(1, "40") + ASSEffect.a(3, "40") + ASSEffect.frx(180) +
                        ASSEffect.t(0, t4 - t3, ASSEffect.a(1, "00").t() + ASSEffect.a(3, "00").t() + ASSEffect.frx(360).t()) +
                        ke.KText));
                    ass_out.Events.Add(
                        ev.StartReplace(t4).EndReplace(t5).TextReplace(
                        ASSEffect.pos(x, y) + ASSEffect.a(1, "00") + ASSEffect.a(3, "00") +                        
                        ke.KText));
                    ass_out.Events.Add(
                        ev.StartReplace(t5).EndReplace(t6).TextReplace(
                        ASSEffect.move(x, y, x + r * 100 - 50, y + 30) + ASSEffect.a(1, "00") + ASSEffect.a(3, "00") +
                        ASSEffect.fad(0, t6 - t5) +
                        ke.KText));

                    ASSColor col = Common.RandomColor(rnd, 1);

                    foreach (ASSPoint pt in mk.Points)
                    {
                        if (!Common.RandomBool(rnd, 0.5)) continue;
                        double r1 = Common.RandomDouble(rnd, kStart - 0.15, kEnd + 0.15);
                        double r0 = r1 - 0.5;
                        double r2 = Common.RandomDouble(rnd, kStart + 0.9, kStart + 1.2);
                        double r3 = r2 + 0.5;
                        int x_0 = Common.RandomInt(rnd, pt.X - 80, pt.X - 30);
                        int y_0 = Common.RandomInt(rnd, pt.Y - 30, pt.Y + 30);
                        int x_1 = Common.RandomInt(rnd, pt.X + 80, pt.X + 30);
                        int y_1 = Common.RandomInt(rnd, pt.Y - 30, pt.Y + 30);
                        ass_out.Events.Add(
                            ev.StartReplace(r0).EndReplace(r1).StyleReplace("pt").TextReplace(
                            ASSEffect.move(x_0, y_0, pt.X, pt.Y) + ASSEffect.c(col) + ASSEffect.fad(r1 - r0, 0) + ptStr));
                        ass_out.Events.Add(
                            ev.StartReplace(r1).EndReplace(r2).StyleReplace("pt").TextReplace(
                            ASSEffect.pos(pt.X, pt.Y) + ASSEffect.c(col) + ptStr));
                        ass_out.Events.Add(
                            ev.StartReplace(r2).EndReplace(r3).StyleReplace("pt").TextReplace(
                            ASSEffect.move(pt.X, pt.Y, x_1, y_1) + ASSEffect.c(col) + ASSEffect.fad(0, r3 - r2) + ptStr));
                    }
                }
            }
            ass_out.SaveFile(OutFileName);
        }
    }
}
