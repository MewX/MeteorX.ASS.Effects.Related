using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.Model;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class Hanasakeru_Seishounen_OP : BaseAnime2
    {
        public Hanasakeru_Seishounen_OP()
        {
            this.FontHeight = 28;
            this.FontSpace = 2;
            this.IsAvsMask = true;
            this.MarginLeft = 20;
            this.MarginBottom = 18;
            this.MarginTop = 20;
            this.MarginRight = 20;
            this.PlayResX = 848;
            this.PlayResY = 480;
            this.InFileName = @"G:\Workshop\hanasakeru\op\op_k.ass";
            this.OutFileName = @"G:\Workshop\hanasakeru\op\op.ass";

            this.WhitespaceWidth = 14;
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS() { Header = ass_in.Header, Events = new List<ASSEvent>() };

            ParticleIllusionExporter pie = new ParticleIllusionExporter();

            for (int iEv = 0; iEv < ass_in.Events.Count; iEv++)
            {
                bool isJp = iEv <= 13;
                int iiEv = isJp ? iEv : iEv - 14;
                //if (iiEv !=4 && iiEv != 5) continue;
                this.MaskStyle = isJp ?
                    "Style: Default,HGSGyoshotai,26,&H00FFFFFF,&HFFFFFFFF,&HFFFFFFFF,&HFFFFFFFF,0,0,0,0,100,100,0,0,0,0,0,5,0,0,0,128" :
                    "Style: Default,方正行楷简体,30,&H00FFFFFF,&HFFFFFFFF,&HFFFFFFFF,&HFFFFFFFF,0,0,0,0,100,100,0,0,0,0,0,5,0,0,0,134";
                ASSEvent ev = ass_in.Events[iEv];
                List<KElement> kelems = ev.SplitK(true);
                int sw = GetTotalWidth(ev);
                int x0 = (isJp) ? MarginLeft : PlayResX - MarginRight - sw;
                int y0 = (!isJp) ? (PlayResY - MarginBottom - FontHeight) : MarginTop;
                int startx0 = x0;
                int kSum = 0;
                string outlines = "";
                for (int iK = 0; iK < kelems.Count; iK++)
                {
                    Console.WriteLine("{0} / {1} : {2} / {3}", iEv + 1, ass_in.Events.Count, iK + 1, kelems.Count);
                    string evStyle = isJp ? "jp" : "cn";
                    string ptStyle = "pt";
                    KElement ke = kelems[iK];
                    Size sz = GetSize(ke.KText);
                    double kStart = ev.Start + kSum * 0.01;
                    double kEnd = kStart + ke.KValue * 0.01;
                    kSum += ke.KValue;
                    int x = x0 + this.FontSpace + sz.Width / 2;
                    int y = y0 + FontHeight / 2;
                    StringMask mask = GetMask(ke.KText, x, y);
                    x0 += this.FontSpace + sz.Width;
                    if (ke.KText.Trim().Length == 0) continue;

                    double t0 = ev.Start - 1 + (double)(x0 - startx0 - sz.Width) / 300.0;
                    double t1 = t0 + 1;
                    double t2 = kStart;
                    //if (iEv <= 3) t2 = ke.KStart_NoSplit;
                    double t3 = kEnd;
                    double t4 = ev.End - 1 + (double)(x0 - startx0 - sz.Width) / 300.0;
                    if (t4 < t3) t4 = t3;
                    double t5 = t4 + 1;

                    if (iEv == 4 && iK == 0)
                    {
                        pie.Add(t2 - 0.35, new ASSPointF(0, y));
                    }
                    if (iEv >= 4 && isJp)
                    {
                        double last = ke.KValue * 0.01;
                        pie.Add(t2, new ASSPointF(x, y));
                        if (last > 0.23 && !Common.IsLetter(ke.KText[0])) pie.Add(t2 + last - 0.23, new ASSPointF(x, y));
                    }

                    if (!isJp)
                    {
                        ass_out.AppendEvent(40, evStyle, t0, t5,
                            pos(x + 1, y + 1) + fad(0.5, 0.5) + a(1, "00") + c(1, "000000") + blur(1) +
                            ke.KText);
                        ass_out.AppendEvent(50, evStyle, t0, t5,
                            pos(x, y) + fad(0.5, 0.5) + a(1, "00") +
                            ke.KText);
                        continue;
                    }

                    {
                        CompositeCurve curve = new CompositeCurve { MinT = t0, MaxT = t1 };
                        curve.AddCurve(t0, t1, new Curve1 { X0 = x, Y0 = y, R = 30 });
                        foreach (ASSPointF pt in curve.GetPath_DT(0.007))
                        {
                            double tmp = pt.Theta * 100;
                            ass_out.AppendEvent(70, evStyle, pt.T, pt.T + 0.4,
                                pos(pt.X, pt.Y) + fad(0, 0.3) + a(1, "00") + be(1) +
                                fsc((int)tmp, (int)tmp) +
                                ke.KText);
                            ass_out.AppendEvent(60, evStyle, pt.T, pt.T + 0.4,
                                pos(pt.X + 1, pt.Y + 1) + fad(0, 0.3) + a(1, "DD") + c(1, "000000") + be(1) +
                                fsc((int)tmp, (int)tmp) +
                                ke.KText);
                        }

                        {
                            ass_out.AppendEvent(40, evStyle, t1, t5,
                                pos(x + 1, y + 1) + fad(0.3, 0.5) + a(1, "00") + c(1, "000000") + blur(1) +
                                ke.KText);
                            ass_out.AppendEvent(50, evStyle, t1, t5,
                                pos(x, y) + fad(0.3, 0.5) + a(1, "00") +
                                ke.KText);
                        }

                        if (iEv <= 3 || Common.IsLetter(ke.KText[0]))
                        {
                            for (double ti = ke.KStart_NoSplit; ti <= ke.KEnd_NoSplit; ti += 0.01)
                            {
                                ass_out.AppendEvent(70, evStyle, ti, ti + 0.35,
                                    move(x, y, Common.RandomDouble(rnd, x - 5, x + 5), Common.RandomDouble(rnd, y - 5, y + 5)) +
                                    fad(0, 0.2) + blur(1) + a(1, "AA") +
                                    ke.KText);
                            }
                        }
                        else
                        {
                            for (double ti = t2; ti <= t3; ti += 0.01)
                            {
                                ass_out.AppendEvent(70, evStyle, ti, ti + 0.35,
                                    move(x, y, Common.RandomDouble(rnd, x - 5, x + 5), Common.RandomDouble(rnd, y - 5, y + 5)) +
                                    fad(0, 0.2) + blur(1) + a(1, "AA") +
                                    ke.KText);
                            }
                        }
                    }
                }
            }

            pie.SaveToFile(@"G:\Workshop\hanasakeru\op\pi_export.txt");

            Console.WriteLine(ass_out.Events.Count);
            ass_out.SaveFile(this.OutFileName);
        }

        class Curve1 : Circle
        {
            public Curve1()
            {
                this.MinT = 0;
                this.MaxT = Math.PI * 2;
            }

            public override ASSPointF GetPointF(double t)
            {
                double tr = R * (1.0 - (t - MinT) / (MaxT - MinT));
                return new ASSPointF { X = X0 + tr * Math.Cos(t), Y = Y0 + tr * Math.Sin(t), T = t, Theta = (t - MinT) / (MaxT - MinT) };
            }
        }
    }
}
