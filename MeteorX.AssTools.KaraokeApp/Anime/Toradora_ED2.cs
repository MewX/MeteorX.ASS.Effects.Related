using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class Toradora_ED2 : BaseAnime
    {
        public Toradora_ED2()
        {
            InFileName = @"G:\Workshop\toradora\ed2_k.ass";
            OutFileName = @"G:\Workshop\toradora\ed2.ass";

            this.FontWidth = 30;
            this.FontHeight = 30;
            this.FontSpace = 0;

            this.PlayResX = 848;
            this.PlayResY = 480;
            this.MarginBottom = 10;
            this.MarginLeft = 15;
            this.MarginRight = 15;
            this.MarginTop = 15;

            this.Font = new System.Drawing.Font("ＤＦＰまるもじ体W3", 30, GraphicsUnit.Pixel);
        }

        public int GetX(int orgX, double startTime, double nowTime, double speed)
        {
            return orgX + (int)(Math.Round(speed * (nowTime - startTime)));
        }

        public int GetX_orgX;
        public double GetX_StartTime;
        public double GetX_Speed;

        public int GetX(double nowTime)
        {
            return GetX(GetX_orgX, GetX_StartTime, nowTime, GetX_Speed);
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS();

            ass_out.Header = ass_in.Header;
            ass_out.Events = new List<ASSEvent>();

            for (int iEv = 0; iEv < ass_in.Events.Count; iEv++)
            {
                ASSEvent ev = ass_in.Events[iEv];
                List<KElement> kelems = ev.SplitK(false);

                /// an7 pos
                int x0 = (PlayResX - GetTotalWidth(ev)) / 2;
                int y0 = PlayResY - MarginBottom - FontHeight;

                Random rnd = new Random();

                int kSum = 0;

                //ASSColor col1 = Common.RandomColor(rnd, 1, new ASSColor { A = 0, R = 20, B = 20, G = 20 }, new ASSColor { A = 0, R = 235, B = 235, G = 235 });
                //ASSColor col2 = Common.RandomColor(rnd, 1, new ASSColor { A = 0, R = col1.R - 40, B = col1.B - 40, G = col1.G - 40 }, new ASSColor { A = 0, R = col1.R + 40, B = col1.B + 40, G = col1.G + 40 });
                ASSColor col1 = Common.RandomColor(rnd, 1);
                ASSColor col2 = Common.RandomColor(rnd, 1);

                while (Math.Abs(col1.R - col2.R) + Math.Abs(col1.G - col2.G) + Math.Abs(col1.B - col2.B) < 100)
                {
                    col1 = Common.RandomColor(rnd, 1);
                    col2 = Common.RandomColor(rnd, 1);
                }

                for (int iK = 0; iK < kelems.Count; iK++)
                {
                    KElement ke = kelems[iK];
                    double r = (double)iK / (double)(kelems.Count - 1);
                    Size sz = GetSize(ke.KText);

                    string colStr = Common.scaleColor(col1, col2, r);

                    /// an5 pos
                    int x = x0 + this.FontSpace + sz.Width / 2;
                    int y = y0 + FontHeight / 2;
                    x0 += this.FontSpace + sz.Width;
                    y0 = y0;

                    if (iEv % 2 != 0 && ev.Start - ass_in.Events[iEv - 1].End < 0.1) y -= FontHeight + 3;

                    double kStart = ev.Start + kSum * 0.01;
                    kSum += ke.KValue;
                    double kEnd = ev.Start + kSum * 0.01;

                    double t0 = ev.Start + r * 0.5 - 1.0;
                    if (iEv % 2 != 0) t0 = ev.Start - r * 0.5 - 1.0;
                    double t01 = t0 + 0.5;
                    double t1 = kStart - 0.1;
                    if (t01 > t1)
                    {
                        t01 = t1;
                        t0 = t01 - 0.5;
                    }
                    double t2 = kStart + 0.2;
                    double t3 = kStart + 0.5;
                    double t4 = ev.End + r * 1.0 - 1.0;
                    if (iEv % 2 != 0) t4 = ev.End - r * 1.0 + 1.0;
                    if (t4 < t3) t4 = t3;
                    double t5 = t4 + 0.5;

                    double speed = -20;
                    int StartX = 848;
                    int EndX = 0;
                    if (iEv % 2 != 0)
                    {
                        speed = 20;
                        StartX = 0;
                        EndX = 848;
                    }

                    GetX_orgX = x;
                    GetX_Speed = speed;
                    GetX_StartTime = ev.Start;

                    ass_out.Events.Add(
                        ev.StartReplace(t0).EndReplace(t01).TextReplace(
                        ASSEffect.move(StartX, y, GetX(t01), y) + ASSEffect.a(1, "FF") + ASSEffect.a(3, "AA") + ASSEffect.c(3, "FFFFFF") +
                        ASSEffect.xbord(2) + ASSEffect.ybord(2) + ASSEffect.be(10) +
                        ke.KText));
                    ass_out.Events.Add(
                        ev.StartReplace(t01).EndReplace(t2).TextReplace(
                        ASSEffect.move(GetX(t01), y, GetX(t2), y) + ASSEffect.a(1, "FF") + ASSEffect.a(3, "AA") + ASSEffect.c(3, "FFFFFF") +
                        ASSEffect.xbord(2) + ASSEffect.ybord(2) + ASSEffect.be(10) +
                        ke.KText));
                    ass_out.Events.Add(
                        ev.StartReplace(t2).EndReplace(t3).TextReplace(
                        ASSEffect.move(GetX(t2), y, GetX(t3), y) + ASSEffect.a(1, "FF") + ASSEffect.a(3, "AA") + ASSEffect.c(3, "FFFFFF") +
                        ASSEffect.xbord(2) + ASSEffect.ybord(2) + ASSEffect.be(10) + ASSEffect.t(0, t3 - t2, ASSEffect.a(1, "77").t() + ASSEffect.c(3, colStr).t()) +
                        ke.KText));
                    ass_out.Events.Add(
                        ev.StartReplace(t3).EndReplace(t4).TextReplace(
                        ASSEffect.move(GetX(t3), y, GetX(t4), y) + ASSEffect.a(1, "FF") + ASSEffect.a(3, "77") + ASSEffect.c(3, colStr) +
                        ASSEffect.xbord(2) + ASSEffect.ybord(2) + ASSEffect.be(10) +
                        ke.KText));
                    ass_out.Events.Add(
                        ev.StartReplace(t4).EndReplace(t5).TextReplace(
                        ASSEffect.move(GetX(t4), y, EndX, y) + ASSEffect.a(1, "FF") + ASSEffect.a(3, "77") + ASSEffect.c(3, colStr) +
                        ASSEffect.xbord(2) + ASSEffect.ybord(2) + ASSEffect.be(10) +
                        ke.KText));

                    ass_out.Events.Add(
                        ev.StartReplace(t0).EndReplace(t01).TextReplace(
                        ASSEffect.move(StartX, y, GetX(t01), y) + ASSEffect.fad(t01 - t0, 0) +
                        ke.KText));
                    ass_out.Events.Add(
                        ev.StartReplace(t01).EndReplace(t1).TextReplace(
                        ASSEffect.move(GetX(t01), y, GetX(t1), y) +
                        ke.KText));
                    ass_out.Events.Add(
                        ev.StartReplace(t1).EndReplace(t2).TextReplace(
                        ASSEffect.move(GetX(t1), y, GetX(t2), y - 10) +
                        ASSEffect.t(0, t2 - t1, ASSEffect.frx(180 * 3).t()) +
                        ke.KText));
                    ass_out.Events.Add(
                        ev.StartReplace(t2).EndReplace(t3).TextReplace(
                        ASSEffect.move(GetX(t2), y - 10, GetX(t3), y) + ASSEffect.frx(180) +
                        ASSEffect.t(0, t3 - t2, ASSEffect.frx(360).t() + ASSEffect.c(1, colStr).t()) +
                        ke.KText));
                    ass_out.Events.Add(
                        ev.StartReplace(t3).EndReplace(t4).TextReplace(
                        ASSEffect.move(GetX(t3), y, GetX(t4), y) + ASSEffect.c(1, colStr) +
                        ke.KText));
                    ass_out.Events.Add(
                        ev.StartReplace(t4).EndReplace(t5).TextReplace(
                        ASSEffect.move(GetX(t4), y, EndX, y) + ASSEffect.c(1, colStr) + ASSEffect.fad(0, t5 - t4) +
                        ke.KText));
                }
            }
            ass_out.SaveFile(OutFileName);
        }
    }
}
