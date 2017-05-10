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
    class Asura_ED : BaseAnime2, IXXEmitter, IXXForceField, IXXGravityPosition
    {
        public Asura_ED()
        {
            this.FontHeight = 38;
            this.FontSpace = 2;
            this.IsAvsMask = true;
            this.MarginLeft = 30;
            this.MarginBottom = 30;
            this.MarginTop = 26;
            this.MarginRight = 30;
            this.PlayResX = 1280;
            this.PlayResY = 720;
            this.InFileName = @"g:\workshop\asura\ed_k.ass";
            this.OutFileName = @"g:\workshop\asura\ed.ass";
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS() { Header = ass_in.Header, Events = new List<ASSEvent>() };

            for (int iEv = 0; iEv < ass_in.Events.Count; iEv++)
            {
                bool isJp = iEv <= 6;
                //if (!isJp || iEv < 4 || iEv > 4) continue;
                this.MaskStyle = isJp ?
                    "Style: Default,DFMincho-UB,38,&H00FFFFFF,&HFFFFFFFF,&HFFFFFFFF,&HFFFFFFFF,0,0,0,0,100,100,0,0,0,0,0,5,0,0,0,128" :
                    "Style: Default,汉仪粗宋繁,38,&H00FFFFFF,&HFFFFFFFF,&HFFFFFFFF,&HFFFFFFFF,1,0,0,0,100,100,0,0,0,0,0,5,0,0,0,134";
                ASSEvent ev = ass_in.Events[iEv];
                List<KElement> kelems = ev.SplitK(!isJp);
                int x0 = MarginLeft;
                if ((isJp && iEv >= 4) || (!isJp && iEv - 7 >= 4)) x0 = PlayResX - MarginRight - GetTotalWidth(ev);
                int y0 = (isJp) ? (PlayResY - MarginBottom - FontHeight) : MarginTop;
                int kSum = 0;

                List<KeyValuePair<double, ASSPoint>> textpath = new List<KeyValuePair<double, ASSPoint>>();

                for (int iK = 0; iK < kelems.Count; iK++)
                {
                    //if (iK > 0) break;
                    Console.WriteLine("{0} / {1} : {2} / {3}", iEv + 1, ass_in.Events.Count, iK + 1, kelems.Count);
                    string evStyle = isJp ? "jp" : "cn";
                    string outlineFontname = isJp ? "DFMincho-UB" : "汉仪粗宋繁";
                    int outlineEncoding = isJp ? 128 : 134;
                    KElement ke = kelems[iK];
                    Size sz = GetSize(ke.KText);
                    if (ke.KText[0] == 'く') x0 += 2;
                    double kStart = ev.Start + kSum * 0.01;
                    double kEnd = kStart + ke.KValue * 0.01;
                    kSum += ke.KValue;
                    int x = x0 + this.FontSpace + sz.Width / 2;
                    int y = y0 + FontHeight / 2;
                    int x_an7 = x0 + 2;
                    int y_an7 = y0;
                    StringMask mask = GetMask(ke.KText, x, y);
                    x0 += this.FontSpace + sz.Width;
                    if (ke.KText.Trim().Length == 0) continue;
                    string outlineString = GetOutline(x - FontHeight / 2, y - FontHeight / 2, ke.KText[0], outlineFontname, outlineEncoding, 38, 0, 262);

                    if (!isJp) y += 2;

                    textpath.Add(new KeyValuePair<double, ASSPoint>(kStart, new ASSPoint { X = x, Y = y, Start = kStart, End = kEnd }));

                    double t0 = ev.Start - 0.5 + iK * 0.07;
                    double t1 = t0 + 0.5;
                    double t2 = kStart;
                    double t3 = kEnd;
                    double t4 = ev.End - 0.6 + iK * 0.07;
                    double t5 = t4 + 0.5;

                    {
                        string red = "1A037C";
                        red = "3B1FB6";
                        string aoi = "9D3653";

                        string scol = iEv <= 1 ? red : aoi;
                        if (iEv >= 4) scol = red;

                        if (isJp)
                        {
                            ass_out.AppendEvent(51, "pt", t2, t2 + 1,
                                pos(0, 0) + fad(0, 0.9) +
                                a(1, "00") + a(3, "77") + blur(5) + bord(5) + t(bord(0).t()) +
                                p(4) + outlineString);
                        }
                        ass_out.AppendEvent(40, "pt", t0, t5,
                            pos(1, 1) + fad(0.5, 0.5) +
                            a(1, "00") + blur(1.3) + c(1, "000000") +
                            p(4) + outlineString);

                        if (true)
                        {
                            double lastptx0 = 0;
                            double lastpty0 = 0;
                            bool first = true;

                            for (double ti = t0 - 1 + Common.RandomDouble(rnd, -0.3, 0.3); ti <= t5 - 1.5; ti += Common.RandomDouble(rnd, 1.2, 1.65))
                            {
                                double ptt0 = ti;
                                double ptt1 = ptt0 + 3;
                                double ptx0 = Common.RandomDouble(rnd, x - 20, x + 20);
                                double pty0 = Common.RandomDouble(rnd, y - 20, y + 20);

                                if (!first)
                                {
                                    for (int i = 0; i < 2; i++)
                                    {
                                        double ptx0_tmp = Common.RandomDouble(rnd, x - 20, x + 20);
                                        double pty0_tmp = Common.RandomDouble(rnd, y - 20, y + 20);
                                        if (Common.GetDistance(lastptx0, lastpty0, ptx0, pty0) < Common.GetDistance(lastptx0, lastpty0, ptx0_tmp, pty0_tmp))
                                        {
                                            ptx0 = ptx0_tmp;
                                            pty0 = pty0_tmp;
                                        }
                                    }
                                }
                                first = false;
                                lastptx0 = ptx0;
                                lastpty0 = pty0;

                                string ptcol = "FFFFFF";
                                double lumsz = 15 + Common.GetDistance(x, y, ptx0, pty0) * 0.5;
                                if (ptt0 >= kStart - 1 && ptt0 <= kEnd - 1)
                                {
                                    lumsz += 0;
                                }
                                //if (iEv <= 3)
                                if (isJp)
                                    if (ptt0 >= t2) ptcol = scol;

                                string ts0 = "";
                                //if (iEv <= 3)
                                if (isJp)
                                    if (ptt0 < t2 && ptt1 > t2) ts0 = t(t2 - ptt0 - 0.01, t2 - ptt0, c(1, scol).t() + c(3, scol).t());

                                int dup = 1;
                                if ((isJp && iEv >= 4) || (!isJp && iEv - 7 >= 4)) dup = 2;

                                while (dup-- > 0)
                                {
                                    ass_out.AppendEvent(50, "pt", ptt0, ptt1,
                                        pos(ptx0, pty0) + clip(4, outlineString) + org(x, y) + t(frz(Common.RandomSig(rnd) * Common.RandomInt(rnd, 80, 120)).t()) +
                                        fad(0.8, 0.8) + t(0, ptt1 - ptt0, bord(lumsz).t() + blur(lumsz).t()) +
                                        a(1, "44") + a(3, "44") + c(1, ptcol) + c(3, ptcol) +
                                        ts0 +
                                        t((ptt1 - ptt0) * 0.7, ptt1, c(1, "000000").t() + c(3, "000000").t()) +
                                        p(1) + "m 0 0 l 1 0 1 1 0 1");
                                }
                            }
                        }
                    }

                    if (!isJp) continue;

                    if (iEv <= 3)
                    {
                        for (int i = 0; i < (kEnd - kStart) * 7; i++)
                        {
                            double ptx0 = Common.RandomDouble(rnd, x - 20, x + 20);
                            double pty0 = Common.RandomDouble(rnd, y - 5, y + 20);
                            double ptx1 = Common.RandomDouble(rnd, ptx0 - 150, ptx0 - 250);
                            double pty1 = Common.RandomDouble(rnd, pty0 - 30, pty0 - 80);
                            double ptt0 = Common.RandomDouble(rnd, kStart, kEnd) - 0.3;
                            double ptt1 = ptt0 + 5;

                            double posx = 0, posy = 0;
                            if (ptx0 < 0 || ptx1 < 0)
                            {
                                posx = Math.Min(ptx0, ptx1);
                                ptx0 -= posx;
                                ptx1 -= posx;
                            }
                            if (pty0 < 0 || pty1 < 0)
                            {
                                posy = Math.Min(pty0, pty1);
                                pty0 -= posy;
                                pty1 -= posy;
                            }
                            string moveStringX = fscx((int)(ptx0 * 100)) + t(0, 5, 3, fscx((int)(ptx1 * 100)).t());
                            string moveStringY = fscy((int)(pty0 * 100)) + t(0, 5, 0.6, fscy((int)(pty1 * 100)).t());
                            string splashString = "";
                            for (double ti = 0; ti <= ptt1 - ptt0; ti += 0.8)
                                splashString += t(ti + 0, ti + 0.4, a(1, "FF").t() + a(3, "FF").t()) + t(ti + 0.4, ti + 0.8, a(1, "22").t() + a(3, "44").t());

                            string ptstr = @"{\p1}m 0 0 l 1 0 1 1 0 1";
                            string par = CreatePolygon(rnd, 5, 10, 6);
                            string parcol = "EC3667";
                            if (iEv <= 1) parcol = "532BFF";

                            for (int j = 0; j < 1; j++)
                            {
                                ass_out.AppendEvent(12, "pt", ptt0 + j * 0.1, ptt1 + j * 0.1,
                                    pos(posx, posy) +
                                    moveStringY +
                                    ptstr + "\\N" + r() +
                                    moveStringX +
                                    ptstr + r() +
                                    fad(0, 0.5) + pos(0, 0) +
                                    a(1, "22") + a(3, "44") + blur(1.5) + bord(1.5) + c(3, parcol) +
                                    splashString +
                                    par);
                            }
                        }
                    }
                }

                if (!isJp) continue;

                //continue;

                if (iEv >= 4)
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

                    NumberPerSecond = 90;
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
                        ass_out.AppendEvent(100, "pt", pair.Key.Born, pair.Key.Born + pair.Key.Life,
                            pos(-100, -100) +
                            pair.Value[1] +
                            ptstr + "\\N" + r() +
                            pair.Value[0] +
                            ptstr + r() +
                            a(1, "22") + a(3, "77") + blur(2.5) + bord(2) + c(3, ptcol) +
                            s);
                    }
                    /*
                    List<KeyValuePair<XXParticleElement, List<ASSPointF>>> result = xxps.RenderPoint();
                    foreach (KeyValuePair<XXParticleElement, List<ASSPointF>> pair in result)
                    {
                        string s = CreatePolygon(rnd, 12, 12, 4);
                        foreach (ASSPointF pt in pair.Value)
                        {
                            pt.T -= 0.3;
                            string ptcol = "532BFF";
                            ass_out.AppendEvent(100, "pt", pt.T, pt.T + xxps.InterpolationPrecision,
                                pos(pt.X, pt.Y) + a(1, "22") + a(3, "77") + blur(2.5) + bord(2) + c(3, ptcol) +
                                s);
                        }
                    }
                     * */
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

            double x = 400.0 * Math.Cos(ag);
            double y = 400.0 * Math.Sin(ag);

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
