using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.Model;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class Asura_OP : BaseAnime2
    {
        public Asura_OP()
        {
            this.FontHeight = 38;
            this.FontSpace = 2;
            this.IsAvsMask = true;
            this.MarginLeft = 30;
            this.MarginBottom = 30;
            this.MarginTop = 26;
            this.MarginRight = 30;
            this.PlayResX = 1280;
            this.PlayResY = 720;
            this.InFileName = @"g:\workshop\asura\op_k.ass";
            this.OutFileName = @"g:\workshop\asura\op.ass";
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS() { Header = ass_in.Header, Events = new List<ASSEvent>() };

            for (int iEv = 0; iEv < ass_in.Events.Count; iEv++)
            {
                bool isJp = iEv <= 9;
                //if (iEv != 0) continue;
                //if (iEv != 9) continue;
                //if (!isJp) continue;
                //if (iEv != 5 && iEv != 6) continue;
                //if (iEv != 19 && iEv != 9) continue;
                //if (iEv < 20) continue;
                ASSEvent ev = ass_in.Events[iEv];
                List<KElement> kelems = ev.SplitK(!isJp);
                int x0 = MarginLeft;
                int y0 = (isJp || iEv >= 20) ? (PlayResY - MarginBottom - FontHeight) : MarginTop;
                int kSum = 0;

                /// 两句英文
                if (iEv >= 20)
                {
                    y0 -= 45;
                    x0 += 4;
                    this.MaskStyle = "Style: Default,DFMincho-UB,30,&H00FFFFFF,&HFFFFFFFF,&HFFFFFFFF,&HFFFFFFFF,0,0,0,0,100,100,0,0,0,0,0,5,0,0,0,128";
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
                        int x_an7 = x0 + 2;
                        int y_an7 = y0;
                        StringMask mask = GetMask(ke.KText, x, y);
                        x0 += this.FontSpace + sz.Width;
                        if (ke.KText.Trim().Length == 0) x0 -= sz.Width / 2;
                        if (ke.KText.Trim().Length == 0) continue;

                        double t0 = ev.Start - 0.5 + iK * 0.07;
                        double t1 = t0 + 0.35;
                        double t2 = kStart;
                        double t3 = kEnd;
                        double t4 = ev.End - 0.5 + iK * 0.07;
                        double t5 = t4 + 0.35;

                        Func<double, double> fPosX = h => (x + (h - ev.Start) * 20);

                        ass_out.AppendEvent(50, "en", t0, t4,
                            move(fPosX(t0), y, fPosX(t4), y) + a(1, "00") +
                            fsc(0, 0) + fad(0.1, 0) +
                            t(0, 0.28, fsc(140, 140).t()) + t(0.28, 0.35, fsc(100, 100).t()) +
                            ke.KText);
                        ass_out.AppendEvent(30, "en", t0, t4,
                            fsc(0, 0) + fad(0.1, 0) +
                            t(0, 0.28, fsc(140, 140).t()) + t(0.28, 0.35, fsc(100, 100).t()) +
                            move(fPosX(t0), y, fPosX(t4), y) + a(3, "44") + bord(3) + blur(3) + c(3, "6888FF") +
                            ke.KText);
                        ass_out.AppendEvent(50, "en", t4, t5,
                            move(fPosX(t4), y, fPosX(t5), y) + a(1, "00") +
                            fad(0, 0.1) +
                            t(0, 0.07, fsc(140, 140).t()) + t(0.07, 0.35, fsc(0, 0).t()) +
                            ke.KText);
                        ass_out.AppendEvent(30, "en", t4, t5,
                            fad(0, 0.1) +
                            t(0, 0.07, fsc(140, 140).t()) + t(0.07, 0.35, fsc(0, 0).t()) +
                            move(fPosX(t4), y, fPosX(t5), y) + a(3, "44") + bord(3) + blur(3) + c(3, "6888FF") +
                            ke.KText);

                        string ptcol = "FF68AD";
                        for (int i = 0; i < 50; i++)
                        {
                            double ptx0 = Common.RandomDouble_Gauss(rnd, x - 2, x + 2);
                            double pty0 = Common.RandomDouble_Gauss(rnd, y - 2, y + 2);
                            double ptag = Common.RandomDouble(rnd, 0, Math.PI * 2);
                            double ptr = Common.RandomDouble_Gauss(rnd, 50, 75);
                            double ptx1 = ptx0 + Math.Cos(ptag) * ptr;
                            double pty1 = pty0 + Math.Sin(ptag) * ptr;
                            double ptt0 = Common.RandomDouble(rnd, t0, t0 + 0.2);
                            double ptt1 = ptt0 + Common.RandomDouble(rnd, 2, 3);

                            string tstr = "";
                            for (double tmpt = 0; tmpt <= ptt1 - ptt0; tmpt += 0.40)
                                tstr += t(tmpt, tmpt + 0.20, a(1, "FF").t() + a(3, "FF").t()) + t(tmpt + 0.20, tmpt + 0.40, a(1, "44").t() + a(3, "88").t());
                            ass_out.AppendEvent(0, "pt", ptt0, ptt1,
                                move(ptx0, pty0, ptx1, pty1) + a(1, "44") + a(3, "88") + c(3, Common.RandomBool(rnd, 0.5) ? ptcol : "FFFFFF") +
                                bord(1.5) + blur(1.5) + fsc(150, 150) + tstr +
                                p(1) + "m 0 0 l 1 0 1 1 0 1");
                        }
                    }
                    continue;
                }

                for (int iK = 0; iK < kelems.Count; iK++)
                {
                    Console.WriteLine("{0} / {1} : {2} / {3}", iEv + 1, ass_in.Events.Count, iK + 1, kelems.Count);
                    if (iEv == 19) isJp = iK <= 10;
                    this.MaskStyle = isJp ?
                        "Style: Default,DFMincho-UB,38,&H00FFFFFF,&HFFFFFFFF,&HFFFFFFFF,&HFFFFFFFF,0,0,0,0,100,100,0,0,0,0,0,5,0,0,0,128" :
                        "Style: Default,汉仪粗宋繁,38,&H00FFFFFF,&HFFFFFFFF,&HFFFFFFFF,&HFFFFFFFF,1,0,0,0,100,100,0,0,0,0,0,5,0,0,0,134";
                    string evStyle = isJp ? "" : "cn";
                    string outlineFontname = isJp ? "DFMincho-UB" : "汉仪粗宋繁";
                    int outlineEncoding = isJp ? 128 : 134;
                    isJp = iEv <= 9;
                    KElement ke = kelems[iK];
                    Size sz = GetSize(ke.KText);
                    if (ke.KText[0] == 'く') x0 += 2;
                    double kStart = ev.Start + kSum * 0.01;
                    double kEnd = kStart + ke.KValue * 0.01;
                    kSum += ke.KValue;
                    int x = x0 + this.FontSpace + sz.Width / 2;
                    int y = y0 + FontHeight / 2;
                    int x_an7 = x0 + 2;
                    int y_an7 = y0;
                    StringMask mask = GetMask(ke.KText, x, y);
                    x0 += this.FontSpace + sz.Width;
                    if (ke.KText.Trim().Length == 0) continue;
                    string outlineString = GetOutline(x - FontHeight / 2, y - FontHeight / 2, ke.KText[0], outlineFontname, outlineEncoding, 38, 0, 262);

                    if (iEv == 9 && iK == 0)
                    {
                        outlineString = GetOutline(x - FontHeight / 2 + 10, y - FontHeight / 2, 'I', outlineFontname, outlineEncoding, 38, 0, 262);
                    }
                    if (iEv == 9 && iK == 2)
                    {
                        outlineString =
                            GetOutline(x - FontHeight / 2 - 29, y - FontHeight / 2, 'w', outlineFontname, outlineEncoding, 38, 0, 262) +
                            GetOutline(x - FontHeight / 2 - 29 + 30 - 12 + 1, y - FontHeight / 2, 'a', outlineFontname, outlineEncoding, 38, 0, 262) +
                            GetOutline(x - FontHeight / 2 - 29 + 60 - 20 - 2, y - FontHeight / 2, 'n', outlineFontname, outlineEncoding, 38, 0, 262) +
                            GetOutline(x - FontHeight / 2 - 29 + 90 - 34, y - FontHeight / 2, 'n', outlineFontname, outlineEncoding, 38, 0, 262) +
                            GetOutline(x - FontHeight / 2 - 29 + 120 - 44, y - FontHeight / 2, 'a', outlineFontname, outlineEncoding, 38, 0, 262);
                    }
                    if (iEv == 9 && iK == 4)
                    {
                        outlineString =
                            GetOutline(x - FontHeight / 2 - 29 - 11 + 30, y - FontHeight / 2, 's', outlineFontname, outlineEncoding, 38, 0, 262) +
                            GetOutline(x - FontHeight / 2 - 29 + 30 - 12 + 1 + 20, y - FontHeight / 2, 'a', outlineFontname, outlineEncoding, 38, 0, 262) +
                            GetOutline(x - FontHeight / 2 - 29 + 60 - 20 - 2 + 19, y - FontHeight / 2, 'y', outlineFontname, outlineEncoding, 38, 0, 262);
                    }

                    if (iEv == 19 && iK <= 10)
                    {
                        outlineString = GetOutline(x - FontHeight / 2 + 8, y - FontHeight / 2, ke.KText[0], outlineFontname, outlineEncoding, 38, 0, 262);
                    }

                    if (!isJp) y += 2;

                    double t0 = ev.Start - 0.5 + iK * 0.07;
                    double t1 = t0 + 0.5;
                    double t2 = kStart;
                    double t3 = kEnd;
                    double t4 = ev.End - 0.6 + iK * 0.07;
                    double t5 = t4 + 0.5;

                    int lumsz = 12;
                    if (iEv == 9 && iK == 2) lumsz = 18;
                    if (iEv == 1 || iEv == 2) lumsz = 15;
                    ASSPointF[] lums = new ASSPointF[1];
                    for (int i = 0; i < lums.Length; i++)
                        lums[i] = new ASSPointF { X = Common.RandomDouble(rnd, x_an7 - 2, x0), Y = Common.RandomDouble(rnd, y - 18, y + 18) };
                    string[] lumcols = { "002BC8" };
                    string[] lumalphas = { "00" };

                    string shadCol1 = "000000";
                    if (!isJp) shadCol1 = "FFFFFF";
                    string shadCol2 = "FFFFFF";

                    string lightCol = "6888FF";

                    if (isJp)
                    {
                        ass_out.AppendEvent(50, evStyle, t2 - 0.15, t3 - 0.15,
                            pos(x + 1, y + 2) + fad(0.1, Common.Min(0.3, t3 - t2 - 0.2)) + fsc(150, 150) + t(fsc(100, 100).t()) +
                            a(1, "00") + a(3, "66") + c(3, lumcols[0]) + bord(3) + blur(3) +
                            ke.KText);
                        ass_out.AppendEvent(8, evStyle, t0, t2 + 0.8,
                            pos(x + 2, y + 2) + fad(0.8, 0.8) +
                            a(1, "77") + c(1, shadCol1) + blur(2) +
                            ke.KText);
                        ass_out.AppendEvent(8, evStyle, t2, t5 - 0.5,
                            pos(x + 2, y + 2) + fad(0.8, 0.8) +
                            a(1, "00") + c(1, shadCol2) + blur(2) +
                            ke.KText);
                    }
                    else
                    {
                        ass_out.AppendEvent(8, evStyle, t0, t5 - 0.5,
                            pos(x + 2, y + 2) + fad(0.8, 0.8) +
                            a(1, "00") + c(1, shadCol1) + blur(2) +
                            ke.KText);
                    }

                    for (int iLum = 0; iLum < lums.Length; iLum++)
                    {
                        string col1 = lumcols[iLum];
                        double lum1_x = lums[iLum].X;
                        double lum1_y = lums[iLum].Y;
                        for (int i = 0; i < 2; i++)
                        {
                            ass_out.AppendEvent(20, "pt", t0, t1,
                                an(5) +
                                clip(4, outlineString) + pos(lum1_x, lum1_y) +
                                a(1, lumalphas[iLum]) + a(3, "00") + c(1, col1) + c(3, col1) +
                                t(bord(lumsz).t() + blur(lumsz).t()) +
                                @"{\p1}m 0 0 l 1 0 1 1 0 1");
                            ass_out.AppendEvent(20, "pt", t1, t4,
                                an(5) +
                                clip(4, outlineString) + pos(lum1_x, lum1_y) +
                                a(1, lumalphas[iLum]) + a(3, "00") + c(1, col1) + c(3, col1) +
                                blur(lumsz) + bord(lumsz) +
                                @"{\p1}m 0 0 l 1 0 1 1 0 1");
                            ass_out.AppendEvent(20, "pt", t4, t5,
                                an(5) +
                                clip(4, outlineString) + pos(lum1_x, lum1_y) +
                                a(1, lumalphas[iLum]) + a(3, "00") + c(1, col1) + c(3, col1) +
                                blur(lumsz) + bord(lumsz) +
                                t(bord(0).t() + blur(0).t()) +
                                @"{\p1}m 0 0 l 1 0 1 1 0 1");
                        }
                    }

                    if (!isJp) continue;

                    /*if (iEv == 9)
                    {
                        string strikeCol = "FF68AD";
                        for (int j = 0; j < 40; j++)
                        {
                            for (int i = 0; i < 20; i++)
                            {
                                double ptt0 = t2 - 0.3 + i * 0.01;
                                double ptt1 = ptt0 + 0.3;
                                string ptCol1 = Common.scaleColor(strikeCol, "FFFFFF", 1 - (double)i / 19.0 * 0.5);
                                string ptAlpha = "77";
                                if (i >= 17) ptAlpha = "00";
                                int ag1 = j * 9 + (int)(((double)i * 0.01) * 360);
                                int ag2 = ag1 + 360;
                                ass_out.AppendEvent(0 + i / 2, "pt", ptt0, ptt1,
                                    move(x + 50, y, x + 50, y - 50) + org(x, y) + blur(2.3 - i * 0.07) + a(1, ptAlpha) + a(3, ptAlpha) + frz(-ag1) +
                                    c(1, ptCol1) + c(3, ptCol1) + t(0, 0.1, fscx(250 - 10 * i).t()) + fscy(100 - i * 5) + t(frz(-ag2).t()) +
                                    p(3) + "m 100 0 b 20 10 -20 10 -100 0 -20 -10 20 -10 100 0");
                            }
                        }
                    }*/
                    if (iEv == 0)
                    {
                        foreach (ASSPoint pt in mask.Points)
                        {
                            double ag = (double)(pt.Y - mask.Y0) / (double)mask.Height * 0.25;
                            int iag = (int)(ag / Math.PI / 2 * 360);
                            ass_out.AppendEvent(70, "pt", t2 - 0.2, t5,
                                pos(pt.X, pt.Y) + frz(iag) + t(fry(180).t()) + fad(0.1, 0.5) +
                                a(1, "F4") + c(1, lightCol) + be(1) +
                                @"{\p2}m 0 0 l -200 0 0 2");
                        }
                    }
                    if (iEv == 1 || iEv == 2)
                    {
                        double ptt0 = t2 - 0.2;
                        double ptt1 = t3 - 0.2;
                        double ptt2 = t3 + 2;
                        if (ptt2 > t4) ptt2 = t4;
                        ass_out.AppendEvent(5, "pt", ptt0, ptt2,
                            pos(x, y) + an(5) + blur(2) + bord(2) + fad(0.3, 0.5) +
                            a(1, "00") + c(1, "FFFFFF") + fsc(50, 50) +
                            t(0, 0.01, fsc(130, 130).t()) +
                            t(ptt1 - ptt0 - 0.3, ptt1 - ptt0, fsc(70, 70).t() + a(1, "AA").t()) +
                            a(3, "44") + c(3, "FF68AD") + t(frz((int)((ptt2 - ptt0) * ((((iEv + iK) % 2 == 1) ? 100 : -100)))).t()) +
                            p(2) + "m 64 14 b 66 10 67 5 66 1 b 59 0 50 0 46 2 b 45 6 47 10 48 15 b 42 15 37 17 32 21 b 31 18 27 13 24 11 b 18 15 14 20 10 25 b 13 29 17 31 21 32 b 17 38 14 43 14 49 b 10 47 5 46 1 47 b 0 53 0 60 1 67 b 5 68 10 66 14 64 b 15 70 17 76 20 81 b 17 83 13 85 10 88 b 13 94 18 98 25 103 b 27 101 31 96 32 92 b 36 95 42 98 48 99 b 47 103 46 107 46 112 b 53 113 59 113 66 112 b 66 107 66 104 64 99 b 70 98 75 95 81 92 b 81 95 84 100 88 103 b 93 100 99 94 102 88 b 99 85 95 83 92 81 b 95 76 97 70 98 65 b 103 66 106 68 111 67 b 113 60 113 55 111 47 b 107 46 103 48 98 49 b 97 44 95 38 92 32 b 95 31 100 29 102 25 b 98 19 94 15 88 11 b 85 13 82 18 81 21 b 76 18 70 15 64 14 l 55 37 b 68 38 75 45 76 57 b 76 68 68 76 56 77 b 44 75 37 68 36 57 b 37 45 44 38 55 37 l 59 37 l 66 15 l 64 14");
                    }
                    if (iEv == 3 || iEv == 4)
                    {
                        string strikeCol = "FF68AD";
                        if (iEv == 3)
                        {
                            for (int i = 0; i < 20; i++)
                            {
                                double ptt0 = t2 - 0.3 + i * 0.01;
                                double ptt1 = ptt0 + 0.3;
                                string ptCol1 = Common.scaleColor(strikeCol, "FFFFFF", (double)i / 19.0 * 0.8);
                                string ptCol2 = Common.scaleColor(strikeCol, "FFFFFF", (double)i / 19.0 * 0.5);
                                string ptAlpha = "77";
                                if (i >= 17) ptAlpha = "00";
                                ass_out.AppendEvent(15, "pt", ptt0, ptt1,
                                    pos(x, y) + blur(2.3 - i * 0.07) + a(1, ptAlpha) + a(3, ptAlpha) + frz(90) +
                                    c(1, ptCol1) + c(3, ptCol1) + t(0, 0.1, fscx(500 - 20 * i).t()) + fscy(100 - i * 5) +
                                    p(3) + "m 100 0 b 20 10 -20 10 -100 0 -20 -10 20 -10 100 0");
                            }
                        }
                        else if (iEv == 4)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if ((iEv + iK + (j / 2)) % 2 == 0) continue;
                                string ptStr = "m 100 0 b 20 10 -20 10 -100 0 -20 -10 20 -10 100 0";
                                if (j >= 2) ptStr = "m 0 -100 b 10 -20 10 20 0 100 -10 20 -10 -20 0 -100";
                                Func<double, ASSPointF> fpos = ti => new ASSPointF();
                                if (j == 2) fpos = ti => new ASSPointF { X = x - 20, Y = y + ti * 200 };
                                else if (j == 3) fpos = ti => new ASSPointF { X = x + 20, Y = y - ti * 200 };
                                else if (j == 0) fpos = ti => new ASSPointF { X = x + ti * 200, Y = y - 20 };
                                else if (j == 1) fpos = ti => new ASSPointF { X = x - ti * 200, Y = y + 20 };
                                for (int i = 0; i < 20; i++)
                                {
                                    double ptt0 = t2 - 0.3 + i * 0.01;
                                    double ptt1 = ptt0 + 0.3;
                                    string ptCol1 = Common.scaleColor(strikeCol, "FFFFFF", (double)i / 19.0 * 0.8);
                                    string ptCol2 = Common.scaleColor(strikeCol, "FFFFFF", (double)i / 19.0 * 0.5);
                                    string ptAlpha = "77";
                                    if (i >= 17) ptAlpha = "00";
                                    ass_out.AppendEvent(15, "pt", ptt0, ptt1,
                                        move(fpos(ptt0 - t2 + 0.3 - 0.3), fpos(ptt1 - t2 + 0.3)) + blur(2.3 - i * 0.07) + a(1, ptAlpha) + a(3, ptAlpha) +
                                        c(1, ptCol1) + c(3, ptCol1) + ((j <= 1) ? (t(0, 0.1, fscx(500 - 20 * i).t()) + fscy(100 - i * 5)) : (t(0, 0.1, fscy(500 - 20 * i).t()) + fscx(100 - i * 5))) +
                                        p(3) + ptStr);
                                }
                            }
                        }
                        {
                            double ptt0 = t2 - 0.2;
                            double ptt1 = t3 - 0.2;
                            double ptt2 = t3 + 2;
                            if (ptt2 > t4) ptt2 = t4;
                            ass_out.AppendEvent(5, "pt", ptt0, ptt2,
                                pos(x, y + 15) + an(5) + blur(2) + bord(2) + fad(0.3, 0.5) +
                                a(1, "00") + c(1, "FFFFFF") + fsc(50, 50) + frx(60) +
                                t(0, 0.01, fsc(130, 130).t()) +
                                t(ptt1 - ptt0 - 0.3, ptt1 - ptt0, fsc(70, 70).t() + a(1, "77").t()) +
                                a(3, "44") + c(3, "FF68AD") + t(frz((int)((ptt2 - ptt0) * ((((iEv + iK) % 2 == 1) ? 100 : -100)))).t()) +
                                p(2) + "m 64 14 b 66 10 67 5 66 1 b 59 0 50 0 46 2 b 45 6 47 10 48 15 b 42 15 37 17 32 21 b 31 18 27 13 24 11 b 18 15 14 20 10 25 b 13 29 17 31 21 32 b 17 38 14 43 14 49 b 10 47 5 46 1 47 b 0 53 0 60 1 67 b 5 68 10 66 14 64 b 15 70 17 76 20 81 b 17 83 13 85 10 88 b 13 94 18 98 25 103 b 27 101 31 96 32 92 b 36 95 42 98 48 99 b 47 103 46 107 46 112 b 53 113 59 113 66 112 b 66 107 66 104 64 99 b 70 98 75 95 81 92 b 81 95 84 100 88 103 b 93 100 99 94 102 88 b 99 85 95 83 92 81 b 95 76 97 70 98 65 b 103 66 106 68 111 67 b 113 60 113 55 111 47 b 107 46 103 48 98 49 b 97 44 95 38 92 32 b 95 31 100 29 102 25 b 98 19 94 15 88 11 b 85 13 82 18 81 21 b 76 18 70 15 64 14 l 55 37 b 68 38 75 45 76 57 b 76 68 68 76 56 77 b 44 75 37 68 36 57 b 37 45 44 38 55 37 l 59 37 l 66 15 l 64 14");
                        }
                    }
                    if (iEv >= 5 && iEv <= 9)
                    {
                        string strikeCol = "FF68AD";
                        double ptt0 = t2 - 0.3;
                        double ptt1 = t3 - 0.2 - 0.1;
                        if (ptt1 > t4) ptt1 = t4;
                        double theta1 = Common.RandomDouble(rnd, 0, Math.PI);
                        List<BaseCurve> curves = new List<BaseCurve>();
                        Circle2 cl = new Circle2 {  X0 = x, Y0 = y, A = 60, B = 27, MinT = 0 + ptt0 * 4, MaxT = (ptt1 - ptt0) * Math.PI * 2 + ptt0 * 4, Theta = theta1, dTheta = 0 };
                        CompositeCurve cc = new CompositeCurve { MinT = ptt0, MaxT = ptt1 };
                        cc.AddCurve(cc.MinT, cc.MaxT, cl);
                        curves.Add(cc);
                        cl = new Circle2 { X0 = x, Y0 = y, A = 60, B = 27, MinT = 0 + ptt0 * 4 + Math.PI, MaxT = Math.PI + (ptt1 - ptt0) * Math.PI * 2 + ptt0 * 4, Theta = theta1, dTheta = 0 };
                        cc = new CompositeCurve { MinT = ptt0, MaxT = ptt1 };
                        cc.AddCurve(cc.MinT, cc.MaxT, cl);
                        curves.Add(cc);
                        for (int i = 0; i < curves.Count; i++)
                        {
                            BaseCurve curve = curves[i];
                            List<ASSPointF> path = curve.GetPath_Dis(6, 7);
                            string strikeCol2 = Common.scaleColor(strikeCol, "FFFFFF", 0.5);
                            foreach (ASSPointF pt in path)
                            {
                                ass_out.AppendEvent((pt.Theta < Math.PI) ? 0 : 100, "pt", pt.T, pt.T + 0.3,
                                    pos(pt.X, pt.Y) + a(1, "44") + a(3, "77") + c(1, "FFFFFF") + c(3, "FFFFFF") +
                                    bord(8) + blur(8) +
                                    t(0, 0.04, c(1, strikeCol2).t() + c(3, strikeCol2).t() + bord(4).t() + blur(4).t()) +
                                    t(0.04, 0.3, c(1, strikeCol).t() + c(3, strikeCol).t() + bord(2).t() + blur(2).t()) +
                                    p(1) + "m 0 0 l 1 0 1 1 0 1");
                                ass_out.AppendEvent((pt.Theta < Math.PI) ? 0 : 100, "pt", pt.T, pt.T + 0.3,
                                    pos(pt.X, pt.Y) + a(1, "44") + a(3, "44") + c(1, "FFFFFF") + c(3, "FFFFFF") +
                                    bord(4) + blur(4) +
                                    t(0, 0.04, bord(2.5).t() + blur(2.5).t()) +
                                    t(0.04, 0.3, bord(1.3).t() + blur(1.3).t()) +
                                    p(1) + "m 0 0 l 1 0 1 1 0 1");
                            }
                        }
                        for (int j = 0; j < 10; j++)
                        {
                            double dz = Common.RandomDouble(rnd, -30, 30);
                            Func<double, int> fz = ti => (int)((theta1 + Math.PI * 0.5) / Math.PI / 2.0 * 360.0 + dz * (ptt1 - ti) / (ptt1 - ptt0));
                            string ptStr = "m 100 0 b 20 10 -20 10 -100 0 -20 -10 20 -10 100 0";
                            for (int i = 0; i < 20; i++)
                            {
                                double pttt0 = ptt0 + i * 0.01;
                                double pttt1 = ptt1;
                                string ptCol1 = Common.scaleColor(strikeCol, "FFFFFF", (double)i / 19.0 * 0.8);
                                string ptCol2 = Common.scaleColor(strikeCol, "FFFFFF", (double)i / 19.0 * 0.5);
                                string ptAlpha = "77";
                                if (i >= 17) ptAlpha = "00";
                                ass_out.AppendEvent(0 + i, "pt", pttt0, pttt1,
                                    fad(0, 0.5) + pos(x, y) + blur(2.3 - i * 0.07) + a(1, ptAlpha) + a(3, ptAlpha) + frz(fz(pttt0)) +
                                    c(1, ptCol1) + c(3, ptCol1) + t(0, 0.1, fscx(500 - 20 * i).t()) + fscy(100 - i * 5) + t(0, (pttt1 - pttt0) * 0.9, frz(fz(pttt1)).t()) +
                                    p(3) + ptStr);
                            }
                        }
                    }
                }
            }

            Console.WriteLine(ass_out.Events.Count);
            ass_out.SaveFile(this.OutFileName);
        }
    }
}
