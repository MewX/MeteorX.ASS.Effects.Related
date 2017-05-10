using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.Model;
using System.Drawing.Text;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class TestAnime : BaseAnime
    {
        public TestAnime()
        {
            InFileName = @"G:\Workshop\touhou\test.ass";
            OutFileName = @"G:\Workshop\touhou\1.ass";

            this.FontWidth = 21;
            this.FontHeight = 26;
            this.FontSpace = 1;

            this.PlayResX = 848;
            this.PlayResY = 480;
            this.MarginBottom = 10;
            this.MarginLeft = 10;
            this.MarginRight = 10;
            this.MarginTop = 10;
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS();

            ass_out.Header = ass_in.Header;
            ass_out.Events = new List<ASSEvent>();

            this.Font = new System.Drawing.Font("HGGyoshotai", 28);
            this.Mask_HeightScale = 1;

            string src = "もう少しだけ";

            /*
            int x0 = 100;
            Random rnd = new Random();
            foreach (char ch in src)
            {
                List<Point> pts = GetMask(ch, x0, 100);
                foreach (Point pt in pts)
                    ass_out.Events.Add(new ASSEvent
                    {
                        Start = 0,
                        End = 1,
                        Layer = 0,
                        Style = "pt",
                        Effect = "",
                        MarginL = "0000",
                        MarginR = "0000",
                        MarginV = "0000",
                        Name = "NTP",
                        Text = ASSEffect.an(5) + ASSEffect.move(pt.X, pt.Y, pt.X - rnd.Next() % 50, pt.Y - rnd.Next() % 80 + 40) +  'o'
                    }
                    );

                x0 += FontWidth;
            }
             * */
            int x0 = 200;
            int y0 = 200;
            int x1 = 200;
            int y1 = 200;

            int p1_x = 100;
            int p1_y = 170;
            int p2_x = 380;
            int p2_y = 250;

            List<ASSPoint> pts0 = GetMask("彼", x0, y0).Points;
            List<ASSPoint> pts1 = GetMask("雷", x1, y1).Points;

            int y0min = 200;
            int y0max = 240;
            int y1min = 200;
            int y1max = 240;

            foreach (ASSPoint pt in pts0)
                ass_out.Events.Add(new ASSEvent
                {
                    Start = 0,
                    End = 1,
                    Layer = 0,
                    Style = "pt",
                    Effect = "",
                    MarginL = "0000",
                    MarginR = "0000",
                    MarginV = "0000",
                    Name = "NTP",
                    Text = ASSEffect.an(5) + ASSEffect.pos(pt.X, pt.Y) + ASSEffect.c(1, Common.scaleColor("7A93EB", "6939C8", y0min, y0max, pt.Y)) + '.'
                });

            Random rnd = new Random();
            bool[] used0 = new bool[pts0.Count];
            for (int i = 0; i < used0.Length; i++)
                used0[i] = false;
            foreach (ASSPoint pt1 in pts1)
            {
                int k0 = rnd.Next() % pts0.Count;
                used0[k0] = true;
                int p1_xx = Common.RandomInt_Gauss(rnd, p1_x, 80);
                int p1_yy = Common.RandomInt_Gauss(rnd, p1_y, 80);
                int p2_xx = Common.RandomInt_Gauss(rnd, p2_x, 80);
                int p2_yy = Common.RandomInt_Gauss(rnd, p2_y, 80);
                string startColor = Common.scaleColor("7A93EB", "6939C8", y0min, y0max, pts0[k0].Y);
                string endColor = Common.scaleColor("A3E789", "D9AB85", y1min, y1max, pt1.Y);
                List<ASSPoint> bez_pts = new Bezier(pts0[k0], new ASSPoint { X = p1_xx, Y = p1_yy }, new ASSPoint { X = p2_xx, Y = p2_yy }, pt1).Create(0.1f);
                for (int i = 0; i + 1 < bez_pts.Count; i++)
                {
                    ass_out.Events.Add(new ASSEvent
                    {
                        Start = 1 + i * 0.2,
                        End = 1 + (i + 1) * 0.2,
                        Layer = 0,
                        Style = "pt",
                        Effect = "",
                        MarginL = "0000",
                        MarginR = "0000",
                        MarginV = "0000",
                        Name = "NTP",
                        Text = ASSEffect.an(5) + ASSEffect.move(bez_pts[i].X, bez_pts[i].Y, bez_pts[i + 1].X, bez_pts[i + 1].Y) +
                        ASSEffect.c(1, Common.scaleColor(startColor, endColor, (double)i / (double)bez_pts.Count)) + '.'
                    });
                }
            }

            double last_end = ass_out.Events[ass_out.Events.Count - 1].End;
            foreach (ASSPoint pt in pts1)
                ass_out.Events.Add(new ASSEvent
                {
                    Start = last_end,
                    End = last_end + 1,
                    Layer = 0,
                    Style = "pt",
                    Effect = "",
                    MarginL = "0000",
                    MarginR = "0000",
                    MarginV = "0000",
                    Name = "NTP",
                    Text = ASSEffect.an(5) + ASSEffect.pos(pt.X, pt.Y) + ASSEffect.c(1, Common.scaleColor("A3E789", "D9AB85", y1min, y1max, pt.Y)) + '.'
                });

            ass_out.SaveFile(OutFileName);
        }
    }
}
