using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.Model;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class BASARA_OP : BaseAnime2
    {
        public BASARA_OP()
        {
            this.FontHeight = 38;
            this.FontSpace = 4;
            this.IsAvsMask = true;
            this.MaskStyle = "Style: Default,DFKoIn-W4,38,&H00FFFFFF,&HFFFFFFFF,&HFFFFFFFF,&HFFFFFFFF,0,0,0,0,100,100,0,0,0,0,0,5,0,0,0,128";
            this.MarginLeft = 30;
            this.MarginBottom = 30;
            this.MarginTop = 30;
            this.MarginRight = 30;
            this.PlayResX = 1280;
            this.PlayResY = 720;
            this.InFileName = @"g:\workshop\basara\01\op_k.ass";
            this.OutFileName = @"g:\workshop\basara\01\op.ass";
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS() { Header = ass_in.Header, Events = new List<ASSEvent>() };

            for (int iEv = 0; iEv < ass_in.Events.Count; iEv++)
            {
                ASSEvent ev = ass_in.Events[iEv];
                List<KElement> kelems = ev.SplitK(false);
                int sw = GetTotalWidth(ev);
                int x0 = (PlayResX - sw) / 2;
                int xxx = x0;
                int y0 = PlayResY - MarginBottom - FontHeight;
                int kSum = 0;
                for (int iK = 0; iK < kelems.Count; iK++)
                {
                    Console.WriteLine("{0} / {1} : {2} / {3}", iEv + 1, ass_in.Events.Count, iK + 1, kelems.Count);
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

                    double t0 = ev.Start - 0.5 + iK * 0.05;
                    double t1 = t0 + 0.3;
                    double t2 = kStart - 0.07;
                    double t3 = kEnd;
                    double t4 = ev.End - 0.5 + iK * 0.05;
                    double t5 = t4 + 0.3;

                    Func<double, string> fMainColor = ti => (ti < t2) ? Common.scaleColor("FFFFFF", "000000", (ti - t1) / 0.2) : "FFFFFF";
                    Func<double, string> fMainAlpha = ti => (ti < t2) ? "55" : "00";

                    ass_out.AppendEvent(60, "jp", t0, t1,
                        move(x - 100, y, x, y) + a(1, "55") + fsc(150, 150) + fad(t1 - t0, 0) +
                        frx(Common.RandomInt(rnd, 200, 500)) +
                        fry(Common.RandomInt(rnd, 200, 500)) +
                        frz(Common.RandomInt(rnd, 200, 500)) +
                        t(frx(0).t() + fry(0).t() + frz(0).t() + fsc(100, 100).t()) +
                        ke.KText);
                    ass_out.AppendEvent(55, "jp", t2, t2 + 0.25,
                        pos(x, y) + a(1, "00") + bord(8) + blur(8) + fad(0, 0.18) + a(3, "00") +
                        ke.KText);
                    {
                        double ti = t1;
                        while (ti < t4)
                        {
                            double ti1 = ti + 0.04;
                            ass_out.AppendEvent(50, "jp", ti, ti1 + 0.04,
                                pos(Common.RandomInt(rnd, x - 3, x + 3), Common.RandomInt(rnd, y - 3, y + 3)) + a(1, fMainAlpha(ti)) + fad(0, Common.RandomDouble(rnd, 0.04, 0.09)) +
                                c(1, fMainColor(ti)) +
                                ke.KText);
                            ti = ti1;
                        }
                        ass_out.AppendEvent(60, "jp", ti, t5,
                            move(x, y, x + 100, y) + a(1, "00") + fad(0, t5 - ti) + c(1, "FFFFFF") +
                            t(frx(Common.RandomInt(rnd, 200, 500)).t() + fry(Common.RandomInt(rnd, 200, 500)).t() + frz(Common.RandomInt(rnd, 200, 500)).t() + fsc(150, 150).t()) +
                            ke.KText);
                    }

                    string cShad = "000000";
                    for (int i = 0; i < 2; i++)
                        ass_out.AppendEvent(40, "jp", t1, t4,
                            pos(x, y) + a(1, "00") + blur(2) + c(1, cShad) +
                            ke.KText);

                    {
                        CompositeCurve curve = new CompositeCurve { MinT = t2 - 0.1, MaxT = t2 + 0.1 };
                        double ptag = Common.RandomDouble(rnd, 0, Math.PI);
                        double ptr = 100;
                        double ptx0 = x + Math.Cos(ptag) * ptr;
                        double pty0 = y - Math.Sin(ptag) * ptr;
                        double ptx1 = x - Math.Cos(ptag) * ptr;
                        double pty1 = y + Math.Sin(ptag) * ptr;
                        Line line = new Line { X0 = ptx0, Y0 = pty0, X1 = ptx1, Y1 = pty1 };
                        curve.AddCurve(curve.MinT, curve.MaxT, line);
                        List<ASSPointF> pts = curve.GetPath_Dis(1, 1.2);
                        foreach (ASSPointF pt in pts)
                        {
                            if (!Common.InRange(0, PlayResX, pt.X) || !Common.InRange(0, PlayResY, pt.Y)) continue;
                            ass_out.AppendEvent(30, "pt", pt.T, pt.T + 0.25,
                                pos(pt.X, pt.Y) + a(1, "00") + a(3, "77") + c(1, "FFD9A1") + c(3, "FFD9A1") + fad(0, 0.1) +
                                bord(4) + blur(4) + t(bord(2).t() + blur(2).t()) +
                                p(1) + "m 0 0 l 1 0 1 1 0 1");
                            ass_out.AppendEvent(32, "pt", pt.T, pt.T + 0.25,
                                pos(pt.X, pt.Y) + a(1, "00") + a(3, "44") + c(1, "FFFFFF") + c(3, "FFFFFF") + fad(0, 0.1) +
                                bord(2.5) + blur(2.5) + t(bord(1.3).t() + blur(1.3).t()) +
                                p(1) + "m 0 0 l 1 0 1 1 0 1");
                        }
                        pts = curve.GetPath_DT(0.01);
                        foreach (ASSPointF pt in pts)
                        {
                            if (!Common.InRange(0, PlayResX, pt.X) || !Common.InRange(0, PlayResY, pt.Y)) continue;
                            ass_out.AppendEvent(35, "pt", pt.T, pt.T + 0.01,
                                pos(pt.X, pt.Y) + a(1, "00") + a(3, "00") + c(1, "FFFFFF") + c(3, "FFFFFF") +
                                bord(13) + blur(13) +
                                p(1) + "m 0 0 l 1 0 1 1 0 1");
                        }
                    }
                }
            }

            Console.WriteLine(ass_out.Events.Count);
            ass_out.SaveFile(this.OutFileName);
        }
    }
}
