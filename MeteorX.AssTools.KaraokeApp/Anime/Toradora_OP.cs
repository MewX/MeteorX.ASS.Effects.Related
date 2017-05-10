using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeteorX.AssTools.KaraokeApp.Effect;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class Toradora_OP
    {
        public string InFileName = @"G:\Workshop\toradora\op_org.ass";
        public string OutFileName = @"G:\Workshop\toradora\op.ass";
        public double Shift = 0;
        public int FontWidth = 26;
        public int FontHeight = 26;
        public int PlayResX = 704;
        public int PlayResY = 396;
        public int MarginBottom = 10;
        public int MarginTop = 10;

        Random rnd = new Random();

        public void Run()
        {
            ASS ass = ASS.FromFile(InFileName);
            ASS outass = new ASS();
            outass.Header = ass.Header;
            outass.Events = new List<ASSEvent>();
            Particle particle = new Particle
            {
                IsMove = false,
                IsRandomColor = false,
                IsPatternScale = true,
                Count = 1,
                FontSize = 26,
                MinLast = 0.5,
                MaxLast = 0.8,
                ParticlePattern = ParticlePatternType.Circle,
                PatternScaleX = 600,
                PatternScaleY = 600,
                Style = "Circle",
                XOffset = 0,
                YOffset = 0
            };
            ASSColor[] colors =
            {
                new ASSColor { A = 0, Index = 1, R = 0xCA, G = 0xB4, B = 0x6D },
                new ASSColor { A = 0, Index = 1, R = 0xEE, G = 0x85, B = 0x57 },
                new ASSColor { A = 0, Index = 1, R = 0xE4, G = 0x30, B = 0x99 },
                new ASSColor { A = 0, Index = 1, R = 0x5D, G = 0xD3, B = 0xEE },
                new ASSColor { A = 0, Index = 1, R = 0xE1, G = 0x32, B = 0x63 }
            };
            for (int i = 0; i < ass.Events.Count; i++)
            {
                ASSEvent ev = ass.Events[i];
                List<KElement> klist = ev.SplitK();
                int ksum = 0;
                ASSColor lastcc = null;
                for (int j = 0; j < klist.Count; j++)
                {
                    int x = (PlayResX - klist.Count * FontWidth) / 2 + j * FontWidth + FontWidth / 2;
                    int y = PlayResY - MarginBottom - FontHeight / 2;
                    if (i == 7) y = MarginTop + FontHeight / 2;
                    double kStart = ksum * 0.01;
                    double kEnd = (ksum + klist[j].KValue) * 0.01;
                    ASSColor cc = colors[(i + j) % colors.Length];
                    /*while (lastcc == cc)
                        cc = colors[rnd.Next(colors.Length)];
                    lastcc = cc;*/
                    if (!Char.IsWhiteSpace(klist[j].KText[0]))
                    {
                        outass.Events.Add(ev.TextReplace(
                            ASSEffect.an(5) + ASSEffect.pos(x, y) + ASSEffect.a(1, "FF") +
                            ASSEffect.t(kStart, kStart + 0.2, cc.ToString()) +
                            (i % 4 <= 1 ?
                            ASSEffect.t(ev.Last - 0.3 - 0.5 * (1 - ((double)j / (double)klist.Count)), ev.Last - 0.5 * (1 - ((double)j / (double)klist.Count)), @"\1a&HFF&\2a&HFF&\3a&HFF&\4a&HFF&" + ((i % 2 == 0) ? @"\frx700" : @"\fry700")) :
                            (i % 4 == 3 ? ASSEffect.t(ev.Last - 0.5 * (1 - ((double)j / (double)klist.Count)), ev.Last - 0.5 * (1 - ((double)j / (double)klist.Count)) + 0.01, @"\1a&HFF&\2a&HFF&\3a&HFF&\4a&HFF&") : ASSEffect.t(ev.Last - 0.8 - 0.5 * (1 - ((double)j / (double)klist.Count)), ev.Last - 0.5 * (1 - ((double)j / (double)klist.Count)), @"\1a&HFF&\2a&HFF&\3a&HFF&\4a&HFF&" + @"\frz700"))) +
                            klist[j].KText
                            ));
                        if (i % 4 == 3)
                            outass.Events.Add(ev.StartReplace(ev.Start + ev.Last - 0.5 * (1 - ((double)j / (double)klist.Count)) + 0.01).EndReplace(ev.Start + ev.Last - 0.5 * (1 - ((double)j / (double)klist.Count)) + 0.01 + 0.2).TextReplace(
                                ASSEffect.an(5) + ASSEffect.move(x, y, x - 40, y) + ASSEffect.fad(0, 0.2) + ASSEffect.c(cc) + klist[j].KText));
                        particle.X = x - FontWidth / 2;
                        particle.Y = y - FontHeight / 2;
                        particle.Start = ev.Start + kStart;
                        particle.End = particle.Start;
                        particle.Color = cc;
                        outass.Events.AddRange(particle.Create());
                    }
                    ksum += klist[j].KValue;
                }
            }
            outass.Shift(Shift);
            outass.SaveFile(OutFileName);
        }
    }
}
