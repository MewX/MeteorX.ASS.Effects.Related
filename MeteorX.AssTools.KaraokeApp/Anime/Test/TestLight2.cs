using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MeteorX.AssTools.KaraokeApp.Anime.Test
{
    class TestLight2 : BaseAnime2
    {
        public TestLight2()
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

            int ox = 300;
            int oy = 300;
            double r = 30;
            Random rnd = new Random();
            int sz = 3;
            int maxsz = 5;
            string cb = "44";
            string cb2 = "11";
            string eb = "55";
            string eb2 = "22";
            string col1 = "FFFC94";
            string col2 = "FF94D1";

            ASSPoint[] pt2 = new ASSPoint[10];
            for (int i = 0; i < pt2.Length; i++)
                pt2[i] = new ASSPoint { X = Common.RandomInt(rnd, ox - 15, ox + 15), Y = Common.RandomInt(rnd, oy - 70, oy - 40) };

            for (double ag = 0; ag < Math.PI * 2; ag += 0.05)
            {
                double t0 = 2 + ag * 0.3;
                double t1 = 2 + Math.PI * 2 * 0.3 + Common.RandomDouble(rnd, 0, 0.3);
                double t2 = 2 + Math.PI * 2 * 0.3 + 1 + Common.RandomDouble(rnd, 0, 0.7);
                double t3 = t2 + 0.8;
                if (Common.RandomBool(rnd, (0.25 + 0.5 * (1.0 - (double)(sz - 1) / (double)(maxsz - 1))))) sz++; else sz--;
                if (sz < 1) sz = 1;
                if (sz > maxsz) sz = maxsz;
                double x = ox + r * Math.Cos(ag);
                double y = oy + r * Math.Sin(ag);

                double r1 = Common.RandomDouble(rnd, 0, 10);
                double ag1 = Common.RandomDouble(rnd, 0, Math.PI * 2);
                double x1 = ox + r1 * Math.Cos(ag1);
                double y1 = oy + r1 * Math.Sin(ag1);

                int pt2i = Common.RandomInt(rnd, 0, pt2.Length - 1);
                double x2 = pt2[pt2i].X;
                double y2 = pt2[pt2i].Y;

                ass_out.AppendEvent(10, "pt", t0, t1,
                    ASSEffect.pos(x, y) +
                    ASSEffect.a(1, "00") + ASSEffect.c(1, "FFFFFF") +
                    ASSEffect.a(3, cb) + ASSEffect.c(3, "FFFFFF") +
                    ASSEffect.bord(1) + ASSEffect.be(1) +
                    ASSEffect.t(0, (t1 - t0) * 1, ASSEffect.bord(sz).t() + ASSEffect.be(sz).t()) +
                    ptstr);
                ass_out.AppendEvent(5, "pt", t0, t1,
                    ASSEffect.pos(x, y) +
                    ASSEffect.a(1, "FF") +
                    ASSEffect.a(3, eb) + ASSEffect.c(3, col1) +
                    ASSEffect.bord(2) + ASSEffect.blur(2) +
                    ASSEffect.t(0, (t1 - t0) * 1, ASSEffect.bord(sz + 1).t() + ASSEffect.blur(sz + 1).t()) +
                    ASSEffect.t(0, (t1 - t0) * 0.5, ASSEffect.c(3, col2).t()) +
                    ASSEffect.t((t1 - t0) * 0.5, t1, ASSEffect.c(3, col1).t()) +
                    ptstr);

                string ctmp = Common.scaleColor(col1, col2, Common.RandomDouble(rnd, 0, 1));
                ass_out.AppendEvent(10, "pt", t1, t2,
                    ASSEffect.move(x, y, x1, y1) +
                    ASSEffect.a(1, "00") + ASSEffect.c(1, "FFFFFF") +
                    ASSEffect.a(3, cb) + ASSEffect.c(3, "FFFFFF") +
                    ASSEffect.bord(sz) + ASSEffect.be(sz) +
                    ptstr);
                ass_out.AppendEvent(5, "pt", t1, t2,
                    ASSEffect.move(x, y, x1, y1) +
                    ASSEffect.a(1, "FF") +
                    ASSEffect.a(3, eb) + ASSEffect.c(3, col1) +
                    ASSEffect.bord(sz + 1) + ASSEffect.blur(sz + 1) +
                    ASSEffect.t(0, t2 - t1, ASSEffect.c(3, ctmp).t()) +
                    ptstr);

                ass_out.AppendEvent(10, "pt", t2, t3,
                    ASSEffect.fad(0, 0.5) +
                    ASSEffect.move(x1, y1, x2, y2) +
                    ASSEffect.a(1, "00") + ASSEffect.c(1, "FFFFFF") +
                    ASSEffect.a(3, cb2) + ASSEffect.c(3, "FFFFFF") +
                    ASSEffect.bord(sz) + ASSEffect.be(sz) +
                    ASSEffect.t(0, (t3 - t2) * 0.5, ASSEffect.bord(0).t() + ASSEffect.be(1).t()) +
                    ptstr);
                ass_out.AppendEvent(5, "pt", t2, t3,
                    ASSEffect.fad(0, 0.2) +
                    ASSEffect.move(x1, y1, x2, y2) +
                    ASSEffect.a(1, "00") + ASSEffect.c(1, "FFFFFF") +
                    ASSEffect.a(3, eb2) + ASSEffect.c(3, ctmp) +
                    ASSEffect.bord(sz + 1) + ASSEffect.blur(sz + 1) +
                    ASSEffect.t(0, (t3 - t2) * 0.5, ASSEffect.bord(1).t() + ASSEffect.blur(1).t()) +
                    ptstr);
            }

            ass_out.SaveFile(OutFileName);
        }
    }
}
