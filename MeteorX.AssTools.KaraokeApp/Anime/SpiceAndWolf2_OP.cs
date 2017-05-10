using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.Model;
using MeteorX.AssTools.KaraokeApp.XXParticle;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class SpiceAndWolf2_OP : BaseAnime2, IXXEmitter, IXXForceField, IXXGravityPosition
    {
        public SpiceAndWolf2_OP()
        {
            this.FontHeight = 26;
            this.FontSpace = -2;
            this.IsAvsMask = true;
            this.MarginLeft = 17;
            this.MarginBottom = 17;
            this.MarginTop = 17;
            this.MarginRight = 17;
            this.PlayResX = 848;
            this.PlayResY = 480;
            this.InFileName = @"G:\Workshop\spice and wolf2\op\op_k.ass";
            this.OutFileName = @"G:\Workshop\spice and wolf2\op\op.ass";
        }

        public override Size GetSize(string s)
        {
            return new Size { Width = this.FontHeight, Height = this.FontHeight };
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS() { Header = ass_in.Header, Events = new List<ASSEvent>() };

            for (int iEv = 0; iEv < ass_in.Events.Count; iEv++)
            {
                bool isJp = true;
                this.MaskStyle = isJp ?
                    "Style: Default,EPSON 行書体Ｍ,26,&H00FFFFFF,&HFFFFFFFF,&HFFFFFFFF,&HFFFFFFFF,0,0,0,0,100,100,0,0,1,0,0,5,0,0,0,128" :
                    "Style: Default,仿宋,36,&H00FFFFFF,&HFFFFFFFF,&HFFFFFFFF,&HFFFFFFFF,0,0,0,0,100,100,0,0,1,0,0,5,0,0,0,1";
                this.FontHeight = isJp ? 26 : 30;
                int jEv = isJp ? iEv : iEv;
                //if (jEv != 0) continue;
                ASSEvent ev = ass_in.Events[iEv];
                List<KElement> kelems = ev.SplitK(true);
                int totalWidth = GetTotalWidth(ev);
                int x0 = (PlayResX - MarginLeft - MarginRight - totalWidth) / 2 + MarginLeft;
                int y0 = (isJp) ? (PlayResY - MarginBottom - FontHeight) : MarginTop;
                int kSum = 0;
                int x0_start = x0;
                string outlines = "";
                List<KeyValuePair<double, ASSPoint>> textpath = new List<KeyValuePair<double, ASSPoint>>();
                string MainColor = "112836";
                if (iEv >= 8) MainColor = "C5A8A0";
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
                    int x_an7 = x0;
                    int y_an7 = y0;
                    x0 += this.FontSpace + sz.Width;
                    if (ke.KText.Trim().Length == 0) continue;
                    StringMask mask = GetMask(ke.KText, x, y);
                    string evStyle = isJp ? "op_jp" : "op_cn";

                    /*
                    string outlineFontname = isJp ? "EPSON 行書体Ｍ" : "方正准圆_GBK";
                    int outlineEncoding = isJp ? 128 : 1;
                    int xoffset = isJp ? 0 : -1;
                    if (isJp && ke.KText[0] == '中') xoffset = -2;
                    string outlineString = GetOutline(x - sz.Width / 2 + xoffset, y - FontHeight / 2, ke.KText[0], outlineFontname, outlineEncoding, FontHeight, 0, 179);
                    outlines += outlineString;
                     * */

                    double t0 = ev.Start - 0.5 + iK * 0.1;
                    double t1 = t0 + 0.5;
                    double t2 = kStart;
                    double t3 = ev.End - 0.5 + iK * 0.1;
                    double t4 = t3 + 0.5;

                    ass_out.AppendEvent(50, evStyle, t0, t4,
                        fad(t1 - t0, t4 - t3) + pos(x, y) +
                        a(1, "00") + c(1, MainColor) +
                        ke.KText);
                    ass_out.AppendEvent(49, evStyle, t0, t4,
                        fad(t1 - t0, t4 - t3) + pos(x + 1.5, y + 1.5) +
                        a(1, "00") + c(1, "000000") + blur(1) +
                        ke.KText);
                    ass_out.AppendEvent(60, evStyle, t2, t2 + 0.5,
                        fad(0, 0.42) + pos(x, y) +
                        a(1, "00") + c(1, "FFFFFF") +
                        ke.KText);
                    ass_out.AppendEvent(60, evStyle, t2, t2 + 1,
                        pos(x + 1.5, y + 1.5) +
                        a(1, "00") + c(1, "FFFFFF") + blur(1) +
                        t(0, 1, 0.8, a(1, "FF").t()) +
                        ke.KText);
                    ass_out.AppendEvent(60, evStyle, t2, t2 + 1,
                        fad(0, 0.42) +
                        pos(x, y) +
                        a(1, "FF") + a(3, "BB") + c(3, "FFFFFF") +
                        blur(3) + bord(2.5) +
                        t(0, 1, 0.8, a(3, "FF").t()) +
                        ke.KText);

                    textpath.Add(new KeyValuePair<double, ASSPoint>(kStart, new ASSPoint { X = x, Y = y, Start = kStart, End = kEnd }));

                    if (iEv == 1)
                    {
                        for (int i = 0; i < 100; i++)
                        {
                            double ptt0 = Common.RandomDouble(rnd, t3, t4);
                            double ptt1 = ptt0 + Common.RandomDouble(rnd, 0.5, 1);
                            double ptx0 = Common.RandomDouble(rnd, x - 17, x + 17) - 25;
                            double pty0 = Common.RandomDouble(rnd, y - 17, y + 17);
                            double ptx1 = ptx0 + Common.RandomDouble(rnd, 30, 50) * 1.4;
                            double pty1 = pty0 + Common.RandomDouble(rnd, 5, 30) * Common.RandomSig(rnd);
                            int tmp1 = Common.RandomInt(rnd, 0, 359);
                            ass_out.AppendEvent(70, "pt", ptt0, ptt1,
                                move(ptx0, pty0, ptx1, pty1) + a(1, "00") + blur(0.5) + a(4, "DD") + c(4, "000000") + shad(2) +
                                fad(0, 0.5) +
                                //c(1, Common.RandomBool(rnd, 0.9) ? MainColor : Common.scaleColor(MainColor, "FFFFFF", 0.2)) +
                                c(1, Common.scaleColor(MainColor, "FFFFFF", Common.RandomDouble(rnd, 0, 0.15))) +
                                frz(tmp1) +
                                CreatePolygon(rnd, 15, 23, 6));

                        }
                    }
                    if (iEv >= 2 && iEv <= 4)
                    {
                        for (int i = 0; i < (kEnd - kStart) * 30; i++)
                        {
                            double ptt0 = Common.RandomDouble(rnd, kStart, kEnd);
                            double ptt1 = ptt0 + Common.RandomDouble(rnd, 0.5, 1) * 2;
                            double ptx0 = Common.RandomDouble(rnd, x - 13, x + 13);
                            double pty0 = Common.RandomDouble(rnd, y - 13, y + 13);
                            double ptx1 = ptx0 - Common.RandomDouble(rnd, 30, 100) * 1.4;
                            double pty1 = pty0 + Common.RandomDouble(rnd, 20, 45);
                            int tmp1 = Common.RandomInt(rnd, 0, 359);
                            ass_out.AppendEvent(70, "pt", ptt0, ptt1,
                                move(ptx0, pty0, ptx1, pty1) + a(1, "00") + blur(0.5) + a(4, "DD") + c(4, "000000") + shad(2) +
                                fad(0, 0.5) +
                                //c(1, Common.RandomBool(rnd, 0.9) ? MainColor : Common.scaleColor(MainColor, "FFFFFF", 0.2)) +
                                c(1, Common.scaleColor(MainColor, "FFFFFF", Common.RandomDouble(rnd, 0, 0.15))) +
                                frz(tmp1) +
                                CreatePolygon(rnd, 15, 23, 6));

                        }
                    }
                }

                if (iEv >= 5)// && iEv <= 7)
                {
                    for (int i = 0; i < textpath.Count - 1; i++)
                        if (textpath[i].Value.End < textpath[i + 1].Value.Start)
                            textpath[i].Value.End = textpath[i + 1].Value.Start;
                    textpath[textpath.Count - 1].Value.End = ev.End;

                    emitterList = new List<ASSPointF>();
                    foreach (KeyValuePair<double, ASSPoint> pair in textpath)
                        emitterList.Add(new ASSPointF
                        {
                            Start = pair.Value.Start,
                            End = pair.Value.End,
                            X = pair.Value.X + Common.RandomDouble(rnd, -20, 20),
                            Y = pair.Value.Y + Common.RandomDouble(rnd, -20, 20)
                        });

                    forceCurve = new CompositeCurve { MinT = ev.Start, MaxT = ev.End + 10 };
                    double lastag = 0;
                    for (double time = forceCurve.MinT; time <= forceCurve.MaxT; time += 0.7)
                    {
                        double ag = Common.RandomDouble(rnd, 0, Math.PI * 2);
                        while (Math.Abs(ag - lastag) < Math.PI * 0.5 || Math.Abs(ag - lastag) > Math.PI * 1.5)
                            ag = Common.RandomDouble(rnd, 0, Math.PI * 2);
                        lastag = ag;
                        double x = 1500.0 * Math.Cos(ag);
                        double y = 800.0 * Math.Sin(ag);
                        forceCurve.AddCurve(time, time + 0.7, new Line { X0 = x, Y0 = y, X1 = 0, Y1 = 0, Acc = 0.7 });
                    }

                    NumberPerSecond = 110;
                    XXParticleSystem xxps = new XXParticleSystem();
                    xxps.Emitter = this;
                    xxps.ForceField = this;
                    xxps.StartTime = ev.Start;
                    xxps.EndTime = ev.End;
                    xxps.InterpolationPrecision = 0.03;
                    xxps.Resistance = 0.04;
                    xxps.Repulsion = -3600;
                    xxps.Gravity = 0;
                    xxps.GravityPosition = this;

                    xxps.InterpolationPrecision = 0.01;
                    List<KeyValuePair<XXParticleElement, List<string>>> result = xxps.RenderT();
                    foreach (KeyValuePair<XXParticleElement, List<string>> pair in result)
                    {
                        string s = CreatePolygon(rnd, 12, 12, 4);
                        string ptstr = @"{\p1}m 0 0 l 1 0 1 1 0 1";
                        string ptcol = "532BFF";
                        ass_out.AppendEvent(70, "pt", pair.Key.Born, pair.Key.Born + pair.Key.Life,
                            pos(-100, -100) +
                            pair.Value[1] +
                            ptstr + "\\N" + r() +
                            pair.Value[0] +
                            ptstr + r() +
                            a(1, "00") + blur((iEv <= 7) ? 0.6 : 0.8) + a(4, "DD") + c(4, "000000") + shad(2) +
                            fad(0, 0.5) +
                            c(1, Common.scaleColor(MainColor, "FFFFFF", Common.RandomDouble(rnd, 0, 0.15))) +
                           CreatePolygon(rnd, 15, 23, 6));
                    }
                }
            }

            Console.WriteLine(ass_out.Events.Count);
            ass_out.SaveFile(this.OutFileName);
        }

        List<ASSPointF> emitterList = null;

        public XXParticleElement GenerateParticleElement(double time)
        {
            double ag = Common.RandomDouble(rnd, 0, 1);
            if (Common.RandomBool(rnd, 0.5)) ag += Math.PI;
            ag += time;

            double x = 200.0 * Math.Cos(ag);
            double y = 200.0 * Math.Sin(ag);

            double ox = 0;
            double oy = 0;
            foreach (ASSPointF pt in emitterList)
                if (time >= pt.Start && time <= pt.End)
                {
                    ox = pt.X;
                    oy = pt.Y;
                    break;
                }

            double mass = 1;
            return new XXParticleElement
            {
                Spin = Common.RandomDouble(rnd, -5, 5) * 0.8,
                Mass = mass,
                Born = time,
                Life = 1,
                Position = new ASSPointF
                {
                    X = Common.RandomDouble(rnd, ox - 5, ox + 5),
                    Y = Common.RandomDouble(rnd, oy - 5, oy + 5)
                },
                Speed = new ASSPointF { X = x, Y = y },
                ForceTimeOffset = Common.RandomDouble(rnd, 0, 2)
            };
        }

        public double NumberPerSecond { get; set; }

        CompositeCurve forceCurve = null;

        public ASSPointF GetForceField(double time)
        {
            return forceCurve.GetPointF(time);
        }

        public ASSPointF GetGravityPosition(double time)
        {
            double ox = 200 + (int)(time / 0.5) * 40;
            double oy = 200;
            return new ASSPointF { X = ox, Y = oy };
        }

    }
}
