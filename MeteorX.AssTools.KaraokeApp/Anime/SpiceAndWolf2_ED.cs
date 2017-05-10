using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.Model;
using MeteorX.AssTools.KaraokeApp.XXParticle;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class SpiceAndWolf2_ED : BaseAnime2
    {
        public SpiceAndWolf2_ED()
        {
            this.FontHeight = 30;
            this.FontSpace = 2;
            this.IsAvsMask = true;
            this.MarginLeft = 17;
            this.MarginBottom = 17;
            this.MarginTop = 17;
            this.MarginRight = 17;
            this.PlayResX = 848;
            this.PlayResY = 480;
            this.InFileName = @"G:\Workshop\spice and wolf2\ed\ed_k.ass";
            this.OutFileName = @"G:\Workshop\spice and wolf2\ed\ed.ass";
            this.WhitespaceWidth = 14;
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS() { Header = ass_in.Header, Events = new List<ASSEvent>() };

            for (int iEv = 0; iEv < ass_in.Events.Count; iEv++)
            {
                bool isJp = true;
                this.MaskStyle = isJp ?
                    "Style: Default,DFGMaruMoji-SL,28,&H00FFFFFF,&HFFFFFFFF,&HFFFFFFFF,&HFFFFFFFF,0,0,0,0,100,100,0,0,1,0,0,5,0,0,0,128" :
                    "Style: Default,仿宋,36,&H00FFFFFF,&HFFFFFFFF,&HFFFFFFFF,&HFFFFFFFF,0,0,0,0,100,100,0,0,1,0,0,5,0,0,0,1";
                this.FontHeight = isJp ? 28 : 30;
                int jEv = isJp ? iEv : iEv;
                //if (jEv != 0) continue;
                ASSEvent ev = ass_in.Events[iEv];
                List<KElement> kelems = ev.SplitK(true);
                int totalWidth = GetTotalWidth(ev);
                int x0 = (PlayResX - MarginLeft - MarginRight - totalWidth) / 2 + MarginLeft;
                int y0 = (isJp) ? (PlayResY - MarginBottom - FontHeight) : MarginTop;
                int kSum = 0;
                int x0_start = x0;
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
                    string evStyle = isJp ? "ed_jp" : "ed_cn";

                    string mainColor = "505E66";

                    double t0 = ev.Start - 0.9 + (x - x0_start) / 848.0 * 3.0;
                    double t1 = t0 + 0.5;
                    double t2 = (ke.IsSplit ? ke.KStart_NoSplit : kStart) - 0.3;
                    double t25 = t2 + 0.6;
                    double t3 = ev.End - 0.9 + (x - x0_start) / 848.0 * 3.0;
                    double t4 = t3 + 0.8;

                    ass_out.AppendEvent(50, evStyle, t0, t1,
                        fad((t1 - t0), 0) + pos(x, y) +
                        a(1, "00") + c(1, mainColor) +
                        fsc(1, 1) + t(0, 0.35, fsc(125, 125).t()) + t(0.35, 0.5, fsc(100, 100).t()) +
                        ke.KText);
                    ass_out.AppendEvent(50, evStyle, t1, t2,
                        pos(x, y) +
                        a(1, "00") + c(1, mainColor) +
                        ke.KText);

                    double[] tl = { t2, t2 + 0.15, t2 + 0.3, t2 + 0.45, t2 + 0.6 };
                    int[] xl = { 100, 80, 100, 115, 100 };
                    int[] yl = { 100, 120, 100, 80, 100 };
                    double[] yo = { 0, -10, 0, 2, 0 };

                    for (int i = 0; i < tl.Length - 1; i++)
                    {
                        ass_out.AppendEvent(50, evStyle, tl[i], tl[i + 1],
                            move(x, y + yo[i], x, y + yo[i + 1]) +
                            a(1, "00") + c(1, mainColor) +
                            fsc(xl[i], yl[i]) +
                            t(fsc(xl[i + 1], yl[i + 1]).t()) +
                            ke.KText);
                    }

                    ass_out.AppendEvent(50, evStyle, t25, t3,
                        pos(x, y) +
                        a(1, "00") + c(1, mainColor) +
                        ke.KText);
                    /*
                    ass_out.AppendEvent(50, evStyle, t3, t4,
                        fad(0, (t4 - t3)) + pos(x, y) +
                        a(1, "00") + c(1, mainColor) +
                        t(0, 0.15, fsc(125, 125).t()) + t(0.15, 0.5, fsc(1, 1).t()) +
                        ke.KText);
                     * */

                    ass_out.AppendEvent(50, evStyle, t3, t3 + 0.5,
                        pos(x, y) + a(1, "00") + c(1, mainColor) +
                        t(0, 0.15, fsc(125, 125).t()) + t(0.15, 0.5, fsc(0, 0).t()) +
                        ke.KText);

                    continue;

                    //原本的矩形
                    double orgrec_x0 = x - sz.Width / 2 - 1;
                    double orgrec_x1 = x + sz.Width / 2 + 1;
                    double orgrec_y0 = y - FontHeight / 2 - 1;
                    double orgrec_y1 = y + FontHeight / 2 + 1;
                    //随机产生两个切割四边形
                    List<ASSPointF> cut0 = new List<ASSPointF>();
                    List<ASSPointF> cut1 = new List<ASSPointF>();
                    if (true)//(Common.RandomBool(rnd, 0.5))
                    {
                        //竖直方向
                        double cutx0 = Common.RandomDouble_Gauss(rnd, orgrec_x0, orgrec_x1, 2);
                        double cutx1 = Common.RandomDouble_Gauss(rnd, orgrec_x0, orgrec_x1, 2);
                        cut0.Add(new ASSPointF { X = orgrec_x0, Y = orgrec_y0 });
                        cut0.Add(new ASSPointF { X = cutx0, Y = orgrec_y0 });
                        cut0.Add(new ASSPointF { X = cutx1, Y = orgrec_y1 });
                        cut0.Add(new ASSPointF { X = orgrec_x0, Y = orgrec_y1 });
                        cut1.Add(new ASSPointF { X = cutx0, Y = orgrec_y0 });
                        cut1.Add(new ASSPointF { X = orgrec_x1, Y = orgrec_y0 });
                        cut1.Add(new ASSPointF { X = orgrec_x1, Y = orgrec_y1 });
                        cut1.Add(new ASSPointF { X = cutx1, Y = orgrec_y1 });
                    }
                    List<List<ASSPointF>> cutList = new List<List<ASSPointF>>();
                    cutList.Add(cut0);
                    cutList.Add(cut1);

                    foreach (List<ASSPointF> cut in cutList)
                    {
                        //累积旋转的角度
                        double rot_x = Common.RandomDouble(rnd, 0, Math.PI * 2) * Common.RandomSig(rnd);
                        double rot_y = Common.RandomDouble(rnd, 0, Math.PI * 2) * Common.RandomSig(rnd);
                        double rot_z = Common.RandomDouble(rnd, 0, Math.PI * 2) * Common.RandomSig(rnd);

                        Func<double, double> frotx = ti => (ti - t3) / (t4 - t3) * rot_x;
                        Func<double, double> froty = ti => (ti - t3) / (t4 - t3) * rot_y;
                        Func<double, double> frotz = ti => (ti - t3) / (t4 - t3) * rot_z;
                        Func<double, int> f1 = ti => (int)(ti / 2.0 / Math.PI * 360.0);

                        double jumpTime = t4 - t3;
                        Func<double, double> forgy = ti => y - 300.0 * (0.25 * jumpTime * jumpTime - ((ti - t3) - 0.5 * jumpTime) * ((ti - t3) - 0.5 * jumpTime));
                        double orgx1 = Common.RandomDouble(rnd, 20, 40) * (cut == cut0 ? -1 : 1) + x;
                        Func<double, double> forgx = ti => (ti - t3) / jumpTime * (orgx1 - x) + x;
                        for (double ti = t3; ti <= t4; ti += 0.04)
                        {
                            double rotx = frotx(ti);
                            double roty = froty(ti);
                            double rotz = frotz(ti);

                            double org_x = forgx(ti);
                            double org_y = forgy(ti);

                            List<ASSPointF> newCut = new List<ASSPointF>();
                            foreach (ASSPointF pt in cut)
                            {
                                double caz = Math.Cos(rotz);
                                double saz = Math.Sin(rotz);
                                double cax = Math.Cos(rotx);
                                double sax = Math.Sin(rotx);
                                double cay = Math.Cos(roty);
                                double say = Math.Sin(roty);

                                double rx = pt.X - x;
                                double ry = pt.Y - y;
                                double rz = 0;

                                double rxx = rx * caz + ry * saz;
                                double ryy = -(rx * saz - ry * caz);
                                double rzz = rz;

                                rx = rxx;
                                ry = ryy * cax + rzz * sax;
                                rz = ryy * sax - rzz * cax;

                                rxx = rx * cay + rz * say;
                                ryy = ry;
                                rzz = rx * say - rz * cay;

                                rzz = Math.Max(rzz, -19000);

                                rx = (rxx * 20000) / (rzz + 20000);
                                ry = (ryy * 20000) / (rzz + 20000);

                                newCut.Add(new ASSPointF
                                {
                                    X = rx + org_x,
                                    Y = ry + org_y
                                });
                            }

                            ass_out.AppendEvent(50, evStyle, ti, ti + 0.04,
                                pos(org_x, org_y) + clip(1, Pts2AssVec(newCut)) +
                                a(1, Common.scaleAlpha("00", "FF", (ti - t3) / (t4 - t3))) + c(1, mainColor) +
                                frx(f1(rotx)) + fry(f1(roty)) + frz(f1(rotz)) +
                                ke.KText);


                            /*
                            ass_out.AppendEvent(50, evStyle, ti, ti + 0.04,
                                pos(org_x, org_y) + a(1, "00") +
                                frx(f1(rotx)) + fry(f1(roty)) + frz(f1(rotz)) +
                                ke.KText);
                            ass_out.AppendEvent(51, "pt", ti, ti + 0.04,
                                pos(0, 0) + a(1, "44") +
                                p(1) + Pts2AssVec(newCut));
                             * */
                        }
                    }

                    /*
                    ass_out.AppendEvent(100, evStyle, t3, t4,
                        clip(1, Pts2AssVec(cut0)) + pos(x, y) + a(1, "00") + c(1, mainColor) +
                        ke.KText);
                    ass_out.AppendEvent(100, evStyle, t3, t4,
                        clip(1, Pts2AssVec(cut1)) + pos(x, y) + a(1, "00") + c(1, mainColor) +
                        ke.KText);
                     * */
                }

            }

            Console.WriteLine(ass_out.Events.Count);
            ass_out.SaveFile(this.OutFileName);
        }
    }
}
