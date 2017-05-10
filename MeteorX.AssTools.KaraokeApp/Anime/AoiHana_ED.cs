using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.Model;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class AoiHana_ED : BaseAnime2
    {
        public AoiHana_ED()
        {
            this.FontHeight = 40;
            this.FontSpace = 2;
            this.IsAvsMask = true;
            this.MarginLeft = 35;
            this.MarginBottom = 35;
            this.MarginTop = 35;
            this.MarginRight = 35;
            this.PlayResX = 1280;
            this.PlayResY = 720;
            this.InFileName = @"G:\Workshop\aoi hana\ed\ed_k.ass";
            this.OutFileName = @"G:\Workshop\aoi hana\ed\ed.ass";
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS() { Header = ass_in.Header, Events = new List<ASSEvent>() };

            double[] light_time_offset = { 3.5, 3.8, 5.9, 4.9, 3.5 };
            double light_spd = 400;

            for (int iEv = 0; iEv < ass_in.Events.Count; iEv++)
            {
                bool isJp = iEv <= 4;
                this.MaskStyle = isJp ?
                    "Style: Default,DFGMaruGothic-Md,38,&H00FFFFFF,&HFFFFFFFF,&HFFFFFFFF,&HFFFFFFFF,0,0,0,0,100,100,0,0,1,0,0,5,0,0,0,128" :
                    "Style: Default,方正准圆_GBK,40,&H00FFFFFF,&HFFFFFFFF,&HFFFFFFFF,&HFFFFFFFF,0,0,0,0,100,100,0,0,1,0,0,5,0,0,0,1";
                this.FontHeight = isJp ? 38 : 40;
                int jEv = isJp ? iEv : iEv - 5;
                //if (!isJp) continue;
                //if (jEv > 0) continue;
                ASSEvent ev = ass_in.Events[iEv];
                List<KElement> kelems = ev.SplitK(true);
                int totalWidth = GetTotalWidth(ev);
                int x0 = (PlayResX - MarginRight - totalWidth - MarginLeft) / 2 + MarginLeft;
                int y0 = (isJp) ? (PlayResY - MarginBottom - FontHeight) : MarginTop;
                int kSum = 0;
                int x0_start = x0;
                int lastx0 = 0;
                string outlines = "";
                for (int iK = 0; iK < kelems.Count; iK++)
                {
                    double sr = (double)iK / (double)(kelems.Count - 1);
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
                    x0 += this.FontSpace + sz.Width;
                    lastx0 = x0;
                    if (ke.KText.Trim().Length == 0) continue;
                    StringMask mask = GetMask(ke.KText, x, y);
                    string evStyle = isJp ? "ed_jp" : "ed_cn";

                    if (ke.KText == "？") x += 15;

                    string outlineFontname = isJp ? "DFGMaruGothic-Md" : "方正准圆_GBK";
                    int outlineEncoding = isJp ? 128 : 1;
                    int xoffset = isJp ? 0 : -1;
                    if (isJp && ke.KText[0] == '中') xoffset = -2;
                    string outlineString = GetOutline(x - sz.Width / 2 + xoffset, y - FontHeight / 2, ke.KText[0], outlineFontname, outlineEncoding, FontHeight, 0, 262);
                    outlines += outlineString;

                    double t0 = ev.Start - 0.5 + iK * 0.1;
                    double t1 = t0 + 0.5;
                    double t2 = kStart;
                    double t3 = t2 + 0.8;
                    double t4 = ev.End - 0.5 + iK * 0.1;
                    double t5 = t4 + 0.5;

                    string main_col = "9699E3";
                    string main_col2 = main_col;
                    if (jEv == 0 || jEv == 1)
                    {
                        main_col = Common.scaleColor("93CB4B", "E4B281", (sr - 0.2) / 0.8);
                        main_col2 = Common.scaleColor("E4B281", "93CB4B", (sr - 0.2) / 0.8);
                    }
                    if (jEv == 3 || jEv == 4)
                    {
                        main_col = Common.scaleColor("9699E3", "9CCFD5", "B0CE6E", (sr - 0.2) / 0.8);
                        main_col2 = Common.scaleColor("B0CE6E", "9CCFD5", "9699E3", (sr - 0.2) / 0.8);
                    }

                    if (!isJp)
                    {
                        ass_out.AppendEvent(50, evStyle, t0, t5,
                            pos(x, y) + a(1, "00") + c(1, main_col) + shad(1) + a(4, "33") + c(4, "FFFFFF") + a(3, "66") +
                            fad(0.5, 0.5) + blur(2) + bord(2) +
                            ke.KText);
                        continue;
                    }

                    double midt = (x0 - sz.Width - (x0_start - 20)) / light_spd + light_time_offset[iEv] + ev.Start - 0.6;
                    ass_out.AppendEvent(50, evStyle, t0, midt + 0.5,
                        pos(x, y) + a(1, "00") + c(1, main_col) + shad(1) + a(4, "33") + c(4, "FFFFFF") + a(3, "66") +
                        fad(0.5, 0.5) + blur(2) + bord(2) +
                        ke.KText);
                    ass_out.AppendEvent(50, evStyle, midt, t5,
                        pos(x, y) + a(1, "00") + c(1, main_col2) + shad(1) + a(4, "33") + c(4, "FFFFFF") + a(3, "66") +
                        fad(0.5, 0.5) + blur(2) + bord(2) +
                        ke.KText);

                    ass_out.AppendEvent(55, evStyle, t2, t3,
                        pos(x, y) + a(1, "55") + c(1, "FFFFFF") + a(3, "55") + c(3, "FFFFFF") + blur(3) + bord(3) + fad(0, t3 - t2) +
                        ke.KText);

                    if (iEv <= 2)
                    {
                        int tmpyy = Common.RandomInt(rnd, 0, 1);
                        for (int i = 0; i < Common.RandomInt(rnd, 1, 2); i++)
                        {
                            double ptt0 = t2 + Common.RandomDouble(rnd, 0, 0.1);
                            double ptx0 = Common.RandomDouble(rnd, x - sz.Width / 2, x + sz.Width / 2);
                            double yd = Common.RandomDouble(rnd, 35, 50);
                            double pty0 = 0;
                            double pty1 = 0;
                            if ((tmpyy + i) % 2 == 0)
                            {
                                pty0 = y - yd;
                                pty1 = y + yd;
                            }
                            else
                            {
                                pty0 = y + yd;
                                pty1 = y - yd;
                            }
                            double spd = 40;
                            string ptcol = (ptt0 < midt + 0.5) ? main_col : main_col2;
                            double ptt1 = ptt0 + Math.Abs(pty0 - pty1) / spd;
                            ass_out.AppendEvent(30, "pt", ptt0, ptt1,
                                an(5) + move(ptx0, pty0, ptx0, pty1) + a(1, "00") + c(1, ptcol) +
                                blur(5) + fs(30) + t(fsc(0, 0).t()) +
                                '●');
                            ass_out.AppendEvent(31, "pt", ptt0, ptt1,
                                an(5) + move(ptx0, pty0, ptx0, pty1) + a(1, "00") + c(1, Common.scaleColor("FFFFFF", ptcol, 0.7)) +
                                blur(2.2) + fs(25) + t(fsc(0, 0).t()) +
                                '●');
                            ass_out.AppendEvent(32, "pt", ptt0, ptt1,
                                an(5) + move(ptx0, pty0, ptx0, pty1) + a(1, "00") +
                                blur(1.8) + fs(12) + t(fsc(0, 0).t()) +
                                '●');
                        }
                    }

                    if (iEv >= 3)
                    {
                        {
                            double ptt0 = t2 - 0.4;
                            double ptt1 = t2;
                            string ptcol = (ptt0 < midt + 0.5) ? main_col : main_col2;
                            double theta1 = Common.RandomDouble(rnd, 0, Math.PI);
                            List<BaseCurve> curves = new List<BaseCurve>();
                            double ra = 30;
                            double rb = 7;
                            Circle2 cl = new Circle2 { NormTheta = false, X0 = x, Y0 = y, A = ra, B = rb, MinT = -Math.PI * 0.5, MaxT = Math.PI * 0.5, Theta = theta1, dTheta = 0 };
                            CompositeCurve cc = new CompositeCurve { MinT = ptt0, MaxT = ptt1 };
                            cc.AddCurve(cc.MinT, cc.MaxT, cl);
                            curves.Add(cc);
                            cl = new Circle2 { NormTheta = false, X0 = x, Y0 = y, A = ra, B = rb, MinT = -Math.PI * 0.5, MaxT = -Math.PI * 1.5, Theta = theta1, dTheta = 0 };
                            cc = new CompositeCurve { MinT = ptt0, MaxT = ptt1 };
                            cc.AddCurve(cc.MinT, cc.MaxT, cl);
                            curves.Add(cc);
                            foreach (BaseCurve curve in curves)
                            {
                                foreach (ASSPointF pt in curve.GetPath_Dis(1, 1.1))
                                {
                                    ass_out.AppendEvent((pt.Theta >= 0 || pt.Theta <= -Math.PI) ? 100 : 0, "pt", pt.T, pt.T + 0.3,
                                        an(5) + pos(pt.X, pt.Y) + a(1, "00") + c(1, "FFFFFF") + a(3, "77") + c(3, ptcol) +
                                        bord(3) + blur(3) + fs(8) + t(fsc(0, 0).t() + bord(0).t()) +
                                        '●');
                                }
                                foreach (ASSPointF pt in curve.GetPath_DT(0.03))
                                {
                                    ass_out.AppendEvent((pt.Theta >= 0 || pt.Theta <= -Math.PI) ? 101 : 1, "pt", pt.T, pt.T + 0.03,
                                        an(5) + pos(pt.X, pt.Y) + a(1, "00") + c(1, "FFFFFF") +
                                        blur(5) + fs(12) +
                                        '●');
                                }
                            }
                            ass_out.AppendEvent(100, evStyle, ptt1, ptt1 + 0.5,
                                pos(x, y) + a(1, "00") + a(3, "00") + bord(3) + blur(3) + fad(0, 0.3) +
                                ke.KText);
                        }

                        for (int i = 0; i < 40; i++)
                        {
                            double ptt0 = t2 + Common.RandomDouble(rnd, 0, 0.1);
                            double ptx0 = x;
                            double pty0 = y;
                            double sc = Common.RandomDouble(rnd, 2, 3);
                            double ptx1 = Common.RandomDouble(rnd, x - sz.Width * sc, x + sz.Width * sc);
                            double pty1 = Common.RandomDouble(rnd, y - sz.Width * sc, y + sz.Width * sc);
                            double spd = 40;
                            string ptcol = (ptt0 < midt + 0.5) ? main_col : main_col2;
                            double ptt1 = ptt0 + Common.GetDistance(ptx0, pty0, ptx1, pty1) / spd;
                            ass_out.AppendEvent(30, "pt", ptt0, ptt1,
                                an(5) + move(ptx0, pty0, ptx1, pty1) + a(1, "00") + c(1, ptcol) +
                                blur(8) + fs(30) + t(fsc(0, 0).t()) +
                                '●');
                            ass_out.AppendEvent(31, "pt", ptt0, ptt1,
                                an(5) + move(ptx0, pty0, ptx1, pty1) + a(1, "00") + c(1, Common.scaleColor("FFFFFF", ptcol, 0.7)) +
                                blur(2.2) + fs(25) + t(fsc(0, 0).t()) +
                                '●');
                            ass_out.AppendEvent(32, "pt", ptt0, ptt1,
                                an(5) + move(ptx0, pty0, ptx1, pty1) + a(1, "00") +
                                blur(1.8) + fs(12) + t(fsc(0, 0).t()) +
                                '●');
                        }
                    }
                }

                if (isJp)
                {
                    double ptt0 = ev.Start + light_time_offset[iEv] - 0.7;
                    double ptx0 = x0_start - 20;
                    double ptx1 = lastx0 + 20;
                    double ptt1 = ptt0 + (ptx1 - ptx0) / light_spd;
                    ass_out.AppendEvent(100, "pt", ptt0, ptt1,
                        clip(4, outlines) + move(ptx0, y0 + FontHeight / 2, ptx1, y0 + FontHeight / 2) +
                        a(1, "55") + frz(-45) + blur(8) + fscx(200) +
                        p(1) + "m 10 -50 l 10 50 -10 50 -10 -50");
                }
            }

            Console.WriteLine(ass_out.Events.Count);
            ass_out.SaveFile(this.OutFileName);
        }
    }
}
