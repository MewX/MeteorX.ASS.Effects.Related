using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.Effect;
using MeteorX.AssTools.KaraokeApp.Model;

namespace MeteorX.AssTools.KaraokeApp.Anime.Test
{
    class TestBezier1 : BaseAnime2
    {
        public TestBezier1()
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
            this.MaskStyle = "Style: Default,DFGMaruMoji-SL,30,&H00FFFFFF,&HFF000000,&H00000000,&HFF000000,0,0,0,0,100,100,2,0,1,2,0,5,30,30,10,128";
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

            string ts = "この空のかなたへと　一人思いはせる";
            int x0 = 50;
            int y0 = 420;
            double stt = 0;
            foreach (char ch in ts)
            {
                Console.WriteLine(ch);
                stt += 0.2;
                if (Char.IsWhiteSpace(ch))
                {
                    x0 += 30;
                    continue;
                }
                StringMask mask = GetMask(ch + "", x0, y0);
                ASSPoint p1 = Common.RandomPoint(rnd, x0 - 25, x0 - 15, y0 - 40, y0 + 40);
                ASSPoint p2 = Common.RandomPoint(rnd, x0 - 45, x0 + 5, y0 - 40, y0 + 40);
                ASSPoint p3 = Common.RandomPoint(rnd, x0 - 45, x0 + 5, y0 - 40, y0 + 40);
                ASSPoint p4 = Common.RandomPoint(rnd, x0 - 25, x0 - 15, y0 - 40, y0 + 40);
                Bezier bz = new Bezier(p1, p2, p3, p4);
                ASSPoint p5 = Common.RandomPoint(rnd, x0 - 25, x0 - 15, y0 - 40, y0 + 40);
                ASSPoint p6 = Common.RandomPoint(rnd, x0 - 45, x0 + 5, y0 - 40, y0 + 40);
                ASSPoint p7 = Common.RandomPoint(rnd, x0 - 45, x0 + 5, y0 - 40, y0 + 40);
                ASSPoint p8 = Common.RandomPoint(rnd, x0 - 25, x0 - 15, y0 - 40, y0 + 40);
                Bezier bz2 = new Bezier(p5, p6, p7, p8);
                Bezier[] bzs = new Bezier[20];
                for (int i = 0; i < bzs.Length; i++)
                    bzs[i] = CreateBezier3(rnd, Common.RandomDouble(rnd, x0  + i * FontWidth - 40, x0 + i * FontWidth + 40), Common.RandomDouble(rnd, y0 - 40, y0 + 40), Common.RandomDouble(rnd, 10, 50), Common.RandomDouble(rnd, 10, 50));
                bz = CreateBezier3(rnd, x0 - 20, y0, 20, 40);
                bz2 = CreateBezier3(rnd, x0 - 20, y0, 20, 40);
                /*foreach (ASSPoint pt in bzs[0].Create(0.01f))
                {
                    ass_out.Events.Add(
                        new ASSEvent
                        {
                            Effect = "",
                            Style = "pt",
                            Layer = 10,
                            MarginL = "0000",
                            MarginR = "0000",
                            MarginV = "0000",
                            Name = "",
                            Start = 0,
                            End = 100,
                            Text = ASSEffect.pos(pt.X, pt.Y) + ASSEffect.a(1, "00") + ASSEffect.a(3, "FF") + ASSEffect.c(1, "FFFFFF") + ptstr
                        });
                }*/
                double t = (float)Common.RandomDouble(rnd, 0, 1);
                foreach (ASSPoint pt in mask.Points)
                {
                    string a1 = Common.ToHex2(255 - pt.Brightness);
                    string tarc = Common.scaleColor("0000FF", "FFFFFF", Math.Abs(Common.RandomDouble_Gauss(rnd, -1, 1, 2)));
                    double te = stt + 1 +(0.5 - Math.Abs(((double)t - 0.5)));
                    ass_out.Events.Add(
                        new ASSEvent
                        {
                            Effect = "",
                            Style = "pt",
                            Layer = 0,
                            MarginL = "0000",
                            MarginR = "0000",
                            MarginV = "0000",
                            Name = "",
                            Start = stt,
                            End = te,
                            Text = ASSEffect.pos(pt.X, pt.Y) + ASSEffect.a(1, a1) + ASSEffect.a(3, "FF") + ASSEffect.c(1, "0000FF") +
                                    ptstr
                        });
                    /*
                    ASSPoint tar = bz.Get(t);
                    if (Common.RandomBool(rnd, 0.5)) tar = bz2.Get(t);
                     ass_out.Events.Add(
                        new ASSEvent
                        {
                            Effect = "",
                            Style = "pt",
                            Layer = 0,
                            MarginL = "0000",
                            MarginR = "0000",
                            MarginV = "0000",
                            Name = "",
                            Start = stt + 1,
                            End = stt + 2 + (0.5 - Math.Abs(((double)t - 0.5))),
                            Text = ASSEffect.move(pt.X, pt.Y, tar.X, tar.Y) + ASSEffect.fad(0, 0.5) +
                                    ASSEffect.a(1, "77") + ASSEffect.a(3, "E7") + ASSEffect.c(1, "FFFFFF") + ASSEffect.c(3, "0000FF") +
                                    ASSEffect.bord(2) + ASSEffect.blur(2) +
                                    ptstr
                        });*/
                    double lastx = pt.X;
                    double lasty = pt.Y;
                    double tt0 = Common.RandomDouble(rnd, 0.2, 1);
                    tt0 = 1;
                    double tt1 = tt0 / 3;
                    t = (float)Common.RandomDouble(rnd, 0, 1);
                    for (int i = 0; i < bzs.Length; i++)
                    {
                        //if (t < 0.5) t = 0.5 - t; else t = 1.5 - t;
                        ASSPoint pb = bzs[i].Get((float)t);
                        double te0 = te + tt0 + (tt1 - Math.Abs(((double)t - 0.5)) / 0.5 * tt1);
                        ass_out.Events.Add(
                            new ASSEvent
                            {
                                Effect = "",
                                Style = "pt",
                                Layer = 0,
                                MarginL = "0000",
                                MarginR = "0000",
                                MarginV = "0000",
                                Name = "",
                                Start = te,
                                End = te0,
                                Text = ASSEffect.move(lastx, lasty, pb.X, pb.Y) + ASSEffect.fad(0, ((i + 1 == bzs.Length) ? 0.5 : 0)) +
                                        ASSEffect.a(1, "77") + ASSEffect.a(3, "E0") + ASSEffect.c(1, "FFFFFF") + ASSEffect.c(3, "0000FF") +
                                        ASSEffect.bord(3) + ASSEffect.blur(3) +
                                        ptstr
                            });
                        lastx = pb.X;
                        lasty = pb.Y;
                        te = te0;
                    }
                }
                break;
                x0 += mask.Width + this.FontSpace;
            }

            ass_out.SaveFile(OutFileName);
        }
    }
}
