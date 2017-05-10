using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MeteorX.AssTools.KaraokeApp.Anime.Test
{
    class TestLight3 : BaseAnime2
    {
        public TestLight3()
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
            string lstr0 = @"{\p1}m -1 0 l 0 -50 0 0";
            string lstr1 = @"{\p1}m -1 0 l 0 0 0 50";

            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS();

            ass_out.Header = ass_in.Header;
            ass_out.Events = new List<ASSEvent>();

            int ox = 300;
            int oy = 300;
            Random rnd = new Random();
            string lcol = "00C6FF";

            for (int count = 0; count < 50; count++)
            {
                double t0 = 2 + Common.RandomDouble(rnd, 0, 1);
                double t1 = t0 + 1;
                double x0 = Common.RandomDouble(rnd, ox - 10, ox + 10);
                double x1 = Common.RandomDouble(rnd, ox - 20, ox + 20);
                //if (Common.RandomBool(rnd, 0.5)) x1 = ox - 20; else x1 = ox + 20;
                int startag = Common.RandomInt(rnd, 0, 90);

                ass_out.AppendEvent(5, "pt", t0, t1,
                    ASSEffect.fad(0.2, 0) + ASSEffect.move(x0, oy, x1, oy) +
                    ASSEffect.a(1, "00") + ASSEffect.c(1, lcol) + ASSEffect.a(3, "FF") +
                    ASSEffect.frx(startag) +
                    ASSEffect.t(0, t1 - t0, ASSEffect.frx(90).t()) +
                    lstr0);
                ass_out.AppendEvent(5, "pt", t0, t1,
                    ASSEffect.fad(0.2, 0) + ASSEffect.move(x0, oy, x1, oy) +
                    ASSEffect.a(1, "00") + ASSEffect.c(1, lcol) + ASSEffect.a(3, "FF") +
                    ASSEffect.frx(startag) +
                    ASSEffect.t(0, t1 - t0, ASSEffect.frx(90).t()) +
                    lstr1);
            }

            ass_out.SaveFile(OutFileName);
        }
    }
}
