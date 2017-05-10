using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class ZokuNatsume_ED : BaseAnime
    {
        int ScaleRate = 1;

        public ZokuNatsume_ED()
        {
            InFileName = @"G:\Workshop\natsume2\ed_k.ass";
            OutFileName = @"G:\Workshop\natsume2\ed.ass";

            this.FontWidth = 33 * ScaleRate;
            this.FontHeight = 28 * ScaleRate;
            this.FontSpace = -4 * ScaleRate;

            this.PlayResX = 848 * ScaleRate;
            this.PlayResY = 480 * ScaleRate;
            this.MarginBottom = 37 * ScaleRate;
            this.MarginLeft = 15 * ScaleRate;
            this.MarginRight = 10 * ScaleRate;
            this.MarginTop = 10 * ScaleRate;

            this.Font = new System.Drawing.Font("DFGKaiSho-Md", 28 * ScaleRate);
        }


        public override void Run()
        {
            ASS ass_in = ASS.FromFile(InFileName);
            ASS ass_out = new ASS();
            ass_out.Header = ass_in.Header;
            ass_out.Events = new List<ASSEvent>();

            for (int iEv = 0; iEv < ass_in.Events.Count; iEv++)
            {
                ASSEvent ev = ass_in.Events[iEv];
                List<KElement> kelems = ev.SplitK(iEv > 130);
                StringBuilder sb = new StringBuilder();
                int kSum = 30;
                ev.Start -= 0.3;
                for (int iK = 0; iK < kelems.Count; iK++)
                {
                    KElement ke = kelems[iK];
                    double kStart = kSum * 0.01;
                    sb.Append(
                        ASSEffect.be(1) +
                        ASSEffect.a(3, "FF") + ASSEffect.a(1, "FF") + ASSEffect.t(0, 0.3, ASSEffect.a(3, "00").t() + ASSEffect.a(1, "00").t()) + 
                        //ASSEffect.t(kStart, kStart + 0.3, ASSEffect.a(1, "77").t()) +
                        ASSEffect.t(kStart + 0, kStart + 0.5, ASSEffect.a(1, "FF").t() + ASSEffect.a(3, "FF").t()) +                        
                        ke.KText + ASSEffect.r()
                        );
                    kSum += ke.KValue;
                    if (ev.Last < kStart + 1.0)
                        ev.End += (kStart + 1.0 - ev.Last);
                }
                ass_out.Events.Add(ev.LayerReplace(iEv).TextReplace(sb.ToString()));
            }

            ass_out.SaveFile(OutFileName);
        }
    }
}