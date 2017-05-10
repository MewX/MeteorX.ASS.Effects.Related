using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MeteorX.AssTools.KaraokeApp.Anime
{
    class Birdy2_OP : BaseAnime
    {
        public Birdy2_OP()
        {
            //InFileName = @"G:\Workshop\birdy2\op_k.ass";
            //utFileName = @"G:\Workshop\birdy2\op.ass";
            //InFileName = @"G:\Workshop\munto\op_k.ass";
            //OutFileName = @"G:\Workshop\munto\op.ass";

            this.FontWidth = 26;
            this.FontHeight = 26;
            this.FontSpace = 0;

            this.PlayResX = 848;
            this.PlayResY = 480;
            this.MarginBottom = 10;
            this.MarginLeft = 15;
            this.MarginRight = 15;
            this.MarginTop = 15;

            this.Font = new System.Drawing.Font("DFGMaruGothic-Md", 17);
        }

        public override void Run()
        {
            ASS ass_in = ASS.FromFile(this.InFileName);
            ASS ass_out = new ASS();

            ass_out.Header = ass_in.Header;
            ass_out.Events = new List<ASSEvent>();

            for (int iEv = 0; iEv < ass_in.Events.Count; iEv++)
            {
                ASSEvent ev = ass_in.Events[iEv];
                List<KElement> kelems = ev.SplitK(false);

                /// an7 pos
                int x0 = MarginLeft;
                int y0 = PlayResY - MarginBottom - FontHeight;

                int kSum = 0;

                for (int i = 0; i < kelems.Count; i++)
                {
                    KElement ke = kelems[i];
                    double r = (double)i / (double)(kelems.Count - 1);
                    Size sz = GetSize(ke.KText);

                    double kStart = kSum * 0.01;
                    double kEnd = kStart + ke.KValue * 0.01;

                    /// an5 pos
                    int x = x0 + this.FontSpace + sz.Width / 2;
                    int y = y0;

                    x0 += this.FontSpace + sz.Width;
                    y0 = y0;

                    double t0 = ev.Start + kStart;
                    double t1 = t0 + 0.2; // K出现
                    double t2 = ev.End - 0.5 + r * 0.5; // 保持
                    if (t1 > t2) t2 = t1;
                    double t3 = t2 + 0.2; // 消失

                    string col = (iEv >= 3 && iEv <= 6) ? "111111" : "EEEEEE";
                    col = "FFDF3A";
                    if (iEv > 1) col = "111111";
                    if (iEv > 6) col = "FFDF3A";

                    ass_out.Events.Add(
                        ev.StartReplace(t0).EndReplace(t1).TextReplace(
                        ASSEffect.pos(x, y) +
                        ASSEffect.c(1, col) + ASSEffect.c(3, col) +
                        ASSEffect.ybord(18) + ASSEffect.be(18) +
                        //ASSEffect.a(1, "00").t() +
                        ASSEffect.t(0, t1 - t0, ASSEffect.a(3, "00").t() + ASSEffect.ybord(1).t() + ASSEffect.be(1).t()) +
                        ke.KText));
                    ass_out.Events.Add(
                        ev.StartReplace(t1).EndReplace(t2).TextReplace(
                        ASSEffect.pos(x, y) +
                        ASSEffect.c(1, col) + ASSEffect.c(3, col) +
                        ASSEffect.blur(1) +
                        //ASSEffect.a(1, "00") + 
                        ASSEffect.a(3, "00") +
                        ke.KText));
                    ass_out.Events.Add(
                        ev.StartReplace(t2).EndReplace(t3).TextReplace(
                        ASSEffect.pos(x, y) +
                        ASSEffect.c(1, col) + ASSEffect.c(3, col) +
                        //ASSEffect.a(1, "00") +
                        ASSEffect.a(3, "00") + ASSEffect.blur(1) +
                        ASSEffect.t(0, t3 - t2, ASSEffect.a(1, "FF").t() + ASSEffect.a(3, "FF").t() + ASSEffect.ybord(18).t() + ASSEffect.blur(18).t()) +
                        ke.KText));

                    kSum += ke.KValue;
                }
            }

            ass_out.SaveFile(OutFileName);
        }
    }
}
