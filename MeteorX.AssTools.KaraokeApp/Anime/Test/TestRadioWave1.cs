using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.Model;

namespace MeteorX.AssTools.KaraokeApp.Anime.Test
{
    class TestRadioWave1 : BaseAnime2
    {
        public TestRadioWave1()
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
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS();

            ass_out.Header = ass_in.Header;
            ass_out.Events = new List<ASSEvent>();

            int ox = PlayResX / 2;
            int oy = PlayResY / 2;
            Random rnd = new Random();

            /*
            BaseCurve curve = new Model.Brown { MinT = 0, MaxT = 10, R = 20, X0 = PlayResX / 2, Y0 = PlayResY / 2, Speed = 200 };
            double freq = 250;
            foreach (ASSPointF pt in curve.GetPath_DT(1.0 / freq))
            {
                ass_out.AppendEvent(0, "pt", pt.T, pt.T + 3,
                    pos(pt.X, pt.Y) + fad(0.3, 2.3) +
                    a(1, "DD") + blur(1) +
                    fsc(1) + t(fsc(70).t()) + frz((int)(pt.T * 360 * 0.3)) +
                    p(1) + "m -100 -40 l 100 -40 100 40 -100 40 c m -95 -35 l -95 35 95 35 95 -35 c");
            }
             * */

            string ptstr = "m 0 -100 b 133 -100 133 100 0 100 b -3 100 -3 92 0 92 b 123 92 123 -92 0 -92 b -3 -92 -3 -100 0 -100";
            for (double ti = 0; ti < 10; ti += 1.0 / 30.0)
            {

                int frz0 = Common.RandomInt(rnd, 0, 359);
                int frz1 = frz0 + 180;
                string frxys = frx(f1()) + fry(f1());
                for (int i = 0; i < 5; i++)
                {
                    double dt = i * 0.04;
                    string s1 = a(1, "55") + blur(0.7);
                    if (i != 2) s1 = a(1, "BB") + blur(2);
                    ass_out.AppendEvent(50, "pt", ti + dt, ti + 2 + dt,
                        pos(ox, oy) + org(ox, oy) + fad(0.7, 1.5) +
                        s1 +
                        frxys +
                        frz(frz0) + t(frz(frz1).t()) +
                        t(fsc(500).t()) +
                        p(4) + ptstr);
                }
            }

            ass_out.SaveFile(OutFileName);
        }

        int f1()
        {
            return (Common.RandomInt_Gauss2(rnd, 30, 15) - 15) + 30 * Common.RandomInt(rnd, 0, 11);
        }
    }
}
