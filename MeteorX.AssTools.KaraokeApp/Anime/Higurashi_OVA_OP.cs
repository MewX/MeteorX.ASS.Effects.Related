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
    class Higurashi_OVA_OP : BaseAnime2, IXXEmitter, IXXForceField, IXXGravityPosition
    {
        public Higurashi_OVA_OP()
        {
            this.FontHeight = 25;
            this.FontSpace = 2;
            this.IsAvsMask = true;
            this.MarginLeft = 20;
            this.MarginBottom = 20;
            this.MarginTop = 20;
            this.MarginRight = 20;
            this.PlayResX = 848;
            this.PlayResY = 480;
            this.InFileName = @"G:\Workshop\higurashi ova\op\op_k.ass";
            this.OutFileName = @"G:\Workshop\higurashi ova\op\op.ass";
        }

        double beatStart = 0 * 60 + 19 + 0.02;
        double BPM = 110;
        Beat[] beatsAr = null;

        class Beat
        {
            public double Time { get; set; }
            public int Strength { get; set; }
            public int Id { get; set; }
            public bool Reg { get; set; }
        }

        Beat[] GetBeats()
        {
            if (beatsAr == null)
            {
                List<Beat> tmp = new List<Beat>();
                for (int i = 0; i * 60.0 / BPM + beatStart <= 90; i++)
                {
                    tmp.Add(new Beat { Time = i * 60.0 / BPM + beatStart, Id = i, Strength = 1 });
                }
                beatsAr = tmp.ToArray();
            }

            return beatsAr;
        }
        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS() { Header = ass_in.Header, Events = new List<ASSEvent>() };

            /*
            foreach (Beat bt in GetBeats())
                ass_out.AppendEvent(0, "op_jp", bt.Time, bt.Time + 0.4,
                    pos(50, 30) + a(1, "00") + c(3, "00") + bord(2) + blur(2) + fad(0, 0.3) +
                    bt.Id.ToString());
             * */

            for (int iEv = 0; iEv < ass_in.Events.Count; iEv++)
            {
                if (iEv == 19) continue;
                bool isJp = iEv <= 9 || iEv == 19;
                int jEv = (iEv >= 10) ? iEv - 10 : iEv;
                if (iEv != 0) continue;
                //if (!isJp) continue;
                //if (iEv != 0 && iEv != 10) continue;
                this.FontHeight = isJp ? 23 : 25;
                this.FontSpace = isJp ? 1 : 1;
                this.MaskStyle = isJp ?
                    "Style: Default,DFSoGei-W7,23,&H00FFFFFF,&HFFFFFFFF,&HFFFFFFFF,&HFFFFFFFF,0,0,0,0,100,100,0,0,1,0,0,5,0,0,0,128" :
                    "Style: Default,方正综艺_GBK,25,&H00FFFFFF,&HFFFFFFFF,&HFFFFFFFF,&HFFFFFFFF,0,0,0,0,100,100,0,0,1,0,0,5,0,0,0,1";
                ASSEvent ev = ass_in.Events[iEv];
                List<KElement> kelems = ev.SplitK(true);
                int x0 = (PlayResX - MarginLeft - MarginRight - GetTotalWidth(ev)) / 2 + MarginLeft;
                int y0 = (iEv <= 9) ? (PlayResY - MarginBottom - FontHeight) : MarginTop;
                int kSum = 0;

                List<KeyValuePair<double, ASSPoint>> textpath = new List<KeyValuePair<double, ASSPoint>>();
                string outlines = "";
                double lastt0 = -1;
                for (int iK = 0; iK < kelems.Count; iK++)
                {
                    //if (iK > 0) break;
                    Console.WriteLine("{0} / {1} : {2} / {3}", iEv + 1, ass_in.Events.Count, iK + 1, kelems.Count);
                    string evStyle = isJp ? "op_jp" : "op_cn";
                    string ptStyle = "op_pt";
                    string outlineFontname = isJp ? "DFSoGei-W7" : "方正综艺_GBK";
                    int outlineEncoding = isJp ? 128 : 1;
                    KElement ke = kelems[iK];
                    Size sz = GetSize(ke.KText);
                    double kStart = ev.Start + kSum * 0.01;
                    double kEnd = kStart + ke.KValue * 0.01;
                    kSum += ke.KValue;
                    int x = x0 + this.FontSpace + sz.Width / 2;
                    int y = y0 + FontHeight / 2;
                    int x_an7 = x0;
                    int y_an7 = y0;
                    StringMask mask = GetMask(ke.KText, x, y);
                    if (ke.KText.Trim().Length == 0) sz.Width = 20;
                    x0 += this.FontSpace + sz.Width;
                    if (ke.KText.Trim().Length == 0) continue;
                    int xOffset = 0;
                    if (!isJp) xOffset = 1;
                    else if (iEv == 9 || iEv == 19) xOffset = 5;
                    string outlineString = GetOutline(x - FontHeight / 2 + xOffset, y - FontHeight / 2, ke.KText[0], outlineFontname, outlineEncoding, FontHeight, 0, 156);
                    outlines += " " + outlineString;

                    textpath.Add(new KeyValuePair<double, ASSPoint>(kStart, new ASSPoint { X = x, Y = y, Start = kStart, End = kEnd }));

                    double t0 = ev.Start - 0.5 + iK * 0.1;
                    if (iEv == 9 || iEv == 19) t0 = ev.Start - 0.5 + iK * 0.04;
                    double t1 = t0 + 0.5;
                    double t2 = kStart;
                    double t3 = kEnd;
                    double t4 = ev.End - 0.5 + iK * 0.1;
                    double t5 = t4 + 0.5;
                    if (iEv == 9 || iEv == 19) t4 = ev.End - 0.5 + iK * 0.04;
                    lastt0 = t0;

                    /*
                    ass_out.AppendEvent(10, ptStyle, ev.Start, ev.End,
                        pos(0, 0) + a(1, "00") + c(1, "0000FF") +
                        p(4) + outlineString);
                    ass_out.AppendEvent(0, evStyle, ev.Start, ev.End,
                        pos(x, y) + a(1, "00") +
                        ke.KText);
                    continue;
                     * */

                    string mainColor = Common.scaleColor("C67A82", "C6A87A", (double)iK / (double)(kelems.Count - 1));
                    string shadColor = Common.scaleColor("77333B", "775D33", 1.0 - (double)iK / (double)(kelems.Count - 1));
                    shadColor = Common.scaleColor("000000", shadColor, 0.7);
                    if (iEv <= 9) shadColor = "FFFFFF";
                    string backColor = "FFFFFF";

                    string mainColor2 = "4A45AB";
                    string shadColor2 = "353377";
                    if ((isJp && iEv > 4) || (!isJp && iEv - 10 > 4))
                    {
                        string tmp = mainColor;
                        mainColor = mainColor2;
                        mainColor2 = tmp;
                        tmp = shadColor;
                        shadColor = shadColor2;
                        shadColor2 = tmp;
                    }
                    if (iEv > 9)
                    {
                        mainColor2 = mainColor;
                        shadColor2 = shadColor;
                    }

                    if (iEv <= 9)
                    {
                        foreach (Beat bt in GetBeats())
                        {
                            if (bt.Time <= t0 || bt.Time >= t4) continue;
                            string btCol = bt.Time <= t2 ? shadColor : shadColor2;
                            if (jEv <= 4)
                            {
                                ass_out.AppendEvent(40, evStyle, bt.Time, bt.Time + 0.4,
                                    pos(x, y) + fad(0, 0.3) +
                                    a(3, "00") + c(3, btCol) + ybord(3) + blur(2.5) +
                                    ke.KText);
                            }
                            else
                            {
                                ass_out.AppendEvent(40, evStyle, bt.Time, bt.Time + 0.4,
                                    pos(x, y) + fad(0, 0.3) +
                                    a(3, "00") + c(3, btCol) + xbord(6) + blur(4.5) +
                                    ke.KText);
                            }
                        }

                        ass_out.AppendEvent(45, evStyle, t0, t2 + 0.5,
                            pos(x, y) + fad(t1 - t0, 0.5) +
                            a(1, "00") + c(1, backColor) + a(3, "00") + c(3, shadColor) + bord(1.5) + blur(1.5) +
                            ke.KText);
                        ass_out.AppendEvent(45, evStyle, t2, t5,
                            pos(x, y) + fad(0.5, t5 - t4) +
                            a(1, "00") + c(1, backColor) + a(3, "00") + c(3, shadColor2) + bord(1.5) + blur(1.5) +
                            ke.KText);
                    }
                    else
                    {
                        ass_out.AppendEvent(45, evStyle, t0, t5,
                            pos(x, y) + fad(t1 - t0, t5 - t4) +
                            a(1, "00") + c(1, backColor) + a(3, "00") + c(3, shadColor) + bord(1.5) + blur(1.5) +
                            ke.KText);
                    }
                    for (int i = 0; i < ((iEv == 9 || iEv == 19) ? 1 : 3); i++)
                    {
                        double rndRange = (iEv == 9 || iEv == 19) ? 8 : (isJp ? 12 : 11);
                        double ptx0 = Common.RandomDouble(rnd, x - rndRange, x + rndRange);
                        double pty0 = Common.RandomDouble(rnd, y - rndRange, y + rndRange);
                        double bordSz = isJp ? 5 : 4.5;
                        double blurSz = isJp ? 4.5 : 4.2;
                        if (iEv <= 9)
                        {
                            ass_out.AppendEvent(50, ptStyle, t0, t2 + 0.5,
                                pos(ptx0, pty0) + fad(t1 - t0, 0.5) + clip(4, outlineString) +
                                a(1, "44") + a(3, "00") + c(1, mainColor) + c(3, mainColor) + bord(bordSz) + blur(blurSz) +
                                p(1) + "m 0 0 l 1 0 1 1 0 1");
                            ass_out.AppendEvent(50, ptStyle, t2, t5,
                                pos(ptx0, pty0) + fad(0.5, t5 - t4) + clip(4, outlineString) +
                                a(1, "44") + a(3, "00") + c(1, mainColor2) + c(3, mainColor2) + bord(bordSz) + blur(blurSz) +
                                p(1) + "m 0 0 l 1 0 1 1 0 1");
                        }
                        else
                        {
                            ass_out.AppendEvent(50, ptStyle, t0, t5,
                                pos(ptx0, pty0) + fad(t1 - t0, t5 - t4) + clip(4, outlineString) +
                                a(1, "44") + a(3, "00") + c(1, mainColor) + c(3, mainColor) + bord(bordSz) + blur(blurSz) +
                                p(1) + "m 0 0 l 1 0 1 1 0 1");
                        }
                    }

                    if (iEv > 9) continue;

                    ass_out.AppendEvent(70, evStyle, t2, t2 + 0.4,
                        pos(x, y) + fad(0.00, 0.28) +
                        a(1, "00") + a(3, "00") + bord(3) + blur(2.8) +
                        ke.KText);

                }

                if (iEv > 9) continue;

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
                    double x = 1000.0 * Math.Cos(ag);
                    double y = 600.0 * Math.Sin(ag);
                    forceCurve.AddCurve(time, time + 0.7, new Line { X0 = x, Y0 = y, X1 = 0, Y1 = 0, Acc = 0.7 });
                }

                NumberPerSecond = 20;
                XXParticleSystem xxps = new XXParticleSystem();
                xxps.Emitter = this;
                xxps.ForceField = this;
                xxps.StartTime = ev.Start;
                xxps.EndTime = ev.End;
                xxps.InterpolationPrecision = 0.03;
                xxps.Resistance = 0.04;
                xxps.Repulsion = -4500;
                xxps.Gravity = 0;
                xxps.GravityPosition = this;

                xxps.InterpolationPrecision = 0.01;
                List<KeyValuePair<XXParticleElement, List<string>>> result = xxps.RenderT();
                foreach (KeyValuePair<XXParticleElement, List<string>> pair in result)
                {
                    string s = CreatePolygon(rnd, 9, 9, 4);
                    string ptstr = @"{\p1}m 0 0 l 1 0 1 1 0 1";
                    string ptcol = (iEv <= 4) ? "532BFF" : "FFC0C6";
                    ass_out.AppendEvent(20, "op_pt", pair.Key.Born, pair.Key.Born + pair.Key.Life,
                        pos(-100, -100) +
                        pair.Value[1] +
                        ptstr + "\\N" + r() +
                        pair.Value[0] +
                        ptstr + r() +
                        a(1, "22") + a(3, "77") + blur(2.5) + bord(2) + c(3, ptcol) +
                        //a(4, "44") + @"{\shad1}" + c(4, "FFFFFF") +
                        s);
                }

                if (isJp)
                {
                    double ptx0 = textpath[0].Value.X - 50;
                    double ptx1 = textpath.Last().Value.X + 50;
                    double pty0 = textpath[0].Value.Y;
                    double spd = 400;
                    foreach (Beat bt in GetBeats())
                    {
                        if (iEv == 9)
                        {
                            double ptt0 = bt.Time - 0.15;
                            if (ptt0 < ev.Start || ptt0 >= ev.End - 0.4) continue;
                            ass_out.AppendEvent(200, "op_pt", ptt0, ptt0 + (ptx1 - ptx0) / spd,
                                (iEv % 2 == 0 ? move(ptx1, pty0, ptx0, pty0) : move(ptx0, pty0, ptx1, pty0)) + clip(4, outlines) +
                                a(1, "00") + blur(3) + frz(-45) + fscx(70) +
                                p(1) + "m -15 -30 l 15 -30 15 30 -15 30");
                        }
                        else
                        {
                            double ptt0 = lastt0;
                            ass_out.AppendEvent(200, "op_pt", ptt0, ptt0 + (ptx1 - ptx0) / spd,
                                move(ptx1, pty0, ptx0, pty0) + clip(4, outlines) +
                                a(1, "00") + blur(3) + frz(-45) + fscx(70) +
                                p(1) + "m -15 -30 l 15 -30 15 30 -15 30");
                            ass_out.AppendEvent(200, "op_pt", ptt0, ptt0 + (ptx1 - ptx0) / spd,
                                move(ptx0, pty0, ptx1, pty0) + clip(4, outlines) +
                                a(1, "00") + blur(3) + frz(-45) + fscx(70) +
                                p(1) + "m -15 -30 l 15 -30 15 30 -15 30");
                            break;
                        }
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

            double x = 150.0 * Math.Cos(ag);
            double y = 150.0 * Math.Sin(ag);

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
