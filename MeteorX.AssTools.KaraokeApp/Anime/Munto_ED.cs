using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class Munto_ED : BaseAnime2
    {
        public Munto_ED()
        {
            InFileName = @"G:\Workshop\munto\ed_k.ass";
            OutFileName = @"G:\Workshop\munto\ed.ass";

            this.FontWidth = 30;
            this.FontHeight = 30;
            this.FontSpace = -1;

            this.PlayResX = 848;
            this.PlayResY = 480;
            this.MarginBottom = 15;
            this.MarginLeft = 15;
            this.MarginRight = 15;
            this.MarginTop = 15;

            this.Font = new System.Drawing.Font("DFGMaruGothic-Md", 30, GraphicsUnit.Pixel);
            this.IsAvsMask = true;
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS();

            ass_out.Header = ass_in.Header;
            ass_out.Events = new List<ASSEvent>();

            string ms1 = "Style: Default,DFGMaruGothic-Md,30,&H00FF0000,&HFF600D00,&H000000FF,&HFF0A5A84,-1,0,0,0,100,100,0,0,0,2,0,5,20,20,20,128";
            string ms3 = "Style: Default,DFGMaruGothic-Md,30,&H000000FF,&HFF600D00,&H00FF0000,&HFF0A5A84,-1,0,0,0,100,100,0,0,0,2,0,5,20,20,20,128";
            string ptstr = @"{\p1}m 0 0 l 1 0 1 1 0 1";
            string pt0Str = @"{\blur2\bord3\p4}m 5 5 s 5 -5 -5 -5 -5 5";

            Random rnd = new Random();
            InitBFS();

            for (int iEv = 0; iEv < ass_in.Events.Count; iEv++)
            {
                //if (iEv != 0) continue;
                ASSEvent ev = ass_in.Events[iEv];
                List<KElement> kelems = ev.SplitK(false);

                this.MaskStyle = ms3;
                double sw = (iEv % 2 == 0) ? 0 : GetTotalWidth(ev);
                /// an7 pos
                int x0 = (iEv % 2 == 0) ? MarginLeft : (int)(PlayResX - MarginRight - sw);
                int startx0 = x0;
                int y0 = PlayResY - MarginBottom - FontHeight;

                int kSum = 0;

                int lum0count = 0;
                List<double> lum0x = new List<double>();
                List<double> lum0y = new List<double>();
                for (int lumx0 = -20; lumx0 < PlayResX; )
                {
                    lumx0 += Common.RandomInt(rnd, 5, 10);
                    lum0x.Add(lumx0);
                    lum0y.Add(Common.RandomInt(rnd, PlayResY - MarginBottom - FontHeight - 2, PlayResY - MarginBottom + 2));
                    lum0count++;
                }

                int lumcount = 0;
                List<double> lumx = new List<double>();
                List<double> lumy = new List<double>();
                for (int lumx0 = -20; lumx0 < PlayResX; )
                {
                    lumx0 += Common.RandomInt(rnd, 25, 45);
                    lumx.Add(lumx0);
                    lumy.Add(Common.RandomInt(rnd, PlayResY - MarginBottom - FontHeight - 10, PlayResY - MarginBottom + 10));
                    lumcount++;
                }

                string[] lumcol =
                {
                    "003DB8",
                    "003DB8",
                    "000000",
                    "B54F00",
                    "0068B8",
                    "B88000"
                };

                string[] lum3 =
                {
                    "00B88F", //23:49.678
                    "003DB8", //23:50.429
                    "5972B8", //23:51.931
                    "B86459" //23:54.516
                };
                double[] lum3t =
                {
                    23 * 60 + 49 + 0.65,
                    23 * 60 + 50 + 0.40,
                    23 * 60 + 51 + 0.91,
                    23 * 60 + 54 + 0.49
                };

                string[] lum4 =
                {
                    "003DB8" //24:06.528
                };
                double[] lum4t =
                {
                    24 * 60 + 6 + 0.50
                };


                for (int iK = 0; iK < kelems.Count; iK++)
                {
                    //if (iK > 3) continue;
                    Console.WriteLine("{0} / {1} : {2} / {3}", iEv + 1, ass_in.Events.Count, iK + 1, kelems.Count);
                    KElement ke = kelems[iK];
                    double r = (double)iK / (double)(kelems.Count - 1);
                    this.MaskStyle = ms3;
                    StringMask mask = GetMask(ke.KText, x0, y0);
                    Size sz = new Size(mask.Width, mask.Height);

                    double kStart = ev.Start + kSum * 0.01;
                    double kEnd = kStart + ke.KValue * 0.01;
                    kSum += ke.KValue;

                    /// an5 pos
                    int x = x0 + this.FontSpace + sz.Width / 2;
                    int y = y0 + FontHeight / 2;

                    this.MaskStyle = ms1;
                    mask = GetMask(ke.KText, x, y);

                    int bakx0 = x0;

                    x0 += this.FontSpace + sz.Width;
                    y0 = y0;

                    if (ke.KText.Trim().Length == 0) continue;

                    double t0 = ev.Start - 1.0 + r * 2;
                    double t1 = t0 + 1;
                    double t11 = kStart - 0.4;
                    double t2 = ev.End;

                    ass_out.Events.Add(
                        ev.StartReplace(t11).EndReplace(t11 + 0.8).TextReplace(
                        ASSEffect.pos(x, y) + ASSEffect.a(1, "00") + ASSEffect.a(3, "00") + ASSEffect.c(1, "FFFFFF") + ASSEffect.c(3, "FFFFFF") +
                        ASSEffect.fad(0.2, 0.6) + ASSEffect.bord(0) + ASSEffect.blur(0) +
                        ASSEffect.t(0, 1, ASSEffect.bord(5).t() + ASSEffect.blur(5).t()) +
                        ke.KText));
                    ass_out.Events.Add(
                        ev.StartReplace(t11 + 0.4).EndReplace(t2 + 0.5).TextReplace(
                        ASSEffect.pos(x, y) + ASSEffect.a(1, "00") + ASSEffect.a(3, "00") + ASSEffect.c(1, "FFFFFF") + ASSEffect.c(3, "FFFFFF") +
                        ASSEffect.fad(0.4, 1) + ASSEffect.bord(2) + ASSEffect.blur(2) +
                        ke.KText));

                    if (iEv == 3)
                        ass_out.Events.Add(
                            ev.StartReplace(ev.Start + 0.5).EndReplace(ev.Start + 2.0).LayerReplace(20).TextReplace(
                            ASSEffect.pos(x, y) + ASSEffect.a(1, "00") + ASSEffect.a(3, "00") + ASSEffect.c(1, "FFFFFF") + ASSEffect.c(3, "FFFFFF") +
                            ASSEffect.fad(0, 0.5) + ASSEffect.bord(9) + ASSEffect.blur(8) +
                            ke.KText));
                    if (iEv == 5)
                        ass_out.Events.Add(
                            ev.StartReplace(ev.End).EndReplace(ev.End + 1.5).LayerReplace(20).TextReplace(
                            ASSEffect.pos(x, y) + ASSEffect.a(1, "00") + ASSEffect.a(3, "00") + ASSEffect.c(1, "FFFFFF") + ASSEffect.c(3, "FFFFFF") +
                            ASSEffect.fad(0.3, 0.5) + ASSEffect.bord(6) + ASSEffect.blur(6) +
                            ke.KText));

                    /*for (int i = 1; i <= 5; i++)
                    {
                        ass_out.Events.Add(
                            ev.StartReplace(kStart + 0).EndReplace(t2 + 0.5).TextReplace(
                            ASSEffect.pos(x, y) + ASSEffect.a(1, "00") + ASSEffect.a(3, "FF") + ASSEffect.c(1, "FFFFFF") +
                            ASSEffect.fad(0.4, 1) + ASSEffect.bord(2) + ASSEffect.blur(2) +
                            ke.KText));
                    }*/

                    int[] ind = CalculateBFSOrder(mask);
                    double[] wt = new double[mask.Points.Count];
                    double[] wt0 = new double[mask.Points.Count];
                    for (int i = 0; i < ind.Length; i++)
                    {
                        ASSPoint pt = mask.Points[i];
                        double ag = Common.GetAngle(pt.X, pt.Y, x, y);
                        double r0 = Common.GetDistance(pt.X, pt.Y, x, y) * 1.0;
                        double r1 = Common.GetDistance(pt.X, pt.Y, x, y) * 1.3;
                        //double r2 = Common.GetDistance(pt.X, pt.Y, x, y) * 1.1;
                        double ptx0 = pt.X;// (double)x + Math.Cos(ag) * r0;
                        double pty0 = pt.Y;// (double)y - Math.Sin(ag) * r0;
                        double ptx = (double)x + Math.Cos(ag) * r1;
                        double pty = (double)y - Math.Sin(ag) * r1;
                        //double ptx2 = (double)x + Math.Cos(ag) * r2;
                        //double pty2 = (double)y - Math.Sin(ag) * r2;
                        string bt = Common.ToHex2((255 - pt.Brightness) * Common.RandomDouble(rnd, 0.7, 0.9));
                        string bt2 = bt;
                        string bt3 = Common.ToHex2((255 - pt.Brightness * 0.8) * Common.RandomDouble(rnd, 0.7, 0.9));
                        double pt2 = kStart + (kEnd - kStart) * (pt.X - bakx0) / mask.Width + Common.RandomDouble_Gauss(rnd, -0.15, 0.00, 2);
                        wt[i] = 1;
                        for (int j = 0; j < lumcount; j++)
                        {
                            double dis = Common.GetDistance(pt.X, pt.Y, lumx[j], lumy[j]) / 30.0;
                            if (wt[i] > dis) wt[i] = dis;
                        }
                        wt0[i] = 1;
                        for (int j = 0; j < lum0count; j++)
                        {
                            double dis = Common.GetDistance(pt.X, pt.Y, lum0x[j], lum0y[j]) / 10.0;
                            if (wt0[i] > dis) wt0[i] = dis;
                        }
                        string col0 = Common.scaleColor("222222", "FFFFFF", wt0[i]);
                        if (iEv == 2) col0 = Common.scaleColor("444444", "FFFFFF", wt0[i]);
                        if (iEv >= 3) col0 = Common.scaleColor("777777", "FFFFFF", wt0[i]);
                        double pt3 = t2 + wt[i] + Common.RandomDouble_Gauss(rnd, -0.08, 0.08, 2);

                        if (iEv >= 2)
                            ass_out.Events.Add(
                                ev.StartReplace(pt2 - 0.1).EndReplace(pt2).StyleReplace("pt").LayerReplace(15).TextReplace(
                                ASSEffect.a(1, "CC") + ASSEffect.c(1, "FFFFFF") + ASSEffect.a(3, "CC") + ASSEffect.c(1, "FFFFFF") +
                                ASSEffect.bord(1) + ASSEffect.be(1) +
                                ASSEffect.pos(ptx0, pty0) + ptstr));

                        if (iEv != 3 && iEv != 4)
                        {
                            string ptc = Common.scaleColor("FFFFFF", lumcol[iEv], wt[i]);
                            ass_out.Events.Add(
                                ev.StartReplace(t0).EndReplace(t1).StyleReplace("pt").TextReplace(
                                ASSEffect.a(1, bt) + ASSEffect.c(1, col0) + ASSEffect.fad(0.1, 0) + ASSEffect.a(3, "FF") +
                                ASSEffect.move(ptx, pty, ptx0, pty0) + ptstr));
                            ass_out.Events.Add(
                                ev.StartReplace(t1).EndReplace(pt2 + 0.2).StyleReplace("pt").TextReplace(
                                ASSEffect.a(1, bt) + ASSEffect.c(1, col0) + ASSEffect.a(3, "FF") +
                                ASSEffect.fad(0, 0.3) +
                                ASSEffect.pos(ptx0, pty0) + ptstr));
                            ass_out.Events.Add(
                                ev.StartReplace(pt2 - 0.1).EndReplace(pt3).StyleReplace("pt").TextReplace(
                                ASSEffect.a(1, bt2) + ASSEffect.c(1, ptc) + ASSEffect.a(3, "FF") +
                                ASSEffect.fad(0.3, 0.3) +
                                ASSEffect.pos(ptx0, pty0) + ptstr));
                        }
                        else if (iEv == 3)
                        {
                            for (int j = 0; j <= lum3.Length; j++)
                            {
                                string ptc = Common.scaleColor("FFFFFF", (j > 0) ? lum3[j - 1] : lumcol[iEv], wt[i]);
                                double tlo = (j > 0) ? lum3t[j - 1] : t0;
                                double thi = (j < lum3.Length) ? lum3t[j] : pt3;
                                Func<double, double> flo = xx => (xx < tlo) ? tlo : xx;
                                Func<double, double> fhi = xx => (xx > thi) ? thi : xx;
                                ass_out.Events.Add(
                                    ev.StartReplace(flo(t0)).EndReplace(fhi(t1)).StyleReplace("pt").TextReplace(
                                    ASSEffect.a(1, bt) + ASSEffect.c(1, col0) + ASSEffect.fad((j == 0) ? 0.1 : 0, 0) + ASSEffect.a(3, "FF") +
                                    ASSEffect.move(ptx, pty, ptx0, pty0) + ptstr));
                                ass_out.Events.Add(
                                    ev.StartReplace(flo(t1)).EndReplace(fhi(pt2 + 0.2)).StyleReplace("pt").TextReplace(
                                    ASSEffect.a(1, bt) + ASSEffect.c(1, col0) + ASSEffect.a(3, "FF") +
                                    ASSEffect.fad(0, (j == lum3.Length) ? 0.3 : 0) +
                                    ASSEffect.pos(ptx0, pty0) + ptstr));
                                ass_out.Events.Add(
                                    ev.StartReplace(flo(pt2 - 0.1)).EndReplace(fhi(pt3)).StyleReplace("pt").TextReplace(
                                    ASSEffect.a(1, bt) + ASSEffect.c(1, ptc) + ASSEffect.a(3, "FF") +
                                    ASSEffect.fad((j == 0) ? 0.3 : 0, (j == lum3.Length) ? 0.3 : 0) +
                                    ASSEffect.pos(ptx0, pty0) + ptstr));
                            }
                        }
                        else if (iEv == 4)
                        {
                            for (int j = 0; j <= lum4.Length; j++)
                            {
                                string ptc = Common.scaleColor("FFFFFF", (j > 0) ? lum4[j - 1] : lumcol[iEv], wt[i]);
                                double tlo = (j > 0) ? lum4t[j - 1] : t0;
                                double thi = (j < lum4.Length) ? lum4t[j] : pt3;
                                Func<double, double> flo = xx => (xx < tlo) ? tlo : xx;
                                Func<double, double> fhi = xx => (xx > thi) ? thi : xx;
                                ass_out.Events.Add(
                                    ev.StartReplace(flo(t0)).EndReplace(fhi(t1)).StyleReplace("pt").TextReplace(
                                    ASSEffect.a(1, bt) + ASSEffect.c(1, col0) + ASSEffect.fad((j == 0) ? 0.1 : 0, 0) + ASSEffect.a(3, "FF") +
                                    ASSEffect.move(ptx, pty, ptx0, pty0) + ptstr));
                                ass_out.Events.Add(
                                    ev.StartReplace(flo(t1)).EndReplace(fhi(pt2 + 0.2)).StyleReplace("pt").TextReplace(
                                    ASSEffect.a(1, bt) + ASSEffect.c(1, col0) + ASSEffect.a(3, "FF") +
                                    ASSEffect.fad(0, (j == lum4.Length) ? 0.3 : 0) +
                                    ASSEffect.pos(ptx0, pty0) + ptstr));
                                ass_out.Events.Add(
                                    ev.StartReplace(flo(pt2 - 0.1)).EndReplace(fhi(pt3)).StyleReplace("pt").TextReplace(
                                    ASSEffect.a(1, bt) + ASSEffect.c(1, ptc) + ASSEffect.a(3, "FF") +
                                    ASSEffect.fad((j == 0) ? 0.3 : 0, (j == lum4.Length) ? 0.3 : 0) +
                                    ASSEffect.pos(ptx0, pty0) + ptstr));
                            }
                        }

                        if (iEv == 2 && Common.RandomBool(rnd, 0.05))
                        {
                            ass_out.Events.Add(
                                ev.StartReplace(pt2).EndReplace(pt2 + 1.5).StyleReplace("pt").LayerReplace(13).TextReplace(
                                ASSEffect.move(ptx0, pty0, ptx0, pty0 + FontHeight + MarginBottom) +
                                ASSEffect.fad(0, 0.6) + ASSEffect.a(1, "CC") + ASSEffect.c(1, "FFFFFF") +
                                ASSEffect.a(3, "00") + ASSEffect.c(3, "FFFFFF") +
                                ASSEffect.t(0, Common.RandomDouble(rnd, 1.5, 3), ASSEffect.fry(-720).t()) +
                                ASSEffect.bord(2) + ASSEffect.blur(2) + CreatePolygon(rnd, 30, 30, 4)));
                        }
                    }

                    if (iEv == 3)
                    {
                        for (int i = 0; i < (int)(Math.Round((kEnd - kStart) * 50)); i++)
                        {
                            ASSPoint pt = new ASSPoint { X = Common.RandomInt(rnd, startx0, startx0 + (int)sw), Y = Common.RandomInt(rnd, y0, y0 + FontHeight) };
                            double pt0 = Common.RandomDouble(rnd, ev.Start, ev.Start + 1.62);
                            double pt1 = pt0 + 0.5;
                            ass_out.Events.Add(
                                ev.StartReplace(pt0).EndReplace(pt1).StyleReplace("pt").TextReplace(
                                ASSEffect.move(pt.X, pt.Y, Common.RandomInt(rnd, pt.X - 30, pt.X - 50), Common.RandomInt(rnd, pt.Y - 35, pt.Y - 55)) +
                                ASSEffect.fad(0, 0.3) + ASSEffect.a(1, "CC") + ASSEffect.c(1, "FFFFFF") +
                                ASSEffect.a(3, "00") + ASSEffect.c(3, "FFFFFF") +
                                ASSEffect.t(0, Common.RandomDouble(rnd, 0.5, 1.5), ASSEffect.frz(Common.RandomInt(rnd, -500, 500)).t() + ASSEffect.fry(Common.RandomInt(rnd, -500, 500)).t() + ASSEffect.frx(Common.RandomInt(rnd, -500, 500)).t()) +
                                ASSEffect.bord(3) + ASSEffect.blur(2) + CreatePolygon(rnd, 10, 25, Common.RandomInt(rnd, 5, 7))));
                        }
                        for (int i = 0; i < (int)(Math.Round((kEnd - kStart) * 200)); i++)
                        {
                            ASSPoint pt = mask.Points[Common.RandomInt(rnd, 0, mask.Points.Count - 1)];
                            double pt0 = Common.RandomDouble(rnd, kStart, kEnd);
                            double pt1 = pt0 + 1.5;
                            string ptc = lumcol[iEv];
                            for (int j = 0; j < lum3t.Length; j++)
                                if (pt0 >= lum3t[j] - 0.5) ptc = lum3[j];
                            ptc = Common.scaleColor(ptc, "FFFFFF", 0.75);
                            ass_out.Events.Add(
                                ev.StartReplace(pt0).EndReplace(pt1).StyleReplace("pt").LayerReplace(5).TextReplace(
                                ASSEffect.move(pt.X, pt.Y, Common.RandomInt(rnd, pt.X + 50, pt.X - 50), Common.RandomInt(rnd, pt.Y + 35, pt.Y - 35)) +
                                ASSEffect.fad(0, 0.3) + ASSEffect.a(1, "00") + ASSEffect.c(1, "FFFFFF") +
                                ASSEffect.a(3, "00") + ASSEffect.c(3, ptc) +
                                ASSEffect.bord(2) + ASSEffect.blur(2) + CreatePolygon(rnd, 15, 15, 6)));
                        }
                    }

                    if (iEv == 4)
                    {
                        for (int i = 0; i < (int)(Math.Round((kEnd - kStart) * 200)); i++)
                        {
                            ASSPoint pt = mask.Points[Common.RandomInt(rnd, 0, mask.Points.Count - 1)];
                            double pt0 = Common.RandomDouble(rnd, kStart, kEnd);
                            double pt1 = pt0 + 1.5;
                            string ptc = lumcol[iEv];
                            if (pt0 >= lum4t[0] - 0.5) ptc = lum4[0];
                            ptc = Common.scaleColor(ptc, "FFFFFF", 0.65);
                            ass_out.Events.Add(
                                ev.StartReplace(pt0).EndReplace(pt1).StyleReplace("pt").LayerReplace(5).TextReplace(
                                ASSEffect.move(pt.X, pt.Y, Common.RandomInt(rnd, pt.X + 50, pt.X - 50), Common.RandomInt(rnd, pt.Y + 35, pt.Y - 35)) +
                                ASSEffect.fad(0, 0.3) + ASSEffect.a(1, "00") + ASSEffect.c(1, "FFFFFF") +
                                ASSEffect.a(3, "00") + ASSEffect.c(3, ptc) +
                                ASSEffect.bord(2) + ASSEffect.blur(2) + CreatePolygon(rnd, 15, 15, 6)));
                        }
                    }

                    if (iEv == 5)
                    {
                        for (int i = 0; i < 100; i++)
                        {
                            ASSPoint pt = new ASSPoint { X = Common.RandomInt(rnd, startx0, startx0 + (int)sw), Y = Common.RandomInt(rnd, y0, y0 + FontHeight) };
                            double pt0 = Common.RandomDouble(rnd, ev.End, ev.End + 1.8);
                            double pt1 = pt0 + 0.5;
                            string ptc = Common.scaleColor(lumcol[iEv], "FFFFFF", 0.9);
                            ass_out.Events.Add(
                                ev.StartReplace(pt0).EndReplace(pt1).StyleReplace("pt").TextReplace(
                                ASSEffect.move(pt.X, pt.Y, Common.RandomInt(rnd, pt.X - 30, pt.X - 50), Common.RandomInt(rnd, pt.Y - 35, pt.Y - 55)) +
                                ASSEffect.fad(0, 0.3) + ASSEffect.a(1, "CC") + ASSEffect.c(1, "FFFFFF") +
                                ASSEffect.a(3, "00") + ASSEffect.c(3, ptc) +
                                ASSEffect.t(0, Common.RandomDouble(rnd, 0.5, 1.5), ASSEffect.frz(Common.RandomInt(rnd, -500, 500)).t() + ASSEffect.fry(Common.RandomInt(rnd, -500, 500)).t() + ASSEffect.frx(Common.RandomInt(rnd, -500, 500)).t()) +
                                ASSEffect.bord(3) + ASSEffect.blur(2) + CreatePolygon(rnd, 10, 25, Common.RandomInt(rnd, 5, 7))));
                        }
                        for (int i = 0; i < (int)(Math.Round((kEnd - kStart) * 200)); i++)
                        {
                            ASSPoint pt = mask.Points[Common.RandomInt(rnd, 0, mask.Points.Count - 1)];
                            double pt0 = Common.RandomDouble(rnd, kStart, kEnd);
                            double pt1 = pt0 + 1.5;
                            string ptc = lumcol[iEv];
                            ptc = Common.scaleColor(ptc, "FFFFFF", 0.75);
                            ass_out.Events.Add(
                                ev.StartReplace(pt0).EndReplace(pt1).StyleReplace("pt").LayerReplace(5).TextReplace(
                                ASSEffect.move(pt.X, pt.Y, Common.RandomInt(rnd, pt.X + 50, pt.X - 50), Common.RandomInt(rnd, pt.Y + 35, pt.Y - 35)) +
                                ASSEffect.fad(0, 0.3) + ASSEffect.a(1, "00") + ASSEffect.c(1, "FFFFFF") +
                                ASSEffect.a(3, "00") + ASSEffect.c(3, ptc) +
                                ASSEffect.bord(2) + ASSEffect.blur(2) + CreatePolygon(rnd, 15, 15, 6)));
                        }
                    }

                }

            }

            ass_out.SaveFile(OutFileName);
            Console.WriteLine("Lines : {0}", ass_out.Events.Count);
        }
    }
}
