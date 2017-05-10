using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MeteorX.AssTools.KaraokeApp.Effect
{
    class Particle2
    {
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

        static Random rnd = new Random();

        public string ColorStart { get; set; }
        public string ColorEnd { get; set; }

        public double ParticleStartTime { get; set; }
        public double ParticleEndTime { get; set; }

        public double TimeStep { get; set; }
        public int ParticlePerStep { get; set; }

        public double MinDX { get; set; }
        public double MaxDX { get; set; }
        public double MinDY { get; set; }
        public double MaxDY { get; set; }
        public double MinDA { get; set; }
        public double MaxDA { get; set; }

        public bool Star { get; set; }
        public int Pt0Size { get; set; }

        public Particle2(string col0, string col1, double pst, double pet, double step, int pps, double dx0, double dx1, double dy0, double dy1, double da0, double da1)
        {
            this.ColorStart = col0;
            this.ColorEnd = col1;
            this.ParticleStartTime = pst;
            this.ParticleEndTime = pet;
            this.TimeStep = step;
            this.ParticlePerStep = pps;
            this.MinDX = dx0;
            this.MaxDX = dx1;
            this.MinDY = dy0;
            this.MaxDY = dy1;
            this.MinDA = da0;
            this.MaxDA = da1;

            this.Star = true;
            this.Pt0Size = 4;
        }

        public List<ASSEvent> Create(IMovingObject imo)
        {
            List<ASSEvent> result = new List<ASSEvent>();
            //string pt0Str = @"{\blur3\bord4\p4}m 10 10 s 10 -10 -10 -10 -10 10";
            string pt0Str = @"{\blur" + (Pt0Size - 1) + @"\bord" + Pt0Size + @"\p4}m 10 10 s 10 -10 -10 -10 -10 10";
            string pt1Str = @"{\p4}m 0 100 l 1 1 100 0 1 -1 0 -100 -1 -1 -100 0 -1 1";
            for (double time = ParticleStartTime; time < ParticleEndTime; time += TimeStep)
            {
                for (int iDot = 0; iDot < ParticlePerStep; iDot++)
                {
                    ASSPointF orgP = imo.GetPosition(time);
                    double orgX = orgP.X;
                    double orgY = orgP.Y;
                    ParticleDot dot = new ParticleDot(orgX, orgY, Common.RandomDouble(rnd, MinDX, MaxDX), Common.RandomDouble(rnd, MinDY, MaxDY), -Common.RandomDouble(rnd, MinDA, MaxDA));
                    double liveTime = -1.0 / dot.dA;
                    double startTime = time;
                    double endTime = time + liveTime;
                    double xEnd = dot.X + dot.dX * liveTime;
                    double yEnd = dot.Y + dot.dY * liveTime;
                    result.Add(new ASSEvent
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
                        Text = ASSEffect.move(dot.X, dot.Y, xEnd, yEnd) + ASSEffect.a(1, "FF") + ASSEffect.c(3, ColorStart) + ASSEffect.fad(0, liveTime) + ASSEffect.t(0, liveTime, ASSEffect.c(3, ColorEnd).t()) + pt0Str
                    });
                    if (Star)
                        result.Add(new ASSEvent
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
                            Text = ASSEffect.move(dot.X, dot.Y - 1, xEnd, yEnd - 1) + ASSEffect.a(1, "77") + ASSEffect.c(1, ColorStart) + ASSEffect.fad(0, liveTime) + ASSEffect.t(0, liveTime, ASSEffect.c(1, ColorEnd).t()) + pt1Str
                        });
                }
            }
            return result;
        }
    }

    interface IMovingObject
    {
        ASSPointF GetPosition(double time);
    }

    class MovingLinear : IMovingObject
    {
        static Random rnd = new Random();

        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public double X0 { get; set; }
        public double Y0 { get; set; }
        public double X1 { get; set; }
        public double Y1 { get; set; }

        public double MinDX { get; set; }
        public double MaxDX { get; set; }
        public double MinDY { get; set; }
        public double MaxDY { get; set; }

        public MovingLinear(double start, double end, double x0, double y0, double x1, double y1)
        {
            this.StartTime = start;
            this.EndTime = end;
            this.X0 = x0;
            this.Y0 = y0;
            this.X1 = x1;
            this.Y1 = y1;

            this.MinDX = this.MaxDX = this.MinDY = this.MaxDY = 0;
        }

        public ASSPointF GetPosition(double time)
        {
            double r = (time - StartTime) / (EndTime - StartTime);
            ASSPointF p = new ASSPointF { X = (X1 - X0) * r + X0, Y = (Y1 - Y0) * r + Y0 };
            if (MinDX < MaxDX) p.X += Common.RandomDouble(rnd, MinDX, MaxDX);
            if (MinDY < MaxDY) p.Y += Common.RandomDouble(rnd, MinDY, MaxDY);
            return p;
        }
    }

    class MovingArc : IMovingObject
    {
        static Random rnd = new Random();
        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public double X0 { get; set; }
        public double Y0 { get; set; }
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double R { get; set; }
        public double MinT { get; set; }
        public double MaxT { get; set; }
        public int GaussRnd { get; set; }

        public MovingArc(double start, double end, double x0, double y0, double x1, double y1, double r, double t0, double t1)
        {
            this.StartTime = start;
            this.EndTime = end;
            this.X0 = x0;
            this.Y0 = y0;
            this.X1 = x1;
            this.Y1 = y1;
            this.R = r;
            this.MinT = t0;
            this.MaxT = t1;

            this.GaussRnd = 1;
        }

        public ASSPointF GetPosition(double time)
        {
            double ra = (time - StartTime) / (EndTime - StartTime);
            ASSPointF org = new ASSPointF { X = (X1 - X0) * ra + X0, Y = (Y1 - Y0) * ra + Y0 };
            double t = Common.RandomDouble_Gauss(rnd, MinT, MaxT, GaussRnd);
            ASSPointF p = new ASSPointF { X = org.X + R * Math.Cos(t), Y = org.Y + R * Math.Sin(t) };
            return p;
        }
    }

    class MovingRound : IMovingObject
    {
        static Random rnd = new Random();
        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public double X0 { get; set; }
        public double Y0 { get; set; }
        public double R { get; set; }
        public double StartT { get; set; }

        public double MinDX { get; set; }
        public double MaxDX { get; set; }
        public double MinDY { get; set; }
        public double MaxDY { get; set; }

        public MovingRound(double start, double end, double x0, double y0, double r, double t0)
        {
            this.StartTime = start;
            this.EndTime = end;
            this.X0 = x0;
            this.Y0 = y0;
            this.R = r;
            this.StartT = t0;

            this.MinDX = this.MaxDX = this.MinDY = this.MaxDY = 0;
        }

        public ASSPointF GetPosition(double time)
        {
            double ra = (time - StartTime) / (EndTime - StartTime);
            double t = StartT + ra * 2.0 * Math.PI;
            ASSPointF p = new ASSPointF { X = X0 + R * Math.Cos(t), Y = Y0 + R * Math.Sin(t) };
            if (MinDX < MaxDX) p.X += Common.RandomDouble(rnd, MinDX, MaxDX);
            if (MinDY < MaxDY) p.Y += Common.RandomDouble(rnd, MinDY, MaxDY);
            return p;
        }
    }

    class MovingSinH : IMovingObject
    {
        static Random rnd = new Random();
        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public double X0 { get; set; }
        public double X1 { get; set; }
        public double Y0 { get; set; }
        public double H { get; set; }
        public double K { get; set; }

        public double MinDX { get; set; }
        public double MaxDX { get; set; }
        public double MinDY { get; set; }
        public double MaxDY { get; set; }

        public MovingSinH(double start, double end, double x0, double x1, double y0, double h, double k)
        {
            this.StartTime = start;
            this.EndTime = end;
            this.X0 = x0;
            this.X1 = x1;
            this.Y0 = y0;
            this.H = h;
            this.K = k;

            this.MinDX = this.MaxDX = this.MinDY = this.MaxDY = 0;
        }

        public ASSPointF GetPosition(double time)
        {
            double ra = (time - StartTime) / (EndTime - StartTime);
            double x = (X1 - X0) * ra + X0;
            ASSPointF p = new ASSPointF { X = x, Y = Y0 + H * Math.Sin(K * x) };
            if (MinDX < MaxDX) p.X += Common.RandomDouble(rnd, MinDX, MaxDX);
            if (MinDY < MaxDY) p.Y += Common.RandomDouble(rnd, MinDY, MaxDY);
            return p;
        }
    }

}
