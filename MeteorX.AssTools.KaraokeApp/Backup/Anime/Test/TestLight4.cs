using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MeteorX.AssTools.KaraokeApp.Anime.Test
{
    class TestLight4 : BaseAnime2
    {
        public TestLight4()
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

            double r1 = 5;
            double r2 = 80;
            Func<double, double, string> f1 = (a, b) => string.Format(" {0} {1}", (int)(Math.Round(a)), (int)(Math.Round(b)));
            string s = @"{\p1}m ";
            string s1 = @"{\p1}m ";
            string s2 = @"{\p1}m ";
            int diag = 3;
            double scale = 0.5;
            bool first = true;
            for (int iag = 0; iag < 360; )
            {
                double ag = (double)iag / 360.0 * Math.PI * 2;
                r1 = Common.RandomDouble(rnd, 5, 12) * scale;
                r2 = Common.RandomDouble(rnd, 80, 120) * scale;
                double x = Math.Cos(ag) * r1;
                double y = Math.Sin(ag) * r1;
                double x3 = Math.Cos(ag) * r2;
                double y3 = Math.Sin(ag) * r2;
                iag += diag;
                ag = (double)iag / 360.0 * Math.PI * 2;
                double x1 = Math.Cos(ag) * r2;
                double y1 = Math.Sin(ag) * r2;
                double x2 = Math.Cos(ag) * r2 * 0.5;
                double y2 = Math.Sin(ag) * r2 * 0.5;
                s += f1(x, y);
                s1 += f1(x, y);
                if (Common.RandomBool(rnd, 0.8)) s2 += f1(x3, y3); else s2 += f1(x, y);
                if (first)
                {
                    s += " l";
                    s1 += " l";
                    s2 += " l";
                    first = false;
                }
                s += f1(x1, y1);
                s1 += f1(x2, y2);
                s2 += f1(x1, y1);
                iag += diag;
            }
            ass_out.AppendEvent(20, "pt", 0, 10,
                ASSEffect.pos(ox, oy) + ASSEffect.a(1, "00") + ASSEffect.c(1, "FFFFFF") + ASSEffect.a(3, "EE") + ASSEffect.blur(0.5) +
                ASSEffect.bord(0) + ASSEffect.be(0) +
                t(fsc(200, 200).t()) +
                s1);
            ass_out.AppendEvent(10, "pt", 0, 10,
                ASSEffect.pos(ox, oy) + ASSEffect.a(1, "00") + ASSEffect.c(1, "306BFF") + ASSEffect.a(3, "EE") + ASSEffect.blur(0.5) +
                t(fsc(200, 200).t()) + 
                s);
            ass_out.AppendEvent(0, "pt", 0, 10,
                ASSEffect.pos(ox, oy) + ASSEffect.a(1, "F7") + ASSEffect.c(1, "306BFF") + ASSEffect.a(3, "EE") + ASSEffect.blur(5) +
                fsc(85, 85) +
                t(fsc(170, 170).t()) +
               s2);

            ass_out.SaveFile(OutFileName);
        }
    }
}
