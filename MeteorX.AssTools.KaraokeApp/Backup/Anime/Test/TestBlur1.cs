using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MeteorX.AssTools.KaraokeApp.Anime.Test
{
    class TestBlur1 : BaseAnime2
    {
        public TestBlur1()
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

            for (int i = 0; i < 10; i++)
            {
                double bl = (double)i / (double)(10 - 1);
                string col = Common.scaleColor("FFFFFF", "0000FF", 1 - bl);
                string alp = Common.scaleAlpha("00", "FF", bl);
                ass_out.AppendEvent(0, "jp", 0, 5,
                    pos(ox + bl * 2, oy + bl * 2) + a(1, alp) + bord(0) + blur(bl * 2) + c(1, col) +
                    "き");
            }

            ass_out.SaveFile(OutFileName);
        }
    }
}
