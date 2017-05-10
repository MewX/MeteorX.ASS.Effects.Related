using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.Model;
using MeteorX.AssTools.KaraokeApp.Toys;
using MeteorX.AssTools.KaraokeApp.XXParticle;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class Umineko_OP : BaseAnime2
    {
        public Umineko_OP()
        {
            this.FontHeight = 38;
            this.FontSpace = 1;
            this.IsAvsMask = true;
            this.MarginLeft = 30;
            this.MarginBottom = 30;
            this.MarginTop = 30;
            this.MarginRight = 30;
            this.PlayResX = 1280;
            this.PlayResY = 720;
            this.InFileName = @"g:\workshop\umineko\op\op_k.ass";
            this.OutFileName = @"g:\workshop\umineko\op\op.ass";
        }

        public override Size GetSize(string s)
        {
            return new Size { Height = FontHeight, Width = FontHeight * s.Length + (s.Length - 1) * FontSpace };
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS() { Header = ass_in.Header, Events = new List<ASSEvent>() };

            for (int iEv = 0; iEv < ass_in.Events.Count; iEv++)
            {
                bool isJp = true;
                //if (iEv > 1) continue;
                this.MaskStyle = isJp ?
                    "Style: Default,DFMincho-UB,38,&H00FFFFFF,&HFFFFFFFF,&HFFFFFFFF,&HFFFFFFFF,0,0,0,0,100,100,0,0,0,0,0,5,0,0,0,128" :
                    "Style: Default,汉仪粗宋繁,38,&H00FFFFFF,&HFFFFFFFF,&HFFFFFFFF,&HFFFFFFFF,1,0,0,0,100,100,0,0,0,0,0,5,0,0,0,134";
                ASSEvent ev = ass_in.Events[iEv];
                List<KElement> kelems = ev.SplitK(false);
                int x0 = (PlayResX - GetTotalWidth(ev) - MarginLeft - MarginRight) / 2 + MarginLeft;
                int x0_start = x0;
                int y0 = MarginTop;
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
                    int x_an7 = x0 + 2;
                    int y_an7 = y0;
                    StringMask mask = GetMask(ke.KText, x, y);
                    x0 += this.FontSpace + sz.Width;
                    if (ke.KText.Trim().Length == 0) continue;
                    string evStyle = "jp";

                    double t0 = ev.Start - 0.5 + iK * 0.1;
                    double t1 = t0 + 0.5;
                    double t2 = kStart;
                    double t3 = kEnd;
                    double t4 = ev.End - 0.5 + iK * 0.1;
                    double t5 = t4 + 0.5;

                    string[,] bordColors =
                                    {
                                        {"FFB9A8","BC5237","6B1905"},
//                                        {"FFE1DA","FFBCAB","EF6C4B"},
                                        {"FFB9A8","BC5237","6B1905"},
                                        {"AFAFFF","595AF1","595AF1"},
                                        {"AFAFFF","595AF1","595AF1"},
                                        {"8DEDFF","5CCFFF","2BABF5"},
                                        {"8DEDFF","5CCFFF","2BABF5"},
                                        {"8DEDFF","5CCFFF","2BABF5"},
                                        {"AFAFFF","595AF1","595AF1"},
                                        {"AFAFFF","595AF1","595AF1"},
                                        {"FFB9A8","BC5237","6B1905"},
                                        {"AFAFFF","595AF1","595AF1"}
                                    };
                    string MainColor = bordColors[iEv, 2];



                    for (int i = -20; i <= 20; i++) // 50 - 70
                    {
                        //string col = Common.scaleColor(MainColor, "FFFFFF", 1.0 - Math.Pow((double)Math.Abs(i) / 20.0, 2));
                        string col = i == 0 ? "FFFFFF" : MainColor;
                        col = Common.scaleColor(MainColor, "FFFFFF", 1.0 - Math.Pow((double)Math.Abs(i) / 20.0, 3));
                        string alp = i == 0 ? "DD" : "DD";
                        ass_out.AppendEvent(70 - Math.Abs(i), evStyle, t0, t1,
                            move(x + i * 2, y, x, y) + fad(0.15, 0) +
                            a(1, "DD") + c(1, col) + blur(Math.Abs(i) * 0.2) +
                            ke.KText);
                        /*ass_out.AppendEvent(70 - Math.Abs(i), evStyle, t1, t4,
                            pos(x + i * 0.06, y) +
                            a(1, "DD") + c(1, col) + blur(Math.Abs(i) * 0.2) +
                            ke.KText);*/
                        { // t1 - t4
                            double ptt0 = t2;
                            double ptt1 = t2;
                            double ptt2 = t2;
                            double ptt3 = t3 + 0.1;
                            if (ptt3 > t4) ptt3 = t4;
                            if (ptt1 > t4)
                            {
                                ptt1 = ptt2 = ptt3;
                            }
                            else if (ptt1 > ptt2)
                            {
                                ptt2 = ptt3;
                            }
                            ass_out.AppendEvent(70 - Math.Abs(i), evStyle, t1, ptt0,
                                pos(x + i * 0.06, y) +
                                a(1, alp) + c(1, col) + blur(Math.Abs(i) * 0.2) +
                                ke.KText);
                            ass_out.AppendEvent(70 - Math.Abs(i), evStyle, ptt0, ptt1,
                                pos(x + i * 0.06, y) +
                                a(1, alp) + c(1, col) + blur(Math.Abs(i) * 0.2) + t(fsc(130, 130).t()) +
                                ke.KText);
                            ass_out.AppendEvent(70 - Math.Abs(i), evStyle, ptt1, ptt2,
                                pos(x + i * 0.06, y) +
                                a(1, alp) + c(1, col) + blur(Math.Abs(i) * 0.2) + fsc(130, 130) +
                                ke.KText);
                            ass_out.AppendEvent(70 - Math.Abs(i), evStyle, ptt2, ptt3,
                                pos(x + i * 0.06, y) +
                                a(1, alp) + c(1, col) + blur(Math.Abs(i) * 0.2) + fsc(130, 130) + t(fsc(100, 100).t()) +
                                ke.KText);
                            ass_out.AppendEvent(70 - Math.Abs(i), evStyle, ptt3, t4,
                                pos(x + i * 0.06, y) +
                                a(1, alp) + c(1, col) + blur(Math.Abs(i) * 0.2) +
                                ke.KText);
                            if (i == 0)
                            {
                                {
                                    double[] timeSegs = { t0 + 0.2, ptt0, ptt1, ptt2, ptt3, t5 - 0.2 };
                                    string[] addStrs = { fad(0.8, 0), t(fsc(130, 130).t()), fsc(130, 130), fsc(130, 130) + t(fsc(100, 100).t()), fad(0, 0.8) };
                                    for (int j = 0; j + 1 < timeSegs.Length; j++)
                                    {
                                        ass_out.AppendEvent(39, evStyle, timeSegs[j], timeSegs[j + 1],
                                            pos(x, y) +
                                            a(3, "33") + c(3, bordColors[iEv, 0]) + bord(3) + blur(3) + addStrs[j] +
                                            ke.KText);
                                        ass_out.AppendEvent(38, evStyle, timeSegs[j], timeSegs[j + 1],
                                            pos(x, y) +
                                            a(3, "33") + c(3, bordColors[iEv, 1]) + bord(5) + blur(5) + addStrs[j] +
                                            ke.KText);
                                        //if (new int[] { 0, 2, 3, 6, 7 }.Contains(iEv)) continue;
                                        ass_out.AppendEvent(37, evStyle, timeSegs[j], timeSegs[j + 1],
                                            pos(x, y) +
                                            a(3, "33") + c(3, bordColors[iEv, 2]) + bord(8) + blur(8) + addStrs[j] +
                                            ke.KText);
                                    }
                                }

                                /*
                                ass_out.AppendEvent(29, evStyle, t0, ptt0,
                                    pos(x + i * 0.06 + 3, y + 3) + fad(0.3, 0) +
                                    a(1, "66") + c(1, "000000") + blur(2) +
                                    ke.KText);
                                ass_out.AppendEvent(29, evStyle, ptt0, ptt1,
                                    pos(x + i * 0.06 + 3, y + 3) +
                                    a(1, "66") + c(1, "000000") + blur(2) + t(fsc(130, 130).t()) +
                                    ke.KText);
                                ass_out.AppendEvent(29, evStyle, ptt1, ptt2,
                                    pos(x + i * 0.06 + 3, y + 3) +
                                    a(1, "66") + c(1, "000000") + blur(2) + fsc(130, 130) +
                                    ke.KText);
                                ass_out.AppendEvent(29, evStyle, ptt2, ptt3,
                                    pos(x + i * 0.06 + 3, y + 3) +
                                    a(1, "66") + c(1, "000000") + blur(2) + fsc(130, 130) + t(fsc(100, 100).t()) +
                                    ke.KText);
                                ass_out.AppendEvent(29, evStyle, ptt3, t5,
                                    pos(x + i * 0.06 + 3, y + 3) + fad(0, 0.3) +
                                    a(1, "66") + c(1, "000000") + blur(2) +
                                    ke.KText);
                                 * */
                            }
                        }
                        ass_out.AppendEvent(70 - Math.Abs(i), evStyle, t4, t5,
                            move(x + i * 0.06, y, x + i * 2, y) + fad(0, 0.15) +
                            a(1, "DD") + c(1, col) + blur(Math.Abs(i) * 0.2) +
                            ke.KText);
                    }
                }
            }

            Console.WriteLine(ass_out.Events.Count);
            ass_out.SaveFile(this.OutFileName);
        }
    }
}
