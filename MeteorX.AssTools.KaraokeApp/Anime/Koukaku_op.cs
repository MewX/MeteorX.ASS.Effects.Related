using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class Koukaku_op : BaseAnime
    {
        public Koukaku_op()
        {
            InFileName = @"G:\Workshop\Koukaku no Regios\op_k.ass";
            OutFileName = @"G:\Workshop\Koukaku no Regios\op.ass";

            this.FontWidth = 30;
            this.FontHeight = 30;
            this.FontSpace = 2;

            this.PlayResX = 848;
            this.PlayResY = 480;
            this.MarginBottom = 15;
            this.MarginLeft = 15;
            this.MarginRight = 15;
            this.MarginTop = 15;

            this.Font = new System.Drawing.Font("DFPOP-SB", 30, System.Drawing.GraphicsUnit.Pixel);

        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS();

            ass_out.Header = ass_in.Header;
            ass_out.Events = new List<ASSEvent>();

            Random rnd = new Random();

            for (int iEv = 0; iEv < ass_in.Events.Count; iEv++)
            {
                ASSEvent ev = ass_in.Events[iEv];
                List<KElement> kelems = ev.SplitK(false);

                /// an7 pos
                int x0 = MarginLeft;
                int y0 = PlayResY - MarginBottom - FontHeight;

                int kSum = 0;

                for (int i = 0; i < kelems.Count; i++)
                {
                    KElement ke = kelems[i];
                    double r = (double)i / (double)(kelems.Count - 1);
                    double r0 = 1.0 - r;
                    Size sz = GetSize(ke.KText);

                    if (Char.IsWhiteSpace(ke.KText[0])) sz.Width = 15;

                    double kStart = kSum * 0.01;
                    double kEnd = kStart + ke.KValue * 0.01;

                    /// an5 pos
                    int x = x0 + FontSpace + sz.Width / 2;
                    int y = y0 + FontHeight / 2;

                    x0 += this.FontSpace + sz.Width;
                    y0 = y0;

                    kSum += ke.KValue;

                    double t0 = ev.Start - r0 * 0.5;
                    double t1 = t0 + 0.2; // 出现
                    double t2 = ev.Start + kStart - 0.1; // 保持框
                    double t3 = t2 + 0.5; // K效果
                    double t4 = ev.End - r0 * 0.5; // 保持
                    double t5 = t4 + 0.2; // 消失

                    double rect_x0 = x - sz.Width / 2 - 2;
                    double rect_y0 = y - FontHeight / 2 - 2;
                    double rect_x1 = x + sz.Width / 2 + 2;
                    double rect_y1 = y + FontHeight / 2 + 2;
                    if (ke.KText.Length > 1)
                    {
                        rect_x0 -= ke.KText.Length * 1;
                        rect_x1 += ke.KText.Length * 1;
                    }
                    if (ke.KText == "with" || ke.KText == "find" || ke.KText == "world")
                    {
                        rect_x0 -= 2;
                        rect_x1 += 1;
                    }
                    if (ke.KText == "DITE")
                    {
                        rect_x0 -= 3;
                        rect_x1 += 3;
                    }
                    if (ke.KText == "alive")
                    {
                        rect_x0 -= 2;
                        rect_x1 += 2;
                    }

                    double rect_xmid = (rect_x0 + rect_x1) * 0.5;
                    double rect_ymid = (rect_y0 + rect_y1) * 0.5;

                    if (ke.KText.Trim().Length > 0)
                    {
                        ass_out.Events.Add(
                            ev.StartReplace(t0).EndReplace(t1).TextReplace(
                            ASSEffect.move(x + 10, y - 10, x, y) +
                            ASSEffect.fad(t1 - t0, 0) +
                            ke.KText));
                        ass_out.Events.Add(
                            CreateRectangle(t0, t1, "draw1", rect_x0, rect_y0, rect_x1, rect_y1, x, y, ASSEffect.move(x + 10, y - 10, x, y) + ASSEffect.fad(t1 - t0, 0))
                            );

                        int xC = 16;
                        int yC = 16;
                        if (ke.KText.Length > 1)
                            xC = (int)(Math.Round((double)yC / (rect_y1 - rect_y0) * (rect_x1 - rect_x0)));
                        switch (iEv)
                        {
                            case 0:
                            case 1:
                                ass_out.Events.Add(
                                    ev.StartReplace(t1).EndReplace(t4).TextReplace(
                                    ASSEffect.pos(x, y) +
                                    ke.KText));
                                ass_out.Events.Add(
                                    CreateRectangle(t1, t2, "draw1", rect_x0, rect_y0, rect_x1, rect_y1)
                                    );

                                ass_out.Events.Add(
                                    CreateRectangle(t2, t3, "draw1", rect_x0, rect_y0, rect_xmid, rect_y1, rect_x0, rect_ymid,
                                    ASSEffect.pos((int)Math.Round(rect_x0), (int)Math.Round(rect_ymid)) + ASSEffect.t(0, t3 - t2, ASSEffect.fry(-90).t()) + ASSEffect.fad(0, t3 - t2)
                                    ));
                                ass_out.Events.Add(
                                    CreateRectangle(t2, t3, "draw1", rect_xmid, rect_y0, rect_x1, rect_y1, rect_x1, rect_ymid,
                                    ASSEffect.pos((int)Math.Round(rect_x1), (int)Math.Round(rect_ymid)) + ASSEffect.t(0, t3 - t2, ASSEffect.fry(90).t()) + ASSEffect.fad(0, t3 - t2)
                                    ));
                                break;
                            case 2:
                                ass_out.Events.Add(
                                    ev.StartReplace(t1).EndReplace(t4).TextReplace(
                                    ASSEffect.pos(x, y) +
                                    ke.KText));
                                ass_out.Events.Add(
                                    CreateRectangle(t1, t2, "draw1", rect_x0, rect_y0, rect_x1, rect_y1)
                                    );

                                ass_out.Events.Add(
                                    CreateRectangle(t2, t3, "draw1", rect_x0, rect_y0, rect_x1, rect_ymid, rect_xmid, rect_y0,
                                    ASSEffect.pos((int)Math.Round(rect_xmid), (int)Math.Round(rect_y0)) + ASSEffect.t(0, t3 - t2, ASSEffect.frx(-90).t()) + ASSEffect.fad(0, t3 - t2)
                                    ));
                                ass_out.Events.Add(
                                    CreateRectangle(t2, t3, "draw1", rect_x0, rect_ymid, rect_x1, rect_y1, rect_xmid, rect_y1,
                                    ASSEffect.pos((int)Math.Round(rect_xmid), (int)Math.Round(rect_y1)) + ASSEffect.t(0, t3 - t2, ASSEffect.frx(90).t()) + ASSEffect.fad(0, t3 - t2)
                                    ));
                                break;
                            case 7:
                            case 8:
                                ass_out.Events.Add(
                                    ev.StartReplace(t1).EndReplace(t4).TextReplace(
                                    ASSEffect.pos(x, y) +
                                    ke.KText));
                                ass_out.Events.Add(
                                    CreateRectangle(t1, t2, "draw1", rect_x0, rect_y0, rect_x1, rect_y1)
                                    );

                                for (int jR = 0; jR < yC; jR++)
                                {
                                    double yy0 = rect_y0 + (rect_y1 - rect_y0) * (double)jR / (double)yC;
                                    double yy1 = yy0 + (rect_y1 - rect_y0) / (double)yC;
                                    double rStart = t2 + jR * 0.05;
                                    if (iEv == 8) rStart = t2 + (yC - jR - 1) * 0.05;
                                    double rEnd = rStart + 0.2;
                                    double yymid = (yy0 + yy1) * 0.5;
                                    ass_out.Events.Add(
                                        CreateRectangle(t2, rStart, "draw1", rect_x0, yy0, rect_x1, yy1)
                                        );
                                    ass_out.Events.Add(
                                        CreateRectangle(rStart, rEnd, "draw1", rect_x0, yy0, rect_x1, yy1, rect_xmid, yymid,
                                        ASSEffect.pos(rect_xmid, yymid) + ASSEffect.fad(0, rEnd - rStart)
                                        ));
                                }
                                break;
                            case 3:
                            case 4:
                            case 5:
                            case 6:
                            default:
                                ass_out.Events.Add(
                                    ev.StartReplace(t1).EndReplace(t2).TextReplace(
                                    ASSEffect.pos(x, y) +
                                    ke.KText));
                                ass_out.Events.Add(
                                    CreateRectangle(t1, t2, "draw1", rect_x0, rect_y0, rect_x1, rect_y1)
                                    );

                                double t3_2 = ev.Start + kEnd;
                                if (t3_2 > t4) t3_2 = t4;
                                for (int iR = 0; iR < (int)(Math.Round((t3_2 - t2) / 0.01)); iR++)
                                {
                                    double rStart = t2 + iR * 0.01;
                                    double rEnd = rStart + 0.01;
                                    ass_out.Events.Add(
                                        ev.StartReplace(rStart).EndReplace(rEnd).TextReplace(
                                        ASSEffect.pos(Common.RandomInt(rnd, x - 3, x + 3), Common.RandomInt(rnd, y - 3, y + 3)) +
                                        ke.KText));
                                }

                                ass_out.Events.Add(
                                    ev.StartReplace(t3_2).EndReplace(t4).TextReplace(
                                    ASSEffect.pos(x, y) +
                                    ke.KText));

                                for (int iR = 0; iR < xC; iR++)
                                    for (int jR = 0; jR < yC; jR++)
                                    {
                                        double xx0 = rect_x0 + (rect_x1 - rect_x0) * (double)iR / (double)xC;
                                        double yy0 = rect_y0 + (rect_y1 - rect_y0) * (double)jR / (double)yC;
                                        double xx1 = xx0 + (rect_x1 - rect_x0) / (double)xC;
                                        double yy1 = yy0 + (rect_y1 - rect_y0) / (double)yC;
                                        double dx = rnd.Next() % 20 + 8;
                                        double dy = rnd.Next() % 20 + 8;
                                        double xx2 = xx0;
                                        double yy2 = yy0;
                                        if (Common.RandomInt_Gauss2(rnd, xC, iR) <= xC / 2) xx2 -= dx; else xx2 += dx;
                                        if (Common.RandomInt_Gauss2(rnd, yC, jR) <= yC / 2) yy2 -= dy; else yy2 += dy;
                                        double rEnd = Common.RandomDouble(rnd, t2 + 0.3, t2 + 1.5);
                                        ass_out.Events.Add(
                                            CreateRectangle(t2, rEnd, "draw1", xx0, yy0, xx1, yy1, xx0, yy0,
                                            ASSEffect.move(xx0, yy0, xx2, yy2) + ASSEffect.fad(0, rEnd - t2)
                                            ));
                                    }
                                break;
                        }

                        ass_out.Events.Add(
                            ev.StartReplace(t4).EndReplace(t5).TextReplace(
                            ASSEffect.move(x, y, x - 10, y + 10) +
                            ASSEffect.fad(0, t5 - t4) +
                            ke.KText));
                    }
                }
            }

            ass_out.SaveFile(OutFileName);
        }
    }
}
