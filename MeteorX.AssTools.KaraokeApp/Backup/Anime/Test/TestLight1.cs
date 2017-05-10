using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MeteorX.AssTools.KaraokeApp.Anime.Test
{
    class TestLight1 : BaseAnime2
    {
        public TestLight1()
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

            int x = 300;
            int y = 200;
            StringMask mask = GetMask("き", x, y);
            bool first = true;
            ass_out.AppendEvent(0, "Default", 0, 10, ASSEffect.pos(265, y) + ASSEffect.an(5) + "拉");
            foreach (ASSPoint pt in mask.Points)
            {
                double ag = (double)(pt.Y - mask.Y0) / (double)mask.Height * 0.25;
                int iag = (int)(ag / Math.PI / 2 * 360);
                //ag = -0.2;
                int yy = (int)(150 * Math.Sin(ag));
                int xx = (int)(150 * Math.Cos(ag));
                ass_out.Events.Add(
                    new ASSEvent
                    {
                        Effect = "",
                        Layer = 0,
                        MarginL = "0000",
                        MarginR = "0000",
                        MarginV = "0000",
                        Name = "NTP",
                        Style = "pt",
                        Start = 1,
                        End = 11,
                        Text = ASSEffect.pos(pt.X, pt.Y) + ASSEffect.a(1, Common.ToHex2(255 - pt.Brightness)) + ASSEffect.c(1, "FFFFFF") + ASSEffect.a(3, "FF") + ptstr
                    }
                    );
                //if (!first) continue;
                first = false;
                ass_out.Events.Add(
                    new ASSEvent
                    {
                        Effect = "",
                        Layer = 1,
                        MarginL = "0000",
                        MarginR = "0000",
                        MarginV = "0000",
                        Name = "NTP",
                        Style = "pt",
                        Start = 1,
                        End = 11,
                        Text = ASSEffect.pos(pt.X, pt.Y) + ASSEffect.frz(iag) + ASSEffect.t(0, 10, ASSEffect.fry(360 * 5).t()) + ASSEffect.an(7) + ASSEffect.a(1, "F0") + ASSEffect.c(1, "7879F5") + ASSEffect.a(3, "FF") + ASSEffect.bord(0) + ASSEffect.be(1) + @"{\p2}m 0 0 l -200 0 0 2" //  @"{\p1}m 0 0 l " + xx + " " + yy + " 0 2"
                    });
            }

            ass_out.SaveFile(OutFileName);
        }
    }
}
