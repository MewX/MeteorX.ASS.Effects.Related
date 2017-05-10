using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MeteorX.AssTools.KaraokeApp.Anime.Test
{
    class TestFlash1 : BaseAnime
    {
        public TestFlash1()
        {
            InFileName = @"G:\Workshop\test\6\0.ass";
            OutFileName = @"G:\Workshop\test\6\1.ass";

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
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS();

            ass_out.Header = ass_in.Header;
            ass_out.Events = new List<ASSEvent>();

            string ptStr = @"{\p1}m 0 0 l 1 0 1 1 0 1";

            string s = "見上げて　祈りさえも";
            int x0 = 100;
            Random rnd = new Random();
            string[] cols = { "FFFFFF", "FF0000" };
            foreach (char ch in s)
            {
                StringMask mask = GetMask(ch + "", x0, 100);
                x0 += mask.Width + FontSpace;
                foreach (ASSPoint pt in mask.Points)
                {
                    foreach (string col in cols)
                    {
                        ass_out.Events.Add(
                            new ASSEvent
                            {
                                Effect = "",
                                Layer = 0,
                                MarginL = "0000",
                                MarginR = "0000",
                                MarginV = "0000",
                                Name = "NTP",
                                Style = "Default",
                                Start = 1,
                                End = 11,
                                Text = ASSEffect.pos(pt.X, pt.Y) + ASSEffect.c(1, col) + ASSEffect.t(0, 10, ASSEffect.c(1, cols[0]).t()) + ASSEffect.t(0, Common.RandomDouble(rnd, 10, 20), ASSEffect.a(1, "FFFFFF").t()) + ptStr
                            }
                            );
                    }
                }
                //break;
            }

            ass_out.SaveFile(OutFileName);
        }
    }
}
