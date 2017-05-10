using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.Effect;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class Akane_OP : BaseAnime
    {
        public Akane_OP()
        {
            this.InFileName = @"G:\Workshop\akane\op\op_org.ass";
            this.OutFileName = @"G:\Workshop\akane\op\op.ass";

            this.FontWidth = 21;
            this.FontSpace = 1;
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS();

            ass_out.Header = ass_in.Header;
            ass_out.Events = new List<ASSEvent>();

            ass_out.Events.Add(ass_in.Events[0]);

            this.Font = new System.Drawing.Font("DFGRuLeiA-W5", 13);
            Particle pt = new Particle
            {
                AreaHeight = 50,
                AreaWidth = 100,
                IsRandomColor = false,
                Color = new ASSColor { A = 0, R = 255, G = 255, B = 255, Index = 1 },
                Count = 10,
                FontSize = this.FontHeight,
                MinLast = 1,
                MaxLast = 2,
                IsMove = true,
                IsPatternScale = true,
                PatternScaleX = 250,
                PatternScaleY = 250,
                ParticlePattern = ParticlePatternType.Circle,
                Style = "particle",
                XOffset = 0,
                YOffset = 0,
                IsRotate = false,
            };
            for (int i = 1; i <= 15; i++)
            {
                ASSEvent ev = ass_in.Events[i];
                List<KElement> kelems = ev.SplitK(false);
                int sumw = GetTotalWidth(ev);
                int x0 = (PlayResX - MarginLeft - MarginRight - sumw) / 2 + MarginLeft;
                int kSum = 0;
                for (int ik = 0; ik < kelems.Count; ik++)
                {
                    KElement elem = kelems[ik];
                    Size sz = this.GetSize(elem.KText);
                    int x = x0;
                    x0 += sz.Width + this.FontSpace;
                    int y = MarginTop;
                    string color = Common.scaleColor("7A93EB", "6939C8", (double)ik / (double)(kelems.Count - 1));
                    double kStart = (double)kSum * 0.01;
                    double kEnd = (double)(kSum + elem.KValue) * 0.01;
                    double kMid = (kStart + kEnd) * 0.5;
                    double kQ1 = kStart + (kEnd - kStart) * 0.25;

                    // an7 -> an5
                    int x5 = x + sz.Width / 2;
                    int y5 = y + sz.Height / 2;
                    
                    ass_out.Events.Add(ev.TextReplace(ASSEffect.pos(x5, y5) + ASSEffect.c(1, color) +
                        ASSEffect.t(0, 0.001, ASSEffect.c(1, "555555").t()) +
                        ASSEffect.t(kStart, kQ1, ASSEffect.c(1, "FFFFFF").t() + ASSEffect.fsc(170, 170).t()) +
                        ASSEffect.t(kQ1, kEnd, ASSEffect.c(1, color).t() + ASSEffect.fsc(100, 100).t()) +
                        elem.KText));

                    pt.X = x;
                    pt.Y = y;
                    pt.Start = ev.Start + kStart;
                    pt.End = ev.Start + kEnd;

                    ass_out.Events.AddRange(pt.Create());

                    kSum += elem.KValue;
                }
            }

            for (int i = 16; i <= 30; i++)
            {
                ass_out.Events.Add(ass_in.Events[i]);
            }

            ass_out.SaveFile(this.OutFileName);
        }
    }
}
