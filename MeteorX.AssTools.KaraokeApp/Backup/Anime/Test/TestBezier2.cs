using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.Effect;
using MeteorX.AssTools.KaraokeApp.Model;

namespace MeteorX.AssTools.KaraokeApp.Anime.Test
{
    class TestBezier2 : BaseAnime2
    {
        public TestBezier2()
        {
            InFileName = @"G:\Workshop\test\7\0.ass";
            OutFileName = @"G:\Workshop\test\7\1.ass";

            this.FontWidth = 30;
            this.FontHeight = 30;
            this.FontSpace = 2;

            this.PlayResX = 848;
            this.PlayResY = 480;
            this.MarginBottom = 10;
            this.MarginLeft = 15;
            this.MarginRight = 15;
            this.MarginTop = 15;

            this.Font = new System.Drawing.Font("ＤＦＰまるもじ体W3", 30, GraphicsUnit.Pixel);
            this.MaskStyle = "Style: Default,DFGMaruMoji-SL,30,&H00FFFFFF,&HFF000000,&H00000000,&HFF000000,0,0,0,0,100,100,2,0,1,2,0,5,30,30,10,128";
            this.IsAvsMask = true;
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS();

            ass_out.Header = ass_in.Header;
            ass_out.Events = new List<ASSEvent>();

            Random rnd = new Random();

            string ptstr = @"{\p1}m 0 0 l 1 0 1 1 0 1";
            Bezier bz1 = CreateBezier3(rnd, 300, 300, 200, 100);
            Bezier bz2 = CreateBezier3(rnd, 300, 300, 200, 100);
            Bezier[] bz = new Bezier[20];
            Line[] li = new Line[bz.Length - 1];
            Circle[] cc = new Circle[bz.Length - 1];
            for (int i = 0; i < bz.Length; i++)
                bz[i] = CreateBezier3(rnd, this.PlayResX / 2, this.PlayResY / 2, this.PlayResX / 2, this.PlayResY / 2);
            for (int i = 0; i < li.Length; i++)
                li[i] = new Line { X0 = bz[i].P3.X, Y0 = bz[i].P3.Y, X1 = bz[i + 1].P0.X, Y1 = bz[i + 1].P0.Y };
            for (int i = 0; i < cc.Length; i++)
                cc[i] = Circle.Create(bz[i].P3.X, bz[i].P3.Y, bz[i + 1].P0.X, bz[i + 1].P0.Y, Common.RandomBool(rnd, 0.5));
            CompositeCurve cv = new CompositeCurve() { MinT = 0, MaxT = bz.Length + li.Length };
            for (int i = 0; i < li.Length; i++)
            {
                cv.AddCurve(i * 2, i * 2 + 1, bz[i]);
                cv.AddCurve(i * 2 + 1, (i + 1) * 2, cc[i]);//li[i]);
            }
            cv.AddCurve(li.Length * 2, li.Length * 2 + 1, bz[bz.Length - 1]);
            foreach (ASSPointF pt in cv.GetPath_Dis(10, 11))
            {
                ass_out.AppendEvent(0, "pt", pt.T, pt.T + 1,
                    ASSEffect.pos(pt.X, pt.Y) + ASSEffect.a(1, "00") + ASSEffect.c(1, "FFFFFF") + ASSEffect.a(3, "FF") +
                    ptstr);
            }

            ass_out.SaveFile(OutFileName);
        }
    }
}
