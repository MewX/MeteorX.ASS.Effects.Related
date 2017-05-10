using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.Effect;
using MeteorX.AssTools.KaraokeApp.Model;

namespace MeteorX.AssTools.KaraokeApp.Anime.Test
{
    class Test033 : BaseAnime2
    {
        public Test033()
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
            this.MaskStyle = "Style: Default,ＤＦＰまるもじ体W3,30,&H00FFFFFF,&HFF000000,&H00000000,&HFF000000,0,0,0,0,100,100,2,0,1,2,0,5,30,30,10,128";
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
            string pt1str = @"{\p4}m 0 0 l 50 49 100 0 51 50 100 100 50 51 0 100 49 50 0 0 m 45 45 s 55 45 55 55 45 55 c{\p0}";

            double dy = -0.5;
            double y = 200;
            int yl = 190;
            int yh = 210;
            int ag = 0;
            for (int x = 100; x <= 700; x++)
            {
                if (y == yh) dy = -0.5;
                if (y == yl) dy = 0.5;
                y += dy;
                ag += 1;

                double t0 = (double)(x - 100) / 100;

                ass_out.AppendEvent(0, "pt", t0, t0 + 2,
                    ASSEffect.pos(x, y) + ASSEffect.an(5) +
                    ASSEffect.a(1, "E0") + ASSEffect.c(1, "FFFFFF") +
                    ASSEffect.a(3, "F0") + ASSEffect.c(3, "FFFFFF") + ASSEffect.bord(2) + ASSEffect.be(1) +
                    ASSEffect.frx(ag) + ASSEffect.fry(ag) + ASSEffect.frz(ag) +
                    ASSEffect.fad(0, 2) +
                    ASSEffect.t(0, 2, ASSEffect.fsc(500, 500).t()) +
                    pt1str);
            }

            ass_out.SaveFile(OutFileName);
        }
    }
}
