using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.Effect;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class Maria4_OP : BaseAnime2
    {
        public Maria4_OP()
        {
            this.InFileName = @"G:\Workshop\maria4\op_org.ass";
            this.OutFileName = @"G:\Workshop\maria4\op.ass";

            this.FontWidth = 26;
            this.FontHeight = 26;
            this.FontSpace = 1;

            this.PlayResX = 640;
            this.PlayResY = 360;
            this.MarginBottom = 10;
            this.MarginLeft = 10;
            this.MarginRight = 10;
            this.MarginTop = 10;
        }

        public override Size GetSize(string s)
        {
            return new Size { Height = this.FontHeight, Width = this.FontWidth };
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS();

            ass_out.Header = ass_in.Header;
            ass_out.Events = new List<ASSEvent>();

            this.Font = new System.Drawing.Font("ＦＡ 瑞筆行書Ｍ", 26, GraphicsUnit.Pixel);
            this.MaskStyle = "Style: Default,ＦＡ 瑞筆行書Ｍ,26,&H00FFCEE7,&HFF0000FF,&H00FF2D49,&HFF0A5A84,-1,0,0,0,100,100,1,0,1,2,0,5,20,20,10,128";

            for (int i = 0; i < 21; i++)
            {
                ASSEvent ev = ass_in.Events[i];
                List<KElement> kelems = ev.SplitK(true);
                if (i >= 11)
                {
                    this.Font = new System.Drawing.Font("華康行書體(P)", 26, GraphicsUnit.Pixel);
                    this.MaskStyle = "Style: Default,華康行書體(P),26,&H00FFCEE7,&HFF0000FF,&H00FF2D49,&HFF0A5A84,-1,0,0,0,100,100,1,0,1,2,0,5,20,20,10,136";
                }
                int sumw = GetTotalWidth(ev);
                int x0 = (PlayResX - MarginLeft - MarginRight - sumw) / 2 + MarginLeft;
                int kSum = 0;
                for (int ik = 0; ik < kelems.Count; ik++)
                {
                    Console.WriteLine("{0} / {1} : {2} / {3}", i + 1, ass_in.Events.Count, ik + 1, kelems.Count);
                    KElement elem = kelems[ik];
                    Size sz = this.GetSize(elem.KText);
                    int x = x0;
                    x0 += sz.Width + this.FontSpace;
                    int y = PlayResY - MarginBottom;
                    if (i == 5) y -= FontHeight + 10;
                    if (i >= 11) y = MarginTop + FontHeight;
                    double kStart = (double)kSum * 0.01;
                    double kEnd = (double)(kSum + elem.KValue) * 0.01;
                    kEnd = kStart + 0.6;
                    double kMid = (kStart + kEnd) * 0.5;
                    double kQ1 = kStart + (kEnd - kStart) * 0.1;

                    double r = (double)ik / (double)(kelems.Count - 1);
                    double r0 = 1.0 - r;

                    int fd_xof = (int)((double)(ik - (kelems.Count - 1) / 2) / (double)(kelems.Count - 1) * (double)PlayResX * 0.2);

                    // an7 -> an5
                    x += sz.Width / 2;
                    y -= FontHeight / 2;

                    // 0.5秒内出现
                    ass_out.Events.Add(ev.StartReplace(ev.Start - r0 * 0.5).EndReplace(ev.Start - r0 * 0.5 + 0.3).TextReplace(
                        ASSEffect.move(x + fd_xof, y, x, y) + ASSEffect.an(5) + ASSEffect.be(1) + ASSEffect.a(1, "FF") + ASSEffect.a(3, "FF") + ASSEffect.fsc(400, 400) +
                        ASSEffect.t(0, 0.3, ASSEffect.fsc(100, 100).t() + ASSEffect.a(1, "00").t() + ASSEffect.a(3, "00").t() + ASSEffect.fry(-360).t()) +
                        elem.KText));

                    double newStart = ev.Start;
                    if (ev.Start - r0 * 0.5 + 0.3 > ev.Start)
                        newStart = ev.Start - r0 * 0.5 + 0.3;
                    else
                        ass_out.Events.Add(ev.StartReplace(ev.Start - r0 * 0.5 + 0.3).EndReplace(ev.Start).TextReplace(
                            ASSEffect.pos(x, y) + ASSEffect.an(5) + ASSEffect.be(1) + elem.KText));

                    //ass_out.Events.Add(ev.TextReplace(
                      //  ASSEffect.pos(x, y) + ASSEffect.an(5) + elem.KText));
                    if (i < 11)
                    {
                        ass_out.Events.Add(ev.StartReplace(newStart).EndReplace(ev.Start + kStart).TextReplace(
                            ASSEffect.pos(x, y) + ASSEffect.an(5) + ASSEffect.be(1) + elem.KText));
                        ass_out.Events.Add(ev.StartReplace(ev.Start + kStart).EndReplace(ev.Start + kEnd).TextReplace(
                            ASSEffect.pos(x, y) + ASSEffect.an(5) + ASSEffect.be(1) +
                            ASSEffect.t(0, kQ1 - kStart, ASSEffect.c(1, "FFFFFF").t() + ASSEffect.c(1, "FFFFFF").t() + ASSEffect.fsc(200, 200).t()) +
                            ASSEffect.t(kQ1 - kStart, kEnd - kStart, ASSEffect.c(3, "A4CEE7").t() + ASSEffect.c(1, "072D49").t() + ASSEffect.fsc(100, 100).t()) +
                            elem.KText));
                        ass_out.Events.Add(ev.StartReplace(ev.Start + kEnd).EndReplace(ev.End).TextReplace(
                            ASSEffect.pos(x, y) + ASSEffect.an(5) + ASSEffect.be(1) + ASSEffect.c(1, "072D49") + ASSEffect.c(3, "A4CEE7") + elem.KText));
                    }
                    else
                    {
                        ass_out.Events.Add(ev.StartReplace(newStart).TextReplace(
                          ASSEffect.pos(x, y) + ASSEffect.an(5) + ASSEffect.be(1) + elem.KText));
                    }

                    // 0.5秒内消失
                    if (i < 11)
                    {
                        ass_out.Events.Add(ev.StartReplace(ev.End + r * 0.5).EndReplace(ev.End + r * 0.5 + 0.3).TextReplace(
                            ASSEffect.move(x, y, x + fd_xof, y) + ASSEffect.be(1) + ASSEffect.an(5) + ASSEffect.c(1, "072D49") + ASSEffect.c(3, "A4CEE7") +
                            ASSEffect.t(0, 0.3, ASSEffect.fsc(400, 400).t() + ASSEffect.a(1, "FF").t() + ASSEffect.a(3, "FF").t() + ASSEffect.fry(-360).t()) +
                            elem.KText));
                        ass_out.Events.Add(ev.StartReplace(ev.End).EndReplace(ev.End + r * 0.5).TextReplace(
                            ASSEffect.pos(x, y) + ASSEffect.an(5) + ASSEffect.be(1) + ASSEffect.c(1, "072D49") + ASSEffect.c(3, "A4CEE7") + elem.KText));
                    }
                    else
                    {
                        ass_out.Events.Add(ev.StartReplace(ev.End + r * 0.5).EndReplace(ev.End + r * 0.5 + 0.3).TextReplace(
                            ASSEffect.move(x, y, x + fd_xof, y) + ASSEffect.be(1) + ASSEffect.an(5) + 
                            ASSEffect.t(0, 0.3, ASSEffect.fsc(400, 400).t() + ASSEffect.a(1, "FF").t() + ASSEffect.a(3, "FF").t() + ASSEffect.fry(-360).t()) +
                            elem.KText));
                        ass_out.Events.Add(ev.StartReplace(ev.End).EndReplace(ev.End + r * 0.5).TextReplace(
                            ASSEffect.pos(x, y) + ASSEffect.an(5) + ASSEffect.be(1) + elem.KText));
                    }

                    kSum += elem.KValue;
                }
            }

            ass_out.SaveFile(OutFileName);
        }
    }
}
