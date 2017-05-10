using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MeteorX.AssTools.KaraokeApp.Anime.Test
{
    class TestParticle2 : BaseAnime
    {
        public TestParticle2()
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

            Random rnd = new Random();

            double totalTime = 20;
            double timeStep = 0.01;
            double particleStopTime = 10;
            int particlePerStep = 5;
            double orgX = 0;
            double orgY = 0;
            //double dOrgY = this.PlayResY / totalTime;
            for (double time = 0; time < totalTime; time += timeStep)
            {
                Console.WriteLine(time);
                if (particleStopTime < time) continue;
                double r = time / particleStopTime;
                double a = time * 3;
                orgX = this.PlayResX * 0.5 + this.PlayResX * 0.55 * Math.Cos(a) * r;
                orgY = this.PlayResY * 0.5 + this.PlayResY * 0.55 * Math.Sin(a) * r;
                for (int iDot = 0; iDot < particlePerStep; iDot++)
                {
                    ParticleDot dot = new ParticleDot(orgX, orgY, Common.RandomDouble(rnd, -30, 30), Common.RandomDouble(rnd, -30, 30), -Common.RandomDouble(rnd, 0.5, 2));
                    double liveTime = -1.0 / dot.dA;
                    double startTime = time;
                    double endTime = time + liveTime;
                    double xEnd = dot.X + dot.dX * liveTime;
                    double yEnd = dot.Y + dot.dY * liveTime;
                    double r0 = 0.8;
                    double g0 = 0.2;
                    double b0 = 1;
                    double r1 = 0;
                    double g1 = 0;
                    double b1 = 1;
                    string colStart = ASSColor.ToBBGGRR(b0 * 256, g0 * 256, r0 * 256);
                    string colEnd = ASSColor.ToBBGGRR(b1 * 256, g1 * 256, r1 * 256);
                    ass_out.Events.Add(new ASSEvent
                    {
                        Effect = "",
                        Layer = 0,
                        MarginL = "0000",
                        MarginR = "0000",
                        MarginV = "0000",
                        Name = "NTP",
                        Style = "Default",
                        Start = startTime,
                        End = endTime,
                        Text = ASSEffect.move(dot.X, dot.Y, xEnd, yEnd) + ASSEffect.c(3, colStart) + ASSEffect.fad(0, liveTime) + ASSEffect.t(0, liveTime, ASSEffect.c(3, colEnd).t()) + pt0Str
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
                        Start = startTime,
                        End = endTime,
                        Text = ASSEffect.move(dot.X, dot.Y - 1, xEnd, yEnd - 1) + ASSEffect.a(1, "77") + ASSEffect.c(1, colStart) + ASSEffect.fad(0, liveTime) + ASSEffect.t(0, liveTime, ASSEffect.c(1, colEnd).t()) + pt1Str
                    });
                }
            }

            ass_out.SaveFile(OutFileName);
        }
    }
}
