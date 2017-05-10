using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MeteorX.AssTools.KaraokeApp.Anime.Test
{
    class TestParticle1 : BaseAnime
    {
        public TestParticle1()
        {
            InFileName = @"G:\Workshop\test\5\0.ass";
            OutFileName = @"G:\Workshop\test\5\1.ass";

            this.FontWidth = 30;
            this.FontHeight = 30;
            this.FontSpace = 1;

            this.PlayResX = 848;
            this.PlayResY = 480;
            this.MarginBottom = 10;
            this.MarginLeft = 15;
            this.MarginRight = 15;
            this.MarginTop = 15;

            this.Font = new System.Drawing.Font("ＤＦＰまるもじ体W3", 30, GraphicsUnit.Pixel);
        }

        class ParticleDot
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double dX { get; set; }
            public double dY { get; set; }
            public double R { get; set; }
            public double G { get; set; }
            public double B { get; set; }
            public double A { get; set; }
            public double dA { get; set; }
            public bool Die { get { return A <= 0; } }

            public ParticleDot(double x, double y, double dx, double dy, double da)
            {
                R = 1;
                G = 1;
                B = 1;
                A = 1;
                X = x;
                Y = y;
                dX = dx;
                dY = dy;
                dA = da;
            }
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS();

            ass_out.Header = ass_in.Header;
            ass_out.Events = new List<ASSEvent>();

            string pt0Str = @"{\blur3\bord4\p4}m 10 10 s 10 -10 -10 -10 -10 10";
            string pt1Str = @"{\p4}m 0 100 l 1 1 100 0 1 -1 0 -100 -1 -1 -100 0 -1 1";

            List<ParticleDot> dots = new List<ParticleDot>();
            Random rnd = new Random();

            double totalTime = 20;
            double timeStep = 0.04;
            double particleStopTime = 10;
            int particlePerStep = 5;
            double orgX = 0;
            double dOrgX = this.PlayResX / particleStopTime;
            double orgY = 0;
            //double dOrgY = this.PlayResY / totalTime;
            for (double time = 0; time < totalTime; time += timeStep)
            {
                Console.WriteLine(time);

                // Draw Dots
                dots = dots.Where(pd => !pd.Die).ToList();
                foreach (ParticleDot dot in dots)
                {
                    dot.A += dot.dA * timeStep;
                    dot.R = dot.R;
                    dot.G = 0.2 * dot.A;
                    dot.B = 0.8 * dot.A;
                    dot.X += dot.dX * timeStep;
                    dot.Y += dot.dY * timeStep;

                    double a0 = (1 - dot.A) * 256;
                    if (a0 < 0) a0 = 0;
                    if (a0 > 255) a0 = 255;
                    string aStr = Common.ToHex2(a0);
                    string cStr = ASSColor.ToBBGGRR(dot.R * 256, dot.G * 256, dot.B * 256);

                    ass_out.Events.Add(new ASSEvent
                    {
                        Effect = "",
                        Layer = 0,
                        MarginL = "0000",
                        MarginR = "0000",
                        MarginV = "0000",
                        Name = "NTP",
                        Style = "Default",
                        Start = time,
                        End = time + timeStep,
                        Text = ASSEffect.pos(dot.X, dot.Y) + ASSEffect.a(1, aStr) + ASSEffect.a(3, aStr) + ASSEffect.c(3, cStr) + pt0Str
                    });
                    ass_out.Events.Add(new ASSEvent
                    {
                        Effect = "",
                        Layer = 0,
                        MarginL = "0000",
                        MarginR = "0000",
                        MarginV = "0000",
                        Name = "NTP",
                        Style = "Default",
                        Start = time,
                        End = time + timeStep,
                        Text = ASSEffect.pos(dot.X, dot.Y - 1) + ASSEffect.a(1, aStr) + ASSEffect.a(3, aStr) + ASSEffect.c(1, cStr) + pt1Str
                    });
                }
                orgX += dOrgX * timeStep;
                //orgY += dOrgY * timeStep;
                orgY = this.PlayResY * 0.5 + this.PlayResY * 0.3 * Math.Sin(time * 2);

                if (particleStopTime < time) continue;
                for (int iDot = 0; iDot < particlePerStep; iDot++)
                    dots.Add(new ParticleDot(orgX, orgY, Common.RandomDouble(rnd, -30, 30), Common.RandomDouble(rnd, -30, 30), -Common.RandomDouble(rnd, 0.5, 1)));
            }

            ass_out.SaveFile(OutFileName);
        }
    }
}
