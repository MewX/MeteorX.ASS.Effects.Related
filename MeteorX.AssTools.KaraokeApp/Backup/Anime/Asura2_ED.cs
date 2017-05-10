using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteorX.AssTools.KaraokeApp.Model;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class Asura2_ED : BaseAnime3
    {
        public Asura2_ED()
        {
            this.FontHeight = 40;
            this.FontSpace = 0;
            this.IsAvsMask = true;
            this.MarginLeft = 30;
            this.MarginBottom = 30;
            this.MarginTop = 30;
            this.MarginRight = 30;
            this.PlayResX = 1280;
            this.PlayResY = 720;
            this.InFileName = @"g:\workshop\asura2\ed\ed_k.ass";
            this.OutFileName = @"g:\workshop\asura2\ed\ed.ass";
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS();

            ass_out.Header = ass_in.Header;
            ass_out.Events = new List<ASSEvent>();

            Random rnd = new Random();

            for (int iEv = 0; iEv < ass_in.Events.Count; iEv++)
            {
                ASSEvent ev = ass_in.Events[iEv];
                List<KElement> kelems = ev.SplitK(true);

                bool isJp = iEv <= 13;
                if (!isJp) continue;
                if (iEv > 1) continue;
                //if (iEv < 11) continue;

                this.FontCharset = (byte)(isJp ? 128 : 1);
                this.FontName = isJp ? "DFSoGei-W5" : "汉仪粗宋繁";
                string evStyle = isJp ? "ed_jp" : "ed_cn";

                int tw = GetTotalWidth(ev);
                int x0 = (PlayResX - MarginLeft - MarginRight - tw) / 2 + MarginLeft;
                int startx0 = x0;
                int y0 = PlayResY - MarginBottom - FontHeight;
                if (iEv == 12) y0 -= FontHeight + 10;
                int kSum = 0;

                string outlines = "";
                List<ASSPoint> bordPoints = new List<ASSPoint>();

                for (int iK = 0; iK < kelems.Count; iK++)
                {
                    Console.WriteLine("{0} / {1} : {2} / {3}", iEv + 1, ass_in.Events.Count, iK + 1, kelems.Count);
                    KElement ke = kelems[iK];
                    Size sz = GetSize(ke.KText);
                    double kStart = ev.Start + kSum * 0.01;
                    double kEnd = kStart + ke.KValue * 0.01;
                    double kStart0 = kStart;
                    double kEnd0 = kEnd;
                    kSum += ke.KValue;
                    kStart = ke.KStart_NoSplit;
                    kEnd = ke.KEnd_NoSplit;
                    int x = x0 + this.FontSpace + sz.Width / 2;
                    int y = y0 + FontHeight / 2;
                    int x_an7 = x0;
                    int y_an7 = y0;
                    x0 += this.FontSpace + sz.Width;
                    if (ke.KText.Trim().Length == 0) continue;

                    string outlineString = GetOutline(x_an7, y_an7, ke.KText[0], FontName, FontCharset, FontHeight, 0, (int)(FontHeight * 8 * 0.86));
                    outlines += outlineString;

                    /*this.MaskStyle = "Style: Default,DFSoGei-W5,40,&HFFFFFFFF,&HFFFFFFFF,&H00FFFFFF,&HFFFFFFFF,0,0,0,0,100,100,1,0,0,0,0,5,0,0,0,1";
                    StringMask bordMask = GetMask(bord(1) + ke.KText, x, y);
                    bordPoints.AddRange(bordMask.Points);*/

                    ass_out.AppendEvent(50, evStyle, ev.Start - 0.2, kStart,
                        pos(x, y) + fad(0.3, 0) + a(1, "33") + c(1, "000000") +
                        ke.KText);
                    ass_out.AppendEvent(50, evStyle, kEnd, ev.End + 0.2,
                        pos(x, y) + fad(0, 0.3) + a(1, "33") + c(1, "000000") +
                        ke.KText);
                    for (int i = 1; i <= 5; i++)
                    {
                        double r = (double)i * 0.5;
                        ass_out.AppendEvent(50 - i, evStyle, ev.Start - 0.2, kEnd,
                            pos(x + r * 1.7, y + r * 1.7) + fad(0.3, 0) + a(1, "77") + c(1, "000000") + blur(r) +
                            ke.KText);
                        ass_out.AppendEvent(50 - i, evStyle, kEnd, ev.End + 0.2,
                            pos(x + r * 1.7, y + r * 1.7) + fad(0, 0.3) + a(1, "77") + c(1, "000000") + blur(r) +
                            ke.KText);
                    }

                }
            }

            ass_out.SaveFile(OutFileName);
            Console.WriteLine("Lines : {0}", ass_out.Events.Count);
        }
    }
}
