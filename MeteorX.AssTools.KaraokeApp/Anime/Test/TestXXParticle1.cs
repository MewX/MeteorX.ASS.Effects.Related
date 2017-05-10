using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.XXParticle;
using MeteorX.AssTools.KaraokeApp.Model;

namespace MeteorX.AssTools.KaraokeApp.Anime.Test
{
    class TestXXParticle1 : BaseAnime2, IXXEmitter, IXXForceField, IXXGravityPosition
    {
        public TestXXParticle1()
        {
            InFileName = @"G:\Workshop\test\7\0.ass";
            OutFileName = @"G:\Workshop\test\7\1.ass";

            this.FontWidth = 30;
            this.FontHeight = 30;
            this.FontSpace = 3;

            this.PlayResX = 848;
            this.PlayResY = 480;
            this.MarginBottom = 10;
            this.MarginLeft = 15;
            this.MarginRight = 15;
            this.MarginTop = 15;

            this.Font = new System.Drawing.Font("HGPSoeiKakugothicUB", 30, GraphicsUnit.Pixel);
            this.MaskStyle = "Style: Default,DFGMaruGothic-Md,30,&H00FF0000,&HFF600D00,&H000000FF,&HFF0A5A84,-1,0,0,0,100,100,0,0,0,2,0,5,20,20,20,128";
            this.IsAvsMask = true;
        }

        public override void Run()
        {
            string ptstr = @"{\p1}m 0 0 l 1 0 1 1 0 1";

            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS();

            ass_out.Header = ass_in.Header;
            ass_out.Events = new List<ASSEvent>();

            forceCurve = new CompositeCurve { MinT = 0, MaxT = 30 };
            double lastag = 0;
            for (double time = forceCurve.MinT; time <= forceCurve.MaxT; time += 1)
            {
                double ag = Common.RandomDouble(rnd, 0, Math.PI * 2);
                while (Math.Abs(ag - lastag) < Math.PI * 0.5 || Math.Abs(ag - lastag) > Math.PI * 1.5)
                    ag = Common.RandomDouble(rnd, 0, Math.PI * 2);
                lastag = ag;
                //ag = 0;
                double x = 1000.0 * Math.Cos(ag);
                double y = 600.0 * Math.Sin(ag);
                forceCurve.AddCurve(time, time + 1, new Line { X0 = x, Y0 = y, X1 = 0, Y1 = 0, Acc = 0.7 });
                //forceCurve.AddCurve(time, time + 1, new Line { X0 = 0, Y0 = 0, X1 = 0, Y1 = 0 });
            }

            NumberPerSecond = 100;
            XXParticleSystem xxps = new XXParticleSystem();
            xxps.Emitter = this;
            xxps.ForceField = this;
            xxps.StartTime = 0;
            xxps.EndTime = 3;
            xxps.InterpolationPrecision = 0.04;
            xxps.Resistance = 0.03;
            xxps.Repulsion = -2200;
            xxps.Gravity = 0;
            xxps.GravityPosition = this;
            List<KeyValuePair<XXParticleElement, List<ASSPointF>>> result = xxps.RenderPoint();
            foreach (KeyValuePair<XXParticleElement, List<ASSPointF>> pair in result)
            {
                string s = CreatePolygon(rnd, 5, 10, 6);
                foreach (ASSPointF pt in pair.Value)
                {
                    ass_out.AppendEvent(0, "pt", pt.T, pt.T + xxps.InterpolationPrecision,
                        pos(pt.X, pt.Y) + a(1, "22") + a(3, "77") + blur(2) + bord(1.5) + c(3, "532BFF") +
                        s);
                    continue;
                    ass_out.AppendEvent(0, "pt", pt.T, pt.T + xxps.InterpolationPrecision,
                        pos(pt.X, pt.Y) + a(1, "00") +
                        ptstr);
                    continue;
                    ASSPointF force = forceCurve.GetPointF(pt.T);
                    ass_out.AppendEvent(0, "pt", pt.T, pt.T + xxps.InterpolationPrecision,
                        pos(0, 0) + an(7) + a(1, "00") + fs(16) +
                        string.Format("{0}, {1}", force.X, force.Y));
                }
            }

            ass_out.SaveFile(OutFileName);
        }

        public XXParticleElement GenerateParticleElement(double time)
        {
            //return new XXParticleElement { Born = time, Life = 10, Position = new ASSPointF { X = 200, Y = 200 + time * 50 }, Speed = new ASSPointF { X = 200, Y = 0 } };
            double ag = Common.RandomDouble(rnd, 0, 1);
            if (Common.RandomBool(rnd, 0.5)) ag += Math.PI;
            double x = 100.0 * Math.Cos(ag);
            double y = 100.0 * Math.Sin(ag);
            double ox = 200 + (int)(time / 0.5) * 40;
            double oy = 200;
            double mass = 1;
            //if (Common.RandomBool(rnd, 0.05)) mass = 100;
            return new XXParticleElement
            {
                ForceTimeOffset = Common.RandomDouble(rnd, 0, 0.4),
                Spin = Common.RandomDouble(rnd, -5, 5) * 1.5,
                Mass = mass,
                Born = time,
                Life = 3,
                Position = new ASSPointF
                {
                    X = Common.RandomDouble(rnd, ox - 5, ox + 5),
                    Y = Common.RandomDouble(rnd, oy - 5, oy + 5)
                },
                Speed = new ASSPointF { X = x, Y = y }
            };
        }

        public double NumberPerSecond { get; set; }

        CompositeCurve forceCurve = null;

        public ASSPointF GetForceField(double time)
        {
            return forceCurve.GetPointF(time);
            double ag = time * 3;
            return new ASSPointF { X = 50.0 * Math.Cos(ag), Y = 50.0 * Math.Sin(ag) };
        }

        public ASSPointF GetGravityPosition(double time)
        {
            double ox = 200 + (int)(time / 0.5) * 40;
            double oy = 200;
            return new ASSPointF { X = ox, Y = oy };
        }
    }
}
