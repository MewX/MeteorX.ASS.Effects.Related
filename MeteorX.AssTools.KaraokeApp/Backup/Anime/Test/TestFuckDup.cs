using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MeteorX.AssTools.KaraokeApp.Anime.Test
{
    class TestFuckDup1 : BaseAnime2
    {
        public TestFuckDup1()
        {
            InFileName = @"G:\Workshop\test\1\0.ass";
            OutFileName = @"G:\Workshop\test\1\1.ass";

            this.PlayResX = 848;
            this.PlayResY = 480;
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS();

            ass_out.Header = ass_in.Header;
            ass_out.Events = new List<ASSEvent>();

            for (int i = 0; i <= 10 * 100; i += 10)
            {
                double t0 = (double)i * 0.01;
                double t1 = t0 + 0.1;
                string  s= t0.ToString("00.00");
                if (i < 5 * 100)
                    s = ((int)t0).ToString("00") + ".00";
                ass_out.AppendEvent(0, "Default", t0, t1,
                    an(5) + pos(PlayResX / 2, PlayResY / 2) +
                    "00:00:" + s);
            }

            ass_out.SaveFile(OutFileName);
        }
    }
}
