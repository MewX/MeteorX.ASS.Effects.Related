using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.Toys;
using MeteorX.AssTools.KaraokeApp.Model;

namespace MeteorX.AssTools.KaraokeApp.Anime.Test
{
    class TestIntense1 : BaseAnime2
    {
        public TestIntense1()
        {
            InFileName = @"G:\Workshop\test\7\0.ass";
            OutFileName = @"G:\Workshop\test\7\2.ass";

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
            string ptstr = @"{\p1}m 0 0 l -31 2 -53 10 -67 7 -53 9 -32 1 -41 -2 -49 -2 -54 -1 -49 -3 -41 -3 -46 -7 -49 -23 -45 -8 -41 -4 -32 0 -29 1 -26 1 -23 1 -10 0 -21 -8 -28 -12 -35 -11 -28 -13 -21 -9 -10 -1 -4 -14 5 -19 13 -25 13 -33 18 -37 24 -35 25 -29 23 -34 18 -36 14 -33 14 -25 10 -22 20 -17 20 -9 13 -5 19 -9 19 -17 9 -21 6 -19 -3 -13 -9 -1 0 -1 25 4 43 2 54 15 43 3 36 3 38 12 47 25 37 13 35 4 25 5 12 3 -8 22 -31 25 -41 32 -32 24 -9 21 11 3";

            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS();

            ass_out.Header = ass_in.Header;
            ass_out.Events = new List<ASSEvent>();

            int ox = 424;
            int oy = 240;
            Random rnd = new Random();

            double tStart = 1;
            double tEnd = 10;
            double particlePerSec = 200;
            string mainCol = "FF205C";

            CompositeCurve curve = Line.Create1(424 - 20, 240 - 20, 424 + 20, 240 + 20, 100, tStart, tEnd);

            for (int iP = 0; iP < particlePerSec * (tEnd - tStart); iP++)
            {
                double t0 = Common.RandomDouble(rnd, tStart, tEnd);
                double life = 0.3;
                double t1 = t0 + life;
                double tmpt = (double)iP / (particlePerSec * (tEnd - tStart)) * (tEnd - tStart) + tStart;
                ASSPointF orgpt = curve.GetPointF(tmpt);
                double x0 = orgpt.X;
                double y0 = orgpt.Y;
                double ag = Common.RandomDouble(rnd, 0, 2 * Math.PI);
                double r = 40;
                double x1 = x0 + r * Math.Cos(ag);
                double y1 = y0 + r * Math.Sin(ag);

                int tmpz = Common.RandomInt(rnd, 100, 200) * Common.RandomSig(rnd);
                tmpz = Common.RandomInt(rnd, 0, 359);

                for (int i = 0; i < 1; i++)
                {
                    ass_out.AppendEvent(10 + i, "pt", t0, t1,
                        move(x0, y0, x1, y1, 0, life * 0.7) + a(1, "77") + a(3, "DD") + fad(0, life * 0.8) +
                        c(1, mainCol) + c(3, mainCol) +
                        bord(3 - i) + be(3 - i) + fsc(30, 30) +
                        t(life * 0.5, life, fsc(0, 0).t()) +
                        frz(tmpz) +
                        ptstr);
                }
                ass_out.AppendEvent(20, "pt", t0, t1,
                    move(x0, y0, x1, y1, 0, life * 0.7) + a(1, "77") + a(3, "DD") + fad(0, life * 0.8) +
                    c(1, "FFFFFF") + c(3, "FFFFFF") +
                    bord(2) + blur(2) + fsc(30, 30) +
                        t(life * 0.5, life, fsc(0, 0).t()) +
                    frz(tmpz) +
                    ptstr);
            }

            ass_out.SaveFile(OutFileName);
        }
    }
}
