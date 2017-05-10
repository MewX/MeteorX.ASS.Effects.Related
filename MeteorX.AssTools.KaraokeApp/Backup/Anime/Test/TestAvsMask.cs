using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MeteorX.AssTools.KaraokeApp.Anime.Test
{
    class TestAvsMask : BaseAnime2
    {
        public TestAvsMask()
        {
            InFileName = @"G:\Workshop\maria holic\ed_k.ass";
            OutFileName = @"G:\Workshop\maria holic\ed.ass";

            this.FontWidth = 35;
            this.FontHeight = 35;
            this.FontSpace = 2;

            this.PlayResX = 848;
            this.PlayResY = 480;
            this.MarginBottom = 15;
            this.MarginLeft = 15;
            this.MarginRight = 15;
            this.MarginTop = 15;

            this.Font = new System.Drawing.Font("DFPMaruMoji-W9", 35, GraphicsUnit.Pixel);

            this.MaskStyle = "Style: Default,DFPMaruMoji-W9,35,&H00FF0000,&HFF111111,&H000000FF,&HFF000000,-1,0,0,0,100,100,0,0,1,2,0,7,30,30,10,128";
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS();

            ass_out.Header = ass_in.Header;
            ass_out.Events = new List<ASSEvent>();

            string ptString = @"{\p8}m 0 0 l 128 0 128 128 0 128";

            for (int iEv = 0; iEv < ass_in.Events.Count; iEv++)
            {
                if (iEv != 0) continue;
                ASSEvent ev = ass_in.Events[iEv];
                List<KElement> kelems = ev.SplitK(true);

                /// an7 pos
                int x0 = (PlayResX - GetTotalWidth(ev)) / 2;
                int y0 = PlayResY - MarginBottom - FontHeight;

                Random rnd = new Random();

                int kSum = 0;

                for (int iK = 0; iK < kelems.Count; iK++)
                {
                    Console.WriteLine("{0} / {1} : {2} / {3}", iEv + 1, ass_in.Events.Count, iK + 1, kelems.Count);

                    KElement ke = kelems[iK];
                    double r = (double)iK / (double)(kelems.Count - 1);
                    StringMask mask = GetMask(ke.KText, x0, y0);
                    Size sz = new Size(mask.Width, mask.Height);

                    /// an5 pos
                    int x = x0 + this.FontSpace + sz.Width / 2;
                    int y = y0 + FontHeight;

                    x0 += this.FontSpace + sz.Width;
                    y0 = y0;

                    foreach (ASSPoint pt in mask.Points)
                    {
                        ass_out.Events.Add(
                            ev.StyleReplace("pt").TextReplace(
                            ASSEffect.pos(pt.X, pt.Y) +
                            ASSEffect.a(1, Common.ToHex2(255 - pt.Brightness)) + ASSEffect.c(1, "FFFFFF") + ASSEffect.a(3, "FF") +
                            ptString
                            ));
                    }
                }
            }
            ass_out.SaveFile(OutFileName);
        }
    }
}
