using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.Model;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class DTB2_OP : BaseAnime3
    {
        public DTB2_OP()
        {
            InFileName = @"G:\Workshop\dtb2\op1\op_k.ass";
            OutFileName = @"G:\Workshop\dtb2\op1\op.ass";

            this.FontWidth = 44;
            this.FontHeight = 44;
            this.FontSpace = 7;

            this.PlayResX = 1280;
            this.PlayResY = 720;
            this.MarginBottom = 20;
            this.MarginLeft = 35;
            this.MarginRight = 35;
            this.MarginTop = 35;

            this.IsAvsMask = true;
        }

        string CreateLight1(Random rnd)
        {
            return CreateLight1(rnd, 120);
        }

        string CreateLight1(Random rnd, double r)
        {
            Func<double, double, string> f1 = (x, y) => string.Format(" {0} {1}", (int)x, (int)y);
            StringBuilder sb = new StringBuilder();
            sb.Append(@"{\p4}");
            r *= 8;
            for (double ag = -0.15; ag <= 0.15; ag += 0.01)
            {
                double l = (Math.Pow(Math.Abs(ag) / 0.15, 0.3) + 0.5) * r;
                sb.Append(" m 0 0 l" + f1(l * Math.Cos(ag), l * Math.Sin(ag)) + " 0 1 c");
            }
            return sb.ToString();
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS();

            ass_out.Header = ass_in.Header;
            ass_out.Events = new List<ASSEvent>();

            Random rnd = new Random();

            for (int iEv = 0; iEv < ass_in.Events.Count; iEv++)
            {
                //if (iEv != 15) continue;
                ASSEvent ev = ass_in.Events[iEv];
                List<KElement> kelems = ev.SplitK(false);

                this.MaskStyle = "Style: Default,TT-曲水B,44,&H00FFFFFF,&HFFFFFFFF,&HFFFFFFFF,&HFFFFFFFF,0,0,0,0,100,100,0,0,0,0,0,5,0,0,0,128";
                this.FontName = "TT-曲水B";
                this.FontCharset = 128;
                this.FontHeight = 44;
                int totalWidth = GetTotalWidth(ev);
                int x0 = (PlayResX - MarginLeft - MarginRight - totalWidth) / 2 + MarginLeft;
                int x0_start = x0;
                int y0 = PlayResY - MarginBottom - FontHeight;
                int kSum = 0;

                CompositeCurve curve = new CompositeCurve { MinT = ev.Start - 0.5, MaxT = ev.End };
                double curve_x0 = x0_start - FontHeight;
                double curve_y0 = y0 + 0.5 * FontHeight;
                double curve_t0 = curve.MinT;

                string[] mainColors =
                {
                    "21184E", // red
                    "4E1842", // purple
                    "4E181A", // blue
                    "184E1E", // green
                    "184E4D", // yellow
                    "393F15"  // another blue...
                };
                string mainColor = mainColors[2];
                if (iEv >= 4 && iEv <= 7) mainColor = mainColors[0];
                if (iEv >= 8 && iEv <= 10) mainColor = mainColors[1];
                if (iEv >= 11) mainColor = mainColors[5];

                mainColor = "000000";

                string[] ptColors =
                {
                    "3600FF", // red
                    "FF00B0", // purple
                    "FF1C00", // blue
                    "00FFFF", // yellow
                    "00FF08", // green
                    "DCFF00"  // another blue...
                };
                string ptColor = ptColors[2];
                if (iEv > 10) ptColor = ptColors[5];
                string ringColor = ptColors[1];
                if (iEv >= 11) ringColor = ptColors[5];
                if (iEv >= 13) ringColor = "FFB100";
                string torchColor = ptColors[1];
                if (iEv >= 11) torchColor = "FF7C00";

                if (iEv == 3) ptColor = Common.scaleColor(ptColor, "FFFFFF", 0.7);
                else ptColor = Common.scaleColor(ptColor, "FFFFFF", 0.5);
                ringColor = Common.scaleColor(ringColor, "FFFFFF", 0.5);
                torchColor = Common.scaleColor(torchColor, "FFFFFF", 0.5);

                for (int iK = 0; iK < kelems.Count; iK++)
                {
                    Console.WriteLine("{0} / {1} : {2} / {3}", iEv + 1, ass_in.Events.Count, iK + 1, kelems.Count);
                    KElement ke = kelems[iK];
                    Size sz = GetSize(ke.KText);
                    double kStart = ev.Start + kSum * 0.01;
                    double kEnd = kStart + ke.KValue * 0.01;
                    kSum += ke.KValue;
                    double x = x0 + this.FontSpace + sz.Width / 2;
                    double y = y0 + FontHeight / 2;
                    int x_an7 = x0;
                    int y_an7 = y0;
                    StringMask mask = GetMask(ke.KText, (int)x, (int)y);
                    x0 += this.FontSpace + sz.Width;
                    if (ke.KText.Trim().Length == 0) continue;

                    curve.AddCurve(curve_t0, kStart, new Line { X0 = curve_x0, Y0 = curve_y0, X1 = x, Y1 = y });
                    curve_x0 = x;
                    curve_y0 = y;
                    curve_t0 = kStart;

                    string evStyle = "op_jp";

                    ass_out.AppendEvent(49, evStyle, ev.Start - 0.3, ev.End + 0.3,
                        pos(x + 1, y + 1) + fad(0.3, 0.3) +
                        a(1, "77") + c(1, "000000") + blur(1) +
                        ke.KText);
                    ass_out.AppendEvent(50, evStyle, ev.Start - 0.3, ev.End + 0.3,
                        pos(x, y) + fad(0.3, 0.3) +
                        a(1, "00") +
                        ke.KText);
                    ass_out.AppendEvent(51, evStyle, ev.Start - 0.3, ev.End + 0.3,
                        pos(x + 2, y + 2) + fad(0.3, 0.3) +
                        a(1, "22") + c(1, mainColor) + blur(1) +
                        ke.KText);

                    double remainStr = 0;
                    if (iEv == 3) remainStr = 0.5;
                    if (iEv == 18) remainStr = 0.5;

                    // light
                    double lastStr = 0.1;

                    // ring particle
                    if (iEv >= 8)
                    {
                        ass_out.AppendEvent(70, "pt", kStart, kStart + 0.4,
                            pos(x, y) + fad(0.04, 0.25) +
                            a(1, "77") + blur(1) +
                            CreateCircle(50.5, 52.5));
                        ass_out.AppendEvent(70, "pt", kStart, kStart + 0.4,
                            pos(x, y) + fad(0.04, 0.25) +
                            a(1, "77") + blur(2) +
                            CreateCircle(49, 54));

                        int ptCount = 150;
                        if (iEv >= 10) ptCount = 250;
                        int ptRange = 12;
                        if (iEv >= 10) ptRange = 20;
                        for (int iPt = 0; iPt < ptCount; iPt++)
                        {
                            double ptag = Common.RandomDouble(rnd, 0, Math.PI * 2);
                            double ptx0 = x + 51.5 * Math.Cos(ptag);
                            double pty0 = y + 51.5 * Math.Sin(ptag);
                            double ptx1 = Common.RandomDouble(rnd, ptx0 - ptRange, ptx0 + ptRange);
                            double pty1 = Common.RandomDouble(rnd, pty0 - ptRange, pty0 + ptRange);
                            double ptt = 0.7;

                            double ptSize = 1;

                            ass_out.AppendEvent(81, "pt", kStart, kStart + ptt,
                                move(ptx0, pty0, ptx1, pty1) + fad(0.04, 0.35) +
                                a(1, "00") + blur(1) + fsc((int)(ptSize * 200)) +
                                p(1) + "m 0 0 l 1 0 1 1 0 1");
                            ass_out.AppendEvent(80, "pt", kStart, kStart + ptt,
                                move(ptx0, pty0, ptx1, pty1) + fad(0.04, 0.35) +
                                a(1, "33") + blur(2) + fsc((int)(ptSize * 300)) + c(1, ringColor) +
                                p(1) + "m 0 0 l 1 0 1 1 0 1");
                        }
                    }

                    // torch light
                    if (iEv >= 10)
                    {
                        ass_out.AppendEvent(70, "pt", kStart, kStart + 0.4,
                            pos(x, y) + fad(0.04, 0.45) +
                            a(1, "CC") + blur(1) + fsc(80) + t(fsc(160).t()) +
                            CreateCircle(50.5, 52.5));
                        ass_out.AppendEvent(70, "pt", kStart, kStart + 0.4,
                            pos(x, y) + fad(0.04, 0.45) +
                            a(1, "CC") + blur(2) + fsc(80) + t(fsc(160).t()) +
                            CreateCircle(49, 54));
                        ass_out.AppendEvent(70, "pt", kStart, kStart + 0.4,
                            pos(x, y) + fad(0.04, 0.45) +
                            a(1, "AA") + blur(1) + fsc(80) + t(fsc(20).t()) +
                            CreateCircle(50.5, 52.5));
                        ass_out.AppendEvent(70, "pt", kStart, kStart + 0.4,
                            pos(x, y) + fad(0.04, 0.45) +
                            a(1, "AA") + blur(2) + fsc(80) + t(fsc(20).t()) +
                            CreateCircle(49, 54));

                        if ((iEv == 10 && iK + 1 == kelems.Count) || iEv >= 15)
                        {
                            double tStep = 0.02;
                            for (double ptt0 = kStart - 0.1; ptt0 < kEnd; ptt0 += tStep)
                            {
                                int startag = Common.RandomInt(rnd, 0, 359);
                                int endag = startag + Common.RandomSig(rnd) * Common.RandomInt(rnd, 50, 100);
                                ass_out.AppendEvent(20, "pt", ptt0, ptt0 + 1,
                                    pos(x, y) + fad(0.3, 0.3) +
                                    a(1, "00") + be(1) + c(1, torchColor) +
                                    frz(startag) + t(frz(endag).t()) +
                                    CreateLight1(rnd));
                                ass_out.AppendEvent(21, "pt", ptt0, ptt0 + 1,
                                    pos(x, y) + fad(0.3, 0.3) +
                                    a(1, "11") + be(1) +
                                    frz(startag) + t(frz(endag).t()) +
                                    CreateLight1(rnd, 60));
                            }
                        }
                    }

                    // starglow
                    foreach (ASSPoint point in mask.Points)
                    {
                        if (!Common.RandomBool(rnd, 0.4)) continue;
                        double xr = (double)(point.X - mask.X0) / mask.Width;
                        double yr = (double)(point.Y - mask.Y0) / mask.Height;
                        Func<double, double> f1 = _x => Math.Pow(Math.Abs(_x) * 2.0, 1.5);

                        ass_out.AppendEvent(70, "pt", kStart, kEnd + lastStr,
                            an(7) + pos(point.X, point.Y) + fad(0.05, kEnd + 0.2 - kStart - 0.05 - 0.08) +
                            a(1, Common.scaleAlpha("FF", "AA", f1(xr))) + be(1) +
                            fscx(100 + f1(xr) * 60) +
                            p(1) + "m 0 0 l 40 0 0 1 -40 0 0 0");
                        ass_out.AppendEvent(70, "pt", kStart, kEnd + lastStr,
                            an(7) + pos(point.X, point.Y) + fad(0.05, kEnd + 0.2 - kStart - 0.05 - 0.08) +
                            a(1, Common.scaleAlpha("FF", "AA", f1(yr))) + be(1) +
                            fscx(100 + f1(yr) * 60) + frz(90) +
                            p(1) + "m 0 0 l 40 0 0 1 -40 0 0 0");

                        if (remainStr > 0)
                        {
                            // remain "starglow"
                            ass_out.AppendEvent(70, "pt", (kEnd + lastStr - kStart) * 0.5 + kStart, ev.End + 0.3,
                                an(7) + pos(point.X, point.Y) + fad((kEnd + lastStr - kStart) * 0.5, 0.3) +
                                a(1, Common.scaleAlpha("FF", "AA", remainStr * f1(xr))) + be(1) +
                                fscx(100 + f1(xr) * 60) +
                                p(1) + "m 0 0 l 40 0 0 1 -40 0 0 0");
                            ass_out.AppendEvent(70, "pt", (kEnd + lastStr - kStart) * 0.5 + kStart, ev.End + 0.3,
                                an(7) + pos(point.X, point.Y) + fad((kEnd + lastStr - kStart) * 0.5, 0.3) +
                                a(1, Common.scaleAlpha("FF", "AA", remainStr * f1(yr))) + be(1) +
                                fscx(100 + f1(yr) * 60) + frz(90) +
                                p(1) + "m 0 0 l 40 0 0 1 -40 0 0 0");

                            // disappear with particles
                            if (Common.RandomBool(rnd, 1))
                            {
                                double ptt0 = ev.End + iK * 0.02;
                                double ptt1 = ptt0 + Common.RandomDouble_Gauss(rnd, 1.2, 2.4, 2);

                                string sFrz = frz(Common.RandomInt(rnd, 0, 359));
                                string sMove = move(point.X, point.Y, point.X + Common.RandomInt(rnd, 100, 200), point.Y);
                                string sMoveT = t(0, ptt1 - ptt0, 0.5, fscx((Math.Abs(Common.RandomDouble_Gauss(rnd, -1, 1, 3)) * 180 + 20) * 100).t());
                                string tS1 = t(0, Common.RandomDouble(rnd, 1, 2) * (ptt1 - ptt0), a(1, "FFFF").t());
                                point.Y += Common.RandomInt(rnd, -5, 5);
                                ass_out.AppendEvent(80, "pt", ptt0, ptt1,
                                    org(point.X, point.Y) + pos(point.X, point.Y) + sMoveT + sFrz +
                                    p(1) + "m 0 0 l 1 0 1 1 0 1" + r() + sFrz + fad(0, 0.4) +
                                    t(0, 0.3, fscx(700).t()) + t(0.3, 1, 2, fscx(150).t()) +
                                    blur(1) + a(1, "00") + tS1 +
                                    p(1) + "m 0 0 l 1 0 1 1 0 1");
                                ass_out.AppendEvent(20, "pt", ptt0, ptt1,
                                    org(point.X, point.Y) + pos(point.X, point.Y) + sMoveT + sFrz +
                                    p(1) + "m 0 0 l 1 0 1 1 0 1" + r() + sFrz + fad(0, 0.4) +
                                    t(0, 0.3, fscx(1000).t()) + t(0.3, 1, 2, fscx(250).t()) +
                                    fscy(150) + c(1, ptColor) +
                                    blur(2) + a(1, "00") + tS1 +
                                    p(1) + "m 0 0 l 1 0 1 1 0 1");
                            }
                        }
                    }

                    ass_out.AppendEvent(60, evStyle, kStart, kEnd + lastStr,
                        pos(x + 1, y + 1) + fad(0.05, kEnd + lastStr - kStart - 0.05 - 0.08) +
                        a(1, "00") +
                        a(3, "22") + bord(2.5) + blur(2.5) +
                        ke.KText);
                    ass_out.AppendEvent(60, evStyle, kStart, kEnd + lastStr,
                        pos(x + 1, y + 1) + fad(0.05, kEnd + lastStr - kStart - 0.05 - 0.08) +
                        a(1, "00") +
                        a(3, "22") + bord(5) + blur(5) +
                        ke.KText);

                    if (remainStr > 0)
                    {
                        ass_out.AppendEvent(60, evStyle, (kEnd + lastStr - kStart) * 0.5 + kStart + iK * 0.02, ev.End + 0.3 + iK * 0.02,
                            pos(x + 1, y + 1) + fad((kEnd + lastStr - kStart) * 0.5, 0.3) +
                            a(1, Common.scaleAlpha("FF", "00", remainStr)) +
                            a(3, Common.scaleAlpha("FF", "22", remainStr)) + bord(2.5) + blur(2.5) +
                            ke.KText);
                        ass_out.AppendEvent(60, evStyle, kStart + iK * 0.02, kEnd + lastStr + iK * 0.02,
                            pos(x + 1, y + 1) + fad(0.05, kEnd + lastStr - kStart - 0.05 - 0.08) +
                            a(1, Common.scaleAlpha("FF", "00", remainStr)) +
                            a(3, Common.scaleAlpha("FF", "22", remainStr)) + bord(5) + blur(5) +
                            ke.KText);

                        ass_out.AppendEvent(60, evStyle, ev.End - 0.1 + iK * 0.02, ev.End + 0.3 + iK * 0.02,
                            pos(x + 1, y + 1) + fad(0.05, 0.2) +
                            a(1, "00") +
                            a(3, "22") + bord(2.5) + blur(2.5) +
                            ke.KText);
                        ass_out.AppendEvent(60, evStyle, ev.End - 0.1 + iK * 0.02, ev.End + 0.3 + iK * 0.02,
                            pos(x + 1, y + 1) + fad(0.05, 0.2) +
                            a(1, "00") +
                            a(3, "22") + bord(5) + blur(5) +
                            ke.KText);
                    }
                }

                if (iEv > 10)
                {
                    curve.AddCurve(curve_t0, curve.MaxT, new Line { X0 = curve_x0, Y0 = curve_y0, X1 = curve_x0, Y1 = curve_y0 });
                    foreach (ASSPointF point in curve.GetPath_DT(0.002))
                    {
                        double ptt0 = point.T;
                        double ptt1 = ptt0 + Common.RandomDouble_Gauss(rnd, 1.2, 1.8, 2);

                        string sFrz = frz(Common.RandomInt(rnd, 0, 359));
                        string sMove = move(point.X, point.Y, point.X + Common.RandomInt(rnd, 100, 200), point.Y);
                        string sMoveT = t(0, ptt1 - ptt0, 0.5, fscx((Math.Abs(Common.RandomDouble_Gauss(rnd, -1, 1, 3)) * 180 + 20) * 100).t());
                        string tS1 = t(0, Common.RandomDouble(rnd, 1, 2) * (ptt1 - ptt0), a(1, "FFFF").t());

                        ass_out.AppendEvent(80, "pt", ptt0, ptt1,
                            org(point.X, point.Y) + pos(point.X, point.Y) + sMoveT + sFrz +
                            p(1) + "m 0 0 l 1 0 1 1 0 1" + r() + sFrz + fad(0, 0.4) +
                            t(0, 0.3, fscx(700).t()) + t(0.3, 1, 2, fscx(150).t()) +
                            blur(1) + a(1, "00") + tS1 +
                            p(1) + "m 0 0 l 1 0 1 1 0 1");
                        ass_out.AppendEvent(20, "pt", ptt0, ptt1,
                            org(point.X, point.Y) + pos(point.X, point.Y) + sMoveT + sFrz +
                            p(1) + "m 0 0 l 1 0 1 1 0 1" + r() + sFrz + fad(0, 0.4) +
                            t(0, 0.3, fscx(1000).t()) + t(0.3, 1, 2, fscx(250).t()) +
                            fscy(150) + c(1, ptColor) +
                            blur(2) + a(1, "00") + tS1 +
                            p(1) + "m 0 0 l 1 0 1 1 0 1");
                    }
                }
            }

            ass_out.SaveFile(OutFileName);
            Console.WriteLine("Lines : {0}", ass_out.Events.Count);
        }
    }
}
