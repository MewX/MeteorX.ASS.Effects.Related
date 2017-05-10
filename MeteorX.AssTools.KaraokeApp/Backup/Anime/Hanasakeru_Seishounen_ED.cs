using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.Model;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class Hanasakeru_Seishounen_ED : BaseAnime2
    {
        public Hanasakeru_Seishounen_ED()
        {
            this.FontHeight = 28;
            this.FontSpace = 2;
            this.IsAvsMask = true;
            this.MarginLeft = 20;
            this.MarginBottom = 20;
            this.MarginTop = 20;
            this.MarginRight = 20;
            this.PlayResX = 848;
            this.PlayResY = 480;
            this.InFileName = @"G:\Workshop\hanasakeru\ed_k.ass";
            this.OutFileName = @"G:\Workshop\hanasakeru\ed.ass";
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS() { Header = ass_in.Header, Events = new List<ASSEvent>() };

            string mainCol = "FF51C5";
            string fCol = "595AFF";

            for (int iEv = 0; iEv < ass_in.Events.Count; iEv++)
            {
                bool isJp = iEv <= 15;
                //if (iEv != 0) continue;
                this.MaskStyle = isJp ?
                    "Style: Default,DFMincho-UB,28,&H00FFFFFF,&HFFFFFFFF,&HFFFFFFFF,&HFFFFFFFF,0,0,0,0,100,100,0,0,0,0,0,5,0,0,0,128" :
                    "Style: Default,汉仪粗宋繁,28,&H00FFFFFF,&HFFFFFFFF,&HFFFFFFFF,&HFFFFFFFF,1,0,0,0,100,100,0,0,0,0,0,5,0,0,0,134";
                ASSEvent ev = ass_in.Events[iEv];
                List<KElement> kelems = ev.SplitK(!isJp);
                if (!isJp)
                    foreach (KElement ke in kelems)
                        ke.KValue = 10;
                int sw = GetTotalWidth(ev);
                int x0 = (!isJp) ? MarginLeft : PlayResX - MarginRight - sw;
                int y0 = (!isJp) ? (PlayResY - MarginBottom - FontHeight) : MarginTop;
                int kSum = 0;
                string outlines = "";
                for (int iK = 0; iK < kelems.Count; iK++)
                {
                    Console.WriteLine("{0} / {1} : {2} / {3}", iEv + 1, ass_in.Events.Count, iK + 1, kelems.Count);
                    string evStyle = isJp ? "Default" : "cn";
                    string outlineFontname = isJp ? "DFMincho-UB" : "汉仪粗宋繁";
                    int outlineEncoding = isJp ? 128 : 134;
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
                    string outlineString = GetOutline(x - FontHeight / 2, y - FontHeight / 2, ke.KText[0], outlineFontname, outlineEncoding, FontHeight, 0, isJp ? 193 : 178);
                    outlines += outlineString;

                    double t0 = ev.Start - 0.5 + iK * 0.08;
                    double t1 = t0 + 0.4;
                    double t2 = kStart - 0.1;
                    double t3 = kEnd;
                    double t4 = ev.End - 0.5 + iK * 0.08;
                    double t5 = t4 + 0.4;

                    ass_out.AppendEvent(30, "pt", t0, t5,
                        pos(2, 2) + fad(0.5, 0.5) + a(1, "00") + c(1, "222222") +
                        blur(2) +
                        p(4) + outlineString);
                    ass_out.AppendEvent(35, "pt", t0, t5,
                        pos(0, 0) + fad(0.5, 0.5) + a(1, "00") + c(1, "FFFFFF") +
                        p(4) + outlineString);
                    double lumX = Common.RandomInt(rnd, x - 12, x + 12);
                    double lumY = Common.RandomInt(rnd, y - 12, y + 12);
                    for (int i = 0; i < 3; i++)
                    {
                        int lumsz = 8 + i * 2;
                        double t24 = t2 + 1;
                        if (t24 > t4) t24 = (t2 + t4) * 0.5;
                        ass_out.AppendEvent(40, "pt", t2, t24,
                            clip(4, outlineString) + pos(lumX, lumY) +
                            a(1, "44") + a(3, "00") + c(1, mainCol) + c(3, mainCol) + t(bord(lumsz).t() + blur(lumsz).t()) +
                            p(1) + "m 0 0 l 1 0 1 1 0 1");
                        ass_out.AppendEvent(40, "pt", t24, t4,
                            clip(4, outlineString) + pos(lumX, lumY) +
                            a(1, "44") + a(3, "00") + c(1, mainCol) + c(3, mainCol) + bord(lumsz) + blur(lumsz) +
                            p(1) + "m 0 0 l 1 0 1 1 0 1");
                        ass_out.AppendEvent(40, "pt", t4, t5,
                            clip(4, outlineString) + pos(lumX, lumY) +
                            a(1, "44") + a(3, "00") + c(1, mainCol) + c(3, mainCol) + bord(lumsz) + blur(lumsz) +
                            t(bord(0).t() + blur(0).t()) +
                            p(1) + "m 0 0 l 1 0 1 1 0 1");
                    }

                    if (!isJp) continue;

                    for (int iP = 0; iP < 10 * (t3 - t2); iP++)
                    {
                        int pid = Common.RandomInt(rnd, 0, mask.Points.Count - 1);
                        ASSPoint orgpt = mask.Points[pid];
                        double ptt0 = Common.RandomDouble(rnd, t2, t3);
                        double ptt1 = ptt0 + 2;
                        double ptx0 = orgpt.X;
                        double pty0 = orgpt.Y;
                        double ag = Common.RandomDouble(rnd, 0, 2 * Math.PI);
                        double ptx1 = ptx0 + Common.RandomDouble(rnd, -160, -100);
                        double pty1 = pty0 + Common.RandomDouble(rnd, 60, 35);

                        string ptstr = CreatePolygon(rnd, 40, 40, 3);
                        int tmpx = Common.RandomInt(rnd, 100, 400);
                        int tmpy = Common.RandomInt(rnd, 100, 400);
                        int tmpz = Common.RandomInt(rnd, 100, 400);

                        for (int i = 0; i < 3; i++)
                        {
                            ass_out.AppendEvent(70 + i, "pt", ptt0, ptt1,
                                move(ptx0, pty0, ptx1, pty1) + a(1, "44") + a(3, "44") +
                                c(1, mainCol) + c(3, mainCol) + t(frx(tmpx).t() + fry(tmpy).t() + frz(tmpz).t()) +
                                blur(3 - i) +
                                ptstr);
                        }
                        ass_out.AppendEvent(90, "pt", ptt0, ptt1,
                            move(ptx0, pty0, ptx1, pty1) + a(1, "44") + a(3, "44") +
                            c(1, "FFFFFF") + c(3, "FFFFFF") + t(frx(tmpx).t() + fry(tmpy).t() + frz(tmpz).t()) +
                            blur(2) +
                            ptstr);
                        ass_out.AppendEvent(60, "pt", ptt0, ptt1,
                            move(ptx0 + 2, pty0 + 3, ptx1 + 2, pty1 + 3) + a(1, "44") + a(3, "44") +
                            c(1, "000000") + c(3, "000000") + t(frx(tmpx).t() + fry(tmpy).t() + frz(tmpz).t()) +
                            blur(1) +
                            ptstr);
                    }
                }
            }

            Console.WriteLine(ass_out.Events.Count);
            ass_out.SaveFile(this.OutFileName);
        }
    }
}
