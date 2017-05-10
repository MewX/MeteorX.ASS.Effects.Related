using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.Effect;
using MeteorX.AssTools.KaraokeApp.Model;

namespace MeteorX.AssTools.KaraokeApp.Anime.Test
{
    class TestDrop1 : BaseAnime2
    {
        public TestDrop1()
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

            char ch = '雨';
            int x = 300;
            int y = 300;
            double r0 = 15;
            double r1 = 30;
            string col1 = "FFFC94";
            string col2 = "FF94D1";
            string col3 = "FFFFFF";
            StringMask mask = GetMask(ch + "", x, y);

            double t0 = 0;
            double t3 = 10;

            ass_out.AppendEvent(0, "Default", t0, t3,
                ASSEffect.pos(x, y) + ASSEffect.a(1, "FF") + ASSEffect.c(1, col2) +
                ASSEffect.a(3, "FF") +
                ch);
            for (int i = 0; i < 1; i++)
            {
                ass_out.AppendEvent(0, "Default", t0, t3,
                    ASSEffect.pos(x, y) + ASSEffect.a(1, "00") + ASSEffect.c(1, col2) +
                    ASSEffect.org(x - 200, y) + ASSEffect.t(0, t3 - t0, ASSEffect.fry(-360).t()) +
                    ASSEffect.a(3, "FF") + ASSEffect.be(1) + ASSEffect.bord(0) +
                    ASSEffect.t(0, (t3 - t0) * 0.125, ASSEffect.fscx(30).t()) +
                    ASSEffect.t((t3 - t0) * 0.125, (t3 - t0) * 0.125 * 2, ASSEffect.fscx(100).t()) +
                    ASSEffect.t(0, (t3 - t0) * 0.25, ASSEffect.fscy(40).t()) +
                    ASSEffect.bord(1) +
                    ch);
            }
            /*
            for (int agi = 0; agi < 360; agi += 20)
            {
                double x0 = x + r0 * Math.Cos(0);
                double y0 = y + r0 * Math.Sin(0);
                double x1 = x + r1 * Math.Cos(0);
                double y1 = y + r1 * Math.Sin(0);

                double t0 = 1;
                double t1 = t0 + 1.8;

                ass_out.AppendEvent(2, "pt", t0, t1,
                    ASSEffect.an(5) +
                    ASSEffect.move(x0, y0, x1, y1) + ASSEffect.org(x, y) + ASSEffect.frz(agi) +
                    ASSEffect.a(1, "00") + ASSEffect.c(1, "FFFFFF") +
                    ASSEffect.a(3, "44") + ASSEffect.c(3, "FFFFFF") +
                    ASSEffect.t(0, t1 - t0, ASSEffect.frz(agi + 360).t()) +
                    ASSEffect.ybord(10) + ASSEffect.xbord(0) + ASSEffect.be(1) +
                    ptstr);

                ass_out.AppendEvent(2, "pt", t0, t1,
                    ASSEffect.an(5) +
                    ASSEffect.move(x0, y0, x1, y1) + ASSEffect.org(x, y) + ASSEffect.frz(agi) +
                    ASSEffect.a(1, "00") + ASSEffect.c(1, "FFFFFF") +
                    ASSEffect.a(3, "22") + ASSEffect.c(3, col1) +
                    ASSEffect.t(0, t1 - t0, ASSEffect.frz(agi + 360).t()) +
                    ASSEffect.ybord(11) + ASSEffect.xbord(0) + ASSEffect.blur(2) +
                    ptstr);
            }
             * */

            ass_out.SaveFile(OutFileName);
        }
    }
}
