using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.Effect;

namespace MeteorX.AssTools.KaraokeApp.Anime.Test
{
    class TestParticle3 : BaseAnime
    {
        public TestParticle3()
        {
            InFileName = @"G:\Workshop\test\5\0.ass";
            OutFileName = @"G:\Workshop\test\5\1.ass";

            this.FontWidth = 30;
            this.FontHeight = 30;
            this.FontSpace = 1;

            this.PlayResX = 848;
            this.PlayResY = 480;
            this.MarginBottom = 10;
            this.MarginLeft = 15;
            this.MarginRight = 15;
            this.MarginTop = 15;

            this.Font = new System.Drawing.Font("ＤＦＰまるもじ体W3", 30, GraphicsUnit.Pixel);
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS();

            ass_out.Header = ass_in.Header;
            ass_out.Events = new List<ASSEvent>();

            //Particle2 Par = new Particle2("FFFFFF", "000000", 0, 2, 0.01, 2, -5, 5, -20, 20, 0.5, 2);
            //ass_out.Events.AddRange(Par.Create(new MovingLinear(0, 2, 0, 240, 848, 240) { MinDY = -10, MaxDY = 10 }));
            Particle2 Par = new Particle2("FFFFFF", "FFCC33", 0, 2, 0.01, 16, 384, 504, -10, 10, 2, 5) { Star = false, Pt0Size = 2 };
            ass_out.Events.AddRange(Par.Create(new MovingArc(0, 2, -100, 240, 848, 240, 60, -1.2, 1.2) { GaussRnd = 3 }));
            /*for (int i = 0; i < 20; i++)
            {
                int x0 = i * 32 + 15;
                int y0 = 480 - 30;
                double t0 = i * 0.3 + 1;
                Particle2 Par = new Particle2("FFCC33", "FF0000", t0, t0 + 0.3, 0.003, 1, -10, 10, -10, 10, 0.5, 1) { Star = false, Pt0Size = 3 };
                ass_out.Events.AddRange(Par.Create(new MovingRound(t0, t0 + 0.3, x0, y0, 40, 0) { MinDX = -2, MaxDX = 2, MinDY = 2, MaxDY = 2 }));
            }*/
            /*Particle2 Par = new Particle2("FFCC33", "FF0000", 1, 1.5, 0.001, 1, -10, 10, -10, 10, 0.5, 1) { Star = false, Pt0Size = 3 };
            ass_out.Events.AddRange(Par.Create(new MovingSinH(1, 1.5, 0, 848, 430, 20, 0.15) { MinDX = -2, MaxDX = 2, MinDY = 2, MaxDY = 2 }));*/

            ass_out.SaveFile(OutFileName);
        }
    }
}
