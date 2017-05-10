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
    class Umineko_ED : BaseAnime2
    {
        public Umineko_ED()
        {
            this.FontHeight = 38;
            this.FontSpace = 4;
            this.IsAvsMask = true;
            this.MarginLeft = 30;
            this.MarginBottom = 30;
            this.MarginTop = 26;
            this.MarginRight = 30;
            this.PlayResX = 1280;
            this.PlayResY = 720;
            this.InFileName = @"g:\workshop\umineko\ed\ed_k.ass";
            this.OutFileName = @"g:\workshop\umineko\ed\ed.ass";
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS() { Header = ass_in.Header, Events = new List<ASSEvent>() };

            for (int iEv = 0; iEv < ass_in.Events.Count; iEv++)
            {
                bool isJp = true;
                this.MaskStyle = isJp ?
                    "Style: Default,DFMincho-UB,38,&H00FFFFFF,&HFFFFFFFF,&HFFFFFFFF,&HFFFFFFFF,0,0,0,0,100,100,0,0,0,0,0,5,0,0,0,128" :
                    "Style: Default,汉仪粗宋繁,38,&H00FFFFFF,&HFFFFFFFF,&HFFFFFFFF,&HFFFFFFFF,1,0,0,0,100,100,0,0,0,0,0,5,0,0,0,134";
                ASSEvent ev = ass_in.Events[iEv];
                List<KElement> kelems = ev.SplitK(true);
                int x0 = (PlayResX - GetTotalWidth(ev) - MarginLeft - MarginRight) / 2 + MarginLeft;
                int x0_start = x0;
                int y0 = MarginTop;
                int kSum = 0;

                for (int iK = 0; iK < kelems.Count; iK++)
                {
                    //if (iK > 0) break;
                    Console.WriteLine("{0} / {1} : {2} / {3}", iEv + 1, ass_in.Events.Count, iK + 1, kelems.Count);
                    string evStyle = isJp ? "jp" : "cn";
                    string outlineFontname = isJp ? "DFMincho-UB" : "汉仪粗宋繁";
                    int outlineEncoding = isJp ? 128 : 134;
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

                    double t0 = ev.Start - 0.7 + (x - x0) / PlayResX * 3.0;
                    double t1 = t0 + 0.5;
                    double t2 = (ke.IsSplit ? ke.KStart_NoSplit : kStart) - 0.05;                    
                    double t25 = t2 + ke.KValue * 0.01;
                    double t21 = 0, t24 = 0;
                    {
                        t21 = (t25 - t2) * 0.3 + t2;
                        if (t21 - t2 > 0.1) t21 = t2 + 0.1;
                        t24 = t21;
                    }
                    double t3 = ev.End - 0.7 + (x - x0) / PlayResX * 3.0;
                    if (t25 > t3) t25 = t3;
                    if (iK == kelems.Count - 1) t25 = t3 + 0.25;
                    double t4 = t3 + 0.5;

                    ass_out.AppendEvent(50, evStyle, t0, t2,
                        pos(x, y) + fad(0.5, 0) + a(1, "33") + c(1, "2425FF") +
                        ke.KText);
                    ass_out.AppendEvent(49, evStyle, t0, t2,
                        pos(x + 1.5, y + 1.5) + fad(0.5, 0) + a(1, "33") + c(1, "000000") + blur(1) +
                        ke.KText);
                    ass_out.AppendEvent(47, evStyle, t0, t2,
                        pos(x, y) + fad(0.5, 0) + a(3, "44") + c(3, "000052") + bord(2.5) + blur(3) +
                        ke.KText);

                    ass_out.AppendEvent(51, evStyle, t2, t21,
                        pos(x, y) + t(fsc(130, 130).t()) + a(1, "00") + c(1, "FFFFFF") + blur(0.8) +
                        ke.KText);
                    ass_out.AppendEvent(51, evStyle, t21, t24,
                        pos(x, y) + fsc(130, 130) + a(1, "00") + c(1, "FFFFFF") + blur(0.8) +
                        ke.KText);
                    ass_out.AppendEvent(51, evStyle, t24, t25,
                        pos(x, y) + fsc(130, 130) + t(fsc(100, 100).t()) + a(1, "00") + c(1, "FFFFFF") + blur(0.8) +
                        ke.KText);
                    ass_out.AppendEvent(51, evStyle, t2, t21,
                        pos(x, y) + t(fsc(130, 130).t()) + a(3, "22") + c(3, "0000FF") + blur(3) + bord(3) +
                        ke.KText);
                    ass_out.AppendEvent(51, evStyle, t21, t24,
                        pos(x, y) + fsc(130, 130) + a(3, "22") + c(3, "0000FF") + blur(3) + bord(3) +
                        ke.KText);
                    ass_out.AppendEvent(51, evStyle, t24, t25,
                        pos(x, y) + fsc(130, 130) + t(fsc(100, 100).t()) + a(3, "22") + c(3, "0000FF") + blur(3) + bord(3) +
                        ke.KText);

                    ass_out.AppendEvent(50, evStyle, t25, t4,
                        pos(x, y) + fad(0, 0.5) + a(1, "33") + c(1, "2425FF") +
                        ke.KText);
                    ass_out.AppendEvent(49, evStyle, t25, t4,
                        pos(x + 1.5, y + 1.5) + fad(0, 0.5) + a(1, "33") + c(1, "000000") + blur(1) +
                        ke.KText);
                    ass_out.AppendEvent(47, evStyle, t25, t4,
                        pos(x, y) + fad(0, 0.5) + a(3, "44") + c(3, "000052") + bord(2.5) + blur(3) +
                        ke.KText);
                }
            }

            Console.WriteLine(ass_out.Events.Count);
            ass_out.SaveFile(this.OutFileName);
        }
    }
}
