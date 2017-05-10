using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.Effect;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class Maria4_ED : BaseAnime
    {
        public Maria4_ED()
        {
            this.InFileName = @"G:\Workshop\maria4\ed_org.ass";
            this.OutFileName = @"G:\Workshop\maria4\ed.ass";

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

            this.Font = new System.Drawing.Font("ＦＡ 瑞筆行書Ｍ", 13);

            Particle pt = new Particle
            {
                AreaHeight = 80,
                AreaWidth = 200,
                IsRandomColor = true,
                Color = new ASSColor { A = 0, R = 255, G = 255, B = 255, Index = 1 },
                Color1 = new ASSColor { A = 0, R = 255, G = 255, B = 255, Index = 1 },
                Color2 = new ASSColor { A = 0, R = 0xEF, G = 0xF7, B = 0xFB, Index = 1 },
                Count = 100,
                FontSize = this.FontHeight,
                MinLast = 1,
                MaxLast = 2,
                IsMove = true,
                MoveStyle = 2,
                IsPatternScale = false,
                PatternScaleX = 250,
                PatternScaleY = 250,
                ParticlePattern = ParticlePatternType.o,
                Style = "pt",
                XOffset = 0,
                YOffset = 0,
                IsRotate = true,
                BE = 1
            };

            for (int i = 0; i < 20; i++)
            {
                if (i >= 10) this.Font = new System.Drawing.Font("華康行書體(P)", 13);
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
                    int y = PlayResY - MarginBottom;
                    if (i >= 10) y = MarginTop + FontHeight;
                    double kStart = (double)kSum * 0.01;
                    double kEnd = (double)(kSum + elem.KValue) * 0.01;
                    kEnd = kStart + 0.6;
                    double kMid = (kStart + kEnd) * 0.5;
                    double kQ1 = kStart + (kEnd - kStart) * 0.1;

                    double r = (double)ik / (double)(kelems.Count - 1);
                    double r0 = 1.0 - r;

                    int fd_xof = (int)((double)(ik - (kelems.Count - 1) / 2) / (double)(kelems.Count - 1) * (double)PlayResX * 0.2);

                    // particle need an7 position
                    pt.X = x;
                    pt.Y = y - FontHeight;
                    pt.Start = ev.Start + kStart;
                    pt.End = ev.Start + kEnd;

                    // an7 -> an5
                    x += FontWidth / 2;
                    y -= FontHeight / 2;

                    // 提前1秒出现
                    ass_out.Events.Add(ev.StartReplace(ev.Start - r0 * 1.0).TextReplace(
                      ASSEffect.fad(0.3, 0) + ASSEffect.be(1) + 
                      ASSEffect.pos(x, y) + ASSEffect.an(5) +
                      ASSEffect.t(kStart + r0 * 1.0, kEnd + r0 * 1.0, ASSEffect.be(10).t() + ASSEffect.a(1, "FF").t()) +
                      ASSEffect.t(kStart + r0 * 1.0 + 0.2, kEnd + r0 * 1.0, ASSEffect.a(3, "FF").t()) +
                      elem.KText));

                    ass_out.Events.AddRange(pt.Create());

                    kSum += elem.KValue;
                }
            }

            ass_out.SaveFile(OutFileName);
        }
    }
}
