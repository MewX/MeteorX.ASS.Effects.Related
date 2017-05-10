using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.Toys;

namespace MeteorX.AssTools.KaraokeApp.Anime.Test
{
    class TestToy1 : BaseAnime2
    {
        public TestToy1()
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
            Random rnd = new Random();

            Func<double, double, string> f1 = (a, b) => string.Format(" {0} {1}", (int)(Math.Round(a)), (int)(Math.Round(b)));
            Toy1 toy1 = new Toy1 { OX = 0, OY = 0, V = 50, MinR = 10, MaxR = 100, MinDAG = 0.2, MaxDAG = 0.5, IsSort = false };
            toy1.Reset();

            double dt = 0.04;
            for (double t = 0; t < 60; t += dt)
            {
                List<ASSPointF> pts = toy1.Next();
                string s = @"{\p1}m";
                s += f1(pts[0].X, pts[0].Y);
                s += " l";
                for (int i = 1; i < pts.Count; i++)
                    s += f1(pts[i].X, pts[i].Y);
                s += f1(pts[0].X, pts[0].Y);
                s += f1(pts[1].X, pts[1].Y);
                s += f1(pts[2].X, pts[2].Y);
                ass_out.AppendEvent(0, "pt", t, t + dt,
                    ASSEffect.pos(ox, oy) + ASSEffect.an(7) +
                    ASSEffect.a(1, "FF") + ASSEffect.c(1, "FFFFFF") +
                    ASSEffect.a(3, "00") + ASSEffect.c(3, "FFFFFF") +
                    ASSEffect.bord(1) + ASSEffect.be(0) +
                    s);
            }

            ass_out.SaveFile(OutFileName);
        }
    }
}
