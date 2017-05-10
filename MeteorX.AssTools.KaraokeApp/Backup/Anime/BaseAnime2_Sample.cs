using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class BaseAnime2_Sample : BaseAnime2
    {
        public BaseAnime2_Sample()
        {
            InFileName = @"G:\Workshop\hayate\ed_k.ass";
            OutFileName = @"G:\Workshop\hayate\ed.ass";

            this.FontWidth = 35;
            this.FontHeight = 35;
            this.FontSpace = 3;

            this.PlayResX = 1280;
            this.PlayResY = 720;
            this.MarginBottom = 25;
            this.MarginLeft = 25;
            this.MarginRight = 25;
            this.MarginTop = 25;

            this.IsAvsMask = true;
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
                List<KElement> kelems = ev.SplitK(false);

                this.MaskStyle = "Style: Default,DFGMaruGothic-Md,35,&H00FF0000,&HFF000000,&HFFFFFFFF,&HFFFF0000,-1,0,0,0,100,100,2,0,1,2,0,5,25,25,25,128";
                int x0 = MarginLeft;
                int startx0 = x0;
                int y0 = PlayResY - MarginBottom - FontHeight;
                int kSum = 0;

                for (int iK = 0; iK < kelems.Count; iK++)
                {
                    Console.WriteLine("{0} / {1} : {2} / {3}", iEv + 1, ass_in.Events.Count, iK + 1, kelems.Count);
                    KElement ke = kelems[iK];
                    Size sz = GetSize(ke.KText);
                    double kStart = ev.Start + kSum * 0.01;
                    double kEnd = kStart + ke.KValue * 0.01;
                    kSum += ke.KValue;
                    int x = x0 + this.FontSpace + sz.Width / 2;
                    int y = y0 + FontHeight / 2;
                    int x_an7 = x0;
                    int y_an7 = y0;
                    StringMask mask = GetMask(ke.KText, x, y);
                    x0 += this.FontSpace + sz.Width;
                    if (ke.KText.Trim().Length == 0) continue;
                }
            }

            ass_out.SaveFile(OutFileName);
            Console.WriteLine("Lines : {0}", ass_out.Events.Count);
        }
    }
}
