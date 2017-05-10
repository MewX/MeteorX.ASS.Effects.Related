using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.Model;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class AoiHana_OP : BaseAnime2
    {
        public AoiHana_OP()
        {
            this.FontHeight = 40;
            this.FontSpace = 2;
            this.IsAvsMask = true;
            this.MarginLeft = 17;
            this.MarginBottom = 17;
            this.MarginTop = 17;
            this.MarginRight = 17;
            this.PlayResX = 1280;
            this.PlayResY = 720;
            this.InFileName = @"G:\Workshop\aoi hana\op\op_k.ass";
            this.OutFileName = @"G:\Workshop\aoi hana\op\op.ass";
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS() { Header = ass_in.Header, Events = new List<ASSEvent>() };

            for (int iEv = 0; iEv < ass_in.Events.Count; iEv++)
            {
                bool isJp = iEv <= 10;
                this.MaskStyle = isJp ?
                    "Style: Default,HGKyokashotai,40,&H00FFFFFF,&HFFFFFFFF,&HFFFFFFFF,&HFFFFFFFF,0,0,0,0,100,100,0,0,1,0,0,5,0,0,0,128" :
                    "Style: Default,仿宋,36,&H00FFFFFF,&HFFFFFFFF,&HFFFFFFFF,&HFFFFFFFF,0,0,0,0,100,100,0,0,1,0,0,5,0,0,0,1";
                this.FontHeight = isJp ? 40 : 36;
                int jEv = isJp ? iEv : iEv - 11;
                //if (jEv != 4) continue;
                ASSEvent ev = ass_in.Events[iEv];
                List<KElement> kelems = ev.SplitK(true);
                int totalWidth = GetTotalWidth(ev);
                int x0 = MarginLeft;
                if (jEv % 2 != 0) x0 = PlayResX - MarginRight - totalWidth;
                int y0 = (isJp) ? (PlayResY - MarginBottom - FontHeight) : MarginTop;
                int kSum = 0;
                int x0_start = x0;
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
                    x0 += this.FontSpace + sz.Width;
                    if (ke.KText.Trim().Length == 0) continue;
                    StringMask mask = GetMask(ke.KText, x, y);
                    string evStyle = isJp ? "op_jp" : "op_cn";

                    string col_aoi = "ADA088";

                    double t0 = ev.Start - Common.RandomDouble(rnd, 0.5, 1.5);
                    double t1 = kStart - 0.1;
                    if (!isJp) t1 = ev.Start + iK * 0.1;
                    double t2 = t1 + 0.3;
                    double t3 = ev.End;
                    if (isJp && iEv == 6 && iK <= 4)
                        t3 = ev.Start + 3;
                    double t4 = t3 + 0.5;

                    double y_start = y - Common.RandomDouble(rnd, 10, 20);
                    double y_end = y;

                    double tmve = ev.Start + 2;
                    if (!isJp) tmve = ev.Start + iK * 0.1;
                    if (tmve > kStart) tmve = kStart;
                    Func<double, double> fPosY = ti => (ti < tmve ? (ti - t0) / (tmve - t0) * (y_end - y_start) + y_start : y_end);

                    ass_out.AppendEvent(50, evStyle, t0, t1,
                        move(x, fPosY(t0), x, fPosY(t1)) + a(1, "00") + a(4, "66") +
                        c(1, "FFFFFF") + c(4, "777777") + shad(1) + fad(0.5, 0) +
                        ke.KText);
                    ass_out.AppendEvent(50, evStyle, t1, t2,
                        move(x, fPosY(t1), x, fPosY(t2)) + a(1, "00") + a(4, "66") +
                        c(1, "FFFFFF") + c(4, "777777") + shad(1) + fad(0, t2 - t1) + t(blur(2.5).t()) +
                        ke.KText);
                    ass_out.AppendEvent(50, evStyle, t1, t2,
                        move(x, fPosY(t1), x, fPosY(t2)) + a(1, "00") + a(4, "66") +
                        c(1, col_aoi) + c(4, "777777") + shad(1) + fad(t2 - t1, 0) + blur(2.5) + t(blur(0).t()) +
                        ke.KText);
                    ass_out.AppendEvent(50, evStyle, t2, t3,
                        move(x, fPosY(t2), x, fPosY(t3)) + a(1, "00") + a(4, "66") +
                        c(1, col_aoi) + c(4, "777777") + shad(1) +
                        ke.KText);
                    if (!(isJp && iEv >= 5))
                    {
                        ass_out.AppendEvent(50, evStyle, t3, t4,
                            move(x, fPosY(t3), x, fPosY(t4)) + a(1, "00") + a(4, "66") +
                            c(1, col_aoi) + c(4, "777777") + shad(1) + fad(0, t4 - t3) + t(blur(2.5).t()) +
                            ke.KText);
                    }

                    ass_out.AppendEvent(49, evStyle, t1, (!(isJp && iEv >= 4)) ? t4 : t3,
                        move(x, fPosY(t1), x, fPosY(t4)) + a(1, "77") + c(1, col_aoi) +
                        fad(0.4, 0.4) + blur(2) +
                        ke.KText);

                    int mo = 4;
                    if (!isJp) mo = 0;
                    ass_out.AppendEvent(40, evStyle, t0, t1,
                        pos(x, y + FontHeight / 2 - mo) + an(2) + a(1, "44") + c(1, "AAAAAA") +
                        blur(1) + frx(125) + fad(t1 - t0, 0) +
                        ke.KText);
                    ass_out.AppendEvent(41, evStyle, t1, t1 + 0.8,
                        pos(x, y + FontHeight / 2 - mo) + an(2) + a(1, "77") + c(1, "333333") +
                        blur(3) + frx(125) + fad(0, 0.8) +
                        ke.KText);
                    ass_out.AppendEvent(40, evStyle, t1, t3,
                        pos(x, y + FontHeight / 2 - mo) + an(2) + a(1, "44") + c(1, "AAAAAA") +
                        blur(1) + frx(125) +
                        ke.KText);
                    ass_out.AppendEvent(40, evStyle, t3, t4,
                        pos(x, y + FontHeight / 2 - mo) + an(2) + a(1, "44") + c(1, "AAAAAA") +
                        blur(1) + frx(125) + fad(0, t4 - t3) +
                        ke.KText);

                    if (!isJp) continue;

                    if (isJp && iEv >= 5)
                    {
                        foreach (ASSPoint pt in mask.Points)
                        {
                            double ptx1 = pt.X - Common.RandomDouble(rnd, 100, 300);
                            double pty1 = pt.Y - Common.RandomDouble(rnd, 10, 100);
                            double ptt0 = t3 + (double)(pt.X - x0_start) / 200 + Common.RandomDouble(rnd, -0.15, 0.15);
                            if (isJp && iEv == 6 && iK > 4) ptt0 -= 1;
                            double ptt1 = ptt0 + Common.RandomDouble(rnd, 0.5, 1);
                            ass_out.AppendEvent(60, "pt", t3, ptt0,
                                pos(pt.X, pt.Y) + a(1, Common.ToHex2(255 - pt.Brightness)) +
                                shad(1) + a(4, "66") + c(4, "777777") + c(1, col_aoi) +
                                p(1) + "m 0 0 l 1 0 1 1 0 1");

                            if (Common.RandomBool(rnd, 0.5))
                            {
                                ass_out.AppendEvent(61, "pt", ptt0, ptt1,
                                    move(pt.X, pt.Y, ptx1, pty1) + a(1, "77") +
                                    fad(0, 0.5) + c(1, "FFFFFF") + frz(Common.RandomInt(rnd, 0, 359)) +
                                    CreatePolygon(rnd, 20, 30, 6));
                                ass_out.AppendEvent(60, "pt", ptt0, ptt1,
                                    move(pt.X, pt.Y, ptx1, pty1) + a(1, "00") +
                                    fad(0, 0.5) + c(1, col_aoi) + frz(Common.RandomInt(rnd, 0, 359)) +
                                    CreatePolygon(rnd, 20, 30, 6));
                            }

                            continue;
                            ass_out.AppendEvent(60, "pt", t3, ptt1,
                                move(pt.X, pt.Y, ptx1, pty1, ptt0 - t3, ptt1 - t3) + a(1, Common.ToHex2(255 - pt.Brightness)) +
                                fad(0, 0.5) + shad(1) + a(4, "66") + c(4, "777777") + c(1, col_aoi) +
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
