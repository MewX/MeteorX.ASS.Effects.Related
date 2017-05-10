using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.Model;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class ZokuNatsume_OP : BaseAnime
    {
        int ScaleRate = 2;

        public ZokuNatsume_OP()
        {
            InFileName = @"G:\Workshop\natsume2\op_k.ass";
            OutFileName = @"G:\Workshop\natsume2\op.ass";

            this.FontWidth = 33 * ScaleRate;
            this.FontHeight = 28 * ScaleRate;
            this.FontSpace = -4 * ScaleRate;

            this.PlayResX = 848 * ScaleRate;
            this.PlayResY = 480 * ScaleRate;
            this.MarginBottom = 37 * ScaleRate;
            this.MarginLeft = 15 * ScaleRate;
            this.MarginRight = 10 * ScaleRate;
            this.MarginTop = 10 * ScaleRate;

            this.Font = new System.Drawing.Font("DFGKaiSho-Md", 28 * ScaleRate);
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(InFileName);
            ASS ass_out = new ASS();
            ass_out.Header = ass_in.Header;
            ass_out.Events = new List<ASSEvent>();

            int y0min = 0;
            int y0max = 100;

            string color1 = "DDC3B2";
            string color2 = "C69A7B";

            List<StringMask> lastMasks = new List<StringMask>();
            List<StringMask> thisMasks = new List<StringMask>();

            Random rnd = new Random();

            /// 所有的不动点最后一次写入
            List<ASSPoint> allPoints = new List<ASSPoint>();

            for (int iEv = 0; iEv < ass_in.Events.Count; iEv++)
            {
                ASSEvent ev = ass_in.Events[iEv];
                List<KElement> kelems = ev.SplitK(false);

                int x0 = MarginLeft;
                int y0 = PlayResY - FontHeight - MarginBottom;

                thisMasks.Clear();
                for (int iK = 0; iK < kelems.Count; iK++)
                {
                    KElement elem = kelems[iK];
                    StringMask mask = GetMask(elem.KText, x0, y0);
                    thisMasks.Add(GetMask(kelems[iK].KText, x0, y0));
                    x0 += mask.Width + this.FontSpace;

                }

                List<ASSPoint> lastPts = new List<ASSPoint>();
                if (iEv > 0)
                {
                    if (Math.Abs(ev.Start - ass_in.Events[iEv - 1].End) < 2)
                        foreach (StringMask sm in lastMasks)
                            lastPts.AddRange(sm.Points);
                }
                if (lastPts.Count == 0)
                {
                    lastPts.Add(new ASSPoint { X = -100 * ScaleRate, Y = y0 - 100 * ScaleRate, Brightness = 255 });
                }

                bool next = (iEv + 1 < ass_in.Events.Count) && (Math.Abs(ev.End - ass_in.Events[iEv + 1].Start) < 2);
                bool[] used = new bool[lastPts.Count];
                for (int i = 0; i < used.Length; i++)
                    used[i] = false;

                for (int iK = 0; iK < kelems.Count; iK++)
                {
                    double r = (double)iK / (double)(kelems.Count - 1);
                    double r0 = 1.0 - r;

                    StringMask mask = thisMasks[iK];
                    List<ASSPoint> pts = mask.Points;

                    foreach (ASSPoint pt in pts)
                    {
                        int zz = 0;
                        if (used.Length > 1)
                            zz = Common.RandomInt_Gauss2(rnd, used.Length, (int)(r * (used.Length - 1)));
                        used[zz] = true;
                        ASSPoint srcpt = lastPts[zz];
                        List<ASSPoint> bez_pts = new Bezier(srcpt,
                            new ASSPoint { X = Common.RandomInt_Gauss(rnd, srcpt.X - 30 * ScaleRate, 80 * ScaleRate), Y = Common.RandomInt_Gauss(rnd, srcpt.Y - 50 * ScaleRate, 80 * ScaleRate) },
                            new ASSPoint { X = Common.RandomInt_Gauss(rnd, pt.X + 30 * ScaleRate, 80 * ScaleRate), Y = Common.RandomInt_Gauss(rnd, pt.Y - 50 * ScaleRate, 80 * ScaleRate) }, pt).Create(0.1f);
                        
                        /// for test
                        double f1 = ev.Start - r0 * 1.0;
                        double f0 = f1 - 1;
                        srcpt.End = f0;
                        pt.Color = Common.scaleColor(color1, color2, mask.Y0, PlayResY - MarginBottom, pt.Y);
                        for (int i = 0; i + 1 < bez_pts.Count; i++)
                        {
                            ASSPoint p0 = bez_pts[i];
                            ASSPoint p1 = bez_pts[i + 1];
                            ass_out.Events.Add(CreateMovingPixel(f0, f0 + 0.1, p0.X, p0.Y, p1.X, p1.Y, Common.scaleColor(srcpt.Color,pt.Color, srcpt.End, f0 + 1, f0), "00"));
                            f0 += 0.1;
                        }
                        pt.Start = f0;
                        pt.End = ev.End;
                        pt.Brightness = 255;
                        allPoints.Add(pt);

                        if (!next)
                            ass_out.Events.Add(CreateMovingPixel(pt.End, pt.End + 1, pt.X, pt.Y, Common.RandomInt_Gauss(rnd, pt.X, 80 * ScaleRate), Common.RandomInt_Gauss(rnd, pt.Y, 80 * ScaleRate), pt.Color, "00", 2));
                    }
                }

                for (int i = 0; i < used.Length; i++)
                    if (!used[i])
                    {
                        ASSPoint pt = lastPts[i];
                        if (pt.End > ev.Start - 1.5)
                        {
                            pt.End = ev.Start - 1.5;
                        }
                    }

                Console.WriteLine(iEv);

                lastMasks.Clear();
                lastMasks.AddRange(thisMasks);
            }

            foreach (ASSPoint pt in allPoints)
                ass_out.Events.Add(CreatePixel(pt.Start, pt.End, pt.X, pt.Y, pt.Color, Common.ToHex2(255 - pt.Brightness)));

            ass_out.SaveFiles(@"G:\Workshop\natsume2\op_", 500000);
        }
    }
}
